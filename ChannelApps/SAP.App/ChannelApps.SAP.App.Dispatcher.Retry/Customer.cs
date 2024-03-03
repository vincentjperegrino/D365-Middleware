using KTI.Moo.ChannelApps.Core.Domain;

namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher;

public class Customer : CompanySettings
{

    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.ICustomerToQueue _dispatcherToQueue;

    public Customer(KTI.Moo.ChannelApps.Core.Domain.Dispatchers.ICustomerToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }


    [Singleton(Mode = SingletonMode.Listener)]
    [FunctionName("SAP_Customer_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-channelapp-customer-dispatcher-poison", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var decodedString = myQueueItem;
        log.LogInformation($"C# Queue trigger function processed: {decodedString}");
        Process(decodedString, CustomerQueueName, ConnectionString, Companyid);
    }

    private bool Process(string decodedString, string QueueName, string ConnectionString, string CompanyID)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, CompanyID);
    }

}
