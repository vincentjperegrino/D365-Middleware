

using KTI.Moo.Extensions.SAP.Service;

namespace KTI.Moo.Extensions.SAP.App.Queue.Dev.Helper;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<Config>
{
    new public Config config = ConfigHelper.Get();
    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");


}

