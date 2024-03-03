using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Polly;

namespace KTI.Moo.ChannelApps.OctoPOS.App.Dispatcher;

public class Customer : CompanySettings
{
    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.ICustomerToQueue _dispatcherToQueue;

    public Customer(KTI.Moo.ChannelApps.Core.Domain.Dispatchers.ICustomerToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [FunctionName("Octopos_Customer_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-octopos-%StoreCode%-channelapp-customer-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
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
            var result = Process(decodedString, CustomerQueueName, ConnectionString, Companyid);

            if (!result)
            {
                log.LogInformation($"Not for dispatch");
            }
        });

        //try
        //{
        //    var decodedString = myQueueItem;
        //    log.LogInformation($"C# Queue trigger function processed: {decodedString}");
        //    Process(decodedString, CustomerQueueName, ConnectionString, Companyid);
        //}
        //catch (Exception ex)
        //{ 
        //    log.LogError(ex.Message);
        //    throw new Exception(ex.Message);
        //}
    }

    private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
    }


}
