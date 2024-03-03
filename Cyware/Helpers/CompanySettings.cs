using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Cyware.Helpers
{
    public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<KTI.Moo.Extensions.Cyware.Services.Config>
    {
        new public KTI.Moo.Extensions.Cyware.Services.Config config = ConfigHelper.Get();

        new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        new public static string CompanyId = Environment.GetEnvironmentVariable("CompanyID");
        new public const string TimerTriggerConfig = "*/10 * * * * *";
        new public static string ExtensionArchiveQueueName = "moo-cyware-extension-archive-queue";
        new public static string ChannelArchiveQueueName = "moo-cyware-channel-archive-queue";
        new public static string FOArchiveQueueName = "moo-cyware-fo-archive-queue";
    }
}
