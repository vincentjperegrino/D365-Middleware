using KTI.Moo.Extensions.Magento.Domain;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace TestMagento.Domain
{
    public class Products : MagentoBase
    {

        private readonly IDistributedCache _cache;

        public Products()
        {
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

        }

        [Fact]
        public void GetProductID_Working()
        {

            long ProductID = 2514;


            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(Stagingconfig, _cache);


            var response = MagentoProduct.Get(ProductID);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Product>(response);
        }

        [Fact]
        public void GetProductSKU_Working()
        {

            var ProductSKU = "COFB099";


            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(Stagingconfig, _cache);


            var response = MagentoProduct.Get(ProductSKU);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Product>(response);
        }


        [Fact]
        public async void AddProduct_Working()
        {
            KTI.Moo.Extensions.Magento.Model.Product ProductModel = new();
            //crmProduct.kti_sku
            ProductModel.sku = "SampleSKU1234";
            //crmProduct.name
            ProductModel.name = "SampleSKU1234";
            //?
            ProductModel.attribute_set_id = 4;
            //crmProduct.listprice
            ProductModel.price = (decimal)1;
            //crmProduct.statuscode
            ProductModel.statuscode = 1;
            //crmProduct.kti_mooenabled
            ProductModel.visibility = 4;
            //?
            ProductModel.type_id = "simple";
            //crmProduct.kti_weight
            ProductModel.stockweight = (decimal)0.5;

            //ProductModel.custom_attributes = new();
            //ProductModel.custom_attributes.Add(new() { attribute_code = "aromatic_profile", ValueInt = 1 });
            //ProductModel.custom_attributes.Add(new() { attribute_code = "description", ValueString = "The Champ Tee keeps you cool and dry while you do your thing. Let everyone know who you are by adding your name on the back for only $10." });

            //crmProduct.description
            ProductModel.description = "SampleSKU1234";
            //to be continued
            ProductModel.extension_attributes = new();
            ProductModel.extension_attributes.category = new()
            {
                //crm 1 parent only
                new()
                {
                    //crmProduct.kti_moosequence
                    position = 0,
                    //crmProduct.parentproductid
                    CategoryId = 3
                }
            };

            var image = await GetImageAsBase64Url("https://ktimoostorage.blob.core.windows.net/ncci-dev/3efa56d8-994e-ed11-bba3-000d3a8562a7_pngwing.com.png?si=MooRead&spr=https&sv=2021-06-08&sr=c&sig=yKThooN0IrTIDohq5Sp8xuyDHZeCjdWMgesmERZF5pc%3D");

            ProductModel.media_gallery_entries = new()
            {
                new KTI.Moo.Extensions.Magento.Model.MediaGallery()
                {
                    media_type ="image",
                    position = 1,
                    disabled = false,
                    types  = new string[]{ "image"},
                    content = new()
                    {
                        base64_encoded_data = image,
                        name = "SampleSKU1234.png",
                        type = "image/png"
                     }
                    

                }

            };

            //ProductModel.extension_attributes.stock_item = new();
            //ProductModel.extension_attributes.stock_item.is_in_stock = true;
            //ProductModel.extension_attributes.stock_item.qtyonhand = 1;
            //ProductModel.extension_attributes.stock_item.ManageStock.config_value = true;
            //ProductModel.extension_attributes.stock_item.ManageStock.is_config_enable = true;

            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(Stagingconfig);
            var response = MagentoProduct.Upsert(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Product>(response);
        }

        [Fact]
        public void UpdateProduct_Working()
        {

            KTI.Moo.Extensions.Magento.Model.Product ProductModel = new();
            ProductModel.sku = "Chocoloco1333441";
            ProductModel.name = "Chocoloco1333441";
            ProductModel.description = "Let everyone know who you are by adding your name on the back for only $10.";
            ProductModel.attribute_set_id = 4;
            ProductModel.price = (decimal)40;
            ProductModel.statuscode = 1;
            ProductModel.visibility = 1;
            ProductModel.type_id = "simple";
            ProductModel.stockweight = (decimal)0.5;
            ProductModel.extension_attributes = new();
            ProductModel.extension_attributes.category = new()
            {
                new()
                {
                    position = 0,
                    CategoryId = 11
                },
                new()
                {
                    position = 1,
                    CategoryId = 12
                }
            };

            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(defaultURL, redisConnectionString, username, password);

            var response = MagentoProduct.Update(ProductModel);

            Assert.True(response);
        }


        [Fact]
        public void UpsertProduct_Working()
        {

            KTI.Moo.Extensions.Magento.Model.Product ProductModel = new();
            ProductModel.sku = "Chocoloco133344123";
            ProductModel.name = "Chocoloco133344123";
            ProductModel.description = "Let everyone know who you are by adding your name on the back for only $10.";
            ProductModel.attribute_set_id = 4;
            ProductModel.price = (decimal)40;
            ProductModel.statuscode = 1;
            ProductModel.visibility = 1;
            ProductModel.type_id = "simple";
            ProductModel.stockweight = (decimal)0.5;
            ProductModel.extension_attributes = new();
            ProductModel.extension_attributes.category = new()
            {
                new()
                {
                    position = 0,
                    CategoryId = 11
                }
            };

            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(defaultURL, redisConnectionString, username, password);

            var response = MagentoProduct.Upsert(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Product>(response);
        }



        [Fact]
        public void DeleteProduct_Working()
        {

            KTI.Moo.Extensions.Magento.Model.Product ProductModel = new();
            ProductModel.sku = "Chocoloco1333441";

            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(defaultURL, redisConnectionString, username, password);

            var response = MagentoProduct.Delete(ProductModel);

            Assert.True(response);
        }



        [Fact]
        public void UpsertProductWithGet_Working()
        {


            KTI.Moo.Extensions.Magento.Domain.Product MagentoProduct = new(Stagingconfig);


            var ProductModel = MagentoProduct.Get("PAR0091");
            ProductModel.visibility = 4;

            ProductModel.extension_attributes.category.Add(new()
            {
                CategoryId = 7,
                position = 0
            });


            var response = MagentoProduct.Upsert(ProductModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Product>(response);
        }

        public async static Task<string> GetImageAsBase64Url(string url)
        {
            var credentials = new NetworkCredential();
            using (var handler = new HttpClientHandler { Credentials = credentials })
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.GetByteArrayAsync(url);
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
