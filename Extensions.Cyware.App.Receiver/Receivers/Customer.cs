using System;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Receivers
{
    public class Customer
    {
        private readonly QueueClient _queueClient;
        private readonly string _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        public Customer()
        {
            _queueClient = new QueueClient(_connectionString ?? "UseDevelopmentStorage=true", "cyware-customer-queue", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            _queueClient.CreateIfNotExists();
        }

        //[FunctionName("Customer")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Customer timer trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
                Process();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool Process()
        {
            var customer = new CustomerModel
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                Phone = "+1-555-555-5555",
                Address = new AddressModel
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    Zipcode = "12345",
                    Country = "United States"
                },

                dataAreaId = "srdf",
                LanguageId = "en-US",
                NameAlias = "FoTest Form Queue",
                OrganizationName = "Juan D.",
                PersonFirstName = "Juan",
                PersonLastName = "Dee",
                PersonPhoneticFirstName = "Wan",
                PersonPhoneticLastName = "Dii",
                PrimaryContactEmail = "JuanD@gmail.com",
                SalesCurrencyCode = "PHP"
            };

            return AddToQueue(customer);
        }

        private bool AddToQueue(CustomerModel customer)
        {
            try
            {
                string customerJson = JsonConvert.SerializeObject(customer);
                _queueClient.SendMessage(customerJson);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
