
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;

[assembly: FunctionsStartup(typeof(KTI.Moo.Operation.Magento.App.Startup))]

namespace KTI.Moo.Operation.Magento.App;

public class Startup : FunctionsStartup
{

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

        var Config = new Config()
        {
            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
            password = System.Environment.GetEnvironmentVariable("config_password"),
            username = System.Environment.GetEnvironmentVariable("config_username"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID")
        };

        var Connectionstring = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
        var IsProduction = Environment.GetEnvironmentVariable("IsProduction") == "1";
        var ClientWebHook = Environment.GetEnvironmentVariable("ClientWebHook");
        var MooWebHook = Environment.GetEnvironmentVariable("MooWebHook");

        var Order_crm_view_link = Environment.GetEnvironmentVariable("Order_crm_view_link");
        var Customer_crm_view_link = Environment.GetEnvironmentVariable("Customer_crm_view_link");

        var CustomerRepushedQueuename = $"{CompanyID}{KTI.Moo.Extensions.Magento.Helper.QueueName.Customer}";
        var OrderRepushedQueuename = $"{CompanyID}{KTI.Moo.Extensions.Magento.Helper.QueueName.Order}";


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
        var StandardExtensionOrder = new KTI.Moo.Extensions.Magento.Domain.Order(Config, serviceProvider.GetService<IDistributedCache>());
        var StandardExtensionCustomer = new KTI.Moo.Extensions.Magento.Domain.Customer(Config, serviceProvider.GetService<IDistributedCache>());

        var StandardCRMCustomer = new KTI.Moo.CRM.Domain.Customer(CompanyID);
        var StandardCRMOrder = new KTI.Moo.CRM.Domain.Order(CompanyID);

        #endregion standard implementation


        if (CompanyID == 3388 || CompanyID == 3389)
        {

            var SearchCRMCustomer = StandardCRMCustomer;

            var SearchCRMOrder = StandardCRMOrder;

            var SearchExtensionCustomer = StandardExtensionCustomer;

            var ExtensionCustomer = StandardExtensionCustomer;

            var SearchExtensionOrder = new KTI.Moo.Extensions.Domain.NCCI.Magento.OrderSearch(StandardExtensionOrder);

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.ICustomer<KTI.Moo.Extensions.Magento.Model.Customer, KTI.Moo.CRM.Model.CustomerBase>>(_ =>
            {
                return new KTI.Moo.Operation.Magento.Domain.Reports.Customer(
                    Connectionstring,
                    IsProduction,
                    CustomerRepushedQueuename,
                    ClientWebHook,
                    MooWebHook,
                    Customer_crm_view_link,
                    Notification,
                    SearchExtensionCustomer,
                    SearchCRMCustomer
                    );
            });

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.IOrder<KTI.Moo.Extensions.Magento.Model.Order, KTI.Moo.CRM.Model.OrderBase>>(_ =>
            {
                return new KTI.Moo.Operation.Magento.Domain.Reports.Order(
                    Connectionstring,
                    IsProduction,
                    OrderRepushedQueuename,
                    ClientWebHook,
                    MooWebHook,
                    Order_crm_view_link,
                    Notification,
                    SearchExtensionOrder,
                    SearchCRMOrder,
                    ExtensionCustomer
                    );
            });
        }






    }
}