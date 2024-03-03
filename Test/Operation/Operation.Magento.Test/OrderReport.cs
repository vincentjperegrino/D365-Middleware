﻿using KTI.Moo.Base.Domain.Queue;
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

namespace Operation.Magento.Test;
public class OrderReport : TestBase
{
    public string _connectionString;
    public bool _IsProduction;
    public string _repushedQueuename;
    public string _moo_webhookURL;
    public string _client_webhookURL;
    public string _crm_view_link;
    public INotification _notificationDomain;
    public KTI.Moo.Extensions.Core.Domain.ISearch<KTI.Moo.Extensions.Magento.Model.DTO.Orders.Search, KTI.Moo.Extensions.Magento.Model.Order> _extensionsSearchDomainStandard;


    public KTI.Moo.Extensions.Core.Domain.ISearch<KTI.Moo.Extensions.Magento.Model.DTO.Orders.Search, KTI.Moo.Extensions.Magento.Model.Order> _extensionsSearchDomain;
    public KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> _crmSearchDomain;
    public KTI.Moo.Extensions.Core.Domain.ICustomer<KTI.Moo.Extensions.Magento.Model.Customer> _extensionCustomer;

    private readonly ILogger _logger; 
    private readonly IDistributedCache _cache;


    public OrderReport()
    {
        _logger = Mock.Of<ILogger>();
        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

        var provider = services.BuildServiceProvider();
        _cache = provider.GetService<IDistributedCache>();
    }

    [Fact]
    public void SampleSendReportToTeams_NCCICustom()
    {
        _connectionstring = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";
        _moo_webhookURL = "https://kationtechnologies365.webhook.office.com/webhookb2/e2b37b7f-995c-404e-ab11-4bda06c6a241@705b9777-fb96-49cb-b57a-9a8fe00addad/IncomingWebhook/5d5e3da1d6c941ef86f95ca63f5e87d4/76f59b09-f545-4490-9e3c-6e5f2447e47d";
        _client_webhookURL = _moo_webhookURL;
        _IsProduction = false;
        _repushedQueuename = "3388-magento-order";
        _crm_view_link = "https://nespresso-devt.crm5.dynamics.com/main.aspx?appid=1400ac71-689c-ec11-b400-002248177277&pagetype=entitylist&etn=salesorder&viewid=bfa79e92-13e4-ed11-8847-002248ecfef0&viewType=1039";
        _notificationDomain = new KTI.Moo.CRM.Domain.Queue.Notification();
        _extensionsSearchDomainStandard = new KTI.Moo.Extensions.Magento.Domain.Order(ProdToStagingconfig, _cache);
        _extensionsSearchDomain = new KTI.Moo.Extensions.Domain.NCCI.Magento.OrderSearch(_extensionsSearchDomainStandard);

        _crmSearchDomain = new KTI.Moo.CRM.Domain.Order(3389);
        _extensionCustomer = new KTI.Moo.Extensions.Magento.Domain.Customer(ProdToStagingconfig);

        var domain = new KTI.Moo.Operation.Magento.Domain.Reports.Order(
            _connectionstring,
            _IsProduction,
            _repushedQueuename,
            _client_webhookURL,
            _moo_webhookURL,
            _crm_view_link,
            _notificationDomain,
            _extensionsSearchDomain,
            _crmSearchDomain,
            _extensionCustomer);

        var PHTtimenow = DateTimeHelper.PHTnow();
        var DateStart = DateTimeHelper.PHT_to_UTC(PHTtimenow.AddDays(-1).Date);
        var DateEnd = DateTime.UtcNow;

        var result = domain.Process(DateStart, DateEnd, _logger);

        Assert.True(result);
    }




}
