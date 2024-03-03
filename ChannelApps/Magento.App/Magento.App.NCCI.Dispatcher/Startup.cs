using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher.Startup))]

namespace KTI.Moo.ChannelApps.Magento.App.NCCI.Dispatcher;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IDistributedCache>(sp =>
        {
            return new RedisCache(new RedisCacheOptions
            {
                Configuration = Environment.GetEnvironmentVariable("config_redisConnectionString")
            });
        });

        var serviceProvider = builder.Services.BuildServiceProvider();

        builder.Services.AddSingleton<ICustomerToQueue>(_ =>
        {
            return new KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento.Customer(serviceProvider.GetService<IDistributedCache>());
        });

        //builder.Services.AddSingleton<ICustomerToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento.Customer>();
        builder.Services.AddSingleton<IProductToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento.Product>();
        builder.Services.AddSingleton<IInventoryToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento.Inventory>();
    }
}
