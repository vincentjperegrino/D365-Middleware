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


namespace Operation.SAP.Test;


public class OrderReport : SAPBase
{
    public bool _IsProduction;
    public string _repushedQueuename;
    public string _moo_webhookURL;
    public string _client_webhookURL;
    public string _crm_view_link;
    public INotification _notificationDomain;
    public KTI.Moo.Extensions.Core.Domain.ISearch<KTI.Moo.Extensions.SAP.Model.DTO.Orders.Search, KTI.Moo.Extensions.SAP.Model.Order> _extensionsSearchDomain;
    public KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> _crmSearchDomain;
    public KTI.Moo.Base.Domain.Dispatchers.IReplicate _ReplicateDomain;
    private readonly ILogger _logger;

    public OrderReport()
    {
        _logger = Mock.Of<ILogger>();
      
    }


    [Fact]
    public void SampleSendReportToTeams()
    {
        _moo_webhookURL = "https://kationtechnologies365.webhook.office.com/webhookb2/e2b37b7f-995c-404e-ab11-4bda06c6a241@705b9777-fb96-49cb-b57a-9a8fe00addad/IncomingWebhook/5d5e3da1d6c941ef86f95ca63f5e87d4/76f59b09-f545-4490-9e3c-6e5f2447e47d";
        _client_webhookURL = _moo_webhookURL;
        _IsProduction = false;
        _crm_view_link = "https://nespresso-devt.crm5.dynamics.com/main.aspx?appid=1400ac71-689c-ec11-b400-002248177277&pagetype=entitylist&etn=salesorder&viewid=bfa79e92-13e4-ed11-8847-002248ecfef0&viewType=1039";
        _notificationDomain = new KTI.Moo.CRM.Domain.Queue.Notification();
        _extensionsSearchDomain = new KTI.Moo.Extensions.SAP.Domain.Order(configProd);
        _crmSearchDomain = new KTI.Moo.CRM.Domain.Dispatchers.NCCI.OrderSearch(3389);
        _ReplicateDomain = new KTI.Moo.CRM.Domain.Order(3389);


        var domain = new KTI.Moo.Operation.SAP.Domain.Reports.Order(
            _IsProduction,
            _client_webhookURL,
            _moo_webhookURL,
            _crm_view_link,
            _notificationDomain,
            _extensionsSearchDomain,
            _crmSearchDomain,
            _ReplicateDomain);

        var PHTtimenow = DateTimeHelper.PHTnow();
        var DateStart = DateTimeHelper.PHT_to_UTC(PHTtimenow.AddDays(-1).Date);
        var DateEnd = DateTime.UtcNow;

        var result = domain.Process(DateStart, DateEnd, _logger);

        Assert.True(result);
    }
}