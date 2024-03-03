using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;

namespace TestMagento.Domain
{

    public class Order : MagentoBase
    {

        private readonly IDistributedCache _cache;

        public Order()
        {
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

        }

        [Fact]
        public void GetOrderModel_Working()
        {

            long OrderId = 11982;

            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(Stagingconfig, _cache);

            var response = MagentoOrder.Get(OrderId);
            //     var json = JsonConvert.SerializeObject(response);
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Order>(response);

        }


        [Fact]
        public void GetOrderItems_Working()
        {

            long OrderId = 2;


            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoOrder.GetItems(OrderId);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.OrderItem>>(response);
        }


        [Fact]
        public void HoldOrder_Working()
        {

            int OrderId = 3;


            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoOrder.Hold(OrderId);

            Assert.IsAssignableFrom<bool>(response);
        }



        [Fact]
        public void UnHoldOrder_Working()
        {

            int OrderId = 3;


            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoOrder.UnHold(OrderId);

            Assert.IsAssignableFrom<bool>(response);
        }

        [Fact]
        public void CancelOrder_Working()
        {

            int OrderId = 2;


            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoOrder.Cancel(OrderId);

            Assert.IsAssignableFrom<bool>(response);
        }



        [Fact]
        public void StatusOrder_Working()
        {

            int OrderId = 3;


            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(defaultURL, redisConnectionString, username, password);


            var response = MagentoOrder.status(OrderId);

            Assert.IsAssignableFrom<string>(response);
        }


        [Fact]
        public void SearchOrderDateRangeWorking()
        {

            DateTime datfrom = DateTime.UtcNow.AddHours(-3);
            DateTime dateto = DateTime.UtcNow.AddMinutes(-5);

            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(Stagingconfig);


            var response = MagentoOrder.GetSearchOrders(datfrom, dateto, pagesize: 100, currentPage: 1);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.DTO.Orders.Search>(response);
        }



        [Fact]
        public void AddWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(Stagingconfig);


            KTI.Moo.Extensions.Magento.Domain.Customer Customer = new(Stagingconfig);

            var customermodel = Customer.Get(60);

            var Model = new KTI.Moo.Extensions.Magento.Model.Order()
            {
                emailaddress = "choidavid01@gmail.com",
                customer_id = 60,
                grand_total = 50,
                base_grand_total = 50,
                store_id = 1,
                name = "100001",

                order_items = new()
                {
                    new()
                    {
                        sku = "COF0084",
                        quantity = 1,
                        priceperunit = 50,
                        productname = "Nespresso Vertuo Double Espresso Chiaro"

                    }
                },
                billing_address = customermodel.address.Where(addrs => addrs.defaultBilling = true).Select(addrs => new KTI.Moo.Extensions.Magento.Model.OrderAddress()
                {
                    address_type = "billing",
                    address_city = addrs.address_city,
                    country_id = "PH",
                    address_id = addrs.address_id,
                    first_name = addrs.first_name,
                    last_name = addrs.last_name,
                    address_postalcode = addrs.address_postalcode,
                    region_id = addrs.region_id,
                    address_line1 = addrs.address_line1,
                    telephone = addrs.telephone,
                }).FirstOrDefault(),

                extension_attributes = new()
                {
                    shipping_assignments = new()
                    {
                        new()
                        {
                            shipping = new()
                            {
                                shipping_address = customermodel.address.Where(addrs => addrs.defaultShipping = true).Select(addrs => new KTI.Moo.Extensions.Magento.Model.OrderAddress()
                                {
                                    address_type = "shipping",
                                    address_city = addrs.address_city,
                                    country_id = "PH",
                                    address_id = addrs.address_id,
                                    first_name = addrs.first_name,
                                    last_name = addrs.last_name,
                                    address_postalcode = addrs.address_postalcode,
                                    region_id = addrs.region_id,
                                    address_line1 = addrs.address_line1,
                                    telephone = addrs.telephone,

                                }).FirstOrDefault(),
                                shipping_method = "freeshipping_freeshipping"
                            },
                            shipping_items = new()
                            {
                                new()
                                {
                                    sku = "COF0084",
                                    quantity = 1,
                                    priceperunit = 50,
                                    productname = "Nespresso Vertuo Double Espresso Chiaro"

                                }
                            }

                        }



                    }



                }



            };

            var response = MagentoOrder.Add(Model);




            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Order>(response);
        }






        [Fact]
        public void AddInvoiceWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(Stagingconfig);


            var Model = MagentoOrder.Get(151);

            var response = MagentoOrder.AddInvoice(Model);

            Assert.IsAssignableFrom<int>(response);
        }



        [Fact]
        public void SearchWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Order MagentoOrder = new(Stagingconfig);


            DateTime datfrom = new DateTime(2022, 08, 12, 6, 25, 0);
            DateTime dateto = new DateTime(2023, 08, 12, 8, 0, 0);

            var response = MagentoOrder.GetSearchOrders(datfrom, dateto, 200, 1);


            var res = JsonConvert.SerializeObject(response);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.Order>>(response.values) ;
        }












    }
}
