using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using KTI.Moo.Extensions.Lazada.Exception;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Lazada.Domain
{
    public class Product : Core.Domain.IProduct<Model.Product>
    {
        private Service.ILazopService _service { get; init; }

        public Product(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public Product(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public Product(Service.ILazopService service)
        {
            this._service = service;
        }

        public Product(Service.Queue.Config config, IDistributedCache cache)
        {
            this._service = new LazopService(config, cache);
        }        
        
        public Product(Service.Queue.Config config, ClientTokens clientTokens)
        {
            this._service = new LazopService(config, clientTokens);
        }




        private Model.Product _get(long? productId = null, string productSku = null)
        {
            if (!productId.HasValue && string.IsNullOrWhiteSpace(productSku))
                throw new ArgumentNullException("Either a lazada ItemId ('product id') or a SellerSku ('product number') should be provided.");

            var parameters = new Dictionary<string, string>();

            if (productId.HasValue)
                parameters.Add("item_id", productId.ToString());

            if (!string.IsNullOrWhiteSpace(productSku))
                parameters.Add("seller_sku", productSku);

            var response = this._service.AuthenticatedApiCall("/product/item/get", parameters, "GET");
            var productJson = JsonDocument.Parse(response).RootElement;

            var attributes = productJson.GetProperty("data").GetProperty("attributes");

            Model.Product product = new(productJson.GetProperty("data").ToString());

            return product;
        }


        public List<Model.Product> GetProductPrice(List<string> skus)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("sku_seller_list", JsonConvert.SerializeObject(skus));

            var response = this._service.AuthenticatedApiCall("/products/get", parameters, "GET");
            var productListJson = JsonDocument.Parse(response).RootElement;

            List<string> ListOfProducts = productListJson.GetProperty("data").GetProperty("products").EnumerateArray().Select(a => a.ToString()).ToList();

            var productPriceList = new List<Model.Product>();

            foreach (var productjson in ListOfProducts)
            {
                Model.Product productmodel = new(productjson.ToString());

                foreach (var products in productmodel.skus)
                {
                    productPriceList.Add(new()
                    {
                        productid = products.SellerSku,
                        price = products.price
                    });
                }
            }

            return productPriceList;

        }


        public List<Model.Product> GetProduct(List<string> skus)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("sku_seller_list", JsonConvert.SerializeObject(skus));

            var response = this._service.AuthenticatedApiCall("/products/get", parameters, "GET");
            var productListJson = JsonDocument.Parse(response).RootElement;

            List<string> ListOfProducts = productListJson.GetProperty("data").GetProperty("products").EnumerateArray().Select(a => a.ToString()).ToList();

            var productPriceList = ListOfProducts.Select(productjson => 
            {
                Model.Product productmodel = new(productjson.ToString());
                return productmodel;

            }).ToList();

            return productPriceList;

        }



        public Model.Product Get(long productId)
        {
            return this._get(productId: productId);
        }

        public Model.Product Get(string productSku)
        {
            return this._get(productSku: productSku);
        }

        public Model.Product Add(Model.Product product)
        {

            product.images = product.images.Select(img => new Images()
            {
                primary = img.primary,
                url = migrateImage(img.url)

            }).ToList();


            if (product.images.Count > 1)
            {
                product.images = ChangeIndexOfPrimary(product.images);
            }


            XElement payload = new("Request", product.ToAddXML());

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/create", parameters, "POST")).RootElement.GetProperty("data");

            // update and return product instance with lazada-generated product fields
            product.item_id = response.GetProperty("item_id").GetInt64();
            foreach (JsonElement e in response.GetProperty("sku_list").EnumerateArray())
            {
                var index = product.skus.FindIndex(i => i.SellerSku == e.GetProperty("seller_sku").ToString());

                product.skus[index].ShopSku = e.GetProperty("shop_sku").ToString();
                product.skus[index].SkuId = e.GetProperty("sku_id").GetInt64();
            }

            return product;
        }

        public bool Update(Model.Product product)
        {
            // Try to fetch ItemId from Lazada if missing.
            // Will throw an error if the product doesn't exist.
            if (product.item_id <= 0)
            {
                var tempProd = this._get(productSku: product.skus[0].SellerSku);
                product.item_id = tempProd.item_id;
            }

            if (product.images is not null || product.images.Count > 0)
            {
                product.images = product.images.Select(img => new Images()
                {
                    primary = img.primary,
                    url = migrateImage(img.url)

                }).ToList();

                if (product.images.Count > 1)
                {
                    product.images = ChangeIndexOfPrimary(product.images);
                }
            }


            XElement payload = new("Request",
                product.ToUpdateXML()
            );

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/update", parameters, "POST"));

            return response.RootElement.GetProperty("code").ValueEquals("0");
        }

        public bool UpdatePriceQuantity(Model.Product product)
        {
            // Try to fetch ItemId from Lazada if missing.
            // Will throw an error if the product doesn't exist.
            //if (product.item_id <= 0)
            //{
            //    var tempProd = this._get(productSku: product.skus[0].SellerSku);
            //    product.item_id = tempProd.item_id;
            //}

            XElement payload = new XElement("Request",
                product.ToUpdatePriceQuantityXML()
            );

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/price_quantity/update", parameters, "POST"));

            return response.RootElement.GetProperty("code").ValueEquals("0");
        }

        public bool UpdatePrice(Model.Product product)
        {
            // Try to fetch ItemId from Lazada if missing.
            // Will throw an error if the product doesn't exist.
            //if (product.item_id <= 0)
            //{
            //    var tempProd = this._get(productSku: product.skus[0].SellerSku);
            //    product.item_id = tempProd.item_id;
            //}

            XElement payload = new XElement("Request",
                product.ToUpdatePriceXML()
            );

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/price_quantity/update", parameters, "POST"));

            return response.RootElement.GetProperty("code").ValueEquals("0");
        }

        public bool UpdatePriceWithSales(Model.Product product, decimal SalesPrice, DateTime SaleStartDate, DateTime SalesEndDate)
        {
            // Try to fetch ItemId from Lazada if missing.
            // Will throw an error if the product doesn't exist.
            //if (product.item_id <= 0)
            //{
            //    var tempProd = this._get(productSku: product.skus[0].SellerSku);
            //    product.item_id = tempProd.item_id;
            //}

            XElement payload = new XElement("Request",
                product.ToUpdatePriceSalesDateRangeXML(SalesPrice, SaleStartDate, SalesEndDate)
            );

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/price_quantity/update", parameters, "POST"));

            return response.RootElement.GetProperty("code").ValueEquals("0");
        }


        public bool UpdateSellableQuantity(Model.Product product)
        {

            XElement payload = new XElement("Request",
                product.ToUpdateSellableQuantityXML()
            );

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/stock/sellable/update", parameters, "POST"));

            return response.RootElement.GetProperty("code").ValueEquals("0");
        }



        // Delete a product
        public bool Delete(Model.Product product)
        {
            List<string> skus = product.skus.Select(s => $"SkuId_{product.item_id}_{s.SkuId}").ToList();

            var parameters = new Dictionary<string, string>
            {
                {"seller_sku_list", JsonConvert.SerializeObject(skus)}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/remove", parameters, "POST"));

            return response.RootElement.GetProperty("code").Equals("0");
        }

        // Deactivate a product
        public bool Deactivate(Model.Product product)
        {
            XElement apiRequestBody = new("Request",
                new XElement("Product",
                    new XElement("ItemId", product.item_id)
                )
            );

            var parameters = new Dictionary<string, string>
            {
                {"apiRequestBody", apiRequestBody.ToString()}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/product/deactivate", parameters, "POST"));

            return response.RootElement.GetProperty("code").ValueEquals("0");
        }

        // Update product if it exists, create if it doesn't.
        public Model.Product Upsert(Model.Product product)
        {
            long ItemId = 0;

            try
            {
                ItemId = this._get(productSku: product.skus[0].SellerSku).item_id;
            }
            catch
            {


            }

            if (ItemId != 0)
            {
                this.Update(product);
                return product;

            }

            return this.Add(product);
        }

        /// <summary>
        /// Returns the attributes for a product and its skus.
        /// </summary>
        /// <param name="categoryId">The ID of the category to get the attributes for.</param>
        /// <param name="languageCode">The language code. Defaults to English (en_US).</param>
        /// <returns></returns>
        public List<Model.Attribute> GetAttributes(int categoryId, string languageCode = "en_US")
        {
            var parameters = new Dictionary<string, string>
            {
                {"primary_category_id", categoryId.ToString()},
                {"language_code", languageCode}
            };

            var response = _service.ApiCall("/category/attributes/get", parameters, "GET");
            var attributesJson = JsonDocument.Parse(response).RootElement.GetProperty("data").EnumerateArray();

            List<Model.Attribute> attributes = new();
            foreach (JsonElement a in attributesJson)
            {
                Model.Attribute attribute = new()
                {
                    Id = a.GetProperty("id").GetInt32(),
                    Name = a.GetProperty("name").ToString(),
                    Label = a.GetProperty("label").ToString(),
                    AttributeType = a.GetProperty("attribute_type").ToString(),
                    InputType = a.GetProperty("input_type").ToString(),
                    IsKeyProp = a.GetProperty("advanced").GetProperty("is_key_prop").GetInt16() == 1,
                    IsSaleProp = a.GetProperty("is_sale_prop").GetInt16() == 1,
                    IsMandatory = a.GetProperty("is_mandatory").GetInt16() == 1,
                    Options = a.GetProperty("options").EnumerateArray().Select(option => new Model.AttributeOption()
                    {
                        Id = option.GetProperty("id").GetInt32(),
                        Name = option.GetProperty("name").ToString(),
                        EnName = option.GetProperty("en_name").ToString()
                    }).ToList()
                };

                attributes.Add(attribute);
            }

            return attributes;
        }

        public bool AddImage(long productId, string url, bool primary = false)
        {
            Images lazImage = new() { primary = primary, url = migrateImage(url) };

            var product = this.Get(productId);
            if (primary)
            {
                product.images.Insert(0, lazImage);
            }
            else
            {
                product.images.Add(lazImage);
            }

            return this.Update(product);
        }

        public bool RemoveImage(long productId, int index)
        {
            if (index < 0)
            {
                return false;
            }

            var product = this.Get(productId);


            if (index > (product.images.Count - 1))
            {
                return false;
            }

            product.images.RemoveAt(index);

            return this.Update(product);

        }

        public bool SetPrimaryImage(long productId, int index)
        {

            if (index < 0)
            {
                return false;
            }

            var product = this.Get(productId);


            if (index > (product.images.Count - 1))
            {
                return false;
            }

            int FirstIndex = 0;

            product.images = ChangeIndex(image: product.images
                                       , OldIndex: index
                                       , NewIndex: FirstIndex);

            product.images[FirstIndex].primary = true;

            return this.Update(product);
        }

        public bool AddVariantImage(long productId, string sku, string url, bool primary = false)
        {
            Images lazImage = new() { primary = primary, url = migrateImage(url) };
            var product = this.Get(sku);

            var productSku = product.skus.Where(s => s.SellerSku.Equals(sku)).First();
            if (primary)
            {
                productSku.Images.Insert(0, lazImage);
            }
            else
            {
                productSku.Images.Add(lazImage);
            }

            return this.Update(product);
        }

        public bool RemoveVariantImage(long productId, string sku, int index)
        {

            if (index < 0)
            {
                return false;
            }

            var product = this.Get(sku);

            var productSku = product.skus.Where(s => s.SellerSku.Equals(sku)).First();

            if (index > (productSku.Images.Count - 1))
            {

                return false;

            }


            productSku.Images.RemoveAt(index);

            return this.Update(product);

        }

        public bool SetVariantPrimaryImage(long productId, string sku, int index)
        {

            if (index < 0)
            {
                return false;
            }

            var product = this.Get(sku);
            var productSku = product.skus.Where(s => s.SellerSku.Equals(sku)).First();

            if (index > (productSku.Images.Count - 1))
            {

                return false;

            }

            int FirstIndex = 0;

            productSku.Images = ChangeIndex(image: productSku.Images
                           , OldIndex: index
                           , NewIndex: FirstIndex);

            productSku.Images[FirstIndex].primary = true;


            return this.Update(product);

        }

        private string migrateImage(string imageUrl)
        {
            XElement payload = new("Request",
                new XElement("Image",
                    new XElement("Url", imageUrl)
                )
            );

            var parameters = new Dictionary<string, string>
            {
                {"payload", payload.ToString()}
            };

            var response = _service.AuthenticatedApiCall("/image/migrate", parameters, "POST");
            return JsonDocument.Parse(response).RootElement.GetProperty("data").GetProperty("image").GetProperty("url").GetString();
        }


        private static List<Images> ChangeIndexOfPrimary(List<Images> image)
        {

            int PrimaryIndex = image.FindIndex(img => img.primary == true);
            int FirstIndex = 0;

            return ChangeIndex(image: image, OldIndex: PrimaryIndex, NewIndex: FirstIndex);

        }


        private static List<Images> ChangeIndex(List<Images> image, int OldIndex, int NewIndex)
        {
            Images OldItemIndex = image[OldIndex];

            image.RemoveAt(OldIndex);
            image.Insert(NewIndex, OldItemIndex);

            return image;

        }
    }
}