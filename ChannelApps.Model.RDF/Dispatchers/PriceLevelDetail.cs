using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class PriceLevelDetail : Core.Domain.Dispatchers.IPriceLevelDetailToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var PriceLevelDetailObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.PriceLevelDetail>(messagequeue);

            if (PriceLevelDetailObject == null)
            {
                return false;
            }

            return SendMessageToQueue(PriceLevelDetailObject, QueueName, QueueConnectionString);
        }

        private static bool SendMessageToQueue(object PriceLevelDetailObject, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(PriceLevelDetailObject);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object PriceLevelDetailObject)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(PriceLevelDetailObject, Formatting.None, settings);
            return Json;
        }
    }
}
