using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Extensions.Magento.App.Queue.Dispatchers;

public class UpdateInventory : CompanySettings
{

    private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;

    public UpdateInventory(Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagement)
    {

        _channelManagement = channelManagement;
    }



    [FunctionName("Update-Inventory-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-%StoreCode%-extension-inventory-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

        try
        {
            var inventory = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Inventory>(myQueueItem);


            string[] AllowedChannel = new string[] {"MAC0009","COF0026","ACC0138","MAC0072" , "COFB152" };

            if (!AllowedChannel.Contains(inventory.product))
            {
                log.LogInformation("Not For inventory sync");
                return;

            }

            var CompanyID = Convert.ToInt32(Companyid);

            var ChannelConfig = _channelManagement.Get(StoreCode);

            var Config = new Magento.Service.Config()
            {
                companyid = Companyid,
                defaultURL = ChannelConfig.kti_defaulturl,
                password = ChannelConfig.kti_password,
                redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
                username = ChannelConfig.kti_username
            };

            Process(myQueueItem, Config);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }

    public static bool Process(string FromDispatcherQueue, Config config)
    {
        KTI.Moo.Extensions.Magento.Domain.Inventory MagentoInventoryDomain = new(config);
        return MagentoInventoryDomain.Update(FromDispatcherQueue);
    }

}
