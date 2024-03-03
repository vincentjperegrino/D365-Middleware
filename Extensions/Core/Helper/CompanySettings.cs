namespace KTI.Moo.Extensions.Core.Helper;

public class CompanySettings<GenericConfig> where GenericConfig : Service.ConfigBase
{
    public GenericConfig config;

    public static string Companyid = "companyid";

    public static string ConnectionString = "AzureQueueConnectionString";

    public static string CustomerQueueName = "{Companyid}-{channel}-customer";

    public static string InvoiceQueueName = "{Companyid}-{channel}-invoice";

    public static string OrderQueueName = "{Companyid}-{channel}-order";

    public static string OrderStatusQueueName = "{Companyid}-{channel}-orderstatus";

    public static string InventoryQueueName = "{Companyid}-{channel}-inventory";

    public static string ProductQueueName = "{Companyid}-{channel}-product";

    public static string StoreCode = "StoreCode";

}
