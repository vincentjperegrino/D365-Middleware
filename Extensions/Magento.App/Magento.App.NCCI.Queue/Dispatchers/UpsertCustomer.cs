using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Dispatchers;

public class UpsertCustomer : CompanySettings
{
    [FunctionName("Update-Customer-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-%ChannelCode%-extension-customer-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        try
        {
            //var customer = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Customer>(myQueueItem);
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
        KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(config);

        var CustomerData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);


        if (CustomerData.ContainsKey("contactid"))
        {
            var contactid = CustomerData["contactid"].Value<string>();
            CustomerData.Remove("contactid");
        }

        var customer_id = 0;

        if (CustomerData.ContainsKey("id"))
        {
            customer_id = CustomerData["id"].Value<int>();
        }


        MagentoCustomerDomain.UpsertID(FromDispatcherQueue, customer_id);

        return true;
    }




}
