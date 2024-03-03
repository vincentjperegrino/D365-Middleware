using System;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Helpers
{
    public class CompanySettings
    {
        new public static string ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    }
}
