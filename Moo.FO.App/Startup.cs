using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using KTI.Moo.Base.Domain.Queue;

[assembly: FunctionsStartup(typeof(KTI.Moo.FO.App.Startup))]
namespace KTI.Moo.FO.App
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Queue Service
            builder.Services.AddSingleton<IQueueService, QueueService>();

            // Blob Service
            builder.Services.AddSingleton<IBlobService, BlobService>();

            // Email Notification
            builder.Services.AddSingleton<IEmailNotification, EmailNotification>();
        }
    }
}
