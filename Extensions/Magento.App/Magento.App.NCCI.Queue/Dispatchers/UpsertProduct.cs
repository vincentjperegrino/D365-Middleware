using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Dispatchers;

public class UpsertProduct : CompanySettings
{
    [FunctionName("Upsert-Product-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-%ChannelCode%-extension-product-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
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
        KTI.Moo.Extensions.Magento.Domain.Product MagentoProductDomain = new(config);

        var ProductData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var productsku = "";
        if (ProductData.ContainsKey("sku"))
        {
            productsku = ProductData["sku"].Value<string>();
        }

        MagentoProductDomain.UpsertID(FromDispatcherQueue, productsku);

        return true;
    }

}


