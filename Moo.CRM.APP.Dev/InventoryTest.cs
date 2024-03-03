using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.CRM.APP.Dev
{
    public class InventoryTest
    {
        [FunctionName("InventoryChecker")]
        public void Run([QueueTrigger("test-inventory", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var product = JsonConvert.DeserializeObject<Model.ProductBase>(myQueueItem);

            KTI.Moo.CRM.Domain.Inventory domain = new(3388, "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net");

            domain.replicate(product.productnumber, log).Wait();

        }
    }
}
