using Azure.Storage.Queues;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class ConfigurationGroup : KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IConfigurationGroupToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var bomObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.ConnfigurationGroup>(messagequeue);

            if (bomObject == null)
            {
                return false;
            }

            return SendMessageToQueue(bomObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object configurationGroupObject, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(configurationGroupObject);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object configurationGroupModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(configurationGroupModel, Formatting.None, settings);
            return Json;
        }
    }
}
