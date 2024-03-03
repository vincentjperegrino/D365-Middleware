using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;


namespace TestMagento.Domain
{
    public class Inventory : MagentoBase
    {

        [Fact]
        public void GetInventoryWorking()
        {

            var ProductID = 37;

            KTI.Moo.Extensions.Magento.Domain.Inventory MagentoInventoryDomain = new(defaultURL, redisConnectionString, username, password);

            var response = MagentoInventoryDomain.Get(ProductID);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Inventory>(response);

        }


        [Fact]
        public void GetInventorySKUWorking()
        {

            var ProductSKU = "Chocoloco12";

            KTI.Moo.Extensions.Magento.Domain.Inventory MagentoInventoryDomain = new(defaultURL, redisConnectionString, username, password);

            var response = MagentoInventoryDomain.Get(ProductSKU);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Inventory>(response);

        }


        [Fact]
        public void UpdateInventoryWorking()
        {

            KTI.Moo.Extensions.Magento.Model.Inventory InventoryModel = new();

            KTI.Moo.Extensions.Magento.Domain.Inventory MagentoInventoryDomain = new(Stagingconfig);

            var ProductSKU = "COF0095";

            InventoryModel.product = ProductSKU;
            InventoryModel.qtyonhand = 12;

            var response = MagentoInventoryDomain.Update(InventoryModel);

            Assert.IsAssignableFrom<bool>(response);

        }


    }
}
