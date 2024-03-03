
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.OctoPOS.App.Queue.Startup))]

namespace KTI.Moo.Extensions.OctoPOS.App.Queue;


public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var cachedService = builder.Services.AddDistributedRedisCache(options =>
         {
             options.Configuration = Environment.GetEnvironmentVariable("config_redisConnectionString");

         });


        var serviceProvider = builder.Services.BuildServiceProvider();

        builder.Services.AddSingleton<Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>>(_ =>
        {
            var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));
            var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement(CompanyID);

            return new KTI.Moo.CRM.Domain.ChannelManagementCached(ChannelManagement, serviceProvider.GetService<IDistributedCache>() );

        });



    }
}

