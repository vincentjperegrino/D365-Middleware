using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Price: Core.Domain.Dispatchers.IPriceToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var PriceObject = JsonConvert.DeserializeObject<PriceHeader>(messagequeue);

            if (PriceObject == null)
            {
                return false;
            }

            return SendMessageToQueue(PriceObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object priceModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(priceModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object priceModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(priceModel, Formatting.None, settings);
            return Json;
        }
    }
}
