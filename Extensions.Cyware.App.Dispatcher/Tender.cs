using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.App.Dispatcher.Helpers;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class Tender : CompanySettings
    {
        private readonly ITender<Model.Tender> _tender;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-tender-dispatcher";

        public Tender(IQueueService queueService, ITender<Model.Tender> tender)
        {
            _queueService = queueService;
            _tender = tender;
        }

        //[FunctionName("Tender")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                Model.Tender tender = JsonConvert.DeserializeObject<Model.Tender>(myQueueItem);
                Model.Tender result = _tender.Upsert(tender);
                log.LogInformation($"C# Tender - Queue trigger function processed: {myQueueItem}");
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
    }
}
