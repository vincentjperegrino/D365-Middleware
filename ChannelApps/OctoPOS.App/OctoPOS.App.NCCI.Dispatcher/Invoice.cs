using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using KTI.Moo.Extensions.Core.Helper;
namespace KTI.Moo.ChannelApps.OctoPOS.App.NCCI.Dispatcher;

public class Invoice : CompanySettings
{
    private readonly IOrderForReceiver orderDomain;

    public Invoice(IOrderForReceiver orderDomain)
    {
        this.orderDomain = orderDomain;
    }

    [FunctionName("OctoPOS_Invoice_Receiver")]
    public void Run([QueueTrigger("%CompanyID%-octopos-invoice", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = ChannelApps.Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();
            // var decodedString = myQueueItem;

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
