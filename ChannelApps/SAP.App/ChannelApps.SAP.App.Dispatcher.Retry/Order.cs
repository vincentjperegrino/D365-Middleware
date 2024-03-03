using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher;

public class Order : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IOrderToQueue _dispatcherToQueue;

    public Order(IOrderToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [Singleton(Mode = SingletonMode.Listener)]
    [FunctionName("SAP_Order_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-channelapp-order-dispatcher-poison", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var decodedString = myQueueItem;

        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        Process(decodedString, OrderQueueName, ConnectionString, Companyid);
    }

    private bool Process(string decodedString, string QueueName, string ConnectionString, string CompanyID)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, CompanyID);
    }

}
