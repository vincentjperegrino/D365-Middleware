
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.OctoPOS.App.Queue.Startup))]

namespace KTI.Moo.Extensions.OctoPOS.App.Queue;


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

        builder.Services.AddSingleton<IPoisonNotification, KTI.Moo.CRM.Domain.Queue.PoisonNotification>();

    }
}

