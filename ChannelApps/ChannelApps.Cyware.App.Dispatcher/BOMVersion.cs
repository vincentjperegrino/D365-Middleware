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
    public class BOMVersion : CompanySettings
    {
        private readonly IBOMVersionToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-bomversion-dispatcher";

        public BOMVersion(IBOMVersionToQueue dispatcherToQueue, IQueueService queueService)
        {
            _dispatcherToQueue = dispatcherToQueue;
            _queueService = queueService;
        }

        //[FunctionName("BOMVersion")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {

            try
            {
                log.LogInformation($"C# BOMVersion queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the order data into a dynamic object
                dynamic payload = JsonConvert.DeserializeObject(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Items.BOMVersions _bomversion = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.BOMVersions>(payload.Body.ToString());

                //if (_bomversion.BOMID == "SITBOM1" || _bomversion.BOMID == "BOM2")
                //{
                Extensions.Cyware.Model.BOMVersion bomversion = new()
                {
                    MANUFACTUREDITEMNUMBER = _bomversion.MANUFACTUREDITEMNUMBER ?? "",
                    BOMID = _bomversion.BOMID ?? "",
                    PRODUCTIONSITEID = _bomversion.PRODUCTSTYLEID ?? "",
                    PRODUCTCONFIGURATIONID = _bomversion.PRODUCTCONFIGURATIONID ?? "",
                    PRODUCTCOLORID = _bomversion.PRODUCTCOLORID ?? "",
                    PRODUCTSIZEID = _bomversion.PRODUCTSIZEID ?? "",
                    PRODUCTSTYLEID = _bomversion.PRODUCTSTYLEID ?? "",
                    PRODUCTVERSIONID = _bomversion.PRODUCTVERSIONID ?? "",
                    ISACTIVE = _bomversion.ISACTIVE ?? "",
                    VALIDFROMDATE = _bomversion.VALIDFROMDATE,
                    FROMQUANTITY = _bomversion.FROMQUANTITY,
                    SEQUENCEID = _bomversion.SEQUENCEID,
                    APPROVERPERSONNELNUMBER = _bomversion.APPROVERPERSONNELNUMBER ?? "",
                    CATCHWEIGHTSIZE = _bomversion.CATCHWEIGHTSIZE,
                    FROMCATCHWEIGHTQUANTITY = _bomversion.FROMCATCHWEIGHTQUANTITY,
                    ISAPPROVED = _bomversion.ISAPPROVED ?? "",
                    ISSELECTEDFORDESIGNER = _bomversion.ISSELECTEDFORDESIGNER ?? "",
                    VALIDTODATE = _bomversion.VALIDTODATE,
                    VERSIONNAME = _bomversion.VERSIONNAME ?? ""
                };

                string jsonString = JsonConvert.SerializeObject(bomversion);
                Process(jsonString, "moo-cyware-extension-bomversion-dispatcher", ConnectionString, CompanyID);
                //}

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
