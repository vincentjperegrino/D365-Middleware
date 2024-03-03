using Domain.KeyVault;
using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;


[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.Lazada.App.Receiver.Store.Startup))]


namespace KTI.Moo.ChannelApps.Lazada.App.Receiver.Store;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        //builder.Services.AddDistributedRedisCache(options =>
        //{
        //    options.Configuration = Environment.GetEnvironmentVariable("config_redisConnectionString");
        //});

        var CompanyID = Environment.GetEnvironmentVariable("CompanyID");
        var AzureQueueConnectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

        if (CompanyID == "3387")
        {
            builder.Services.AddSingleton<IOrderForReceiver>(_ =>
            {
                return new ChannelApps.Lazada.Implementations.KTIdev.Order(AzureQueueConnectionString, CompanyID);
            });
        }

        if (CompanyID == "3388" || CompanyID == "3389")
        {
            builder.Services.AddSingleton<IOrderForReceiver>(_ =>
            {
                return new ChannelApps.Lazada.Implementations.NCCI.Order_Plugin(AzureQueueConnectionString, CompanyID);

            });
        }

        if (CompanyID == "3390" || CompanyID == "3391")
        {
            builder.Services.AddSingleton<IOrderForReceiver>(_ =>
            {
                return new ChannelApps.Lazada.Implementations.CCPI.Order(AzureQueueConnectionString, CompanyID);

            });
        }

        if (CompanyID == "3392" || CompanyID == "3393")
        {
            builder.Services.AddSingleton<IOrderForReceiver>(_ =>
            {
                return new ChannelApps.Lazada.Implementations.BII.Order(AzureQueueConnectionString, CompanyID);

            });
        }

    }
}
