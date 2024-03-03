using System;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Polly;

namespace KTI.Moo.Extensions.OctoPOS.App.Queue.Dispatchers;

public class UpsertProduct : CompanySettings
{

   // private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;
    private readonly IDistributedCache _cache;

    public UpsertProduct( IDistributedCache cache)
    {
        _cache = cache;
    }
    //public UpsertProduct(Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagement, IDistributedCache cache)
    //{
    //    _channelManagement = channelManagement;
    //    _cache = cache;
    //}

    [FunctionName("Upsert-Product-Octopos")]
    public void Run([QueueTrigger("%CompanyID%-octopos-%StoreCode%-extension-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        var MaxNumberOfRetry = 5;

        var retryPolicy = Policy.Handle<System.Exception>().WaitAndRetry(
        MaxNumberOfRetry, // number of retries
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
        (exception, timeSpan, retryCount, context) =>
        {
            log.LogInformation(exception.Message);
        });

        retryPolicy.Execute(() =>
        {

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var CompanyID = Convert.ToInt32(Companyid);

            Process(myQueueItem, config);

        });


        //log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        //try
        //{
  
        //    var CompanyID = Convert.ToInt32(Companyid);

        //  //  var ChannelConfig = _channelManagement.Get(StoreCode);

        //    //var Config = new OctoPOS.Service.Config()
        //    //{
        //    //    companyid = Companyid,
        //    //    defaultURL = ChannelConfig.kti_defaulturl,
        //    //    password = ChannelConfig.kti_password,
        //    //    redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
        //    //    username = ChannelConfig.kti_username,
        //    //    apiAuth = ChannelConfig.kti_appkey

        //    //};

        //    Process(myQueueItem, config);
        //}
        //catch (System.Exception ex)
        //{
        //    log.LogInformation(ex.Message);
        //    throw new System.Exception(ex.Message);
        //}


    }

    public bool Process(string FromDispatcherQueue, Config config)
    {
        KTI.Moo.Extensions.OctoPOS.Domain.Product ProductDomain = new(config, _cache);

        var ProductData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var productsku = "";
        if (ProductData.ContainsKey("ProductSKU"))
        {
            productsku = ProductData["ProductSKU"].Value<string>();
        }

        ProductDomain.Upsert(FromDispatcherQueue, productsku);

        return true;
    }

}


