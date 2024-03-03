using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace KTI.Moo.ChannelApps.Cyware.App.Queue.Poison
{
    public class Store : CompanySettings
    {
        private readonly IPoison _PoisonQueue;

        public Store(IPoison poisonQueue)
        {
            _PoisonQueue = poisonQueue;
        }

        //[FunctionName("Store-Retry")]
        public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string mainQueueName = "rdf-cyware-channelapp-store-dispatcher";
            string posionQueueName = $"{mainQueueName}-poison";

            // Create main QueueClient object
            QueueClient mainQueue = new(ConnectionString, mainQueueName);
            mainQueue.CreateIfNotExists();

            // Create poison QueueClient object
            QueueClient poisonQueue = new(ConnectionString, posionQueueName);
            poisonQueue.CreateIfNotExists();

            _PoisonQueue.ReturnToMainQueueFromPoisonQueue(poisonQueue, mainQueue, log);
        }
    }
}
