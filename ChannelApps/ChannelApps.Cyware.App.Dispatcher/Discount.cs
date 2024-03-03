using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.ChannelApps.Cyware.App.Dispatcher
{
    public class Discount : CompanySettings
    {
        private readonly IDiscountToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapps-discount-dispatcher";

        public Discount(IQueueService queueService, IDiscountToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Discount")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Discount queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the data into a Queue Content object
                QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the data into specified models
                global::Moo.Models.Dtos.Sales.Discount d365_discount = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Sales.Discount>(payload.Body);

                Extensions.Cyware.Model.Discount discount = new()
                {
                    evtNum = d365_discount.DiscountCode ?? "0",
                    evtDsc = d365_discount.Name ?? "",
                    evtFdt = d365_discount.ValidFrom.ToString() ?? DateTime.MinValue.ToString("yyyyMMdd"),
                    evtTdt = d365_discount.ValidTo.ToString() ?? DateTime.MinValue.ToString("yyyyMMdd")
                };

                string jsonString = JsonConvert.SerializeObject(discount);
                Process(jsonString, "moo-cyware-extension-discount-dispatcher", ConnectionString, CompanyID);
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
                    QueueErrorMessage queueItemWithErrorMessage = new()
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
