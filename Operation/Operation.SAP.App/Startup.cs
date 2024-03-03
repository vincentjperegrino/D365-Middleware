using KTI.Moo.Base.Domain.Queue;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;

[assembly: FunctionsStartup(typeof(KTI.Moo.Operation.SAP.App.Startup))]
namespace KTI.Moo.Operation.SAP.App;


public class Startup : FunctionsStartup
{

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

        var Config = new KTI.Moo.Extensions.SAP.Service.Config()
        {
            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
            password = System.Environment.GetEnvironmentVariable("config_password"),
            username = System.Environment.GetEnvironmentVariable("config_username"),
            companyDB = System.Environment.GetEnvironmentVariable("config_companyDB"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
        };

        var Connectionstring = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
        var IsProduction = Environment.GetEnvironmentVariable("IsProduction") == "1";
        var StoreCode = Environment.GetEnvironmentVariable("StoreCode");
        var ClientWebHook = Environment.GetEnvironmentVariable("ClientWebHook");
        var MooWebHook = Environment.GetEnvironmentVariable("MooWebHook");
        var Customer_crm_view_link = Environment.GetEnvironmentVariable("Customer_crm_view_link");
        var Order_crm_view_link = Environment.GetEnvironmentVariable("Order_crm_view_link");

        #region standard implementation

        builder.Services.AddSingleton<IDistributedCache>(sp =>
        {
            return new RedisCache(new RedisCacheOptions
            {
                Configuration = Environment.GetEnvironmentVariable("config_redisConnectionString")
            });
        });

        var serviceProvider = builder.Services.BuildServiceProvider();

        var Notification = new KTI.Moo.CRM.Domain.Queue.Notification();

        var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel(CompanyID);

        var CachedChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, serviceProvider.GetService<IDistributedCache>());

        var StandardExtensionCustomer = new KTI.Moo.Extensions.SAP.Domain.Customer(Config);

        var StandardExtensionOrder = new KTI.Moo.Extensions.SAP.Domain.Order(Config);

        var StandardCRMOrder = new KTI.Moo.CRM.Domain.Order(CompanyID);

        #endregion standard implementation


        if (CompanyID == 3388 || CompanyID == 3389)
        {

            var SearchCRMCustomer = new KTI.Moo.CRM.Domain.Dispatchers.NCCI.CustomerSearch(CompanyID);

            var SearchCRMOrder = new KTI.Moo.CRM.Domain.Dispatchers.NCCI.OrderSearch(CompanyID);

            var SearchExtensionCustomer = StandardExtensionCustomer;

            var SearchExtensionOrder = StandardExtensionOrder;

            var OrderReplicate = StandardCRMOrder;

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.ICustomer<KTI.Moo.Extensions.SAP.Model.Customer, KTI.Moo.CRM.Model.CustomerBase>>(_ =>
            {
                return new KTI.Moo.Operation.SAP.Domain.Reports.Customer(
                    Connectionstring,
                    IsProduction,
                    StoreCode,
                    ClientWebHook,
                    MooWebHook,
                    Customer_crm_view_link,
                    Notification,
                    SearchExtensionCustomer,
                    SearchCRMCustomer,
                    CachedChannelManagement
                    );
            });

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.IOrder<KTI.Moo.Extensions.SAP.Model.Order, KTI.Moo.CRM.Model.OrderBase>>(_ =>
            {        
                return new KTI.Moo.Operation.SAP.Domain.Reports.Order(
                    IsProduction,
                    ClientWebHook,
                    MooWebHook,
                    Order_crm_view_link,
                    Notification,
                    SearchExtensionOrder,
                    SearchCRMOrder,
                    OrderReplicate
                    );
            });
        }


    }
}
