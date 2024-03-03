using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace ChannelApps.Cyware.App.Receiver
{
    public class Customer
    {
        //[FunctionName("FO-Customer")]
        public void Run([QueueTrigger("cyware-customer-queue", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
                Process(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool Process(string queueItem)
        {
            //var queue = JsonConvert.DeserializeObject<JObject>(queueItem);
            var CustomerDomain = new KTI.Moo.ChannelApps.Cyware.Implementations.Customer(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            return CustomerDomain.customerProcess(queueItem);
        }
    }
}
