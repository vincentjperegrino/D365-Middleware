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
    public class ProductCategory : CompanySettings
    {
        private readonly IProductCategoryToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        private const string QueueName = "rdf-cyware-channelapp-productcategory-dispatcher";

        public ProductCategory(IQueueService queueService, IProductCategoryToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("ProductCategory")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the data into a Queue Content object
                QueueMessageContent productCategoryData = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Items.ProductCategory productCategory_d365 = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Items.ProductCategory>(productCategoryData.Body.ToString());
                Extensions.Cyware.Model.ProductCategory productCategory = new()
                {
                    department = productCategory_d365.CategoryCode?.Length >= 3 ? productCategory_d365.CategoryCode.Substring(0, 3) : null,
                    sub_dept = productCategory_d365.CategoryCode?.Length >= 6 ? productCategory_d365.CategoryCode.Substring(0, 6) : null,
                    cy_class = productCategory_d365.CategoryCode?.Length >= 9 ? productCategory_d365.CategoryCode.Substring(0, 9) : null,
                    sub_class = productCategory_d365.CategoryCode?.Length >= 12 ? productCategory_d365.CategoryCode.Substring(0, 12) : null,
                    name = productCategory_d365.FriendlyName ?? "",
                    planned_gm = ""
                };

                string jsonString = JsonConvert.SerializeObject(productCategory);
                Process(jsonString, "moo-cyware-extension-productcategory-dispatcher", ConnectionString, CompanyID);
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
