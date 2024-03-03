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
    public class BOMHeader : CompanySettings
    {
        private readonly IBOMHeaderToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapps-bomheader-dispatcher";


        public BOMHeader(IBOMHeaderToQueue dispatcherToQueue, IQueueService queueService)
        {
            _dispatcherToQueue = dispatcherToQueue;
            _queueService = queueService;
        }

        //[FunctionName("BOMHeader")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# BOMHeader queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the order data into a dynamic object
                dynamic payload = JsonConvert.DeserializeObject(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Items.BillOfMaterialsHeader bomheader = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.BillOfMaterialsHeader>(payload.Body.ToString());

                Extensions.Cyware.Model.BOMHeader _bomHeader = new()
                {
                    BOMID = bomheader.BOMID ?? "",
                    MANUFACTUREDITEMNUMBER = bomheader.MANUFACTUREDITEMNUMBER ?? "",
                    PRODUCTIONSITEID = bomheader.PRODUCTIONSITEID ?? "",
                    PRODUCTCONFIGURATIONID = bomheader.PRODUCTCONFIGURATIONID ?? "",
                    PRODUCTSTYLEID = bomheader.PRODUCTSTYLEID ?? "",
                    ISACTIVE = bomheader.ISACTIVE ?? "",
                    FROMQUANTITY = bomheader.FROMQUANTITY,
                    VALIDFROMDATE = bomheader.validfromdate,
                    APPROVERPERSONNELNUMBER = bomheader.APPROVERPERSONNELNUMBER ?? "",
                    BOMNAME = bomheader.BOMNAME ?? "",
                    ISAPPROVED = bomheader.ISAPPROVED ?? ""
                };
                string jsonString = JsonConvert.SerializeObject(_bomHeader);
                Process(jsonString, "moo-cyware-extension-bomheader-dispatcher", ConnectionString, CompanyID);   
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
