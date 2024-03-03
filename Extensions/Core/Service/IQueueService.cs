using Azure.Storage.Queues;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Service
{
    public interface IQueueService
    {
        public IEnumerable<Dictionary<string ,object>>RetrieveAndPopMessage(string connectionString, string queueName, string archiveQueueName, int maxMessage, ILogger logger, bool isStoreTrans = false);
        //public bool MoveMesssageToOtherQueue(string destinationConnectionString, string destinationQueueName, QueueMessage message);
        public bool RemoveMessageFromQueue(QueueClient queueClient, QueueMessage message);
        bool MoveToPoisonQueueFromMainQueue(QueueClient mainQueue, QueueClient poisonQueue, string messageId, string popReceipt, string updatedQueueItem);
    }
}

