using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Messages.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Services
{
    public class QueueService : IQueueService
    {
        public IEnumerable<Dictionary<string, object>> RetrieveAndPopMessage(string connectionString, string queueName, string archiveQueueName, int maxMessage, ILogger logger, bool isStoreTrans = false)
        {
            List<string> messageIds = new List<string>();
            List<Dictionary<string, object>> messages = new List<Dictionary<string, object>>();

            // Create a QueueClient instance using the connection string and queue name
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            queueClient.CreateIfNotExists();

            int ctr = 1;
            while (true)
            {
                ////Receive a batch of messages from the queue
                Response<QueueMessage[]> response = queueClient.ReceiveMessages(maxMessages: 32); // Adjust batch size as per your requirements

                ////If no messages are received, exit loop
                if (response.Value.Length == 0)
                {
                    break;
                }

                ////Process each message in the batch   
                foreach (QueueMessage message in response.Value)
                {
                    // Check if the message's MessageId already exists in messageIds
                    if (!messageIds.Contains(message.MessageId))
                    {
                        ////Insert the message's MessageId into messageIds
                        messageIds.Add(message.MessageId);

                        ////Decode the message body
                        JObject data = JObject.Parse(DecryptMessage(message.Body.ToString()));

                        string dataJsonString = data["Data"].Value<string>();

                        // Deserialize the "Data" string as a separate JObject
                        JObject eventData = JsonConvert.DeserializeObject<JObject>(dataJsonString);

                        string batchResponse = data.Value<string>("BatchResponse");

                        if (!string.IsNullOrEmpty(batchResponse))
                        {
                            eventData["Error"] = batchResponse;
                        }

                        if (isStoreTrans)
                        {
                            // Add the error_message property to the eventData dictionary
                            eventData["Transaction numbers"] = data.Value<string>("ErrorMessage");
                        }
                        else
                        {
                            // Add the error_message property to the eventData dictionary
                            eventData["Error message"] = data.Value<string>("ErrorMessage");
                        }

                        // Convert eventData to Dictionary<string, object>
                        Dictionary<string, object> result = eventData.ToObject<Dictionary<string, object>>();

                        ////Add the message
                        messages.Add(result);
                        ////Remove message from poison queue
                        ////RemoveMessageFromQueue(queueClient, message);

                        ///Move message to archive queue
                        MovePoisonToArchiveQueue(queueClient, connectionString, archiveQueueName, message, logger);
                        ctr++;
                    }
                    else
                    {
                        // MessageId already exists, do nothing
                    }
                }
            }
            logger.LogInformation($"{messages.Count} messages retrieved in {queueName}.");
            return messages;
        }


        public string DecryptMessage(string message)
        {
            byte[] data = Convert.FromBase64String(message);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public bool RemoveMessageFromQueue(QueueClient queueClient, QueueMessage message)
        {
            QueueClient PoisonQueueClient = queueClient;
            try
            {
                PoisonQueueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MoveToPoisonQueueFromMainQueue(QueueClient mainQueue, QueueClient poisonQueue, string messageId, string popReceipt, string updatedQueueItem)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(updatedQueueItem);
            string base64EncodedMessage = Convert.ToBase64String(jsonBytes);

            // Send the message to the poison queue
            poisonQueue.SendMessage(base64EncodedMessage);

            // If both messageId and popReceipt are provided, delete the message from the main queue
            if (!string.IsNullOrEmpty(messageId) && !string.IsNullOrEmpty(popReceipt))
            {
                // Delete the message from the main queue
                mainQueue.DeleteMessage(messageId, popReceipt);
            }

            return true;
        }

        public bool MovePoisonToArchiveQueue(QueueClient poisonQueue, string connectionString, string archiveQueueName, QueueMessage payload, ILogger logger)
        {
            try
            {
                ///Build the message, include poison queue in message body. for archive  messages processor to know where to put back the message. 
                var messageWithPoisonQueue = new
                {
                    Data = JObject.Parse(DecryptMessage(payload.Body.ToString())),
                    PoisonQueue = poisonQueue.Name
                };

                string serializedMessage = JsonConvert.SerializeObject(messageWithPoisonQueue);

                // Send the message to the archive queue
                ///Create QueueClient for  ArchiveQueue
                //QueueClient archiveQueue = new(connectionString, "moo-cyware-extension-archive-queue"); ////Harcoded archive queue.
                QueueClient archiveQueue = new(connectionString, archiveQueueName); ////Harcoded archive queue.
                archiveQueue.CreateIfNotExists();
                archiveQueue.SendMessage(serializedMessage);

                // Delete the message from poison queue
                poisonQueue.DeleteMessage(payload.MessageId, payload.PopReceipt);

                logger.LogInformation($"Message with id: {payload.MessageId}, is moved to archive queue.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Something went wrong while moving message to archive queue. " + ex);
                return false;

            }
        }
    }

}
