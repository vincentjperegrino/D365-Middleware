using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class ProductBarcode : Core.Domain.Dispatchers.IProductBarcodeToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var ProductBarcodeObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.ProductBarcode>(messagequeue);

            if (ProductBarcodeObject == null)
            {
                return false;
            }

            return SendMessageToQueue(ProductBarcodeObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object ProductBarcodeModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(ProductBarcodeModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object ProductBarcodeModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(ProductBarcodeModel, Formatting.None, settings);
            return Json;
        }
    }
}
