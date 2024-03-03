
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;

[assembly: FunctionsStartup(typeof(KTI.Moo.Operation.OctoPOS.App.Startup))]
namespace KTI.Moo.Operation.OctoPOS.App;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

        var Config = new Config()
        {
            apiAuth = System.Environment.GetEnvironmentVariable("config_apiAuth"),
            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
            password = System.Environment.GetEnvironmentVariable("config_password"),
            username = System.Environment.GetEnvironmentVariable("config_username"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),
        };

        var Connectionstring = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
        var IsProduction = Environment.GetEnvironmentVariable("IsProduction") == "1";
        var StoreCode = Environment.GetEnvironmentVariable("StoreCode");
        var ClientWebHook = Environment.GetEnvironmentVariable("ClientWebHook");
        var MooWebHook = Environment.GetEnvironmentVariable("MooWebHook");
        var Customer_crm_view_link = Environment.GetEnvironmentVariable("Customer_crm_view_link");
        var Order_crm_view_link = Environment.GetEnvironmentVariable("Order_crm_view_link");
        var InvoiceRepushedQueuename = $"{CompanyID}{KTI.Moo.Extensions.OctoPOS.Helper.QueueName.Invoice}";

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


        var StandardExtensionInvoice = new KTI.Moo.Extensions.OctoPOS.Domain.Invoice(Config, serviceProvider.GetService<IDistributedCache>());
        var StandardExtensionCustomer = new KTI.Moo.Extensions.OctoPOS.Domain.Customer(Config, serviceProvider.GetService<IDistributedCache>());

        var StandardCRMCustomer = new KTI.Moo.CRM.Domain.Customer(CompanyID); 
        var StandardCRMOrder = new KTI.Moo.CRM.Domain.Order(CompanyID);

        #endregion standard implementation

        if (CompanyID == 3388 || CompanyID == 3389)
        {
            var SearchCRMCustomer = StandardCRMCustomer;

            var SearchExtensionCustomer = StandardExtensionCustomer;

            var SearchCRMOrder = StandardCRMOrder;

            var SearchExtensionInvoice = new KTI.Moo.Extensions.Domain.NCCI.OctoPOS.InvoiceSearch(StandardExtensionInvoice);

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.ICustomer<KTI.Moo.Extensions.OctoPOS.Model.Customer, KTI.Moo.CRM.Model.CustomerBase>>(_ =>
            {
                return new KTI.Moo.Operation.OctoPOS.Domain.Reports.Customer(
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

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.IInvoiceToOrderReport<KTI.Moo.Extensions.OctoPOS.Model.Invoice, KTI.Moo.CRM.Model.OrderBase>>(_ =>
            {
                return new KTI.Moo.Operation.OctoPOS.Domain.Reports.InvoiceToOrder(
                    Connectionstring,
                    IsProduction,
                    InvoiceRepushedQueuename,
                    ClientWebHook,
                    MooWebHook,
                    Order_crm_view_link,
                    Notification,
                    SearchExtensionInvoice,
                    SearchCRMOrder
                    );
            });
        }
    }
}
