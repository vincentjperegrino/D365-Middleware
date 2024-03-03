using Azure.Storage.Queues;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.Cyware.App.Dispatcher
{
    public class ConfigurationGroup : CompanySettings
    {
        private readonly IConfigurationGroup<KTI.Moo.Extensions.Cyware.Model.ConnfigurationGroup> _configurationGroup;
        private readonly IQueueService _queueService;
        private const string QueueName = "moo-cyware-extension-configurationgroup-dispatcher";

        public ConfigurationGroup(IConfigurationGroup<ConnfigurationGroup> configurationGroup, IQueueService queueService)
        {
            _configurationGroup = configurationGroup;
            _queueService = queueService;
        }

        //[FunctionName("ConfigurationGroup")]
        public void Run([QueueTrigger(QueueName, Connection = "AzureWebJobsStorage")] string myQueueItem, int dequeueCount, string Id, string PopReceipt, ILogger log)
        {
            try
            {
                var onfigurationgGroup = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Cyware.Model.ConnfigurationGroup>(myQueueItem);
                var result = _configurationGroup.Upsert(onfigurationgGroup);
                log.LogInformation($"C# ConfigurationGroup - Queue trigger function processed: {myQueueItem}");
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
