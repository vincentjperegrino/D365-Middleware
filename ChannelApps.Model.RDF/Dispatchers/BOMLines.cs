using Azure.Storage.Queues;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers 
{
    public class BOMLines : KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IBOMLinesToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var bomLines = JsonConvert.DeserializeObject<Extensions.Cyware.Model.BOMLines>(messagequeue);

            if (bomLines == null)
            {
                return false;
            }

            return SendMessageToQueue(bomLines, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object bomLines, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(bomLines);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object bomLines)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(bomLines, Formatting.None, settings);
            return Json;
        }
    }
}
