using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Cyware.App.Queue.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace KTI.Moo.Extensions.Cyware.App.Queue.Poison
{
    public class DiscountProduct : CompanySettings
    {
        private readonly IPoison _PoisonQueue;

        public DiscountProduct(IPoison poisonQueue)
        {
            _PoisonQueue = poisonQueue;
        }

        //[FunctionName("DiscountProduct-Retry")]
        public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string mainQueueName = "moo-discountproduct-queue";
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
