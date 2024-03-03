using Azure.Storage.Queues;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class DiscountLocation : CompanySettings
    {
        private readonly IDiscountLocation<KTI.Moo.Extensions.Cyware.Model.DiscountLocation> _discountLocation;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-discountlocation-dispatcher";

        public DiscountLocation(IDiscountLocation<Model.DiscountLocation> discountLocation, IQueueService queueService)
        {
            _discountLocation = discountLocation;
            _queueService = queueService;
        }

        //[FunctionName("DiscountLocation")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                var discountLocation = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.DiscountLocation>(myQueueItem);
                var result =  _discountLocation.Upsert(discountLocation);
                log.LogInformation($"C# DiscountLocation - Queue trigger function processed: {myQueueItem}");
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
