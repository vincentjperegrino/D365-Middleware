using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Products : Core.Domain.Dispatchers.IProductsToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var ProductsObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.Products>(messagequeue);

            if (ProductsObject == null)
            {
                return false;
            }

            return SendMessageToQueue(ProductsObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object ProductsModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(ProductsModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object ProductsModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(ProductsModel, Formatting.None, settings);
            return Json;
        }
    }
}
