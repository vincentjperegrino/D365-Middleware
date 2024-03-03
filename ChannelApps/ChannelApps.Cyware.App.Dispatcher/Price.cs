using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Price : CompanySettings
    {
        private readonly IPriceToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-prices-dispatcher";

        public Price(IQueueService queueService, IPriceToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Price")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Price queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the data into a Queue Content object
                QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the data into specified models
                Prices d365_prices = JsonConvert.DeserializeObject<Prices>(payload.Body);

                PriceHeader cy_price = new()
                {
                    evtNum = d365_prices.evtNum ?? "0",
                    evtDsc = d365_prices.evtDesc ?? "",
                    evtFdt = d365_prices.evtFdt ?? "",
                    evtTdt = d365_prices.evtTdt ?? ""
                };

                string jsonString = JsonConvert.SerializeObject(cy_price);
                Process(jsonString, "moo-cyware-extension-price-dispatcher", ConnectionString, CompanyID);
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
