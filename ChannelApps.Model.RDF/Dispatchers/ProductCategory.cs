using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class ProductCategory : Core.Domain.Dispatchers.IProductCategoryToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var ProductCategoryObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.ProductCategory>(messagequeue);

            if (ProductCategoryObject == null)
            {
                return false;
            }

            return SendMessageToQueue(ProductCategoryObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object ProductCategoryModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(ProductCategoryModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object ProductCategoryModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(ProductCategoryModel, Formatting.None, settings);
            return Json;
        }
    }
}
