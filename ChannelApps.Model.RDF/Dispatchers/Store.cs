using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Store : Core.Domain.Dispatchers.IStoreToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var StoreObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.Store>(messagequeue);

            if (StoreObject == null)
            {
                return false;
            }

            return SendMessageToQueue(StoreObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object StoreModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(StoreModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object StoreModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(StoreModel, Formatting.None, settings);
            return Json;
        }
    }
}
