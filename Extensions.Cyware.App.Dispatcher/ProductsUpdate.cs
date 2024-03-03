using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTI.Moo.Extensions.Core.Domain;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class ProductsUpdate : CompanySettings
    {
        private readonly IProducts<Model.Products> _products;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-productupdate-dispatcher";

        public ProductsUpdate(IQueueService queueService, IProducts<Model.Products> products)
        {
            _queueService = queueService;
            _products = products;
        }

        //[FunctionName("ProductUpdate")]  ////DISABLED TEMPORARILY, POLL51 WAS CREATED AT THE SAME TIME WITH POLL53 IN PRODUCTINITIAL MODULE.
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                Model.Products products = JsonConvert.DeserializeObject<Model.Products>(myQueueItem);
                Model.Products result = _products.Upsert(products, "POLL51.DWN");
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
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
                    var queueItemWithErrorMessage = new
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
