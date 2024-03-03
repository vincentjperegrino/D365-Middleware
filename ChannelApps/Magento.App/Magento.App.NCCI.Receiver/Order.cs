
using KTI.Moo.ChannelApps.Core.Domain.Receivers;

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Receiver
{
    public class Order : CompanySettings
    {
        private readonly IOrderForReceiver orderDomain;

        public Order(IOrderForReceiver orderDomain)
        {
            this.orderDomain = orderDomain;
        }

        [FunctionName("MagentoOrder_ChannelApps")]
        public void Run([QueueTrigger("%CompanyID%-magento-order", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {

            try
            {
                var decodedString = Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();

                //   var decodedString = myQueueItem;

                log.LogInformation($"C# Queue trigger function processed: {decodedString}");

                orderDomain.WithCustomerProcess(decodedString);

            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }

        }

    }
}
