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
    public class Product : OctoPOSBase
    {
        private readonly KTI.Moo.Extensions.OctoPOS.Domain.Product OctoPOSProduct;
        private readonly IDistributedCache _cache;

        public Product()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            OctoPOSProduct = new(CongfigTest, _cache);

        }

        [Fact]
        public void IFworkingGetProduct()
        {

            string ProductCode = "BUN-1234";

            var response = OctoPOSProduct.Get(ProductCode);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Product>(response);
        }

        [Fact]
        public void IFworkingGetListProduct()
        {

            int PageNumber = 1;

            var response = OctoPOSProduct.GetList(PageNumber);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.OctoPOS.Model.Product>>(response);
        }


        [Fact]
        public void IFworkingAddProductSimple()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Product ProductModel = new();


            ProductModel.productid = "MACB052-Sample393";
            ProductModel.description = "KTICreations";
            ProductModel.price = (decimal)99.99;


            var response = OctoPOSProduct.Add(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Product>(response);
        }


        [Fact]
        public void IFworkingAddProduct()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Product ProductModel = new();


            ProductModel.productid = "MACB052-Sample38";
            ProductModel.sku = "MACB052-Sample38";
            ProductModel.description = "KTICreations";
            ProductModel.price = (decimal)39.39;
            ProductModel.ProductBarcode = "MACB052-Sample38";
            ProductModel.category = "Espresso";
            ProductModel.size = "L";
            ProductModel.suppliername = "SUPPLIERko bronze";

            ProductModel.ProductAttributes.Add(new()
            {
                AttributeType = ProductAttributeTypeHelper.Color,
                AttributeCode = "Golden Silver bronze",
                AttributeName = "Golden Silver bronze",
                AttributeStatus = 1

            });
            ProductModel.ProductAttributes.Add(new()
            {
                AttributeType = ProductAttributeTypeHelper.Brand,
                AttributeCode = "NESPRESSO bronze",
                AttributeName = "NESPRESSO bronze",
                AttributeStatus = 1

            });


            var response = OctoPOSProduct.Add(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Product>(response);
        }


        [Fact]
        public void IFworkingUpdateProduct()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Product ProductModel = new();


            ProductModel.productid = "MACB052-Sample391";
            ProductModel.sku = "MACB052-Sample391";
            ProductModel.description = "KTICreations";
            ProductModel.price = (decimal)99.99;
            ProductModel.category = "Coffee";


            var response = OctoPOSProduct.Upsert(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Product>(response);
        }





        [Fact]
        public void IFworkingUpdateSerializeProduct()
        {

            string ProductCode = "ITAERO002A";

            var ProductModel = OctoPOSProduct.Get(ProductCode);

            ProductModel.HasSerial = true;

            var response = OctoPOSProduct.Update(ProductModel);

            Assert.IsAssignableFrom<bool>(response);
        }


        [Fact]
        public void IFworkingUpsertProduct()
        {

            string ProductCode = "ITAERO002A";

            var ProductModel = OctoPOSProduct.Get(ProductCode);

            var response = OctoPOSProduct.Upsert(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Product>(response);
        }







    }
}
