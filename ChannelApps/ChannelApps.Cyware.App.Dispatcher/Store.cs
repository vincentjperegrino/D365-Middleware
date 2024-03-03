using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Store : CompanySettings
    {
        private readonly IStoreToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapps-stores-dispatcher";

        public Store(IQueueService queueService, IStoreToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Store")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {

            try
            {
                log.LogInformation($"C# Store queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the data into a Queue Content object
                QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the order data into specified models
                Stores stores_d365 = JsonConvert.DeserializeObject<Stores>(payload.Body.ToString());

                Extensions.Cyware.Model.Store store = new()
                {
                    StoreNumber = stores_d365.strNum ?? "",
                    Name = stores_d365.stNam ?? "",
                    Address_Line1 = TrimHelper.SplitAndTrim(stores_d365.strAd1, 30, 0),
                    Address_Line2 = TrimHelper.SplitAndTrim(stores_d365.strAd1, 30, 1),
                    Address_Line3 = TrimHelper.SplitAndTrim(stores_d365.strAd1, 30, 2),
                    PhoneNumber = stores_d365.strPhn ?? "",
                    ManagerName = stores_d365.stMngr ?? "",
                    StoreOffice = stores_d365.strHdo ?? "",
                    Currency = stores_d365.strCod ?? "",
                    TaxCurrency = stores_d365.strTxc ?? "",
                    Language = stores_d365.strLan ?? "",
                };
                string jsonString = JsonConvert.SerializeObject(store);
                Process(jsonString, "moo-cyware-extension-stores-dispatcher", ConnectionString, CompanyID);
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
