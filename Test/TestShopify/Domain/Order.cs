using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestShopify.Domain
{
    public class Order : Model.ShopifyBase
    {
        KTI.Moo.Extensions.Shopify.Domain.Order _domain;

        public Order()
        {
            _domain = new KTI.Moo.Extensions.Shopify.Domain.Order(TestConfig2);
        }

        //[Fact]
        //public void GetOrder_Success()
        //{
        //    //assemble
        //    var Id = 48510666408255;


        //    //act
        //    var result = _domain.Get(Id);


        //    //assert
        //    Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Order>(result);
        //}




    }
}
