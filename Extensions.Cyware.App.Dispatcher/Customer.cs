using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class Customer : CompanySettings
    {
        private readonly ICustomer<KTI.Moo.Extensions.Cyware.Model.Customer> _customer;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-customer-dispatcher";

        public Customer(ICustomer<Model.Customer> customer, IQueueService queueService)
        {
            _customer = customer;
            _queueService = queueService;
        }

        //[FunctionName("Customer")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                var customer = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.Customer>(myQueueItem);
                var result = _customer.Upsert(customer);
                log.LogInformation($"C# Customer - Queue trigger function processed: {myQueueItem}");
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
    }
}
