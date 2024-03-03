
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Receivers;

namespace KTI.Moo.Magento.App.NCCI.Receiver
{
    public class Customer : CompanySettings
    {
        private readonly ICustomer<ChannelApps.Model.NCCI.Receivers.Customer> _customerDomain;

        public Customer(ICustomer<ChannelApps.Model.NCCI.Receivers.Customer> customerDomain)
        {
            this._customerDomain = customerDomain;
        }

        [FunctionName("MagentoCustomer_ChannelApps")]
        public void Run([QueueTrigger("%CompanyID%-magento-customer", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {

            try
            {
                var decodedString = ChannelApps.Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();

                log.LogInformation($"C# Queue trigger function processed: {decodedString}");

                var result = _customerDomain.DefautProcess(decodedString);

                if (!result)
                {
                    log.LogInformation("Customer existing and not modified");
                    return;
                }

                log.LogInformation("Customer for upsert");
                return;

            }
            catch (Exception ex)
            {

                log.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
