using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class DiscountType : CompanySettings
    {
        private readonly IDiscountTypeToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-discounttype-dispatcher";

        public DiscountType(IDiscountTypeToQueue dispatcherToQueue, IQueueService queueService)
        {
            _dispatcherToQueue = dispatcherToQueue;
            _queueService = queueService;
        }

        //[FunctionName("DiscountType")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the order data into a dynamic object
                dynamic discountTypeData = JsonConvert.DeserializeObject(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Sales.DiscountType discountType_D365 = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Sales.DiscountType>(discountTypeData.Body.ToString());
                Extensions.Cyware.Model.DiscountType discountType = new()
                {
                    DiscountCode = discountType_D365.discount_cd ?? "",
                    DiscountTypeCd = discountType_D365.discount_type_cd ?? "",
                    Description = discountType_D365.description ?? "",
                    DiscType = "",
                    Readonly = "1",
                    DiscValue = discountType_D365.disc_value ?? "",
                    StartDate = DateTime.TryParse(discountType_D365.StartDate, out var startdate) ? startdate : default(DateTime),
                    EndDate = DateTime.TryParse(discountType_D365.end_date, out var enddate) ? enddate : default(DateTime),
                    MinAmount = double.TryParse(discountType_D365.min_amount, out var minamount) ? minamount : default(double),
                    MaxAmount = double.TryParse(discountType_D365.max_amount, out var maxamount) ? maxamount : default(double),
                    DiscountRule = discountType_D365.discount_rule ?? "",
                    AccountTypeCode = discountType_D365.acc_type_codeTypeCode ?? "",
                    RequireAccount = discountType_D365.require_account ?? "",
                    FreeItem = discountType_D365.free_item ?? "",
                    FreeItemLimit = int.TryParse(discountType_D365.free_item_limit, out var freeitemlimit) ? freeitemlimit : default(int),
                    FreeItemQty = int.TryParse(discountType_D365.free_item_qty, out var freeitemqty) ? freeitemqty : default(int),
                    EventNumber = int.TryParse(discountType_D365.event_num, out var eventnum) ? eventnum : default(int),
                    PostedOn = DateTime.TryParse(discountType_D365.posted_on, out var postedOn) ? postedOn : default(DateTime),
                    ExportCurrentPrice = discountType_D365.export_current_price ?? "",
                    Posted = int.TryParse(discountType_D365.posted, out var posted) ? posted : default(int),
                };
                string jsonString = JsonConvert.SerializeObject(discountType);
                Process(jsonString, "moo-cyware-extension-discounttype-dispatcher", ConnectionString, CompanyID);
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
