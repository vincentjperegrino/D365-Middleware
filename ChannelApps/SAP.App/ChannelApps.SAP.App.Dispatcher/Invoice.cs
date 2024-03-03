
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;

namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher;

public class Invoice : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IInvoiceToQueue _dispatcherToQueue;

    public Invoice(IInvoiceToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [Singleton(Mode = SingletonMode.Listener)]
    [FunctionName("SAP_Invoice_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-sap-channelapp-invoice-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var decodedString = myQueueItem;

        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        Process(decodedString, InvoiceQueueName, ConnectionString, Companyid);
    }


    private bool Process(string decodedString, string QueueName, string ConnectionString, string CompanyID)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, CompanyID);
    }

}
