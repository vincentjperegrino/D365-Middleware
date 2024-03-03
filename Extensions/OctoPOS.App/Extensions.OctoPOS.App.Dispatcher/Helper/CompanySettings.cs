

using KTI.Moo.Extensions.OctoPOS.Service;

namespace KTI.Moo.Extensions.OctoPOS.App.Dispatcher.Helper;

public class CompanySettings : Core.Helper.CompanySettings<Config>
{
    new public Config config = ConfigHelper.Get();
    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");
    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
    new public static string CustomerQueueName = $"{Companyid}{OctoPOS.Helper.QueueName.Customer}";
    new public static string InvoiceQueueName = $"{Companyid}{OctoPOS.Helper.QueueName.Invoice}";
    new public static string OrderQueueName = $"{Companyid}{OctoPOS.Helper.QueueName.Order}";
}

