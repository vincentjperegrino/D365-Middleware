using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using System.Text;

namespace KTI.Moo.Cyware.App.Dispatcher
{
    public class ForEx : CompanySettings
    {
        private readonly IForExToQueue _dispatcherToQueue;
        private readonly IQueueService _queueService;
        //private const string QueueName = "rdf-cyware-channelapps-forex-dispatcher";
        private const string QueueName = "test-forex";

        public ForEx(IQueueService queueService, IForExToQueue dispatcherToQueue)
        {
            _queueService = queueService;
            _dispatcherToQueue = dispatcherToQueue;
        }

        //[FunctionName("ForEX")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue Trigger function processed: {myQueueItem}");

                // Deserialize the data into a Queue Content object
                QueueMessageContent forExData = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

                // Deserialize the order data into specified models
                global::Moo.Models.Dtos.Finance.ForEx forEx_d365 = JsonConvert.DeserializeObject<global::Moo.Models.Dtos.Finance.ForEx>(forExData.Body.ToString());
                Extensions.Cyware.Model.ForEx forEx = new()
                {
                    from_currency_code = forEx_d365.FromCurrency ?? "",
                    to_currency_code = forEx_d365.ToCurrency ?? "",
                    currency_exch_rate = forEx_d365.Rate,
                    code = "",
                    conversion_rate_type = forEx_d365.RateTypeName ?? "",
                    effective_date = forEx_d365.StartDate.ToString() ?? DateTime.MinValue.ToString(),
                    rounding_multiple = "",
                    rounding_multiple_to = "",
                    currency_exch_rate_mt = ""
                };
                string jsonString = JsonConvert.SerializeObject(forEx);
                Process(jsonString, "moo-cyware-extension-forex-dispatcher", ConnectionString, CompanyID);
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
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(updatedQueueItem);
                    string base64EncodedMessage = Convert.ToBase64String(jsonBytes);

                    _queueService.MoveToPoisonQueueFromMainQueue(mainQueue, poisonQueue, Id, PopReceipt, base64EncodedMessage);
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
