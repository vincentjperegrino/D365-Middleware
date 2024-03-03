using KTI.Moo.Cyware.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Queue.Helpers
{
    public class CompanySettings : KTI.Moo.Extensions.Core.Helper.CompanySettings<KTI.Moo.Extensions.Cyware.Services.Config>
    {
        new public KTI.Moo.Extensions.Cyware.Services.Config config = ConfigHelper.Get();

        new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        new public static string CompanyId = Environment.GetEnvironmentVariable("CompanyID");
    }
}
