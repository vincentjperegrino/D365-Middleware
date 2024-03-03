using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestShopify.Domain
{
    public class Inventory : Model.ShopifyBase
    {
        KTI.Moo.Extensions.Shopify.Domain.Inventory _domain;


        public Inventory()
        {
            _domain = new KTI.Moo.Extensions.Shopify.Domain.Inventory(TestConfig2);
        }

        [Fact]
        public void GetInventory_Success()
        {
            //assemble
            var Id = 89557205311;


            //act
            var result = _domain.Get(Id);


            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Inventory>(result);
        }

        [Fact]
        public void UpdateInventory_Success()
        {
            //assemble
            var inventory = new KTI.Moo.Extensions.Shopify.Model.Inventory()
            {
                id = 49119200510271,

                sku = "prod2",
                tracked = true,
               




            };

            //act
            var result = _domain.Update(inventory);



            //assert
            Assert.IsAssignableFrom<bool>(result);
        }


    }
}
    
