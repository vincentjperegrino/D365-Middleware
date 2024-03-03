using Azure.Storage.Queues;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.RDF.Dispatchers
{
    public class DiscountTypeProduct : Core.Domain.Dispatchers.IDiscountTypeProductToQueue
    {
        public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
        {
            var DiscountTypeProductObject = JsonConvert.DeserializeObject<Extensions.Cyware.Model.DiscountTypeProduct>(messagequeue);

            if (DiscountTypeProductObject == null)
            {
                return false;
            }

            return SendMessageToQueue(DiscountTypeProductObject, QueueName, QueueConnectionString);
        }


        private static bool SendMessageToQueue(object DiscountTypeProductModel, string QueueName, string ConnectionString)
        {
            var Json = GetJsonForMessageQueue(DiscountTypeProductModel);

            QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExists();
            queueClient.SendMessage(Json);

            return true;
        }

        private static string GetJsonForMessageQueue(object DiscountTypeProductModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(DiscountTypeProductModel, Formatting.None, settings);
            return Json;
        }
    }
}
