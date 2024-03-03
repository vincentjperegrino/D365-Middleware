using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Cyware.Model.DTO;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class ProductBarcode : CompanySettings
    {
        private readonly IProductBarcodeToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-barcode-dispatcher";

        public ProductBarcode(IQueueService queueService, IProductBarcodeToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("ProductBarcode")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the data into a Queue Content object
                QueueMessageContent productBarcodeData = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Items.ProductBarcode productBarcode_d365 = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.ProductBarcode>(productBarcodeData.Body.ToString());
                Extensions.Cyware.Model.ProductBarcode productBarcode = new()
                {
                    product_code = productBarcode_d365.BarCode ?? "",
                    sku_number = productBarcode_d365.sku_number ?? "",
                    upc_type = productBarcode_d365.upc_type.Length < 5 ? productBarcode_d365.upc_type : productBarcode_d365.upc_type.Substring(0, 5) ?? "", //productBarcode_d365.upc_type ?? "",
                    upc_unit_of_measure = productBarcode_d365.upc_unit_of_measure ?? ""
                };

                string jsonString = JsonConvert.SerializeObject(productBarcode);
                Process(jsonString, "moo-cyware-extension-productbarcode-dispatcher", ConnectionString, CompanyID);
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
