

using KTI.Moo.Extensions.SAP.Service;

namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher.Helpers;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<Config>
{
    new public Config config = ConfigHelper.Get();

    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");
    new public static string StoreCode = Environment.GetEnvironmentVariable("StoreCode");

    new public static string InventoryQueueName = $"{Companyid}-{Environment.GetEnvironmentVariable("InventoryQueueName")}";

    new public static string CustomerQueueName = $"{Companyid}-sap-{StoreCode}-extension-customer-dispatcher";
    new public static string OrderQueueName = $"{Companyid}-sap-{StoreCode}-extension-order-dispatcher";
    new public static string InvoiceQueueName = $"{Companyid}-sap-{StoreCode}-extension-invoice-dispatcher";


}
