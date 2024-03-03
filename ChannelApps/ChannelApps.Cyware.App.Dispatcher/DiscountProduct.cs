using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Moo.Models.Dtos.Sales;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class DiscountProduct : CompanySettings
    {
        private readonly IDiscountProductToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-discountproduct-dispatcher";

        public DiscountProduct(IQueueService queueService, IDiscountProductToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("DiscountProduct")]
        public void Run([QueueTrigger("rdf-cyware-channelapp-discountproduct-dispatcher", Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the data into a Queue Content object
                QueueMessageContent discountProductData = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the order data into specified models
                DiscountDetails discountProduct_d365 = JsonConvert.DeserializeObject<DiscountDetails>(discountProductData.Body.ToString());
                Extensions.Cyware.Model.DiscountProduct discountProduct = new()
                {
                    sku_number = discountProduct_d365.ProductNumber ?? "",
                    upc_code = discountProduct_d365.UpcCode ?? "",
                    upc_type = discountProduct_d365.UpcType ?? "",
                    price_event_number = discountProduct_d365.PriceEventNumber ?? "",
                    currency_code = discountProduct_d365.CurrencyCode ?? "",
                    price_book = discountProduct_d365.PriceBook ?? "",
                    start_date = discountProduct_d365.StartDate,
                    end_date = discountProduct_d365.EndDate,
                    promo_flag_yn = discountProduct_d365.INVENTORYSTATUS, ////For Checking
                    event_price_multiple = "",
                    event_price = discountProduct_d365.OFFERPRICE.ToString() ?? "",
                    price_method_code = discountProduct_d365.PriceMethodCode,
                    mix_match_code = discountProduct_d365.MIXANDMATCHNUMBEROFITEMSNEEDED,
                    deal_quantity = discountProduct_d365.DealQuantity,
                    deal_price = discountProduct_d365.OFFERPRICE.ToString() ?? "",
                    buy_quantity = (int)(discountProduct_d365.LEASTQUANTITY),
                    buy_value = discountProduct_d365.OFFERPRICE,  ////For Checking
                    buy_value_type = discountProduct_d365.BuyValueType ?? "",
                    qty_end_value = discountProduct_d365.QuantityEndValue,
                    quantity_break = discountProduct_d365.QuantityBreak,
                    quantity_group_price = discountProduct_d365.OFFERPRICE.ToString() ?? "",
                    quantity_unit_price = discountProduct_d365.OFFERPRICE.ToString() ?? "",
                    cust_promo_code = discountProduct_d365.CustPromoCode ?? "",
                    cust_number = discountProduct_d365.CustNumber,
                    precedence_level = discountProduct_d365.PrecedenceLevel,
                    default_currency = discountProduct_d365.CurrencyCode ?? "",
                    default_price_book = discountProduct_d365.PriceBook,
                };
                string jsonString = JsonConvert.SerializeObject(discountProduct);
                Process(jsonString, "moo-cyware-extension-discountproduct-dispatcher", ConnectionString, CompanyID);
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
