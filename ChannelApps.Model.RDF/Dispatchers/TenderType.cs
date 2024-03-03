using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class TenderType : Core.Domain.Dispatchers.ITenderTypeToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var TenderTypeObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.TenderType>(messagequeue);

            if (TenderTypeObject == null)
            {
                return false;
            }

            return SendMessageToQueue(TenderTypeObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object TenderTypeModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(TenderTypeModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object TenderTypeModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(TenderTypeModel, Formatting.None, settings);
            return Json;
        }
    }
}
