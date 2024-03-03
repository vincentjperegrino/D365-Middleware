using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using Moo.FO.App.Helper;
using KTI.Moo.Extensions.Cyware.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Azure.WebJobs.Host;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Moo.FO.App.Batch
{
    public class Receiver : CompanySettings
    {
        private readonly IQueueService _queueService;
        private readonly IBlobService _blobService;
        private readonly IEmailNotification _emailNotification;
        private const string QueueName = "fo-storetransactions-queue";
        private readonly string storetransactioncontainer = Environment.GetEnvironmentVariable("StoreTransactionContainer");
        private List<string> transactionNumbers;
        private readonly Stopwatch stopwatch = new();
        public Receiver(IQueueService queueService, IBlobService blobService, IEmailNotification emailNotification)
        {
            _queueService = queueService;
            _blobService = blobService;
            _emailNotification = emailNotification;
        }

        [FunctionName("FO-StoreTransactions-Receiver")]
        public async Task RunAsync([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            stopwatch.Start();
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

                int company = int.Parse(CompanyId);

                var domainType = Base.Helpers.DomainType.order;

                if (company == 0)
                {
                    throw new Exception("Attribute companyid missing.");
                }

                if (string.IsNullOrEmpty(domainType))
                {
                    throw new Exception("Attribute domainType missing.");
                }
                long batchJobId = await WaitForSyncCompletion(dequeueCount, log, company);

                await StoreTransactionProcess(log, myQueueItem, company, domainType, batchJobId);
            }
            catch (Exception ex)
            {
                if (dequeueCount >= 5)
                {
                    // Create Main QueueClient object
                    QueueClient mainQueue = new(ConnectionString, QueueName);

                    // Create poison QueueClient object
                    string poisonQueueName = $"{mainQueue.Name}-poison";
                    QueueClient poisonQueue = new(ConnectionString, poisonQueueName);
                    poisonQueue.CreateIfNotExists();

                    // Create object for current data and error message
                    QueueErrorMessage queueItemWithErrorMessage = new()
                    {
                        Data = myQueueItem,
                        ErrorMessage = JsonConvert.SerializeObject(transactionNumbers),
                        BatchResponse = ex.Message
                    };

                    string updatedQueueItem = JsonConvert.SerializeObject(queueItemWithErrorMessage);

                    _queueService.MoveToPoisonQueueFromMainQueue(mainQueue, poisonQueue, Id, PopReceipt, updatedQueueItem);
                    return;
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task StoreTransactionProcess(ILogger log, string blobFiles, int? company, string domainType, long batchJobId)
        {
            bool setBatchJob = true;
            if (domainType != Base.Helpers.DomainType.order)
                return;

            StoreTransactions storeTransactions = JsonConvert.DeserializeObject<StoreTransactions>(blobFiles);
            string batchId = FileHelper.GetFileName(storeTransactions.Headers);
            string batchError = batchId;

            string headersBatch = _blobService.ReadFile(storetransactioncontainer, storeTransactions.Headers);
            string linesBatch = _blobService.ReadFile(storetransactioncontainer, storeTransactions.Lines);
            string discountsBatch = !String.IsNullOrEmpty(storeTransactions.Discounts) ? _blobService.ReadFile(storetransactioncontainer, storeTransactions.Discounts) : "";
            string tendersBatch = _blobService.ReadFile(storetransactioncontainer, storeTransactions.Payments);

            List<string> headersLocation = new List<string>();
            List<string> linesLocation = new List<string>();
            List<string> discountsLocation = new List<string>();

            transactionNumbers = GetTransactionNumbers(headersBatch);
            int headerCount = StringHelper.CountKeywordOccurrences(headersBatch, $"--{batchId}");
            int lineCount = StringHelper.CountKeywordOccurrences(linesBatch, $"--{batchId}");
            int discountCount = StringHelper.CountKeywordOccurrences(discountsBatch, $"--{batchId}");
            int paymentCount = StringHelper.CountKeywordOccurrences(tendersBatch, $"--{batchId}");

            try
            {
                log.LogInformation("Header Process");
                batchError = $"Headers_{batchId}";
                string headersResponse = await BatchProcessAsync(log, company, batchId, headersBatch, headerCount);
                headersLocation = GetLocations(company, headersResponse);
                await _blobService.CreateFile(storetransactioncontainer, $"Temp/{batchError}.txt", headersResponse);

                log.LogInformation("Lines Process");
                batchError = $"Lines_{batchId}";
                string linesResponse = await BatchProcessAsync(log, company, batchId, linesBatch, lineCount);
                linesLocation = GetLocations(company, linesResponse);
                await _blobService.CreateFile(storetransactioncontainer, $"Temp/{batchError}.txt", linesResponse);

                string discountsResponse = "";
                if (!string.IsNullOrEmpty(discountsBatch))
                {
                    log.LogInformation("Discounts Process");
                    batchError = $"Discounts_{batchId}";
                    discountsResponse = await BatchProcessAsync(log, company, batchId, discountsBatch, discountCount);
                    discountsLocation = GetLocations(company, discountsResponse);
                    await _blobService.CreateFile(storetransactioncontainer, $"Temp/{batchError}.txt", discountsResponse);
                }

                log.LogInformation("Tenders Process");
                batchError = $"Tenders_{batchId}";
                string tendersResponse = await BatchProcessAsync(log, company, batchId, tendersBatch, paymentCount);
                await _blobService.CreateFile(storetransactioncontainer, $"Temp/{batchError}.txt", discountsResponse);

                // Archive success response content
                await ArchiveResponsesAsync(batchId, headersResponse, linesResponse, discountsResponse, tendersResponse);

                // Send Email Notification
                SendEmail(transactionNumbers, batchId, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Rollback Process");

                string errorResponse = $"Failed/{batchError}.txt";
                await _blobService.CreateFile(storetransactioncontainer, errorResponse, ex.Message);

                if (!string.IsNullOrEmpty(discountsBatch))
                {
                    await RollbackProcessAsync(log, company, discountsLocation);
                }
                await RollbackProcessAsync(log, company, linesLocation);
                await RollbackProcessAsync(log, company, headersLocation);

                setBatchJob = false;

                throw new Exception(errorResponse);
            }
            finally
            {
                FOBatchJob foBatchJob = new((int)company);

                // Update KationPOS_OngoingSynching to No
                var updateSyncObject = new { KationPOS_OngoingSynching = "No" };
                string updateSyncString = JsonConvert.SerializeObject(updateSyncObject);
                await foBatchJob.UpdateSync(log, updateSyncString);

                // Set batch job status to Waiting
                if (setBatchJob)
                {
                    var setJobStatusObject = new { batchJobId };
                    string setJobStatusString = JsonConvert.SerializeObject(setJobStatusObject);
                    await foBatchJob.SetBatchJobToWaiting(log, setJobStatusString);
                }
            }
        }

        private async Task<long> WaitForSyncCompletion(int dequeueCount, ILogger log, int? company)
        {
            int retryCounter = 0;
            TimeSpan initialDelay = TimeSpan.FromSeconds(5); // Initial retry delay is 5 seconds
            const int maxRetryCount = 3;
            const int maxAccumulatedMinutes = 9;

            try
            {
                while (true)
                {
                    var batchJob = await GetStatusAsync(log, company);
                    if (batchJob.Status == "Executing")
                    {
                        retryCounter++;
                        TimeSpan retryDelay = initialDelay * (1 << (int)Math.Min(retryCounter, maxRetryCount));
                        log.LogInformation($"Time elapsed: {stopwatch.Elapsed.TotalMinutes}");
                        if (stopwatch.Elapsed.TotalMinutes >= maxAccumulatedMinutes)
                        {
                            if (dequeueCount >= 5)
                            {
                                SendTimeoutNotification(batchJob.BatchJobRecId.ToString(), log);
                            }
                            throw new FunctionTimeoutException();
                        }
                        log.LogInformation($"RetryCount: {retryCounter}  -- Retrying in {retryDelay.TotalSeconds} seconds...");
                        await Task.Delay(retryDelay);
                    }
                    else
                    {
                        break;
                    }
                }

                // Get batchJobId
                FOBatchJob foBatchJob = new((int)company);
                dynamic POSParameter = await foBatchJob.GetPOSParameters<dynamic>(log);
                long batchJobId = POSParameter.KationPOS_SyncBatchJobID;

                // Update KationPOS_OngoingSynching to Yes
                var jsonObject = new { KationPOS_OngoingSynching = "Yes" };
                string jsonString = JsonConvert.SerializeObject(jsonObject);
                await foBatchJob.UpdateSync(log, jsonString);

                return batchJobId;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> BatchProcessAsync(ILogger log, int? company, string batchId, string content, int requestCount)
        {
            Domain.Batch.StoreTransactionBatch batchDomain = new((int)company);
            return await batchDomain.upsert(log, batchId, content, requestCount);
        }

        private List<string> GetLocations(int? company, string responseContent)
        {
            Domain.Batch.StoreTransactionBatch batchDomain = new((int)company);
            return batchDomain.ProcessResponse(responseContent);
        }

        private async Task RollbackProcessAsync(ILogger log, int? company, List<string> locations)
        {
            if (locations.Count > 0)
            {
                Domain.Batch.StoreTransactionBatch batchDomain = new((int)company);
                await batchDomain.delete(log, locations);
            }
        }

        private async Task<dynamic> GetStatusAsync(ILogger log, int? company)
        {
            FOBatchJob foBatchJob = new((int)company);

            dynamic POSParameters = await foBatchJob.GetPOSParameters<dynamic>(log);

            dynamic BatchJobStatus = await foBatchJob.GetBatchJobStatus<dynamic>(log, POSParameters.KationPOS_SyncBatchJobID.ToString());

            return BatchJobStatus;
        }

        private IEnumerable<Dictionary<string, object>> TransformData(List<string> transactionNumbers)
        {
            // Create a list of transactions based on provided transaction numbers
            var transactions = transactionNumbers.Select(transactionNumber =>
            {
                var transactionParts = transactionNumber.Split('-');

                return new Dictionary<string, object>
                {
                    { "Register", transactionParts[0] },
                    { "Transaction Number", transactionParts[1] },
                    { "Status", "Integrated" }
                };
            });

            return transactions;
        }

        private List<string> GetTransactionNumbers(string data)
        {
            string pattern = @"(?:\""TransactionNumber\"":""(\d+)"".*?\""Terminal\"":""(\d+)"")";

            // Match the pattern against the data and extract terminal and transaction numbers
            List<string> terminalTransactionNumbers = Regex.Matches(data, pattern)
                .Select(m => $"{int.Parse(m.Groups[2].Value)}-{m.Groups[1].Value}")
                .OrderBy(s => s, StringComparer.Ordinal)
                .ToList();

            return terminalTransactionNumbers;
        }

        private async Task ArchiveResponsesAsync(string batchId, string headersResponses, string linesResponses, string discountsResponses, string tendersResponses)
        {
            string headerPath = $"Success/Headers/{batchId}.txt";
            string linePath = $"Success/Lines/{batchId}.txt";
            string discountPath = $"Success/Discounts/{batchId}.txt";
            string tenderPath = $"Success/Tenders/{batchId}.txt";

            await _blobService.CreateFile(storetransactioncontainer, headerPath, headersResponses);
            await _blobService.CreateFile(storetransactioncontainer, linePath, linesResponses);
            if (!string.IsNullOrEmpty(discountsResponses))
            {
                await _blobService.CreateFile(storetransactioncontainer, discountPath, discountsResponses);
            }
            await _blobService.CreateFile(storetransactioncontainer, tenderPath, tendersResponses);
        }

        private void SendEmail(List<string> transactionNumbers, string fileName, ILogger log)
        {
            string subject = "[Transaction] Successful Integration";
            string messageBody = "Good day!\r\n\r\nStore transactions have been successfully integrated to Dynamics 365.\r\nPlease see attached for the list of transaction numbers that were integrated.\r\n\r\nJoel's Place";
            IEnumerable<Dictionary<string, object>> attachmentData = TransformData(transactionNumbers);
            _emailNotification.Notify(subject, messageBody, attachmentData, fileName, log);
        }

        private void SendTimeoutNotification(string jobId, ILogger log)
        {
            string dateToday = DateTime.Now.AddHours(8).ToString("dddd, dd MMMM yyyy");
            string subject = "!IMPORTANT [Transaction] Stuck Batch Job";
            string messageBody = $"Good day!\r\n\r\n\r\nWe've encountered an issue with the Dynamics 365 Finance & Operations batch job. It appears to be stuck in an \"Executing\" status.\r\n\r\nTo resolve this issue and continue store transaction operations, please follow these steps:\r\n\r\n1. Cancel and withhold the batch job with the following job ID: {jobId}.\r\n2. Re-upload the pollog that was generated on {dateToday}.\r\n\r\nThank you for your attention to this matter.\r\n\r\n\r\nJoel's Place";
            _emailNotification.NotifyWithoutAttachment(subject, messageBody, log);
        }
    }
}

