using KTI.Moo.Extensions.Magento.Service;

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher.Helpers;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<Config>
{
    new public Config config = ConfigHelper.Get();

    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");

    new public static string StoreCode = Environment.GetEnvironmentVariable("StoreCode");

    new public static string InventoryQueueName = $"{Companyid}-magento-{StoreCode}-extension-inventory-dispatcher";

    new public static string CustomerQueueName = $"{Companyid}-magento-{StoreCode}-extension-customer-dispatcher";

    new public static string ProductQueueName = $"{Companyid}-magento-{StoreCode}-extension-product-dispatcher";



    //public static string InvoiceQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InvoiceQueueName")}";

    //public static string OrderQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("OrderQueueName")}";

}
