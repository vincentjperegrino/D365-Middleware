

namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Helper;

public class CompanySettings : Core.Helper.CompanySettings<Config>
{
    new public Config config = ConfigHelper.Get();
    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");  
    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
    new public static string ChannelCode = Environment.GetEnvironmentVariable("ChannelCode");
    new public static string CustomerQueueName = $"{Companyid}{Magento.Helper.QueueName.Customer}";
    new public static string InvoiceQueueName = $"{Companyid}{Magento.Helper.QueueName.Invoice}";
    new public static string OrderQueueName = $"{Companyid}{Magento.Helper.QueueName.Order}";
}

