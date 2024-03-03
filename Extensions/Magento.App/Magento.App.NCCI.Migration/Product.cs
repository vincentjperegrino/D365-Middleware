using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.App.NCCI.Migration;

public class Product
{
    [FunctionName("ProductsMigration")]
    public void Run([QueueTrigger("3389-magento-migration", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            process(myQueueItem, log);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }


    public bool process(string myQueueItem, ILogger log)
    {
        var config = Helper.ConfigHelper.Get();

        Magento.Domain.Product Domain = new(config);

        var Model = JsonConvert.DeserializeObject<Magento.Model.Product>(myQueueItem);

        var InventoryUpdateModel = Domain.Get(Model.sku);

        if(InventoryUpdateModel == null || InventoryUpdateModel.id == 0)
        {
            log.LogInformation("Not exsiting Product in Magento");
            throw new System.Exception("Not exsiting Product in Magento");
        }

        InventoryUpdateModel.visibility = 4;
        InventoryUpdateModel.extension_attributes.stock_item.qtyonhand = (double)Model.quantityonhand;

        Domain.Update(InventoryUpdateModel);

        return true;
    }
}
