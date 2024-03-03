using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.CRM.App.Dispatchers
{
    public class Inventory : Helpers.CompanySettings
    {
        private readonly Base.Domain.Dispatchers.IInventory<KTI.Moo.CRM.Model.ChannelManagement.Inventory> _dispatcherInventory;
 
        public Inventory(Base.Domain.Dispatchers.IInventory<Model.ChannelManagement.Inventory> dispatcherInventory)
        {
            _dispatcherInventory = dispatcherInventory;
        }

        [FunctionName("CRM-Dispatcher-Inventory")]
        public void Run([QueueTrigger("%CompanyID%-dispatcher-inventory", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {

            var decodedString = Helpers.Decode.Base64(myQueueItem);

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");

            var channel = JsonConvert.DeserializeObject<KTI.Moo.CRM.Model.ChannelManagement.Inventory>(decodedString);

            _dispatcherInventory.DispatchProcess(channel, ConnectionString, log);
        }
    }
}
