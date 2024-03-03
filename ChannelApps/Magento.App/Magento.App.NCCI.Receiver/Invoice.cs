

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Receiver;

public class Invoice : CompanySettings
{
    //For Testing
    private Invoice(string _companyid, string _connectionstring)
    {
        Companyid = _companyid;
        ConnectionString = _connectionstring;
    }

    public Invoice()
    {


    }


    [FunctionName("MagentoInvoice_ChannelApps")]
    public void Run([QueueTrigger("%CompanyID%-magento-invoice", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            var decodedString = Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");

            Process(decodedString);
        }
        catch (Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new Exception(ex.Message);
        }

    }

    private static bool Process(string decodedString)
    {

        var InvoiceDomain = new ChannelApps.Magento.Invoice<ChannelApps.Model.NCCI.Receivers.Invoice,
                                                            ChannelApps.Model.NCCI.Receivers.Order,
                                                            ChannelApps.Model.NCCI.Receivers.Customer>(connectionString: ConnectionString,
                                                                                             companyId: Companyid);

        return InvoiceDomain.With_Customer_Order_Process(decodedString);
    }
}
