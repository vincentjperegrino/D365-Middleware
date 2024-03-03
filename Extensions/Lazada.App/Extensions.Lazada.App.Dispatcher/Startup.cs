using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(KTI.Moo.Extensions.Lazada.App.Dispatcher.Startup))]

namespace KTI.Moo.Extensions.Lazada.App.Dispatcher;


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

            return new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, serviceProvider.GetService<IDistributedCache>());

        });

        var Config = new Lazada.Service.Queue.Config()
        {
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            AppKey = System.Environment.GetEnvironmentVariable("config_AppKey"),
            AppSecret = System.Environment.GetEnvironmentVariable("config_AppSecret"),
            Region = System.Environment.GetEnvironmentVariable("config_Region"),
            BaseSourceUrl = System.Environment.GetEnvironmentVariable("config_sourceURL"),
            SellerId = System.Environment.GetEnvironmentVariable("config_SellerId"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
        };

        serviceProvider = builder.Services.BuildServiceProvider();


        builder.Services.AddSingleton<KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>>(_ =>
        {

            var ChannelManagementCached = serviceProvider.GetService<Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>>();

            var LazadaClientToken = new KTI.Moo.Extensions.Lazada.Domain.ClientToken(Config);

            return new KTI.Moo.Extensions.Lazada.Domain.Queue.ClientToken_SalesChannel<Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(LazadaClientToken, ChannelManagementCached);

        });


    }
}

