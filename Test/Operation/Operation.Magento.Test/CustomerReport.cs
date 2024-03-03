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

namespace Operation.Magento.Test
{
    public class CustomerReport : TestBase
    {
        public string _connectionString;
        public bool _IsProduction;
        public string _repushedQueuename;
        public string _moo_webhookURL;
        public string _client_webhookURL;
        public string _crm_view_link;
        public INotification _notificationDomain;
        public KTI.Moo.Extensions.Core.Domain.ISearch<KTI.Moo.Extensions.Magento.Model.DTO.Customers.Search, KTI.Moo.Extensions.Magento.Model.Customer> _extensionsSearchDomain;
        public KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.CustomerSearch, KTI.Moo.CRM.Model.CustomerBase> _crmSearchDomain;
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;


        public CustomerReport()
        {
            _logger = Mock.Of<ILogger>();
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();
        }


        [Fact]
        public void SampleSendReportToTeams()
        {
            _connectionstring = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";
            _moo_webhookURL = "https://kationtechnologies365.webhook.office.com/webhookb2/e2b37b7f-995c-404e-ab11-4bda06c6a241@705b9777-fb96-49cb-b57a-9a8fe00addad/IncomingWebhook/5d5e3da1d6c941ef86f95ca63f5e87d4/76f59b09-f545-4490-9e3c-6e5f2447e47d";
            _client_webhookURL = _moo_webhookURL;
            _IsProduction = false;
            _repushedQueuename = "3388-magento-customer";
            _crm_view_link = "https://nespresso-devt.crm5.dynamics.com/main.aspx?appid=1400ac71-689c-ec11-b400-002248177277&pagetype=entitylist&etn=contact&viewid=930e1e6d-10e4-ed11-8847-002248ecfef0&viewType=1039";
            _notificationDomain = new KTI.Moo.CRM.Domain.Queue.Notification();
            _extensionsSearchDomain = new KTI.Moo.Extensions.Magento.Domain.Customer(ProdToStagingconfig, _cache);
            _crmSearchDomain = new KTI.Moo.CRM.Domain.Customer(3388);

            var domain = new KTI.Moo.Operation.Magento.Domain.Reports.Customer(
                _connectionstring,
                _IsProduction,
                _repushedQueuename,
                _client_webhookURL,
                _moo_webhookURL,
                _crm_view_link,
                _notificationDomain,
                _extensionsSearchDomain,
                _crmSearchDomain);

            var DateStart = DateTimeHelper.PHT_to_UTC(new DateTime(2023, 04, 18, 00, 00, 00));
            var DateEnd = DateTimeHelper.PHT_to_UTC(new DateTime(2023, 04, 19, 23, 59, 59));

            var result = domain.Process(DateStart, DateEnd, _logger);


            Assert.True(result);
        }



        [Fact]
        public void SendRepushFunction()
        {
            _connectionstring = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";
            _moo_webhookURL = "https://kationtechnologies365.webhook.office.com/webhookb2/e2b37b7f-995c-404e-ab11-4bda06c6a241@705b9777-fb96-49cb-b57a-9a8fe00addad/IncomingWebhook/5d5e3da1d6c941ef86f95ca63f5e87d4/76f59b09-f545-4490-9e3c-6e5f2447e47d";
            _client_webhookURL = _moo_webhookURL;
            _IsProduction = false;
            _repushedQueuename = "3388-magento-customer";
            _crm_view_link = "https://nespresso-devt.crm5.dynamics.com/main.aspx?appid=1400ac71-689c-ec11-b400-002248177277&pagetype=entitylist&etn=contact&viewid=930e1e6d-10e4-ed11-8847-002248ecfef0&viewType=1039";
            _notificationDomain = new KTI.Moo.CRM.Domain.Queue.Notification();
            _extensionsSearchDomain = new KTI.Moo.Extensions.Magento.Domain.Customer(ProdToStagingconfig, _cache);
            _crmSearchDomain = new KTI.Moo.CRM.Domain.Customer(3388);


            var domain = new KTI.Moo.Operation.Magento.Domain.Reports.Customer(
                _connectionstring,
                _IsProduction,
                _repushedQueuename,
                _client_webhookURL,
                _moo_webhookURL,
                _crm_view_link,
                _notificationDomain,
                _extensionsSearchDomain,
                _crmSearchDomain);

            var DateStart = DateTimeHelper.PHT_to_UTC(new DateTime(2023, 04, 17, 00, 00, 00));
            var DateEnd = DateTimeHelper.PHT_to_UTC(new DateTime(2023, 04, 17, 23, 59, 59));

            var customers = domain.GetListFromExtention(DateStart, DateEnd);

            var result = domain.SendToRetryQueue(customers, _logger);

            Assert.IsType<string>(result);
        }





    }
}
