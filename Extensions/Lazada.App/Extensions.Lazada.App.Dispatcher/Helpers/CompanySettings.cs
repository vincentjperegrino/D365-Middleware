


using System;

namespace KTI.Moo.Extensions.Lazada.App.Dispatcher.Helpers;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<KTI.Moo.Extensions.Lazada.Service.Queue.Config>
{
    new public KTI.Moo.Extensions.Lazada.Service.Queue.Config config = ConfigHelper.Get();

    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");
    new public static string StoreCode = Environment.GetEnvironmentVariable("StoreCode");

    new public static string OrderQueueName = $"moo-lazada-channelapps-order-receiver";


}
