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
    public class BOMLines : CompanySettings
    {
        private readonly IBOMLinesToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapps-bomlines-dispatcher";

        public BOMLines(IBOMLinesToQueue dispatcherToQueue, IQueueService queueService)
        {
            _dispatcherToQueue = dispatcherToQueue;
            _queueService = queueService;
        }

        //[FunctionName("BOMLines")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# BOMLines queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

                // Deserialize the order data into a dynamic object
                dynamic payload = JsonConvert.DeserializeObject(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Items.BillOfMaterialsDetails _bomLines = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.BillOfMaterialsDetails>(payload.Body.ToString());

                Extensions.Cyware.Model.BOMLines bomlines = new()
                {
                    BOMID = _bomLines.BOMID ?? "",
                    LINECREATIONSEQUENCENUMBER = _bomLines.LINECREATIONSEQUENCENUMBER,
                    CATCHWEIGHTQUANTITY = _bomLines.CATCHWEIGHTQUANTITY,
                    CONFIGURATIONGROUPID = _bomLines.CONFIGURATIONGROUPID ?? "",
                    CONSTANTSCRAPQUANTITY = _bomLines.CONSTANTSCRAPQUANTITY,
                    CONSUMPTIONCALCULATIONCONSTANT = _bomLines.CONSUMPTIONCALCULATIONCONSTANT,
                    CONSUMPTIONCALCULATIONMETHOD = _bomLines.CONSUMPTIONCALCULATIONMETHOD ?? "",
                    CONSUMPTIONSITEID = _bomLines.CONSUMPTIONSITEID ?? "",
                    CONSUMPTIONTYPE = _bomLines.CONSUMPTIONTYPE ?? "",
                    CONSUMPTIONWAREHOUSEID = _bomLines.CONSUMPTIONWAREHOUSEID ?? "",
                    FLUSHINGPRINCIPLE = _bomLines.FLUSHINGPRINCIPLE ?? "",
                    ISCONSUMEDATOPERATIONCOMPLETE = _bomLines.ISCONSUMEDATOPERATIONCOMPLETE ?? "",
                    ISRESOURCECONSUMPTIONUSED = _bomLines.ISRESOURCECONSUMPTIONUSED ?? "",
                    ITEMNUMBER = _bomLines.ITEMNUMBER ?? "",
                    LINENUMBER = _bomLines.LINENUMBER,
                    LINETYPE = _bomLines.LINETYPE ?? "",
                    PHYSICALPRODUCTDENSITY = _bomLines.PHYSICALPRODUCTDENSITY,
                    PHYSICALPRODUCTDEPTH = _bomLines.PHYSICALPRODUCTDEPTH,
                    PHYSICALPRODUCTHEIGHT = _bomLines.PHYSICALPRODUCTHEIGHT,
                    PHYSICALPRODUCTWIDTH = _bomLines.PHYSICALPRODUCTWIDTH,
                    POSITIONNUMBER = _bomLines.POSITIONNUMBER ?? "",
                    PRODUCTCOLORID = _bomLines.PRODUCTCOLORID ?? "",
                    PRODUCTCONFIGURATIONID = _bomLines.PRODUCTCONFIGURATIONID ?? "",
                    PRODUCTSIZEID = _bomLines.PRODUCTSIZEID ?? "",
                    PRODUCTSTYLEID = _bomLines.PRODUCTSTYLEID ?? "",
                    PRODUCTUNITSYMBOL = _bomLines.PRODUCTUNITSYMBOL ?? "",
                    PRODUCTVERSIONID = _bomLines.PRODUCTVERSIONID ?? "",
                    QUANTITY = _bomLines.quantity,
                    QUANTITYDENOMINATOR = _bomLines.QUANTITYDENOMINATOR,
                    QUANTITYROUNDINGUPMULTIPLES = _bomLines.QUANTITYROUNDINGUPMULTIPLES,
                    ROUNDINGUPMETHOD = _bomLines.ROUNDINGUPMETHOD ?? "",
                    ROUTEOPERATIONNUMBER = _bomLines.ROUTEOPERATIONNUMBER,
                    SUBBOMID = _bomLines.SUBBOMID ?? "",
                    SUBROUTEID = _bomLines.SUBROUTEID ?? "",
                    VALIDFROMDATE = _bomLines.VALIDFROMDATE,
                    VALIDTODATE = _bomLines.VALIDTODATE,
                    VARIABLESCRAPPERCENTAGE = _bomLines.VARIABLESCRAPPERCENTAGE,
                    VENDORACCOUNTNUMBER = _bomLines.VENDORACCOUNTNUMBER ?? "",
                    WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE = _bomLines.WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE ?? "",
                    WILLCOSTCALCULATIONINCLUDELINE = _bomLines.WILLCOSTCALCULATIONINCLUDELINE ?? "",
                    WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES = _bomLines.WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES ?? "",
                    WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES = _bomLines.WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES ?? ""
                };

                string jsonString = JsonConvert.SerializeObject(bomlines);
                Process(jsonString, "moo-cyware-extension-bomlines-dispatcher", ConnectionString, CompanyID);
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
