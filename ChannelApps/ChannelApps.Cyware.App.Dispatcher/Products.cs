using System;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class Products : CompanySettings
    {
        private readonly IProductsToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-product-dispatcher";

        public Products(IQueueService queueService, IProductsToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("Products")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                //// Deserialize the order data into a dynamic object
                dynamic productData = JsonConvert.DeserializeObject(myQueueItem);

                ////Deserialize the order data into specified models
                Product product = JsonConvert.DeserializeObject<Product>(productData.Body.ToString());



                string inputString = product.DefaultLedgerDimensionDisplayValue;
                string stringWithoutSlashes = inputString.Replace("\\", "");
                string stringWithoutHyphens = stringWithoutSlashes.Trim('-');


                string dept, sub_dept, cy_class, sub_class;

                dept = stringWithoutHyphens.Length >= 3 ? stringWithoutHyphens.Substring(0, 3) : "";
                sub_dept = stringWithoutHyphens.Length >= 10 ? stringWithoutHyphens.Substring(4, 6) : "";
                cy_class = stringWithoutHyphens.Length >= 20 ? stringWithoutHyphens.Substring(11, 9) : "";
                sub_class = stringWithoutHyphens.Length >= 33 ? stringWithoutHyphens.Substring(21, 12) : "";

                Extensions.Cyware.Model.Products products = new()
                {
                    sku_number = product.sku_number ?? "",
                    check_digit = 0,
                    item_description = (product.item_description?.Length > 30) ? product.item_description.Substring(0, 30) : product.item_description ?? "",
                    style_vendor = 0,
                    style_number = "",
                    color_prefix = int.TryParse(product.color, out int _color_prefix) ? _color_prefix : 0,
                    color_code = int.TryParse(product.color, out int _color_code) ? _color_code : 0,
                    size_code = product.size ?? "",
                    dimension = product.size ?? "",
                    set_code = "",
                    hazardous_code = product.hazardous_code ?? "",
                    substitute_sku = 0,
                    status_code = "",
                    price_prompt = product.price_prompt ?? "",
                    no_tickets_item = 0,
                    department = dept, //product.department ?? "",
                    sub_department = sub_dept, //product.sub_department ?? "",
                    cy_class = cy_class, //product.cy_class ?? "",
                    sub_class = sub_class, // product.sub_class ?? "",
                    sku_type = product.sku_type ?? "",
                    buy_code_cs = "",
                    buy_um = product.buy_um ?? "",
                    primary_vendor = int.TryParse(product.PrimaryVendorAccountNumber, out int _primary_vendor) ? _primary_vendor : 0,
                    vendor_number = product.PrimaryVendorAccountNumber ?? "",
                    selling_um = product.SalesUnitSymbol ?? "",
                    case_pack = double.TryParse(product.case_pack, out double _case_pack) ? _case_pack : 0,
                    min_order_qty = int.TryParse(product.min_order_qty, out int _min_order_qty) ? _min_order_qty : 0,
                    order_at = 0,
                    maximum_anytime = int.TryParse(product.maximum_anytime, out int _maximum_anytime) ? _maximum_anytime : 0,
                    vat_code = product.vat_code ?? "",
                    tax_1_code = product.tax_1_code ?? "",
                    tax_2_code = "",
                    tax_3_code = "",
                    tax_4_code = "",
                    register_item_desc = (product.name?.Length > 18) ? product.name.Substring(0, 18) : product.name ?? "",
                    allow_discount_yn = product.allow_discount_yn ?? "",
                    sku_hst_code = "",
                    controlled_stock = "",
                    pos_comment = product.pos_comment ?? "",
                    print_set_detail_yn = product.print_set_detail_yn ?? "",
                    ticket_type_reg = "",
                    ticket_type_ad = "",
                    season_code = product.season_code ?? "",
                    currency_code = product.currency_code ?? "",
                    core_sku = 0,
                    menu_category = product.advanced_notes_group
                };

                string jsonString = JsonConvert.SerializeObject(products);
                Process(jsonString, "moo-cyware-extension-productinitial-dispatcher", ConnectionString, CompanyID);
            }
            catch (Exception ex)
            {
                if (dequeueCount > 5)
                {
                    //// Create Main QueueClient object
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
