using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Lazada.App.Dispatcher;

public class Inventory : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IInventoryToQueue _dispatcherToQueue;

    public Inventory(IInventoryToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [FunctionName("Lazada_Inventory_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-channelapp-inventory-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        var decodedString = myQueueItem;

        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        _dispatcherToQueue.DispatchMessage(decodedString, InventoryQueueName, ConnectionString, Companyid);
    }

}
