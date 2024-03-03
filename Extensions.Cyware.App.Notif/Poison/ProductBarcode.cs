using System;
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Azure.WebJobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Cyware.App.Queue.Helpers;

namespace KTI.Moo.Extensions.Cyware.App.Queue.Poison
{
    public class ProductBarcode : CompanySettings
    {
        private readonly IPoison _PoisonQueue;

        public ProductBarcode(IPoison poisonQueue)
        {
            _PoisonQueue = poisonQueue;
        }

        //[FunctionName("ProductBarcode-Retry")]
        public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string mainQueueName = "moo-productbarcode-queue";
            string poisonQueueName = $"{mainQueueName}-poison";

            // Create main QueueClient object
            QueueClient mainQueue = new(ConnectionString, mainQueueName);
            mainQueue.CreateIfNotExists();

            // Create poison QueueClient object
            QueueClient poisonQueue = new(ConnectionString, poisonQueueName);
            poisonQueue.CreateIfNotExists();

            _PoisonQueue.ReturnToMainQueueFromPoisonQueue(poisonQueue, mainQueue, log);
        }
    }
}
