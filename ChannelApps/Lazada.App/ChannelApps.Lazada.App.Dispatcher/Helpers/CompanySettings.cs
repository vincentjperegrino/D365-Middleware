using KTI.Moo.Extensions.Lazada.Service;

namespace KTI.Moo.ChannelApps.Lazada.App.Dispatcher.Helpers;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<KTI.Moo.Extensions.Lazada.Service.Queue.Config>
{
    new public KTI.Moo.Extensions.Lazada.Service.Queue.Config config = ConfigHelper.Get();

    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");

    new public static string StoreCode = Environment.GetEnvironmentVariable("StoreCode");

    new public static string InventoryQueueName = $"{Companyid}-lazada-{StoreCode}-extension-inventory-dispatcher";

   // new public static string CustomerQueueName = $"{Companyid}-lazada-{ChannelCode}-extension-customer-dispatcher";

    new public static string ProductQueueName = $"{Companyid}-lazada-{StoreCode}-extension-product-dispatcher";

    new public static string OrderStatusQueueName = $"{Companyid}-lazada-{StoreCode}-extension-orderstatus-dispatcher";

    //public static string InvoiceQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InvoiceQueueName")}";

    //public static string OrderQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("OrderQueueName")}";

}
