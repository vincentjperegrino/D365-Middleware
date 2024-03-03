using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using KTI.Moo.ChannelApps.Cyware.Services;
using ChannelApps.Cyware.Helpers;
using KTI.Moo.Cyware.Helpers;

[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.Cyware.App.Queue.Startup))]
namespace KTI.Moo.ChannelApps.Cyware.App.Queue
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(ChannelAppCywareConfigHelper.Get());
            builder.Services.AddSingleton(ConfigHelper.Get());

            builder.Services.AddSingleton<IPoison, CRM.Domain.Queue.Poison>();
           // builder.Services.AddSingleton<IChannelAppBlobService, ChannelAppBlobService>();

            builder.Services.AddTransient<IEmailNotification, KTI.Moo.Extensions.Cyware.Services.EmailNotification>();
            builder.Services.AddTransient<IQueueService, QueueService>();
            builder.Services.AddTransient<INotification, TeamsNotification>();
            builder.Services.AddTransient<IArchiveQueue, ArchiveQueueService>();
        }
    }
}
