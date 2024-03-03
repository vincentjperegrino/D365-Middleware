using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelApps.Cyware.App.Receiver.Helpers
{
    public class CompanySettings
    {
        new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        new public static string CompanyId = Environment.GetEnvironmentVariable("CompanyId");
    }
}
