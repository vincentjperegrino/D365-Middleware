using System;
using System.ComponentModel.Design;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.ChannelApps.Lazada.App.Dispatcher;

public class OrderStatus : CompanySettings
{
    private readonly IOrderStatus _dispatcherToQueue;

    public OrderStatus(IOrderStatus dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [FunctionName("Lazada_OrderStatus_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-channelapp-orderstatus-dispatcher", Connection = "AzureQueueConnectionString")]string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = myQueueItem;

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");
      
            _dispatcherToQueue.DispatchMessage(decodedString, OrderStatusQueueName, ConnectionString, Companyid);
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            throw new Exception(ex.Message);
        }

    }

}
