using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class ForEx : Core.Domain.Dispatchers.IForExToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var ForExObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.ForEx>(messagequeue);

            if (ForExObject == null)
            {
                return false;
            }

            return SendMessageToQueue(ForExObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object ForExModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(ForExModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object ForExModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(ForExModel, Formatting.None, settings);
            return Json;
        }
    }
}
