using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Register : Core.Domain.Dispatchers.IRegisterToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var RegisterObject = JsonConvert.DeserializeObject<RegisterReading>(messagequeue);

            if (RegisterObject == null)
            {
                return false;
            }

            return SendMessageToQueue(RegisterObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object RegisterModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(RegisterModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object RegisterModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(RegisterModel, Formatting.None, settings);
            return Json;
        }
    }
}
