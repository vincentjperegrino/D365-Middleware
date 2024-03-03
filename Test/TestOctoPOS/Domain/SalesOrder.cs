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
    public class SalesOrder : OctoPOSBase
    {

        private readonly KTI.Moo.Extensions.OctoPOS.Domain.Order OctoPOSOrder;
        private readonly IDistributedCache _cache;

        public SalesOrder()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            OctoPOSOrder = new(CongfigTest, _cache);

        }


        [Fact]
        public void IFworkingGetSalesOrder()
        {

            string orderid = "1234asda222";

            var response = OctoPOSOrder.Get(orderid);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Order>(response);
        }

        [Fact]
        public void IFworkingGetItemsSalesOrder()
        {

            string orderid = "1234asda222";

            var response = OctoPOSOrder.GetItems(orderid);

            Assert.IsAssignableFrom<IEnumerable<KTI.Moo.Extensions.OctoPOS.Model.OrderItem>>(response);
        }

        [Fact]
        public void IFworkingGetVoidStatusSalesOrder()
        {

            string orderid = "1234asda222";

            var response = OctoPOSOrder.GetVoidStatus(orderid);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Order>(response);
        }

        [Fact]
        public void IFworkingAddSalesOrder()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Order OrderModel = new();

            OrderModel.SalesOrderNumber = "1234asda22211asdasa";
            OrderModel.CustomerCode = "Sample1234";
            //  OrderModel.SalesOrderDate = DateTimeHelper.PHTnow();
            //OrderModel.CashierCode = "admin";
            //OrderModel.Status = 1;
            OrderModel.Terminal = "TJ001";
            OrderModel.description = "Far fetch shooting ssad";
            OrderModel.Location = "WELF01";
            OrderModel.DeliveryAddress1 = "Manila";
            OrderModel.DeliveryAddress2 = "Manila";
            OrderModel.DeliveryStatus = "DELIVERED";
            OrderModel.NetSalesAmount = (decimal)170.33;
            OrderModel.TotalDiscountAmount = 0;
            OrderModel.TaxAmount = (decimal)162.25;
            OrderModel.IsTaxExclusive = 1;
            OrderModel.WholeReceiptDiscount = 0;
            OrderModel.freightamount = 0;
            OrderModel.TaxPercentage = 0;
            OrderModel.CurrencyCode = "SGD";
            OrderModel.ReserveInventory = 1;

            //   OrderModel.SalesOrderItems = new();

            OrderModel.SalesOrderItems.Add(new()
            {

                productid = "MACB052-Sample39",
                quantity = 1,
                baseamount = 180,
                costpriceperunit = 1,
                manualdiscountamount = 65,
                DiscountPercentage = 50,
                ItemNetAmount = 180,
                TaxAmount = 12,
                TaxPercentage = 7,


            });




            var response = OctoPOSOrder.Add(OrderModel);

            Assert.IsAssignableFrom<bool>(response);
        }


        [Fact]
        public void IFworkingVoidSalesOrder()
        {

            var SalesOrderNumber = "1231231231samplessss1332222111";

            var OrderModel = OctoPOSOrder.Get(SalesOrderNumber);

            var response = OctoPOSOrder.Void(OrderModel);

            Assert.IsAssignableFrom<bool>(response);
        }
    }
}
