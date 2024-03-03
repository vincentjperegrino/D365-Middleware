using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain.Queue;

public class Poison : Base.Domain.Queue.IPoison
{

    public bool ReturnToMainQueueFromPoisonQueue(QueueClient PoisonQueueClient, QueueClient MainQueueClient, ILogger log, int maxmessage = 32)
    {
        QueueMessage[] retrievedMessage = PoisonQueueClient.ReceiveMessages(maxmessage);
        var TotalMessage = retrievedMessage.Length;

        if (TotalMessage > 0)
        {
            var Success_Transfer_To_Main = ReadFromCurrentQueueMessage(MainQueueClient, retrievedMessage, log);
            if (Success_Transfer_To_Main)
            {
                var SuccessDeleteMessageInQueue = DeleteMessageInQueue(PoisonQueueClient, retrievedMessage, log);
                if (SuccessDeleteMessageInQueue)
                {
                    log.LogInformation($"Popped {retrievedMessage.Length} Messages in Queue");
                    ReturnToMainQueueFromPoisonQueue(PoisonQueueClient, MainQueueClient, log);
                }
            }
        }

        return true;
    }

    public bool ReadFromCurrentQueueMessage(QueueClient MainQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0)
    {

        var CurrentMessage = retrievedMessages[currentcount];

        var TotalIteration = retrievedMessages.Length - 1;

        //Send Message back to Main queue
        MainQueueClient.SendMessage(CurrentMessage.Body.ToString());

        if (TotalIteration > currentcount)
        {
            ReadFromCurrentQueueMessage(MainQueueClient, retrievedMessages, log, ++currentcount);
        }

        return true;
    }

    public bool DeleteMessageInQueue(QueueClient PoisonQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0)
    {
        log.LogInformation($"Popping Messages in Queue");

        var CurrentMessage = retrievedMessages[currentcount];

        var TotalIteration = retrievedMessages.Length - 1;

        PoisonQueueClient.DeleteMessage(CurrentMessage.MessageId, CurrentMessage.PopReceipt);

        if (TotalIteration > currentcount)
        {
            DeleteMessageInQueue(PoisonQueueClient, retrievedMessages, log, ++currentcount);
        }

        return true;
    }


}
