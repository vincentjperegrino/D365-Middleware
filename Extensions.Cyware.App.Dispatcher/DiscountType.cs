using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class DiscountType : CompanySettings
    {
        private readonly IDiscountType<Model.DiscountType> _discountType;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-discounttype-dispatcher";

        public DiscountType(IDiscountType<Model.DiscountType> discountType, IQueueService queueService)
        {
            _discountType = discountType;
            _queueService = queueService;
        }

        //[FunctionName("DiscountType")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                Model.DiscountType discountProduct = JsonConvert.DeserializeObject<Model.DiscountType>(myQueueItem);
                Model.DiscountType result = _discountType.Upsert(discountProduct);
                log.LogInformation($"C# DiscountType - Queue trigger function processed: {myQueueItem}");
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
