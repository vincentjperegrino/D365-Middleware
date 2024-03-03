using Domain.KeyVault;
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;


[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.OctoPOS.App.NCCI.Dispatcher.Startup))]

namespace KTI.Moo.ChannelApps.OctoPOS.App.NCCI.Dispatcher;

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

        if (CompanyID == "3388" || CompanyID == "3389")
        {
            builder.Services.AddSingleton<IOrderForReceiver>(_ =>
            {
                return new KTI.Moo.ChannelApps.OctoPOS.Implementation.NCCI.Order(AzureQueueConnectionString, CompanyID);

            });    
            
            builder.Services.AddSingleton<ICustomer<ChannelApps.Model.NCCI.Receivers.Customer>>(_ =>
            {
                return new KTI.Moo.ChannelApps.OctoPOS.Implementation.NCCI.Customer(AzureQueueConnectionString, CompanyID);
            });
        }

    }
}
