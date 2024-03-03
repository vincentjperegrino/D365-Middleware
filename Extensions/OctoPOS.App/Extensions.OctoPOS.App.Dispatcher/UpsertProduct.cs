using System;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.OctoPOS.App.Dispatcher;

public class UpsertProduct : CompanySettings
{

    private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;


    public UpsertProduct(Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagement)
    {
        _channelManagement = channelManagement;
    }


    [FunctionName("Upsert-Product-Octopos")]
    public void Run([QueueTrigger("%CompanyID%-octopos-%StoreCode%-extension-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        try
        {
            //var customer = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Customer>(myQueueItem);
            var CompanyID = Convert.ToInt32(Companyid);

            var ChannelConfig = _channelManagement.Get(StoreCode);

            var Config = new OctoPOS.Service.Config()
            {
                companyid = Companyid,
                defaultURL = ChannelConfig.kti_defaulturl,
                password = ChannelConfig.kti_password,
                redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
                username = ChannelConfig.kti_username,
                apiAuth = ChannelConfig.kti_appkey
                
            };

            Process(myQueueItem, config);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }


    }

    public static bool Process(string FromDispatcherQueue, Config config)
    {
        KTI.Moo.Extensions.OctoPOS.Domain.Product MagentoProductDomain = new(config);

        var ProductData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var productsku = "";
        if (ProductData.ContainsKey("ProductSKU"))
        {
            productsku = ProductData["ProductSKU"].Value<string>();
        }

        MagentoProductDomain.Upsert(FromDispatcherQueue, productsku);

        return true;
    }

}


