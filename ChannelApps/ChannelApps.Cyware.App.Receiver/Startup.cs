using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;

[assembly: FunctionsStartup(typeof(ChannelApps.Cyware.App.Receiver.Startup))]
namespace ChannelApps.Cyware.App.Receiver
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Queue Service
            builder.Services.AddTransient<IQueueService, QueueService>();

            // Blob Service
            builder.Services.AddSingleton<IBlobService, BlobService>();

            //builder.Services.AddSingleton<Customer>();
            //builder.Services.AddSingleton<StoreTransactions>();
        }
    }
}
