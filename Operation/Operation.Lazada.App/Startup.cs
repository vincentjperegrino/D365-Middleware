
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Lazada.Model;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;

[assembly: FunctionsStartup(typeof(KTI.Moo.Operation.Lazada.App.Startup))]

namespace KTI.Moo.Operation.Lazada.App;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var CompanyID = Convert.ToInt32(Environment.GetEnvironmentVariable("CompanyID"));

        var Congfig = new KTI.Moo.Extensions.Lazada.Service.Queue.Config()
        {
            defaultURL = System.Environment.GetEnvironmentVariable("config_defaultURL"),
            redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"),
            AppKey = System.Environment.GetEnvironmentVariable("config_AppKey"),
            AppSecret = System.Environment.GetEnvironmentVariable("config_AppSecret"),
            Region = System.Environment.GetEnvironmentVariable("config_Region"),
            SellerId = System.Environment.GetEnvironmentVariable("config_SellerId"),
            companyid = System.Environment.GetEnvironmentVariable("CompanyID"),

        };

        var Connectionstring = Environment.GetEnvironmentVariable("AzureQueueConnectionString");
        var IsProduction = Environment.GetEnvironmentVariable("IsProduction") == "1";
        var OrderRepushedQueuename = Environment.GetEnvironmentVariable("RepushedQueuename");
        var ClientWebHook = Environment.GetEnvironmentVariable("ClientWebHook");
        var MooWebHook = Environment.GetEnvironmentVariable("MooWebHook");
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

        var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel();

        var ChannelManagementCached = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, serviceProvider.GetService<IDistributedCache>());

        var LazadaClientToken = new KTI.Moo.Extensions.Lazada.Domain.ClientToken(Congfig);

        var clientTokenDomain = new KTI.Moo.Extensions.Lazada.Domain.Queue.ClientToken_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(LazadaClientToken, ChannelManagementCached);
        clientTokenDomain.CompanyID = CompanyID;

        var ChannelConfig = clientTokenDomain.GetbyLazadaSellerID(Congfig.SellerId);

        var ClientTokenLazada = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
        {
            AccessToken = ChannelConfig.kti_access_token,
            RefreshToken = ChannelConfig.kti_refresh_token,
            AccessExpiration = ChannelConfig.kti_access_expiration,
            RefreshExpiration = ChannelConfig.kti_refresh_expiration
        };

        var StandardExtensionOrder = new KTI.Moo.Extensions.Lazada.Domain.Order(Congfig, ClientTokenLazada);

        var StandardCRMCustomer = new KTI.Moo.CRM.Domain.Customer(CompanyID);

        var StandardCRMOrder = new KTI.Moo.CRM.Domain.Order(CompanyID);

        #endregion standard implementation

        if (CompanyID == 3388 || CompanyID == 3389)
        {
            var SearchExtensionOrder = StandardExtensionOrder;
            var SearchCRMOrder = StandardCRMOrder;
            var SearchCRMCustomer = StandardCRMCustomer;

            builder.Services.AddSingleton<KTI.Moo.Operation.Core.Domain.Reports.IOrder<KTI.Moo.Extensions.Lazada.Model.OrderHeader, KTI.Moo.CRM.Model.OrderBase>>(_ =>
            {
                return new KTI.Moo.Operation.Lazada.Domain.Reports.Order(
                    Connectionstring,
                    IsProduction,
                    OrderRepushedQueuename,
                    ClientWebHook,
                    MooWebHook,
                    Order_crm_view_link,
                    Congfig.SellerId,
                    Notification,
                    SearchExtensionOrder,
                    SearchCRMOrder,
                    SearchCRMCustomer
                    );
            });
        }






    }
}