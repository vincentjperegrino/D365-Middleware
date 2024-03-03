using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class Customer : Core.Domain.Dispatchers.ICustomerToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var CustomerObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.Customer>(messagequeue);

            if (CustomerObject == null)
            {
                return false;
            }

            return SendMessageToQueue(CustomerObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object customerModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(customerModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object customerModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(customerModel, Formatting.None, settings);
            return Json;
        }
    }
}
