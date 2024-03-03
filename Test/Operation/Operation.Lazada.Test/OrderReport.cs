using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Operation.Lazada.Test;
public class OrderReport : LazadaBase
{
    public string _connectionString;
    public bool _IsProduction;
    public string _repushedQueuename;
    public string _moo_webhookURL;
    public string _client_webhookURL;
    public string _crm_view_link;

    public INotification _notificationDomain;
    public KTI.Moo.Extensions.Core.Domain.ISearch<KTI.Moo.Extensions.Lazada.Model.DTO.OrderSearch, KTI.Moo.Extensions.Lazada.Model.OrderHeader> _extensionsSearchDomain;

    public KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> _crmSearchDomain;
    public KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.CustomerSearch, KTI.Moo.CRM.Model.CustomerBase> _crmSearchCustomerDomain;



    private readonly ILogger _logger;
    private readonly IDistributedCache _cache;

    private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain;

    public OrderReport()
    {
        _logger = Mock.Of<ILogger>();

        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

        var provider = services.BuildServiceProvider();

        _cache = provider.GetService<IDistributedCache>();


        var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel();

        var ChannelManagementCached = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, _cache);

        var LazadaClientToken = new KTI.Moo.Extensions.Lazada.Domain.ClientToken(CongfigTest);

        clientTokenDomain = new KTI.Moo.Extensions.Lazada.Domain.Queue.ClientToken_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(LazadaClientToken, ChannelManagementCached);

    }

    [Fact]
    public void SampleSendReportToTeams_NCCICustom()
    {

        clientTokenDomain.CompanyID = 3388;
        var ChannelConfig = clientTokenDomain.GetbyLazadaSellerID(CongfigTest.SellerId);

        var ClientTokenLazada = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
        {
            AccessToken = ChannelConfig.kti_access_token,
            RefreshToken = ChannelConfig.kti_refresh_token,
            AccessExpiration = ChannelConfig.kti_access_expiration,
            RefreshExpiration = ChannelConfig.kti_refresh_expiration
        };

        KTI.Moo.Extensions.Lazada.Domain.Order OrderDomain = new(CongfigTest, ClientTokenLazada);

        _connectionstring = "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net";
        _moo_webhookURL = "https://kationtechnologies365.webhook.office.com/webhookb2/e2b37b7f-995c-404e-ab11-4bda06c6a241@705b9777-fb96-49cb-b57a-9a8fe00addad/IncomingWebhook/5d5e3da1d6c941ef86f95ca63f5e87d4/76f59b09-f545-4490-9e3c-6e5f2447e47d";
        _client_webhookURL = _moo_webhookURL;
        _IsProduction = false;
        _repushedQueuename = "moo-lazada-channelapps-order-receiver-test";
        _crm_view_link = "https://nespresso-devt.crm5.dynamics.com/main.aspx?appid=1400ac71-689c-ec11-b400-002248177277&pagetype=entitylist&etn=salesorder&viewid=bfa79e92-13e4-ed11-8847-002248ecfef0&viewType=1039";
        _notificationDomain = new KTI.Moo.CRM.Domain.Queue.Notification();
        _extensionsSearchDomain = OrderDomain;
        _crmSearchDomain = new KTI.Moo.CRM.Domain.Order(clientTokenDomain.CompanyID);
        _crmSearchCustomerDomain = new KTI.Moo.CRM.Domain.Customer(clientTokenDomain.CompanyID);

        var domain = new KTI.Moo.Operation.Lazada.Domain.Reports.Order(
            _connectionstring,
            _IsProduction,
            _repushedQueuename,
            _client_webhookURL,
            _moo_webhookURL,
            _crm_view_link,
            ChannelConfig.kti_sellerid,
            _notificationDomain,
            _extensionsSearchDomain,
            _crmSearchDomain,
            _crmSearchCustomerDomain);

        var PHTtimenow = DateTimeHelper.PHTnow();
        var DateStart = DateTimeHelper.PHT_to_UTC(PHTtimenow.AddDays(-11).Date);
        var DateEnd = DateTime.UtcNow;

        var result = domain.Process(DateStart, DateEnd, _logger);

        Assert.True(result);
    }




}
