using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.App.Dispatcher.Helpers
{
    public class CompanySettings
    {
        new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        new public static string CompanyID = Environment.GetEnvironmentVariable("CompanyID");
    }
}
