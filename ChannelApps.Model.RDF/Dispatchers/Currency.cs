using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Currency : Core.Domain.Dispatchers.ICurrencyToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var CurrencyObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.Currency>(messagequeue);

            if (CurrencyObject == null)
            {
                return false;
            }

            return SendMessageToQueue(CurrencyObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object CurrencyModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(CurrencyModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object CurrencyModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(CurrencyModel, Formatting.None, settings);
            return Json;
        }
    }
}
