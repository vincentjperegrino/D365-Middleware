using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.ApplicationInsights;
using Polly;
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
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-channelapp-order-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, string dequeueCount, ILogger log)
    {


        var MaxNumberOfRetry = 5;

        var retryPolicy = Policy.Handle<Exception>().WaitAndRetry(
        MaxNumberOfRetry, // number of retries
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
        (exception, timeSpan, retryCount, context) =>
        {
            log.LogInformation(exception.Message);
        });


        retryPolicy.Execute(() =>
        {
            var decodedString = myQueueItem;

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");

            Process(decodedString, OrderQueueName, ConnectionString, Companyid);
            return;
        });

    }

    private bool Process(string decodedString, string QueueName, string ConnectionString, string CompanyID)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, CompanyID);
    }

}
