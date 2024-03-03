using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class ProductPrice : CompanySettings
    {
        private readonly IProductPriceToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-productprice-dispatcher";

        public ProductPrice(IQueueService queueService, IProductPriceToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("ProductPrice")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the data into a Queue Content object
                QueueMessageContent productPriceData = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the data into specified models
                ProductPrices d365_productPrice = JsonConvert.DeserializeObject<ProductPrices>(productPriceData.Body);
                Extensions.Cyware.Model.ProductPrice productPrice = new()
                {
                    sku_number = d365_productPrice.sku_number ?? "",
                    upc_code = d365_productPrice.upc_code,
                    upc_type = d365_productPrice.upc_type,
                    price_event_number = d365_productPrice.price_event_number ?? "",
                    currency_code = d365_productPrice.currency_code ?? "",
                    price_book = d365_productPrice.price_book ?? "",
                    start_date = d365_productPrice.PriceApplicableFromDate,
                    end_date = d365_productPrice.end_date,
                    promo_flag_yn = "N",  //d365_productPrice.promo_flag_yn ?? "",
                    event_price_multiple = d365_productPrice.event_price_multiple,
                    event_price = d365_productPrice.event_price,
                    price_method_code = d365_productPrice.price_method_code,
                    mix_match_code = d365_productPrice.mix_match_code,
                    deal_quantity = d365_productPrice.deal_quantity,
                    deal_price = d365_productPrice.deal_price,
                    buy_quantity = int.TryParse(d365_productPrice.buy_quantity.ToString(), out var buyQuantity) ? buyQuantity : default(int),
                    buy_value = d365_productPrice.buy_value,
                    buy_value_type = d365_productPrice.buy_value_type ?? "",
                    qty_end_value = d365_productPrice.qty_end_value,
                    quantity_break = d365_productPrice.quantity_break,
                    quantity_group_price = d365_productPrice.quantity_group_price,
                    quantity_unit_price = d365_productPrice.quantity_unit_price,
                    cust_promo_code = d365_productPrice.cust_promo_code ?? "",
                    cust_number = d365_productPrice.cust_number ?? "",
                    precedence_level = int.TryParse(d365_productPrice.precedence_level.ToString(), out var precedenceLevel) ? precedenceLevel : default(int),
                    default_currency = d365_productPrice.default_currency ?? "",
                    default_price_book = d365_productPrice.default_price_book ?? "",
                };

                string jsonString = JsonConvert.SerializeObject(productPrice);
                Process(jsonString, "moo-cyware-extension-productprice-dispatcher", ConnectionString, CompanyID);
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
