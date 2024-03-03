using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Discount : Core.Domain.Dispatchers.IDiscountToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var DiscountObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.Discount>(messagequeue);

            if (DiscountObject == null)
            {
                return false;
            }

            return SendMessageToQueue(DiscountObject, QueueName, QueueConnectionString);
        }

        private static bool SendMessageToQueue(object DiscountObject, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(DiscountObject);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object DiscountObject)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(DiscountObject, Formatting.None, settings);
            return Json;
        }
    }
}
