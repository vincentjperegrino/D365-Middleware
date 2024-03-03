using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTI.Moo.Extensions.Magento.Model;
using System.Net.Http;
using KTI.Moo.Extensions.Magento.Exception;
using System.Text.Json;
using System.Collections;
using KTI.Moo.Extensions.Magento.Model.DTO.Products;
using KTI.Moo.Extensions.Magento.Helper;
using KTI.Moo.Extensions.Magento.Model.Helpers;
using Azure;
using System.IO;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Magento.Domain
{
    public class Product : IProduct<Model.Product>
    {
        private IMagentoService _service { get; init; }
        public const string APIDirectory = "/products";



        public Product(Config config)
        {
            this._service = new MagentoService(config);
        }

        public Product(Config config, IDistributedCache cache)
        {
            this._service = new MagentoService(config, cache);
        }

        public Product(string defaultDomain, string redisConnectionString, string username, string password)
        {

            this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);

        }

        public Product(IMagentoService service)
        {
            this._service = service;
        }



        public Model.Product Add(string FromDispatcherQueue)
        {
            if (string.IsNullOrWhiteSpace(FromDispatcherQueue))
            {
                throw new ArgumentException("Invalid FromDispatcherQueue", nameof(FromDispatcherQueue));
            }

            try
            {
                var path = APIDirectory;

                var method = "POST";

                var isAuthenticated = true;

                var productDetails = JsonConvert.DeserializeObject<dynamic>(FromDispatcherQueue);

                Model.DTO.Products.ForDispatcher.Add ProductAddModel = new();

                ProductAddModel.product = productDetails;

                ProductAddModel.saveOptions = false;

                var stringContent = GetContent(ProductAddModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnProductData = JsonConvert.DeserializeObject<Model.Product>(response, settings);

                if (string.IsNullOrWhiteSpace(ReturnProductData.sku))
                {

                    throw new System.Exception(response);

                }

                return ReturnProductData;
            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + APIDirectory;

                var classname = "MagentoProduct, Method: Add";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }

        }

        public Model.Product Add(Model.Product productDetails)
        {
            if (productDetails is null)
            {
                throw new ArgumentNullException(nameof(productDetails));
            }

            try
            {
                var path = APIDirectory;

                var method = "POST";

                var isAuthenticated = true;

                var CheckProductData = Get(productDetails.sku);

                if (!string.IsNullOrWhiteSpace(CheckProductData.sku))
                {
                    string errormesage = "Product already exist";

                    string domain = _service.DefaultURL + path;

                    string classname = "MagentoProduct, Method: Add";

                    throw new MagentoIntegrationException(domain, classname, errormesage);

                }


                Model.DTO.Products.Add ProductAddModel = new();

                ProductAddModel.product = productDetails;

                ProductAddModel.saveOptions = false;

                var stringContent = GetContent(ProductAddModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnProductData = JsonConvert.DeserializeObject<Model.Product>(response, settings);

                if (string.IsNullOrWhiteSpace(ReturnProductData.sku))
                {
                    throw new System.Exception(response);
                }

                return ReturnProductData;

            }
            catch (System.Exception ex)
            {

                var domain = _service.DefaultURL + APIDirectory;

                var classname = "MagentoProduct, Method: Add";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }


        }

        public Model.Product Get(long productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            try
            {
                var path = APIDirectory;
                var isAuthenticated = true;
                var method = "GET";

                var field = "entity_id";
                var value = productId.ToString();
                var condition_type = "eq";


                Dictionary<string, string> parameters = new();

                parameters.AddSearchParameter(field, value, condition_type);


                var response = _service.ApiCall(path, method, parameters, isAuthenticated);


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };


                var SearchResult = JsonConvert.DeserializeObject<Search>(response, settings);


                if (SearchResult.total_count == 0)
                {
                    var returnNothing = new Model.Product();
                    return returnNothing;

                }

                var ProductSkuResult = Get(SearchResult.values.FirstOrDefault().sku);

                return ProductSkuResult;

            }
            catch
            {
                return new Model.Product();
            }
        }

        public Model.Product Get(string productSku)
        {
            if (string.IsNullOrWhiteSpace(productSku))
            {
                throw new ArgumentException("Invalid productSku", nameof(productSku));
            }

            try
            {
                var path = APIDirectory + "/" + productSku;

                var isAuthenticated = true;
                var method = "GET";


                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnProductData = JsonConvert.DeserializeObject<Model.Product>(response, settings);

                return ReturnProductData;

            }
            catch
            {
                return new Model.Product();
            }



        }

        public bool Update(Model.Product productDetails)
        {
            if (productDetails is null)
            {
                throw new ArgumentNullException(nameof(productDetails));
            }

            if (string.IsNullOrWhiteSpace(productDetails.sku))
            {
                var domain = _service.DefaultURL;

                var resonse = domain + "MagentoProduct, Method: Update. Product SKU is required";

                throw new ArgumentException(resonse);

            }

            try
            {
                var path = APIDirectory + "/" + productDetails.sku;

                var isAuthenticated = true;
                var method = "PUT";


                Model.DTO.Products.Update ProductUpdateModel = new();

                ProductUpdateModel.product = productDetails;

                ProductUpdateModel.saveOptions = false;

                var stringContent = GetContent(ProductUpdateModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnProductData = JsonConvert.DeserializeObject<Model.Product>(response, settings);


                if (string.IsNullOrWhiteSpace(ReturnProductData.sku))
                {
                    throw new System.Exception(response);
                }

                return true;

            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + APIDirectory + "/" + productDetails.sku;

                var classname = "MagentoProduct, Method: Update";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }

        }

        public Model.Product Update(string FromDispatcherQueue, string sku)
        {

            if (string.IsNullOrWhiteSpace(FromDispatcherQueue))
            {
                throw new ArgumentException("Invalid FromDispatcherQueue", nameof(FromDispatcherQueue));
            }

            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("Invalid sku", nameof(sku));
            }

            try
            {

                var path = APIDirectory + "/" + sku;

                var isAuthenticated = true;
                var method = "PUT";

                var productDetails = JsonConvert.DeserializeObject<dynamic>(FromDispatcherQueue);

                Model.DTO.Products.ForDispatcher.Update ProductUpdateModel = new();

                ProductUpdateModel.product = productDetails;

                ProductUpdateModel.saveOptions = false;

                var stringContent = GetContent(ProductUpdateModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnProductData = JsonConvert.DeserializeObject<Model.Product>(response, settings);

                if (string.IsNullOrWhiteSpace(ReturnProductData.sku))
                {
                    throw new System.Exception(response);
                }

                return ReturnProductData;

            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + APIDirectory + "/" + sku;

                var classname = "MagentoProduct, Method: Update";

                throw new MagentoIntegrationException(domain, classname, ex.Message);

            }

        }


        public bool Delete(Model.Product productDetails)
        {
            if (productDetails is null)
            {
                throw new ArgumentNullException(nameof(productDetails));
            }

            if (string.IsNullOrWhiteSpace(productDetails.sku))
            {
                var domain = _service.DefaultURL;

                var resonse = domain + "MagentoProduct, Method: Delete. Product SKU is required";

                throw new ArgumentException(resonse);

            }

            try
            {
                var path = APIDirectory + "/" + productDetails.sku;
                var isAuthenticated = true;
                var method = "DEL";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnResponse = JsonConvert.DeserializeObject<bool>(response, settings);

                if (ReturnResponse == false)
                {
                    throw new System.Exception(response);
                }

                return ReturnResponse;

            }
            catch (System.Exception ex)
            {

                var domain = _service.DefaultURL + APIDirectory + "/" + productDetails.sku;

                var classname = "MagentoProduct, Method: Delete";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }

        }


        public Model.Product Upsert(Model.Product product)
        {

            var _product = Get(product.sku);

            if (_product == null || string.IsNullOrWhiteSpace(_product.sku))
            {
                return Add(product);
            }

            if (Update(product))
            {
                return Get(product.sku);
            }

            var domain = _service.DefaultURL;

            var classname = "MagentoProduct, Method: Upsert";

            throw new MagentoIntegrationException(domain, classname);

        }



        private static StringContent GetContent(object models)
        {

            var JsonSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore

            };

            var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

            return new StringContent(json, Encoding.UTF8, "application/json");

        }

        public bool AddImage(long productId, string externalImageUrl, bool primary = false)
        {
            throw new NotImplementedException();
        }

        public bool RemoveImage(long productId, int index)
        {
            throw new NotImplementedException();
        }

        public bool SetPrimaryImage(long productId, int index)
        {
            throw new NotImplementedException();
        }

        public Model.Product UpsertID(string FromDispatcherQueue, string sku)
        {
            var ExistingSKU = Get(sku);

            if (!string.IsNullOrWhiteSpace(ExistingSKU.sku))
            {
                return Update(FromDispatcherQueue, sku);
            }

            return Add(FromDispatcherQueue);
        }
    }
}
