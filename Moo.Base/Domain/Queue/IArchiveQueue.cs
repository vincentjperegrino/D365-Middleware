using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain.Queue
{
    public interface IArchiveQueue
    {
        bool ReturnToMainQueueFromArchiveQueue(QueueClient ArchiveQueueClient, ILogger log, int maxmessage = 32);
        bool ReadFromCurrentQueueMessage(QueueClient ArchiveQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0);
        bool DeleteMessageInQueue(QueueClient ArchiveQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0);

    }
}
