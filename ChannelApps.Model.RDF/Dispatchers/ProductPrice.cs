using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class ProductPrice : Core.Domain.Dispatchers.IProductPriceToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var ProductPriceObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.ProductPrice>(messagequeue);

            if (ProductPriceObject == null)
            {
                return false;
            }

            return SendMessageToQueue(ProductPriceObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object ProductPriceModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(ProductPriceModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object ProductPriceModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(ProductPriceModel, Formatting.None, settings);
            return Json;
        }
    }
}
