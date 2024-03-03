using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.OctoPOS.App.Dispatcher;

public class Product : CompanySettings
{

    private readonly KTI.Moo.ChannelApps.Core.Domain.Dispatchers.IProductToQueue _dispatcherToQueue;

    public Product(ChannelApps.Core.Domain.Dispatchers.IProductToQueue dispatcherToQueue)
    {
        _dispatcherToQueue = dispatcherToQueue;
    }

    [FunctionName("OctoPOS_Product_Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-octopos-%StoreCode%-channelapp-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
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

            Process(decodedString, ProductQueueName, ConnectionString, Companyid);

        });

        //try
        //{
        //    var decodedString = myQueueItem;

        //    log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        //    Process(decodedString, ProductQueueName, ConnectionString, Companyid);
        //}
        //catch (Exception ex)
        //{
        //    log.LogError(ex.Message);
        //    throw new Exception(ex.Message);
        //}
    }


    private bool Process(string decodedString, string QueueName, string QueueConnectionString, string CompanyID)
    {
        return _dispatcherToQueue.DispatchMessage(decodedString, QueueName, QueueConnectionString, CompanyID);
    }
}
