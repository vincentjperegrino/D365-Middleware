using KTI.Moo.Extensions.OctoPOS.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOctoPOS.Model;
using Xunit;

namespace TestOctoPOS.Domain
{
    public class Invoice : OctoPOSBase
    {

        private KTI.Moo.Extensions.OctoPOS.Domain.Invoice OctoPOSInvoice;
        private readonly IDistributedCache _cache;

        public Invoice()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();



        }


        [Fact]
        public void IFworkingGetInvoiceBenchmark()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);

            benchmark_Moo.Methods methods = new();
            var invoice = methods.GetInvoice();
            var response = methods.mapinvoice(invoice);

            Assert.IsAssignableFrom<benchmark_Moo.MapToThisInvoiceModel>(response);
        }

        [Fact]
        public void IFworkingGetInvoiceBenchmarkwithdynamic()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);
            benchmark_Moo.Methods methods = new();
            var invoice = methods.GetInvoice();
            var response = methods.mapinvoicewithdynamic(invoice);

            Assert.IsAssignableFrom<benchmark_Moo.MapToThisInvoiceModel>(response);
        }

        [Fact]
        public void IFworkingGetInvoice()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);
            //string InvoiceID = "TJ001S000130";
            string InvoiceID = "RB005S031185";

            var response = OctoPOSInvoice.Get(InvoiceID);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Invoice>(response);
        }

        [Fact]
        public void IFworkingGetItemsInvoice()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);
            string InvoiceID = "249S00004782g1f111f";

            var response = OctoPOSInvoice.GetItemList(InvoiceID);

            Assert.IsAssignableFrom<IEnumerable<KTI.Moo.Extensions.OctoPOS.Model.OrderItem>>(response);
        }

        [Fact]
        public void IFworkingVoidInvoice()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);
            var InvoiceID = "249S00004782g1f111f";

            var InvoiceModel = OctoPOSInvoice.Get(InvoiceID);

            var response = OctoPOSInvoice.Void(InvoiceModel);

            Assert.IsAssignableFrom<bool>(response);
        }

        [Fact]
        public void IFworkingGetInvoiceList_with_StartDateEndDate()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);
            DateTime startdate = new DateTime(2022, 5, 1, 0, 0, 0);
            DateTime enddate = DateTimeHelper.PHTnow();

            var response = OctoPOSInvoice.SearchInvoiceList(startdate, enddate);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.OctoPOS.Model.Invoice>>(response);
        }

        [Fact]
        public void IFworkingAddInvoice()
        {
            OctoPOSInvoice = new(CongfigTest, _cache);
            KTI.Moo.Extensions.OctoPOS.Model.Invoice InvoiceModel = new();

            InvoiceModel.InvoiceDate = DateTimeHelper.PHTnow();
            InvoiceModel.CustomerCode = "KTI_SAMPLE39";
            InvoiceModel.kti_sourceid = "InvoiceSampleBundleProduct44";
            InvoiceModel.ReturnInvoiceNumber = "InvoiceSampleBundleProduct4";
            InvoiceModel.OrginalInvoiceNumber = "InvoiceSampleBundleProduct4";

            InvoiceModel.InvoiceType = "credit note";
            // InvoiceModel.InvoiceType = "normal";
            InvoiceModel.CustomerAddress = "Manilass";
            InvoiceModel.Location = "WELF01";
            InvoiceModel.Terminal = "TJ001";
            InvoiceModel.totalamount = 1000;
            InvoiceModel.description = "added by KTI";
            InvoiceModel.CashierCode = CashierCodeHelper.Admin;
            InvoiceModel.SalesmanCode = CashierCodeHelper.Admin;
            InvoiceModel.Status = 1;
            //InvoiceModel.freightamount = (decimal)50.00;


            InvoiceModel.InvoiceItems = new();

            InvoiceModel.InvoiceItems.Add(new()
            {
                baseamount = 1000,
                RetailSalesPrice = (decimal)1000,
                tax = (decimal)107,
                manualdiscountamount = (decimal)100,
                quantity = 1,
                productid = "BUN-1234",
                description = "Just 1"
            });

            InvoiceModel.PaymentItems = new();

            InvoiceModel.PaymentItems.Add(new()
            {
                Amount = 1000,
                PaymentDate = DateTimeHelper.PHTnow(),
                PaymentMode = "CASH",
                CurrencyCode = "PHP"
            });


            var response = OctoPOSInvoice.Add(InvoiceModel);

            Assert.IsAssignableFrom<bool>(response);
        }





        [Fact]
        public void IFworkingGetInvoiceList_with_StartDateEndDateNewInvoiceSearch()
        {
            OctoPOSInvoice = new(ConfigProduction, _cache);
            //OctoPOSInvoice = new(CongfigTest, _cache);

            DateTime startdate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow().AddMinutes(-20);
            DateTime enddate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow().AddMinutes(+5);
            int pagenumber = 1;


            var response = OctoPOSInvoice.SearchInvoiceListWithdetails(startdate, enddate, pagenumber);

            if (response.values is null || response.values.Count <= 0)
            {
                Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices.Search>(response);
            }

            var listoflocation = response.values.Select(invoice => invoice.Location).Distinct().ToList();
            //  var listofsalesperson = response.Invoices.Select(invoice => invoice.Location).Distinct().ToList();           

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices.Search>(response);
        }


        [Fact]
        public void IFworkingGetInvoiceList_Search_Get()
        {
            OctoPOSInvoice = new(ConfigProduction, _cache);
            //OctoPOSInvoice = new(CongfigTest, _cache);

            DateTime startdate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow().AddDays(-1);
            DateTime enddate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow();
            int currentpage = 1;
            int pagesize = 100;

            //   var InvoiceList = new List<KTI.Moo.Extensions.OctoPOS.Model.Invoice>();

            var response = OctoPOSInvoice.Get(startdate, enddate, pagesize, currentpage);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices.Search>(response);
        }

        [Fact]
        public void IFworkingGetInvoiceList_Search_GetALL()
        {
            OctoPOSInvoice = new(ConfigProduction, _cache);
            //OctoPOSInvoice = new(CongfigTest, _cache);

            DateTime startdate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow().AddDays(-1);
            DateTime enddate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow();
            int currentpage = 1;
            int pagesize = 100;

            var InvoiceList = new List<KTI.Moo.Extensions.OctoPOS.Model.Invoice>();

            var response = OctoPOSInvoice.GetAll(InvoiceList, startdate, enddate, pagesize, currentpage);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.OctoPOS.Model.Invoice>>(response);
        }

        [Fact]
        public void IFworkingGetInvoiceList_Search_GetALL_Default()
        {
            OctoPOSInvoice = new(ConfigProduction, _cache);
            //OctoPOSInvoice = new(CongfigTest, _cache);

            DateTime startdate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow().AddDays(-1);
            DateTime enddate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow();

            var response = OctoPOSInvoice.GetAll(startdate, enddate);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.OctoPOS.Model.Invoice>>(response);
        }



        [Fact]
        public void Compare_CustomDomain_and_ExtensionDomainNCCI()
        {

            KTI.Moo.Extensions.OctoPOS.Domain.Invoice CustomInvoiceDomain = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);

            var StandardOrderDomain = new KTI.Moo.Extensions.OctoPOS.Domain.Invoice(ConfigProduction, _cache);

            var ExtensionsDomain = new KTI.Moo.Extensions.Domain.NCCI.OctoPOS.Invoice(StandardOrderDomain);

            var invoiceid = "RB017S007284";

            var CustomInvoice = CustomInvoiceDomain.Get(invoiceid);
            var ExtensionInvoice = ExtensionsDomain.Get(invoiceid);

            foreach (var items in CustomInvoice.InvoiceItems)
            {
                Assert.Equal(items.manualdiscountamount, ExtensionInvoice.InvoiceItems.Where(item => item.productid == items.productid).FirstOrDefault().manualdiscountamount);
                Assert.Equal(items.tax, ExtensionInvoice.InvoiceItems.Where(item => item.productid == items.productid).FirstOrDefault().tax);
                Assert.Equal(items.priceperunit, ExtensionInvoice.InvoiceItems.Where(item => item.productid == items.productid).FirstOrDefault().priceperunit);
            }
        }


        [Fact]
        public void Compare_CustomDomain_and_ExtensionDomainNCCI_SearchbyDate()
        {

            KTI.Moo.Extensions.OctoPOS.Domain.Invoice CustomInvoiceDomain = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(ConfigProduction, _cache);

            var StandardOrderDomain = new KTI.Moo.Extensions.OctoPOS.Domain.Invoice(ConfigProduction, _cache);

            var ExtensionsDomain = new KTI.Moo.Extensions.Domain.NCCI.OctoPOS.InvoiceSearch(StandardOrderDomain);

            var DateStart = DateTime.UtcNow.AddHours(-8);
            var DateEnd = DateTime.UtcNow;

            var CustomInvoice = CustomInvoiceDomain.SearchInvoiceListWithdetails(DateStart, DateEnd, 1);

            var ExtensionInvoice = ExtensionsDomain.Get(DateStart, DateEnd, 100, 1);

            Assert.Equal(CustomInvoice.values.Count, ExtensionInvoice.values.Count);

            foreach (var invoice in CustomInvoice.values)
            {
                foreach (var items in invoice.InvoiceItems)
                {
                    Assert.Equal(items.manualdiscountamount, ExtensionInvoice.values.Where(item => item.invoicenumber == invoice.invoicenumber).First().InvoiceItems.Where(item => item.productid == items.productid).FirstOrDefault().manualdiscountamount);
                    Assert.Equal(items.tax, ExtensionInvoice.values.Where(item => item.invoicenumber == invoice.invoicenumber).First().InvoiceItems.Where(item => item.productid == items.productid).FirstOrDefault().tax);
                    Assert.Equal(items.priceperunit, ExtensionInvoice.values.Where(item => item.invoicenumber == invoice.invoicenumber).First().InvoiceItems.Where(item => item.productid == items.productid).FirstOrDefault().priceperunit);
                }
            }
        }
    }
}
