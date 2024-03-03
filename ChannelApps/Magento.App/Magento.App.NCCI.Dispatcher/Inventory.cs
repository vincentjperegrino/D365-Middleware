
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher;

public class Inventory : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IInventoryToQueue _dispatcherToQueue;

    public Inventory(IInventoryToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [FunctionName("Magento_Inventory_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-magento-%StoreCode%-channelapp-inventory-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        var decodedString = myQueueItem;

        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        Process(decodedString, InventoryQueueName, ConnectionString, Companyid);
    }

    private bool Process(string decodedString, string QueueName, string ConnectionString , string CompanyID)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString , CompanyID); 
    }
}
