using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.ChannelApps.Cyware.App.Dispatcher
{
    public class Customer : CompanySettings
    {
        private readonly ICustomerToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapps-customer-dispatcher";



        public Customer(ICustomerToQueue dispatcherToQueue, IQueueService queueService)
        {
            _dispatcherToQueue = dispatcherToQueue;
            _queueService = queueService;
        }

        //[FunctionName("Customer")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Customer queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the order data into a dynamic object
                dynamic payload = JsonConvert.DeserializeObject(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Customer.DTO_Customer customer_d365 = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Customer.DTO_Customer>(payload.Body.ToString());

                Extensions.Cyware.Model.Customer customer = new()
                {
                    CustomerId = customer_d365.CustomerAccount ?? "",
                    CurrencyCode = customer_d365.SalesCurrencyCode ?? "",
                    LocationCode = customer_d365.location_code ?? "",
                    firstname = customer_d365.first_name ?? "",
                    middlename = customer_d365.middle_initial?.TrimStart().Substring(0, 1) ?? "",
                    lastname = customer_d365.last_name ?? "",
                    CompanyName = customer_d365.company_name ?? "",
                    Remarks = customer_d365.soremarks ?? "",
                    NameAlias = customer_d365.NameAlias ?? "",
                    birthdate = customer_d365.birthday ?? "",
                    PrimaryContactEmail = customer_d365.email ?? "",
                    ContactNumber = customer_d365.cellphone ?? "",
                    Name = customer_d365.OrganizationName ?? "",
                    Type = customer_d365.PartyType ?? "",
                    CustomerGroup = customer_d365.CustomerGroupId ?? "",
                    PriceGroup = customer_d365.DiscountPriceGroupId ?? ""

                };

                string jsonString = JsonConvert.SerializeObject(customer);
                Process(jsonString, "moo-cyware-extension-customer-dispatcher", ConnectionString, CompanyID);
            }
            catch (Exception ex)
            {
                if (dequeueCount > 5)
                {
                    // Create Main QueueClient object
                    QueueClient mainQueue = new(ConnectionString, QueueName);

                    // Create poison QueueClient object
                    string poisonQueueName = $"{mainQueue.Name}-poison";
                    QueueClient poisonQueue = new(ConnectionString, poisonQueueName);
                    poisonQueue.CreateIfNotExists();

                    // Create object for current data and error message
                    var queueItemWithErrorMessage = new
                    {
                        Data = myQueueItem,
                        ErrorMessage = ex.Message
                    };

                    string updatedQueueItem = JsonConvert.SerializeObject(queueItemWithErrorMessage);

                    _queueService.MoveToPoisonQueueFromMainQueue(mainQueue, poisonQueue, Id, PopReceipt, updatedQueueItem);
                    return;
                }
                throw new Exception(ex.Message);
            }

        }

        public bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}
