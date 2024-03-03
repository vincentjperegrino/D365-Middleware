using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using KTI.Moo.Cyware.Helpers;

[assembly: FunctionsStartup(typeof(KTI.Moo.FO.App.Queue.Startup))]
namespace KTI.Moo.FO.App.Queue
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Config
            builder.Services.AddSingleton(ConfigHelper.Get());

            builder.Services.AddSingleton<IBlobService, BlobService>();
            builder.Services.AddSingleton<IPoison, CRM.Domain.Queue.Poison>();
            builder.Services.AddTransient<IEmailNotification, EmailNotification>();
            builder.Services.AddTransient<IQueueService, QueueService>();
            builder.Services.AddTransient<IArchiveQueue, ArchiveQueueService>();
        }
    }
}
