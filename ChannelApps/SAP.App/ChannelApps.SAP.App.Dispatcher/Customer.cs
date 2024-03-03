using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System.Collections.Generic;
using System.Threading;

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
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-channelapp-customer-dispatcher", Connection = "AzureQueueConnectionString" )] string myQueueItem, string dequeueCount, ILogger log)
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
            Process(decodedString, CustomerQueueName, ConnectionString, Companyid);
            return;

        });

    }

    private bool Process(string decodedString, string QueueName, string ConnectionString, string CompanyID)
    {

        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, CompanyID);
    }

}
