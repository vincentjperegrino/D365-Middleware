using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Services
{
    public class ArchiveQueueService : CompanySettings,  IArchiveQueue
    {
        public bool ReturnToMainQueueFromArchiveQueue(QueueClient ArchiveQueueClient, ILogger log, int maxmessage = 32)
        {
            try
            {
                QueueMessage[] retrievedMessage = ArchiveQueueClient.ReceiveMessages(32);
                var TotalMessage = retrievedMessage.Length;
                if (TotalMessage > 0)
                {
                    var Success_Transfer_To_Main = ReadFromCurrentQueueMessage(ArchiveQueueClient, retrievedMessage, log);
                    if (Success_Transfer_To_Main)
                    {
                        var SuccessDeleteMessageInQueue = DeleteMessageInQueue(ArchiveQueueClient, retrievedMessage, log);
                        if (SuccessDeleteMessageInQueue)
                        {
                            log.LogInformation($"Popped {retrievedMessage.Length} Messages in Queue");
                            ReturnToMainQueueFromArchiveQueue(ArchiveQueueClient, log);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.LogError($"Something went wrong while reading message from archive queue. {ex.Message}");
                return false;
            }

        }

        public bool ReadFromCurrentQueueMessage(QueueClient archiveQueue, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0)
        {
            try
            {
                var CurrentMessage = retrievedMessages[currentcount];
                var TotalIteration = retrievedMessages.Length - 1;

                ///Deserialized payload.
                var payload = JsonConvert.DeserializeObject<JObject>(CurrentMessage.Body.ToString());
                JObject dataObject = payload["Data"] as JObject;
                string mainQueueName =  payload.Value<string>("PoisonQueue").Replace("-poison", "");
                var message = payload.Value<JToken>("Data");


                ///Convert message to base64.
                byte[] jsonBytes = Encoding.UTF8.GetBytes(message["Data"].ToString());
                string base64EncodedMessage = Convert.ToBase64String(jsonBytes);


                //Send Message back to Main queue
                QueueClient mainQueue = new(ConnectionString, mainQueueName);
                mainQueue.CreateIfNotExists();
                mainQueue.SendMessage(base64EncodedMessage);

                if (TotalIteration > currentcount)
                {
                    ReadFromCurrentQueueMessage(archiveQueue, retrievedMessages, log, ++currentcount);
                }
                return true;

            }
            catch (Exception ex)
            {
                log.LogError($"Something went wrong while reading message from ArchiveQueue. {ex.Message}.");
                return false;
            }

            
        }
        
        public bool DeleteMessageInQueue(QueueClient ArchiveQueueClient, QueueMessage[] retrievedMessages, ILogger log, int currentcount = 0)
        {
            try
            {
                log.LogInformation($"Popping Messages in Queue");

                var CurrentMessage = retrievedMessages[currentcount];

                var TotalIteration = retrievedMessages.Length - 1;

                ArchiveQueueClient.DeleteMessage(CurrentMessage.MessageId, CurrentMessage.PopReceipt);

                if (TotalIteration > currentcount)
                {
                    DeleteMessageInQueue(ArchiveQueueClient, retrievedMessages, log, ++currentcount);
                }

                return true;
            }
            catch (Exception ex)
            {
                log.LogError($"Something went wrong while deleting message from ArchiveQueue. {ex.Message}.");
                return false;
            }

        }

       
    }
}
