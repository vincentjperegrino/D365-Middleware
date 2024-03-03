
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.Magento.App.Queue.Startup))]

namespace KTI.Moo.Extensions.Magento.App.Queue;


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

        builder.Services.AddSingleton<Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>>(_ =>
        {
            var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));
            var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel(CompanyID);

            return new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, serviceProvider.GetService<IDistributedCache>() );

        });



    }
}

