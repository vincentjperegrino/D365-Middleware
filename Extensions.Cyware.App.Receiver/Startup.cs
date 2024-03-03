using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.Cyware.App.Receiver.Startup))]
namespace KTI.Moo.Extensions.Cyware.App.Receiver
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Config
            builder.Services.AddSingleton(ConfigHelper.Get());

            // Blob Service
            builder.Services.AddSingleton<IBlobService, BlobService>();

            // Queue Service
            builder.Services.AddSingleton<IQueueService, QueueService>();
        }
    }
}
