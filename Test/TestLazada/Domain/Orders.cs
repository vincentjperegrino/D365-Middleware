using Xunit;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Azure;
using KTI.Moo.Extensions.Core.Helper;

namespace TestLazada.Domain
{
    public class Orders : Base.LazadaBase
    {

        private readonly KTI.Moo.Extensions.Lazada.Domain.Order OrderDomain;
        private readonly KTI.Moo.Extensions.Lazada.Domain.Queue.OrderTax OrderDomainWithTax;
        private readonly IDistributedCache _cache;

        private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain;
        public Orders()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

            var provider = services.BuildServiceProvider();

            _cache = provider.GetService<IDistributedCache>();

            var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel();

            var ChannelManagementCached = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, _cache);

            var LazadaClientToken = new KTI.Moo.Extensions.Lazada.Domain.ClientToken(CongfigTNCCI);

            clientTokenDomain = new KTI.Moo.Extensions.Lazada.Domain.Queue.ClientToken_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(LazadaClientToken, ChannelManagementCached);

            clientTokenDomain.CompanyID = 3389;

            var ChannelConfig = clientTokenDomain.GetbyLazadaSellerID(CongfigTNCCI.SellerId);

            var ClientTokenLazada = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

            //ClientTokenLazada = clientTokenDomain.Refresh(ClientTokenLazada , ChannelConfig);

            OrderDomain = new(CongfigTNCCI, ClientTokenLazada);
            OrderDomainWithTax = new(CongfigTNCCI, ClientTokenLazada);
        }

        [Fact]
        public void TestGetOrdersIFworking()
        {

            long id = 631397663095446;

            var response = OrderDomain.Get(id);

            var stringresponse = JsonConvert.SerializeObject(response);
            Assert.IsAssignableFrom<OrderHeader>(response);

        }

        [Fact]
        public void TestGetOrdersItemsIFworking()
        {

            long id = 591313025819135;

            var response = OrderDomain.GetItems(id);


            Assert.IsAssignableFrom<IEnumerable<OrderItem>>(response);


        }

        [Fact]
        public void TestGetOrdersWithIFworking()
        {

            string id = "604259773855852";

            var response = OrderDomainWithTax.GetTax_Exclusive(id);


            var stringresponse = JsonConvert.SerializeObject(response);
            Assert.IsAssignableFrom<OrderHeader>(response);

        }


        [Fact]
        public void TestDiscountworking()
        {

            var unitprice = (decimal)7633.0357;

            var manualdiscount = 20;


            var taxMultiplier = (decimal)1.12;

            decimal pricewithTax = unitprice * taxMultiplier;

            var Fullamountwithtax = (pricewithTax * (decimal)1);
            var Percent = 100;
            var DecimalPlaces = 4;

            var response = Math.Round((manualdiscount * Percent / Fullamountwithtax), DecimalPlaces);

            Assert.IsAssignableFrom<decimal>(response);

        }
               
        
        [Fact]
        public void TestGetOrdersyDate()
        {

            var DateStart = new DateTime(2023, 5, 1, 0, 0, 0);
            var DateEnd = new DateTime(2023, 5, 2, 0, 0, 0);


            var response = OrderDomain.GetAll(DateStart,DateEnd);

            Assert.IsAssignableFrom<List<OrderHeader>>(response);


        }


        //[Fact]
        //public void Test_SetInvoiceNumber_IFworking()
        //{

        //    var orderitemid = "";
        //    var invoice = "";

        //    var response = OrderDomain.SetInvoiceItem(id);

        //    Assert.IsAssignableFrom<IEnumerable<OrderItem>>(response);


        //}
    }
}