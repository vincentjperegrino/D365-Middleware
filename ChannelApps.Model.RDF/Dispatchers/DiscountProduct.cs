using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class DiscountProduct : Core.Domain.Dispatchers.IDiscountProductToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var DiscountProductObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.DiscountProduct>(messagequeue);

            if (DiscountProductObject == null)
            {
                return false;
            }

            return SendMessageToQueue(DiscountProductObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object DiscountProductModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(DiscountProductModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object DiscountProductModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(DiscountProductModel, Formatting.None, settings);
            return Json;
        }
    }
}
