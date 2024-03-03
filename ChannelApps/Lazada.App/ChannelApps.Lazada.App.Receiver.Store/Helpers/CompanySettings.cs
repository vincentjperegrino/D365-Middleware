namespace KTI.Moo.ChannelApps.Lazada.App.Receiver.Store.Helpers;

public class CompanySettings
{
    public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");

    public static string StoreCode = Environment.GetEnvironmentVariable("StoreCode");

    //public static string CustomerQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("CustomerQueueName")}";

    //public static string InvoiceQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InvoiceQueueName")}";

    //public static string OrderQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("OrderQueueName")}";

}
