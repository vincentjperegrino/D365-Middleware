using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace KTI.Moo.Extensions.Cyware.App.Queue.Archive
{
    public class ArchiveToMain : CompanySettings
    {
        private readonly IArchiveQueue _archiveQueue;

        public ArchiveToMain(IArchiveQueue archiveQueue)
        {
            _archiveQueue = archiveQueue;
        }

        //[FunctionName("ArchiveToMain")]
        public void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Create archive QueueClient object
            QueueClient archiveQueue = new(ConnectionString, ExtensionArchiveQueueName);
            archiveQueue.CreateIfNotExists();

            _archiveQueue.ReturnToMainQueueFromArchiveQueue(archiveQueue, log);
        }
    }

}
