using System;
using KTI.Moo.ChannelApps.Core.Domain;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher;

public class Customer : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.ICustomerToQueue _dispatcherToQueue;

    public Customer(KTI.Moo.ChannelApps.Core.Domain.Dispatchers.ICustomerToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    //public Customer()
    //{
    //    _dispatcherToQueue = new KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento.Customer();
    //}


    [FunctionName("Magento_Customer_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-magento-%StoreCode%-channelapp-customer-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = myQueueItem;
            log.LogInformation($"C# Queue trigger function processed: {decodedString}");
            Process(decodedString, CustomerQueueName, ConnectionString, Companyid);
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            throw new Exception(ex.Message);
        }
    }

    private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);

    }



}
