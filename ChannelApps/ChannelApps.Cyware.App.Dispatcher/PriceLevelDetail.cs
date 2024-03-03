using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Core.Helper;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;

namespace ChannelApps.Cyware.App.Dispatcher
{
    public class PriceLevelDetail
    {
        private readonly IPriceLevelDetailToQueue _dispatcherToQueue;
        private readonly string _queueName = "moo-priceleveldetail-queue";
        private readonly string _connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private readonly string _companyID = Environment.GetEnvironmentVariable("CompanyID");

        public PriceLevelDetail(IPriceLevelDetailToQueue dispatcherToQueue)
        {
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("PriceLevelDetail")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Price Level Detail queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
                Prices d365_price = new Prices()
                {
                    dataAreaId = "ABC",
                    RecordId = 123456789,
                    PriceApplicableFromDate = DateTime.Now,
                    WillSearchContinue = "Yes",
                    SalesPriceQuantity = 10.5m,
                    QuantityUnitySymbol = "kg",
                    ProductNumber = "P12345",
                    AttributeBasedPricingId = "ABCD",
                    ProductSizeId = "S123",
                    ItemNumber = "I001",
                    ProductVersionId = "V001",
                    PriceCurrencyCode = "USD",
                    ToQuantity = 20.5m,
                    FixedPriceCharges = 5.99m,
                    WillDeliveryDateControlDisregardLeadTime = "No",
                    PriceApplicableToDate = DateTime.Now.AddDays(30),
                    PriceWarehouseId = "W001",
                    SalesLeadTimeDays = 7,
                    FromQuantity = 5.5m,
                    CustomerAccountNumber = "CUST001",
                    PriceCustomerGroupCode = "GROUP001",
                    Price = 9.99,
                    PriceSiteId = "SITE001",
                    IsGenericCurrencySearchEnabled = "Yes",
                    ProductColorId = "COLOR001",
                    ProductconfigurationId = "CONFIG001",
                    ProductStyleId = "STYLE001",
                    moosourcesystem = "SystemA",
                    mooexternalid = "EXT123",
                    companyid = 1
                };
                KTI.Moo.Extensions.Cyware.Model.PriceLevelDetail priceLevelDetail = new ()
                {
                    pluevt = "Test EVT",
                    plusku = "TEST SKU",
                    plufrd = "TEST FRD",
                    pluprc = "TEST PRC"
                };
                string jsonString = JsonConvert.SerializeObject(priceLevelDetail);
                Process(jsonString, _queueName, _connectionString, _companyID);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }


    }
}
