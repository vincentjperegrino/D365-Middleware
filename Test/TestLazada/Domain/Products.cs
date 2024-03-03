using Xunit;
using KTI.Moo.Extensions.Lazada.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace TestLazada.Domain
{
    public class Products : Base.LazadaBase
    {

        private KTI.Moo.Extensions.Lazada.Domain.Product ProductDomain;
        private readonly IDistributedCache _cache;
        private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain;


        public Products()
        {


            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

            var provider = services.BuildServiceProvider();

            _cache = provider.GetService<IDistributedCache>();

            var ChannelManagement = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannel();

            var ChannelManagementCached = new KTI.Moo.CRM.Domain.ChannelManagement.SalesChannelCached(ChannelManagement, _cache);

            var LazadaClientToken = new KTI.Moo.Extensions.Lazada.Domain.ClientToken(CongfigTNCCI);

            clientTokenDomain = new KTI.Moo.Extensions.Lazada.Domain.Queue.ClientToken_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel>(LazadaClientToken, ChannelManagementCached);
  
            //   ClientTokenLazada = clientTokenDomain.Refresh(ClientTokenLazada , ChannelConfig);

        }


        [Fact]
        public void TestGetProducts_SKU_IFworking()
        {

            clientTokenDomain.CompanyID = 3389;

            var ChannelConfig = clientTokenDomain.GetbyLazadaSellerID(CongfigTNCCI.SellerId);

            var ClientTokenLazada = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

            ProductDomain = new(CongfigTNCCI, ClientTokenLazada);

            string SKU = "ITESSE101A";

            var response = ProductDomain.Get(SKU);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Lazada.Model.Product>(response);

        }



        [Fact]
        public void TestGetProductsList_SKU_IFworking()
        {

            var SKUList = new List<string>()
            {
               "COF0094",
               "COF0095"

            };

            var response = ProductDomain.GetProductPrice(SKUList);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Lazada.Model.Product>>(response);

        }









        [Fact]
        public void TestAddImagePrimaryIFworking()
        {

            string SKU = "You know You know";

            var ProductModel = ProductDomain.Get(SKU);

            long ProductIDs = ProductModel.item_id;

            bool primary = true;

            //string urls = "https://static.wikia.nocookie.net/agk/images/f/f1/UgandanKnuckles.png/revision/latest/scale-to-width-down/1000?cb=20180707171405";
            //string urls = "https://www.pngitem.com/pimgs/m/155-1551088_sanic-tribute-by-hetaliashero-on-deviant-transparent-sanic.png";
            string urls = "https://i.kym-cdn.com/photos/images/original/001/431/201/40f.png";

            var response = ProductDomain.AddImage(productId: ProductIDs, url: urls, primary: primary);

            Assert.IsAssignableFrom<bool>(response);

        }

        [Fact]
        public void TestAddImageNotPrimaryIFworking()
        {

            string SKU = "You know the waes";

            var ProductModel = ProductDomain.Get(SKU);

            long ProductIDs = ProductModel.item_id;

            bool primary = false;

            //string urls = "https://static.wikia.nocookie.net/agk/images/f/f1/UgandanKnuckles.png/revision/latest/scale-to-width-down/1000?cb=20180707171405";
            //string urls = "https://www.pngitem.com/pimgs/m/155-1551088_sanic-tribute-by-hetaliashero-on-deviant-transparent-sanic.png";
            string urls = "https://i.kym-cdn.com/photos/images/original/001/431/201/40f.png";

            var response = ProductDomain.AddImage(productId: ProductIDs, url: urls, primary: primary);

            Assert.IsAssignableFrom<bool>(response);


        }


        [Fact]
        public void TestAddProduct()
        {


            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new(categoryId: 25150);
            // or product.primary_category = 25150;
            product.name = "Test For images";
            product.description = "Images uploaded";
            product.brand = "No Brand";

            Sku sku = new();
            sku.SellerSku = "Test122";
            sku.quantity = 10;
            sku.price = 10;
            sku.package_height = 10;
            sku.package_length = 10;
            sku.package_width = 10;
            sku.package_weight = 10;
            product.skus = new() { sku };

            product.images.Add(new()
            {
                primary = false,
                url = "https://static.wikia.nocookie.net/agk/images/f/f1/UgandanKnuckles.png/revision/latest/scale-to-width-down/1000?cb=20180707171405"
            });

            product.images.Add(new()
            {
                primary = true,
                url = "https://i.kym-cdn.com/photos/images/original/001/431/201/40f.png"
            });


            var response = ProductDomain.Add(product);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Lazada.Model.Product>(response);

        }



        [Fact]
        public void TestUpsertProduct()
        {

            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new(categoryId: 25150);
            // or product.primary_category = 25150;
            product.name = "Test For images";
            product.description = "Images uploaded";
            product.brand = "No Brand";

            Sku sku = new();
            sku.SellerSku = "Test122";
            sku.quantity = 10;
            sku.price = 10;
            sku.package_height = 10;
            sku.package_length = 10;
            sku.package_width = 10;
            sku.package_weight = 10;
            product.skus = new() { sku };

            product.images.Add(new()
            {
                primary = false,
                url = "https://static.wikia.nocookie.net/agk/images/f/f1/ugandanknuckles.png/revision/latest/scale-to-width-down/1000?cb=20180707171405"
            });

            //product.images.Add(new()
            //{
            //    primary = false,
            //    url = "https://i.kym-cdn.com/photos/images/original/001/431/201/40f.png"
            //});

            //product.images.Add(new()
            //{
            //    primary = true,
            //    url = "https://i.ytimg.com/vi/U3i8B1xAjA0/maxresdefault.jpg"
            //});

            //product.images.Add(new()
            //{
            //    primary = true,
            //    url = "https://wallpaperaccess.com/full/155603.jpg"
            //});

            ////large image
            //// lazada code 303
            //product.images.Add(new()
            //{
            //    primary = true,
            //    url = "https://upload.wikimedia.org/wikipedia/commons/3/3d/LARGE_elevation.jpg"
            //});

            ////large image
            //// lazada code 302 max dimension
            //product.images.Add(new()
            //{
            //    primary = true,
            //    url = "https://wallpaperaccess.com/full/2442857.jpg"
            //});


            var response = ProductDomain.Upsert(product);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Lazada.Model.Product>(response);

        }


        [Fact]
        public void TestSetPrimaryImageProduct()
        {

            string SKU = "You know You know";

            var ProductModel = ProductDomain.Get(SKU);

            var SelectedIndex = 2;

            var response = ProductDomain.SetPrimaryImage(ProductModel.item_id, SelectedIndex);

            Assert.IsAssignableFrom<bool>(response);

        }


        [Fact]
        public void TestRemoveImageProduct()
        {


            string SKU = "Test122";

            var ProductModel = ProductDomain.Get(SKU);

            var SelectedIndex = 0;

            var response = ProductDomain.RemoveImage(ProductModel.item_id, SelectedIndex);

            Assert.IsAssignableFrom<bool>(response);

        }


        [Fact]
        public void TestAddVariantImageProduct()
        {


            string SKU = "39G";

            var ProductModel = ProductDomain.Get(SKU);

            //string urls = "https://static.wikia.nocookie.net/agk/images/f/f1/UgandanKnuckles.png/revision/latest/scale-to-width-down/1000?cb=20180707171405";
            //string urls = "https://www.pngitem.com/pimgs/m/155-1551088_sanic-tribute-by-hetaliashero-on-deviant-transparent-sanic.png";
            // string urls = "https://i.kym-cdn.com/photos/images/original/001/431/201/40f.png";
            string urls = "https://i.ytimg.com/vi/U3i8B1xAjA0/maxresdefault.jpg";


            bool primary = true;

            var response = ProductDomain.AddVariantImage(ProductModel.item_id, SKU, urls, primary);

            Assert.IsAssignableFrom<bool>(response);

        }

        [Fact]
        public void TestSetVariantPrimaryImageProduct()
        {


            string SKU = "39G";

            var ProductModel = ProductDomain.Get(SKU);

            var SelectedIndex = 1;

            var response = ProductDomain.SetVariantPrimaryImage(ProductModel.item_id, SKU, SelectedIndex);

            Assert.IsAssignableFrom<bool>(response);

        }

        [Fact]
        public void TestRemoveVariantImageProduct()
        {


            string SKU = "39G";

            var ProductModel = ProductDomain.Get(SKU);

            var SelectedIndex = 1;

            var response = ProductDomain.RemoveVariantImage(ProductModel.item_id, SKU, SelectedIndex);

            Assert.IsAssignableFrom<bool>(response);

        }



        [Fact]
        public void TestUpdateSellableQuantity()
        {

            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new()
            {
                skus = new List<Sku>()
                {
                    new Sku()
                    {
                    SellerSku = "123INTERNAL_SKU1",
                    quantity = 10,
                    }
                }
            };


            var response = ProductDomain.UpdateSellableQuantity(product);

            Assert.True(response);

        }

        [Fact]
        public void TestUpdatePrice()
        {

            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new()
            {
                skus = new List<Sku>()
                {
                    new Sku()
                    {
                    SellerSku = "123INTERNAL_SKU1",
                    price = 130,
                    }
                }
            };

            var response = ProductDomain.UpdatePrice(product);

            Assert.True(response);

        }

        [Fact]
        public void TestUpdatePriceQuantity()
        {

            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new()
            {
                skus = new List<Sku>()
                {
                    new Sku()
                    {
                    SellerSku = "123INTERNAL_SKU1",
                    price = 10,
                    quantity = 10,
                    }
                }
            };


            var response = ProductDomain.UpdatePriceQuantity(product);

            Assert.True(response);

        }


        [Fact]
        public void TestUpdatePriceWithSales()
        {

            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new()
            {
                skus = new List<Sku>()
                {
                    new Sku()
                    {
                    SellerSku = "123INTERNAL_SKU1",
                    price = 1000,
                    }
                }
            };


            var SalesPrice = (decimal)900.0;

            var SaleStart = DateTime.Now.AddMonths(-1);
            var SaleEnd = DateTime.Now.AddMonths(1);

            var response = ProductDomain.UpdatePriceWithSales(product, SalesPrice, SaleStart, SaleEnd);

            Assert.True(response);

        }




        [Fact]
        public void TestUpsertProductNoQuantity()
        {

            // create basic product
            KTI.Moo.Extensions.Lazada.Model.Product product = new(categoryId: 25150);
            // or product.primary_category = 25150;
            product.name = "Test Wahducts1";
            product.description = "Images uploaded   <HTML><H1> this is H1</h1></HTML> <H2> this is H2</h2>";
            product.brand = "No Brand";

            Sku sku = new();
            sku.SellerSku = "Test12233";
            sku.price = 10;
            sku.package_height = 10;
            sku.package_length = 10;
            sku.package_width = 10;
            sku.package_weight = 10;
            product.skus = new() { sku };

            product.images.Add(new()
            {
                primary = false,
                url = "https://i.ytimg.com/vi/U3i8B1xAjA0/maxresdefault.jpg"
            });

            var response = ProductDomain.Upsert(product);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Lazada.Model.Product>(response);

        }


    }
}
