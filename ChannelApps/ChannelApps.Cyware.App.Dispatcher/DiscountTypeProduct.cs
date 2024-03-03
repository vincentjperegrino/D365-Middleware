using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.ChannelApps.Cyware.App.Dispatcher
{
    public class DiscountTypeProduct : CompanySettings
    {
        private readonly IDiscountTypeProductToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-discounttypeproduct-dispatcher";

        public DiscountTypeProduct(IDiscountTypeProductToQueue dispatcherToQueue, IQueueService queueService)
        {
            _dispatcherToQueue = dispatcherToQueue;
            _queueService = queueService;
        }

        //[FunctionName("DiscountTypeProduct")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the order data into a dynamic object
                dynamic discountTypeData = JsonConvert.DeserializeObject(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Sales.DiscountTypeProduct _discountTypeProduct_d365 = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Sales.DiscountTypeProduct>(discountTypeData.Body.ToString());

                decimal discount_percentage_1 = decimal.TryParse(_discountTypeProduct_d365.discount_percentage_1, out var discountpercentage_1) ? discountpercentage_1 : default(decimal);


                Extensions.Cyware.Model.DiscountTypeProduct discountType = new()
                {
                    ItemCode = _discountTypeProduct_d365.item_code ?? "",
                    DisccountCd = _discountTypeProduct_d365.discount_cd ?? "",
                    DiscValueInAmount = discount_percentage_1 != 0 ? discount_percentage_1 : _discountTypeProduct_d365.disc_value,
                    DiscValueInPercentage = discount_percentage_1.ToString(),
                    //DiscType = _discountTypeProduct_d365.disc_type,
                    DiscType = discount_percentage_1 != 0 ? "1" : "0",
                    ItemGroup = _discountTypeProduct_d365.item_group ?? "",
                    FreeItem = _discountTypeProduct_d365.free_item ?? "",
                    FreeItemQty = _discountTypeProduct_d365.free_item_qty ?? "",
                    Tolerance = _discountTypeProduct_d365.tolerance ?? "",
                    EventNum = _discountTypeProduct_d365.event_num ?? "",
                    ReqAmount = _discountTypeProduct_d365.req_amount ?? "",
                    ReqQty = _discountTypeProduct_d365.req_qty ?? "",
                    PartyCodeType = _discountTypeProduct_d365.party_code_type ?? "",
                    AccountSelection = _discountTypeProduct_d365.account_selection ?? "",
                    Configuration = _discountTypeProduct_d365.configuration ?? "",
                    //Site = _discountTypeProduct_d365.Site,
                    Warehouse = _discountTypeProduct_d365.warehouse ?? "",
                    From = _discountTypeProduct_d365.from,
                    To = _discountTypeProduct_d365.to,
                    Unit = _discountTypeProduct_d365.unit_id ?? "",
                    Currency = _discountTypeProduct_d365.currency ?? "",
                    AttributeBasedPricingID = _discountTypeProduct_d365.attribute_based_pricing_id ?? "",
                    DimensionValidation = _discountTypeProduct_d365.dimension_validation ?? "",
                    TradeAgreementValidation = _discountTypeProduct_d365.trade_agreement_validation ?? "",
                    DimensionNumber = _discountTypeProduct_d365.dimension_number ?? "",
                    DiscountPercentage2 = _discountTypeProduct_d365.discount_percentage_2 ?? "",
                    DisregardLeadTime = _discountTypeProduct_d365.disregard_lead_time ?? "",
                    FindNext = _discountTypeProduct_d365.find_next ?? "",
                    FromDate = _discountTypeProduct_d365.Priceapplicablefromdate,
                    IncludeInUnitPrice = _discountTypeProduct_d365.incl_in_unit_price ?? "",
                    IncludeGenericCurrency = _discountTypeProduct_d365.include_generic_currency ?? "",
                    LeadTime = _discountTypeProduct_d365.lead_time ?? "",
                    Log = _discountTypeProduct_d365.log ?? "",
                    Module = _discountTypeProduct_d365.module ?? "",
                    PriceAgreements = _discountTypeProduct_d365.price_agreement ?? "",
                    PriceCharges = _discountTypeProduct_d365.price_charges ?? "",
                    PriceUnit = _discountTypeProduct_d365.price_unit ?? "",
                    ToDate = _discountTypeProduct_d365.to_date,
                    FromTime = _discountTypeProduct_d365.from_time,
                    ToTime = _discountTypeProduct_d365.to_time ?? ""
                };
                string jsonString = JsonConvert.SerializeObject(discountType);
                Process(jsonString, "moo-cyware-extension-discounttypeproduct-dispatcher", ConnectionString, CompanyID);
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
