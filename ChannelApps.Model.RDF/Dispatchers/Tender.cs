using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Tender : Core.Domain.Dispatchers.ITenderToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var TenderObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.Tender>(messagequeue);

            if (TenderObject == null)
            {
                return false;
            }

            return SendMessageToQueue(TenderObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object TenderModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(TenderModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object TenderModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(TenderModel, Formatting.None, settings);
            return Json;
        }
    }
}
