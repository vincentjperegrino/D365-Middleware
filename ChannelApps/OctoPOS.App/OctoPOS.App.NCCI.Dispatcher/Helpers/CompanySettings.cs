namespace KTI.Moo.ChannelApps.OctoPOS.App.NCCI.Dispatcher.Helpers;

public class CompanySettings
{
    public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");

    //public static string CustomerQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("CustomerQueueName")}";

    //public static string InvoiceQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InvoiceQueueName")}";
}
