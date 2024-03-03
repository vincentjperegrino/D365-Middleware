using KTI.Moo.Extensions.Lazada.Service;

namespace KTI.Moo.ChannelApps.Lazada.App.Receiver.Helpers;

public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<KTI.Moo.Extensions.Lazada.Service.Queue.Config>
{
    new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

    new public static string Companyid = Environment.GetEnvironmentVariable("CompanyID");

}
