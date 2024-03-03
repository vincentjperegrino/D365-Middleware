using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTI.Moo.Extensions.Core.Domain;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.CRM.Model.DTO;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class DiscountProduct : CompanySettings
    {
        private readonly IDiscountProduct<Model.DiscountProduct> _discountProduct;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-discountproduct-dispatcher";
        public DiscountProduct(IQueueService queueService, IDiscountProduct<Model.DiscountProduct> discountProduct)
        {
            _queueService = queueService;
            _discountProduct = discountProduct;
        }

        //[FunctionName("DiscountProduct")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                Model.DiscountProduct discountProduct = JsonConvert.DeserializeObject<Model.DiscountProduct>(myQueueItem);
                Model.DiscountProduct result = _discountProduct.Upsert(discountProduct);
                log.LogInformation($"C# DiscountProduct - Queue trigger function processed: {myQueueItem}");
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
