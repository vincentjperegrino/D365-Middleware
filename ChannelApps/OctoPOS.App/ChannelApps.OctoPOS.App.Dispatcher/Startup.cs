using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;


[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.OctoPOS.App.Dispatcher.Startup))]

namespace KTI.Moo.ChannelApps.OctoPOS.App.Dispatcher;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<ICustomerToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.OctoPOS.Customer>();

        builder.Services.AddSingleton<IProductToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.OctoPOS.Product>();

        builder.Services.AddSingleton<IPoisonNotification, KTI.Moo.CRM.Domain.Queue.PoisonNotification>();

        //builder.Services.AddSingleton(provider =>
        //{
        //    var myServiceType = Type.GetType(Environment.GetEnvironmentVariable("CustomerQueueService"));
        //    return (ICustomerToQueue)ActivatorUtilities.CreateInstance(provider, myServiceType);
        //});

        //builder.Services.AddSingleton(provider =>
        //{
        //    var myServiceType = Type.GetType(Environment.GetEnvironmentVariable("ProductQueueService"));
        //    return (IProductToQueue)ActivatorUtilities.CreateInstance(provider, myServiceType);
        //});

        //builder.Services.AddSingleton(provider =>
        //{
        //    var myServiceType = Type.GetType(Environment.GetEnvironmentVariable("PoisonNotificationService"));
        //    return (IPoisonNotification)ActivatorUtilities.CreateInstance(provider, myServiceType);
        //});


    }
}
