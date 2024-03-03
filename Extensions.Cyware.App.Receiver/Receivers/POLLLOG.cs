using System;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using System.Collections.Generic;
using System.IO;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using System.Threading.Tasks;
using KTI.Moo.Extensions.Cyware.App.Receiver.Helpers;
using System.Linq;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Receivers
{
    public class PollLog : CompanySettings
    {
        private readonly IBlobService _blobService;
        private readonly IQueueService _queueService;
        private readonly string storetransactioncontainer = Environment.GetEnvironmentVariable("StoreTransactionContainer");
        private const string QueueName = "poll-storetransactions-queue";

        public PollLog(IBlobService blobService, IQueueService queueService)
        {
            _blobService = blobService;
            _queueService = queueService;
        }

        [FunctionName("PollLog")]
        public async Task Run([BlobTrigger("%PollContainer%/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes at" + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
                if (!name.Contains("archive"))
                {
                    await ProcessAsync(myBlob, Environment.GetEnvironmentVariable("PollContainer"), name);
                }
            }
            catch (Exception ex)
            {
                // Create Main QueueClient object
                QueueClient mainQueue = new(ConnectionString, QueueName);

                // Create poison QueueClient object
                string poisonQueueName = $"{mainQueue.Name}-poison";
                QueueClient poisonQueue = new(ConnectionString, poisonQueueName);
                poisonQueue.CreateIfNotExists();

                var FileObject = new { Filename = name };

                // Create object for current data and error message
                QueueErrorMessage queueItemWithErrorMessage = new()
                {
                    Data = JsonConvert.SerializeObject(FileObject),
                    ErrorMessage = ex.Message
                };

                string updatedQueueItem = JsonConvert.SerializeObject(queueItemWithErrorMessage);

                _queueService.MoveToPoisonQueueFromMainQueue(mainQueue, poisonQueue, "", "", updatedQueueItem);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ProcessAsync(Stream stream, string containerName, string blobName)
        {
            // Read file
            var pollRecord = _blobService.ReadStreamFile(stream);

            // Move to archive folder
            string destinationBlobName = $"archive/{blobName}";
            _blobService.MoveBlob(containerName, blobName, destinationBlobName);

            string unixNewline = "\n";

            //string[] records = pollRecord.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] records = pollRecord.Split(new[] { unixNewline }, StringSplitOptions.RemoveEmptyEntries);

            // Check for EOF marker
            string EOFLine = records[records.Length - 1].Trim();

            if (!EOFLine.StartsWith("99"))
            {
                // If the first two characters are not "99", throw an error
                throw new InvalidOperationException("EOF Marker '99' not found or misplaced.");
            }

            List<FOSalesTransactionHeader> headers = new List<FOSalesTransactionHeader>();
            List<FOSalesTransactionDetail> details = new List<FOSalesTransactionDetail>();
            List<FOSalesTenderDetail> tenders = new List<FOSalesTenderDetail>();
            List<FOSalesTransactionDiscount> discounts = new List<FOSalesTransactionDiscount>();

            foreach (string record in records)
            {
                string[] fields = record.Split('/');

                if (fields.Length >= 8)
                {
                    string recordType = fields[0];

                    switch (recordType)
                    {
                        case "07":
                            // Discount details record
                            //Console.WriteLine("Discount Details Record:");
                            var transDiscount = new FOSalesTransactionDiscount(fields);
                            discounts.Add(transDiscount);
                            break;
                        case "03":
                            // Sales header record
                            //Console.WriteLine("Sales Header Record:");
                            var transHeader = new FOSalesTransactionHeader(fields);
                            headers.Add(transHeader);
                            break;
                        case "04":
                            // Sales detail record
                            var transDetails = new FOSalesTransactionDetail(fields);
                            details.Add(transDetails);
                            break;
                        case "05":
                            // Sales tender record
                            var transPayments = new FOSalesTenderDetail(fields);
                            tenders.Add(transPayments);
                            break;
                        case "11":
                            // Cash out totals record
                            break;
                        case "12":
                            // Cash in record
                            break;
                        case "99":
                            // Reason code record
                            break;
                        default:
                            // Unknown record type
                            break;
                    }
                }
                else
                {
                    // Invalid record
                }
            }
            string blobNameWithoutExtension = FileHelper.GetFileName(blobName);
            StoreTransactions blobFiles = await CreateBlobAsync(blobNameWithoutExtension, headers, details, tenders, discounts);

            return ProcessToQueue(blobFiles);
        }

        private async Task<StoreTransactions> CreateBlobAsync(string blobName, List<FOSalesTransactionHeader> headers, List<FOSalesTransactionDetail> details, List<FOSalesTenderDetail> tenders, List<FOSalesTransactionDiscount> discounts)
        {
            string headersBlob = $"POS/Headers/{blobName}.json";
            string linesBlob = $"POS/Lines/{blobName}.json";
            string discountsBlob = $"POS/Discounts/{blobName}.json";
            string paymentsBlob = $"POS/Tenders/{blobName}.json";

            await _blobService.CreateFile(storetransactioncontainer, headersBlob, JsonConvert.SerializeObject(headers));
            await _blobService.CreateFile(storetransactioncontainer, linesBlob, JsonConvert.SerializeObject(details));
            // Check if discounts list is not empty before creating the discounts blob
            if (discounts.Any())
            {
                await _blobService.CreateFile(storetransactioncontainer, discountsBlob, JsonConvert.SerializeObject(discounts));
            }
            else
            {
                // Assign an empty string to discountsBlob if discounts list is empty
                discountsBlob = "";
            }
            await _blobService.CreateFile(storetransactioncontainer, paymentsBlob, JsonConvert.SerializeObject(tenders));

            StoreTransactions storeTransactionsBlob = new()
            {
                Headers = headersBlob,
                Lines = linesBlob,
                Discounts = discountsBlob,
                Payments = paymentsBlob
            };

            return storeTransactionsBlob;
        }

        public bool ProcessToQueue(StoreTransactions blobFiles)
        {
            try
            {
                QueueClient _queueClient = new(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "cyware-storetransactions-queue", new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });

                _queueClient.CreateIfNotExists();

                // Convert StoreTransactions object to a JSON string
                string jsonBlobFiles = JsonConvert.SerializeObject(blobFiles);

                // Send the JSON string as a queue message
                _queueClient.SendMessage(jsonBlobFiles);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}