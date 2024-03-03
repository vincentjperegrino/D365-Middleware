﻿using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.SAP.App.Dispatcher.Dev.Startup))]

namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher.Dev;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<ICustomerToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.SAP.Customer>();
        builder.Services.AddSingleton<IOrderToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.SAP.Order>();
        builder.Services.AddSingleton<IInvoiceToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.SAP.Invoice>();
    }
}
