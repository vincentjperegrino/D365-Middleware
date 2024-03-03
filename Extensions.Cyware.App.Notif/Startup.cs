using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.Cyware.App.Queue.Startup))]
namespace KTI.Moo.Extensions.Cyware.App.Queue
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Config
            builder.Services.AddSingleton(ConfigHelper.Get());

            // BLOB Service Class
            builder.Services.AddSingleton<IBlobService, BlobService>();


            // Retry function
            builder.Services.AddSingleton<IPoison, CRM.Domain.Queue.Poison>();

            // Email Notification Functions
            builder.Services.AddTransient<IEmailNotification, KTI.Moo.Extensions.Cyware.Services.EmailNotification>();
            builder.Services.AddTransient<INotification, KTI.Moo.Extensions.Cyware.Services.TeamsNotification>();
            builder.Services.AddTransient<IQueueService, QueueService>();
            builder.Services.AddTransient<IArchiveQueue, ArchiveQueueService>();







        }
    }
}
