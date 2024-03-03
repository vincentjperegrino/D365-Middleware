using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.CRM.Model.DTO;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class Price : CompanySettings
    {
        private readonly IPrices<PriceHeader> _prices;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-price-dispatcher";

        public Price(IQueueService queueService, IPrices<PriceHeader> prices)
        {
            _queueService = queueService;
            _prices = prices;
        }

        //[FunctionName("Price")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {

            try
            {
                var priceHeader = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.PriceHeader>(myQueueItem);
                var result = _prices.Upsert(priceHeader);
                log.LogInformation($"C# PriceHeader - Queue trigger function processed: {myQueueItem}");
            }
            catch (Exception ex)
            {
                if (dequeueCount > 5)
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
                        ErrorMessage = ex.Message
                    };

                    string updatedQueueItem = JsonConvert.SerializeObject(queueItemWithErrorMessage);

                    _queueService.MoveToPoisonQueueFromMainQueue(mainQueue, poisonQueue, Id, PopReceipt, updatedQueueItem);
                    return;
                }
                throw new Exception(ex.Message);
            }
        }
    }
}

