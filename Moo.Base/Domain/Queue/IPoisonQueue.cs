using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain.Queue;

public interface IPoison
{

    bool ReturnToMainQueueFromPoisonQueue(QueueClient PoisonQueueClient, QueueClient MainQueueClient, ILogger log, int maxmessage = 32);
    bool ReadFromCurrentQueueMessage(QueueClient MainQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0);
    bool DeleteMessageInQueue(QueueClient PoisonQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0);
}
