using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using KTI.Moo.Extensions.Core.Helper;
namespace KTI.Moo.ChannelApps.OctoPOS.App.NCCI.Dispatcher;

public class InvoiceMigration : CompanySettings
{
    private readonly IOrderForReceiver orderDomain;

    public InvoiceMigration(IOrderForReceiver orderDomain)
    {
        this.orderDomain = orderDomain;
    }

    [FunctionName("OctoPOS_Invoice_Receiver_Migration")]
    public void Run([QueueTrigger("%CompanyID%-octopos-invoice-migration", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = ChannelApps.Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();
            // var decodedString = myQueueItem;

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");
            orderDomain.DefautProcess(decodedString);

        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new Exception(ex.Message);

        }

    }
}
