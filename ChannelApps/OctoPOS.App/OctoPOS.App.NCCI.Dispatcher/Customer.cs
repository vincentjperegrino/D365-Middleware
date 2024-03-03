using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.Extensions.Core.Helper;

namespace KTI.Moo.ChannelApps.OctoPOS.App.NCCI.Dispatcher;

public class Customer : CompanySettings
{
    private readonly ICustomer<ChannelApps.Model.NCCI.Receivers.Customer> _customerDomain;

    public Customer(ICustomer<ChannelApps.Model.NCCI.Receivers.Customer> customerDomain)
    {
        this._customerDomain = customerDomain;
    }


    [FunctionName("OctoPOS_Customer_Receiver")]
    public void Run([QueueTrigger("%CompanyID%-octopos-customer", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = ChannelApps.Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();

            //   var decodedString = myQueueItem;

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
