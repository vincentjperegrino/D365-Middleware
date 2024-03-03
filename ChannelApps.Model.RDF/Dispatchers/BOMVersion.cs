using Azure.Storage.Queues;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class BOMVersion : KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IBOMVersionToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var bomObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.BOMVersion>(messagequeue);

            if (bomObject == null)
            {
                return false;
            }

            return SendMessageToQueue(bomObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object bomObject, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(bomObject);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object bomModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(bomModel, Formatting.None, settings);
            return Json;
        }
    }
}
