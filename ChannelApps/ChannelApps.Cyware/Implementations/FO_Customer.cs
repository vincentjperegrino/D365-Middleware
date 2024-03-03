using Azure.Storage.Queues;
using Domain.Models.Items;
using KTI.Moo.Extensions.OctoPOS.Model.DTO;
using Newtonsoft.Json;
using System.Text;

namespace KTI.Moo.ChannelApps.Cyware.Implementations
{
    public class Customer
    {
        private readonly QueueClient _queueClient;

        public Customer(string connectionString)
        {
            _queueClient = new QueueClient(connectionString, "fo-customer-queue");
            _queueClient.CreateIfNotExists();
        }

        public bool customerProcess(string customerDetails)
        {

            return addCustomerToQueue(customerDetails);
        }

        public bool addCustomerToQueue(string customerDetails)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<ChannelApps.Model.RDF.Receivers.Customer>(customerDetails);

                payload.companyId = "3392";
                payload.domainType = "customer";
                payload.CustomerGroupId = "ARTLYLTYCU";

                string custStr = JsonConvert.SerializeObject(payload);
                //var Customer = new ChannelApps.Model.RDF.Receivers.Customer(customerModel);

                byte[] messageBytes = Encoding.UTF8.GetBytes(custStr);
                _queueClient.SendMessage(Convert.ToBase64String(messageBytes));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
