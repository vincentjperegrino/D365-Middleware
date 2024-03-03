using KTI.Moo.Extensions.OctoPOS.Service;

namespace KTI.Moo.ChannelApps.OctoPOS.App.Dispatcher.Helpers;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<Config>
{
    new public Config config = ConfigHelper.Get();

    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");

    new public static string StoreCode = Environment.GetEnvironmentVariable("StoreCode");

    new public static string InventoryQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InventoryQueueName")}";

    new public static string CustomerQueueName = $"{Companyid}-octopos-{StoreCode}-extension-customer-dispatcher";

    new public static string ProductQueueName = $"{Companyid}-octopos-{StoreCode}-extension-product-dispatcher";



    //public static string InvoiceQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InvoiceQueueName")}";

    //public static string OrderQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("OrderQueueName")}";

}
