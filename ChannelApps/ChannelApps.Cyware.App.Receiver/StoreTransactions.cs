using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using ChannelApps.Cyware.App.Receiver.Helpers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Azure.Amqp.Framing;

namespace ChannelApps.Cyware.App.Receiver
{
    public class StoreTransactions : CompanySettings
    {
        private readonly IQueueService _queueService;
        private readonly IBlobService _blobService;
        private const string QueueName = "cyware-storetransactions-queue";
        private readonly string storetransactioncontainer = Environment.GetEnvironmentVariable("StoreTransactionContainer");
        private List<string> transactionNumbers;

        public StoreTransactions(IQueueService queueService, IBlobService blobService)
        {
            _queueService = queueService;
            _blobService = blobService;
        }

        [FunctionName("FO-StoreTransactions")]
        public async Task Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
                bool result = await ProcessAsync(log, myQueueItem);
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
        }

        public async Task<bool> ProcessAsync(ILogger log, string queueItem)
        {
            int companyID = int.Parse(CompanyId);
            KTI.Moo.Extensions.Cyware.Model.StoreTransactions blobFiles = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.StoreTransactions>(queueItem);
            string headersBatch = _blobService.ReadFile(storetransactioncontainer, blobFiles.Headers);
            transactionNumbers = GetTransactionNumbers(headersBatch);
            D365FOConfig config = await ConfigProcess(log, companyID);
            List<FOSalesTransactionHeader> headers = JsonConvert.DeserializeObject<List<FOSalesTransactionHeader>>(_blobService.ReadFile(storetransactioncontainer, blobFiles.Headers));
            List<FOSalesTransactionDetail> lines = JsonConvert.DeserializeObject<List<FOSalesTransactionDetail>>(_blobService.ReadFile(storetransactioncontainer, blobFiles.Lines));
            List<FOSalesTransactionDiscount> discounts = string.IsNullOrEmpty(blobFiles.Discounts)
                ? new List<FOSalesTransactionDiscount>()
                : JsonConvert.DeserializeObject<List<FOSalesTransactionDiscount>>(_blobService.ReadFile(storetransactioncontainer, blobFiles.Discounts));
            List<FOSalesTenderDetail> tenders = JsonConvert.DeserializeObject<List<FOSalesTenderDetail>>(_blobService.ReadFile(storetransactioncontainer, blobFiles.Payments));

            var storeTransactionsDomain = new Cyware.StoreTransactions(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), _blobService, storetransactioncontainer);
            return await storeTransactionsDomain.StoreTransactionsProcess(headers, lines, discounts, tenders, config);
        }

        private async Task<D365FOConfig> ConfigProcess(ILogger log, int? company)
        {
            FOConfig foConfig = new((int)company);

            return await foConfig.GetAllConfig(log);
        }

        private List<string> GetTransactionNumbers(string data)
        {
            string pattern = @"(?:\""TransactionNumber\"":""(\d+)"".*?\""Terminal\"":""(\d+)"")";

            // Match the pattern against the data and extract terminal and transaction numbers
            List<string> terminalTransactionNumbers = Regex.Matches(data, pattern)
                .Select(m => $"{ m.Groups[2].Value}-{m.Groups[1].Value}")
                .OrderBy(s => s, StringComparer.Ordinal)
                .ToList();

            return terminalTransactionNumbers;
        }

    }
}
