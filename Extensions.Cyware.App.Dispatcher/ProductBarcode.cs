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
    public class ProductBarcode : CompanySettings
    {
        private readonly IProductBarcode<Model.ProductBarcode> _productBarcode;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-productbarcode-dispatcher";


        public ProductBarcode(IQueueService queueService, IProductBarcode<Model.ProductBarcode> productBarcode)
        {
            _queueService = queueService;
            _productBarcode = productBarcode;
        }

        //[FunctionName("ProductBarcode")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                Model.ProductBarcode productBarcode = JsonConvert.DeserializeObject<Model.ProductBarcode>(myQueueItem);
                Model.ProductBarcode result = _productBarcode.Upsert(productBarcode);
                log.LogInformation($"C# ProductBarcode - Queue trigger function processed: {myQueueItem}");
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
