using Azure.Storage.Queues;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class BOMHeader : CompanySettings
    {
        private readonly IBOMHeader<KTI.Moo.Extensions.Cyware.Model.BOMHeader> _bomHeader;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-bomheader-dispatcher";

        public BOMHeader(IBOMHeader<Model.BOMHeader> bomHeader, IQueueService queueService)
        {
            _bomHeader = bomHeader;
            _queueService = queueService;
        }

        //[FunctionName("BOMHeader")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                var bomHeader = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.BOMHeader>(myQueueItem);
                var result = _bomHeader.Upsert(bomHeader);
                log.LogInformation($"C# BOMHeader - Queue trigger function processed: {myQueueItem}");
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
    }
}
