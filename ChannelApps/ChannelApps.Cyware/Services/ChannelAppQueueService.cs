using Azure.Storage.Queues;
using ChannelApps.Cyware.Helpers;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelApps.Cyware.Services
{
    public class ChannelAppQueueService :  IChannelAppQueueService
    {        
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var extensionModel = JsonConvert.DeserializeObject<ExtensionQueueMessageModel>(messagequeue);

            if (extensionModel == null)
            {
                return false;
            }

            return SendMessageToQueue(extensionModel, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object extensionModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(extensionModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object customerModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(customerModel, Formatting.None, settings);
            return Json;
        }
    }
}
