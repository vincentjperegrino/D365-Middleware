using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class Discount : CompanySettings
    {
        private readonly IDiscount<KTI.Moo.Extensions.Cyware.Model.Discount> _discount;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-discount-dispatcher";

        public Discount(IQueueService queueService, IDiscount<Model.Discount> discount)
        {
            _queueService = queueService;
            _discount = discount;
        }

        //[FunctionName("Discount")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                var discount = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.Discount>(myQueueItem);
                var result = _discount.Upsert(discount);
                log.LogInformation($"C# Discount - Queue trigger function processed: {myQueueItem}");
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
