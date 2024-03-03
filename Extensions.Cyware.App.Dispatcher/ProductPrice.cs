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
    public class ProductPrice : CompanySettings
    {
        private readonly IProductPrice<Model.ProductPrice> _productPrice;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-productprice-dispatcher";

        public ProductPrice(IQueueService queueService, IProductPrice<Model.ProductPrice> productPrice)
        {
            _queueService = queueService;
            _productPrice = productPrice;
        }

        //[FunctionName("ProductPrice")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                Model.ProductPrice productPrice = JsonConvert.DeserializeObject<Model.ProductPrice>(myQueueItem);
                Model.ProductPrice result = _productPrice.Upsert(productPrice);
                log.LogInformation($"C# ProductPrice - Queue trigger function processed: {myQueueItem}");
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
