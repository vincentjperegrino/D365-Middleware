using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.Magento.App.Queue.Dispatchers;

public class UpsertCustomer : CompanySettings
{

    private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;


    public UpsertCustomer(Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagement)
    {

        _channelManagement = channelManagement;
    }

    [FunctionName("Update-Customer-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-%StoreCode%-extension-customer-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        try
        {
            //var customer = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Customer>(myQueueItem);

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

            Process(myQueueItem, Config, log);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }

    public static bool Process(string FromDispatcherQueue, Config config, ILogger log)
    {
        KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(config);

        var CustomerData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        //var contactid = "";

        //if (CustomerData.ContainsKey("contactid"))
        //{
        //    contactid = CustomerData["contactid"].Value<string>();
        //    CustomerData.Remove("contactid");
        //}

        var customer_id = 0;

        if (CustomerData.ContainsKey("id"))
        {
            customer_id = CustomerData["id"].Value<int>();
        }

        if (customer_id == 0)
        {
            log.LogInformation($"Customer does not exists in Magento");

            return true;
        }

        var taxvat = "";

        if (CustomerData.ContainsKey("taxvat"))
        {
            taxvat = CustomerData["taxvat"].Value<string>();
        }

        var Customer = MagentoCustomerDomain.Get(customer_id);
        if (Customer.taxvat == taxvat)
        {
            log.LogInformation($"Customer taxvat/CMID no update");
            return true;
        }

        MagentoCustomerDomain.UpsertID(FromDispatcherQueue, customer_id);

        return true;
    }

}
