using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.ChannelApps.Lazada.App.Dispatcher;

public class Product : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IProductToQueue _dispatcherToQueue;

    public Product(ChannelApps.Core.Domain.Dispatchers.IProductToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [FunctionName("Lazada_Product_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-channelapp-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = myQueueItem;

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");
    
            _dispatcherToQueue.DispatchMessage(decodedString, ProductQueueName, ConnectionString, Companyid);
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            throw new Exception(ex.Message);
        }
    }

}
