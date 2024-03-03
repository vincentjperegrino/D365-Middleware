using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Dispatchers;

public class UpdateInventory : CompanySettings
{
    [FunctionName("Update-Inventory-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-extension-inventory-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

        try
        {
         //   var inventory = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Inventory>(myQueueItem);
            Process(myQueueItem, config);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }

    public static bool Process(string FromDispatcherQueue , Config config)
    {
        KTI.Moo.Extensions.Magento.Domain.Inventory MagentoInventoryDomain = new(config);
        return MagentoInventoryDomain.Update(FromDispatcherQueue); 
    }
}
