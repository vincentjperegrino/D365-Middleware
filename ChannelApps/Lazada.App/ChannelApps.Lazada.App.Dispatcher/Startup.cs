using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.ChannelApps.Lazada.App.Dispatcher.Startup))]


namespace KTI.Moo.ChannelApps.Lazada.App.Dispatcher;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {

        var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

        if (CompanyID == 3387)
        {
            builder.Services.AddSingleton<IProductToQueue, KTI.Moo.ChannelApps.Model.KTIdev.Dispatchers.Lazada.Product>();
            builder.Services.AddSingleton<IInventoryToQueue, KTI.Moo.ChannelApps.Model.KTIdev.Dispatchers.Lazada.Inventory>();
            builder.Services.AddSingleton<IOrderStatus, KTI.Moo.ChannelApps.Model.Dispatchers.Lazada.OrderStatus>();
        }

        if (CompanyID == 3388 || CompanyID == 3389)
        {
            builder.Services.AddSingleton<IProductToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Lazada.Product>();
            builder.Services.AddSingleton<IInventoryToQueue, KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Lazada.Inventory>();
            builder.Services.AddSingleton<IOrderStatus, KTI.Moo.ChannelApps.Model.Dispatchers.Lazada.OrderStatus>();
        }

        if (CompanyID == 3390 || CompanyID == 3391)
        {
            builder.Services.AddSingleton<IProductToQueue, KTI.Moo.ChannelApps.Model.CCPI.Dispatchers.Lazada.Product>();
            builder.Services.AddSingleton<IInventoryToQueue, KTI.Moo.ChannelApps.Model.CCPI.Dispatchers.Lazada.Inventory>();
            builder.Services.AddSingleton<IOrderStatus, KTI.Moo.ChannelApps.Model.Dispatchers.Lazada.OrderStatus>();
        }


    }
}
