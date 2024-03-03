using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(KTI.Moo.CRM.App.Startup))]

namespace KTI.Moo.CRM.App
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

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
                //  var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));
                var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel(CompanyID);

                return new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, serviceProvider.GetService<IDistributedCache>());

            });
            
            builder.Services.AddSingleton<Base.Domain.IChannelManagementInventory<KTI.Moo.CRM.Model.ChannelManagement.Inventory>>(_ =>
            {
         
                return new KTI.Moo.CRM.Domain.ChannelManagement.Inventory(CompanyID);
            });

            builder.Services.AddSingleton<Base.Domain.Dispatchers.IDefault<Model.ChannelManagement.SalesChannel>, CRM.Domain.Dispatchers.KTIdev.Default>();

            if (CompanyID == 3387)
            {
                builder.Services.AddSingleton<Base.Domain.Dispatchers.IDefault<Model.ChannelManagement.SalesChannel>, CRM.Domain.Dispatchers.KTIdev.Default>();
                builder.Services.AddSingleton<Base.Domain.Dispatchers.IInventory<Model.ChannelManagement.Inventory>, CRM.Domain.Dispatchers.KTIdev.Inventory>();
                builder.Services.AddSingleton<Base.Domain.Dispatchers.IProduct, CRM.Domain.Dispatchers.KTIdev.Product>();
            }

            if (CompanyID == 3388 || CompanyID == 3389)
            {
                builder.Services.AddSingleton<Base.Domain.Dispatchers.IDefault<Model.ChannelManagement.SalesChannel>, CRM.Domain.Dispatchers.NCCI.Default>();

                builder.Services.AddSingleton<Base.Domain.Dispatchers.IInventory<Model.ChannelManagement.Inventory>, CRM.Domain.Dispatchers.NCCI.Inventory>();
                builder.Services.AddSingleton<Base.Domain.Dispatchers.IProduct, CRM.Domain.Dispatchers.NCCI.Product>();
            }

            if (CompanyID == 3390 || CompanyID == 3391)
            {

                builder.Services.AddSingleton<Base.Domain.Dispatchers.IInventory<Model.ChannelManagement.Inventory>, CRM.Domain.Dispatchers.CCPI.Inventory>();
                builder.Services.AddSingleton<Base.Domain.Dispatchers.IProduct, CRM.Domain.Dispatchers.CCPI.Product>();
            }



        }
    }
}
