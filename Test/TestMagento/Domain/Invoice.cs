using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;

namespace TestMagento.Domain
{

    public class Invoice : MagentoBase
    {
        private readonly IDistributedCache _cache;

        public Invoice()
        {
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

        }

        [Fact]
        public void GetInvoice_Working()
        {

            long InvoiceId = 1;


            KTI.Moo.Extensions.Magento.Domain.Invoice MagentoInvoice = new(config);

            var response = MagentoInvoice.Get(InvoiceId);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Invoice>(response);

        }


        [Fact]
        public void GetInvoiceItem_Working()
        {

            long InvoiceId = 1;


            KTI.Moo.Extensions.Magento.Domain.Invoice MagentoInvoice = new(defaultURL, redisConnectionString, username, password);

            var response = MagentoInvoice.GetItemList(InvoiceId);

            Assert.IsAssignableFrom<IEnumerable<KTI.Moo.Extensions.Magento.Model.InvoiceItem>>(response);

        }


        [Fact]
        public void SearchOrderDateRangeWorking()
        {

            DateTime datfrom = new DateTime(2021, 06, 09, 12, 46, 00);
            DateTime dateto = DateTime.UtcNow;


            KTI.Moo.Extensions.Magento.Domain.Invoice MagentoInvoice = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoInvoice.GetSearchInvoice(datfrom, dateto, pagesize: 10, currentPage: 1);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.DTO.Invoices.Search>(response);
        }


        [Fact]
        public void SearchInvoiceByOrderID()
        {

            List<KTI.Moo.Extensions.Magento.Model.Order> OrderList = new()
            {
                new()
                {
                    order_id = 8
                },
                new()
                {
                    order_id = 10
                }

            };

            KTI.Moo.Extensions.Magento.Domain.Invoice MagentoInvoice = new(Stagingconfig);

            var response = MagentoInvoice.GetSearchInvoiceByOrderIDList(OrderList, pagesize: 10, currentPage: 1);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.DTO.Invoices.Search>(response);
        }


    }
}
