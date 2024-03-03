using Azure;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Exception;
using KTI.Moo.Extensions.OctoPOS.Model;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Domain
{
    public class Product : IProduct<Model.Product>
    {
        private readonly IOctoPOSService _service;
        public const string APIDirectory = "/product";
        private readonly string _companyid;


        public Product(Config config, IDistributedCache cache)
        {
            this._service = new OctoPOSService(cache, config);
            _companyid = config.companyid;
        }

        public Product(IOctoPOSService service)
        {
            this._service = service;
        }

        public Model.Product Get(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                throw new ArgumentException("Invalid productCode", nameof(productCode));
            }

            try
            {
                var path = APIDirectory + "/" + productCode;

                var isAuthenticated = true;
                var method = "GET";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnProductData = JsonConvert.DeserializeObject<Model.Product>(response, settings);

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0)
                {
                    ReturnProductData.companyid = companyid;
                }

                return ReturnProductData;

            }
            catch
            {
                return new Model.Product();
            }
        }


        public List<Model.Product> GetList(int currentPage)
        {
            if (currentPage < 1)
            {
                string message = "GetList: currentPage can't be below zero.";

                throw new ArgumentException(message);

            }

            try
            {
                Model.DTO.Products.Search SearchData = GetSearchList(currentPage);

                var ReturnList = SearchData.values;

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0 && ReturnList is not null && ReturnList.Count > 0)
                {
                    ReturnList.Select(product =>
                    {
                        product.companyid = companyid;

                        return product;

                    }).ToList();
                }
                return ReturnList;
            }
            catch
            {
                return new List<Model.Product>();
            }
        }

        private Model.DTO.Products.Search GetSearchList(int currentPage)
        {
            if (currentPage < 1)
            {
                string message = "GetSearchList: currentPage can't be below zero.";

                throw new ArgumentException(message);
            }
            try
            {
                string path = APIDirectory + "/list";

                bool isAuthenticated = true;
                string method = "GET";

                Dictionary<string, string> parameters = new()
                {
                   { "page", currentPage.ToString() }
                };

                var response = _service.ApiCall(path, method, parameters, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                Model.DTO.Products.Search SearchData = JsonConvert.DeserializeObject<Model.DTO.Products.Search>(response, settings);

                return SearchData;
            }
            catch
            {
                return new Model.DTO.Products.Search();
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
                var ProductChecker = Get(productDetails.productid);

                if (string.IsNullOrWhiteSpace(productDetails.productid))
                {
                    string errormesage = "Add: Invalid Productid";

                    throw new System.Exception(errormesage);
                }

                if (!string.IsNullOrWhiteSpace(ProductChecker.productid))
                {
                    string errormesage = "Productid already exist";
                    throw new System.Exception(errormesage);
                }

                var ReturnProduct = AddUpdateProduct(productDetails, Method: "Add");

                return ReturnProduct;
            }
            catch (System.Exception ex)
            {

                string domain = _service.DefaultURL + "add";

                string classname = "OctoPOSProduct, Method: Add";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }

        }

        public bool Update(Model.Product productDetails)
        {
            if (productDetails is null)
            {
                throw new ArgumentNullException(nameof(productDetails));
            }
            try
            {
                var ProductChecker = Get(productDetails.productid);

                if (string.IsNullOrWhiteSpace(ProductChecker.productid))
                {
                    string errormesage = "Product does not exist";

                    throw new System.Exception(errormesage);
                }

                var returnedProudctData = AddUpdateProduct(productDetails, Method: "Update");

                if (string.IsNullOrWhiteSpace(returnedProudctData.productid))
                {
                    return false;
                }

                return true;

            }
            catch (System.Exception ex)
            {

                string domain = _service.DefaultURL + "add";

                string classname = "OctoPOSProduct, Method: Update";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);

            }


        }

        public Model.Product Upsert(Model.Product productDetails)
        {
            return AddUpdateProduct(productDetails: productDetails, Method: "Upsert");
        }

        private Model.Product AddUpdateProduct(Model.Product productDetails, string Method)
        {
            try
            {
                string path = APIDirectory + "/add";

                bool isAuthenticated = true;
                string method = "POST";

                var stringContent = GetContent(productDetails);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                if (response != "OK")
                {
                    throw new System.Exception(response);
                }

                var ProductData = Get(productDetails.productid);

                return ProductData;
            }
            catch (System.Exception ex)
            {
                string domain = _service.DefaultURL + APIDirectory + "/add";

                string classname = "OctoPOSProduct, Method: " + Method;

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);

            }

        }


        public Model.Product Upsert(string FromDispatcherQueue, string sku)
        {
            return AddUpdateProduct(FromDispatcherQueue, sku, Method: "Upsert");
        }

        private Model.Product AddUpdateProduct(string FromDispatcherQueue, string sku, string Method)
        {
            try
            {

                string path = APIDirectory + "/add";

                bool isAuthenticated = true;
                string method = "POST";

                var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                if (response != "OK")
                {
                    throw new System.Exception(response);
                }

                var ProductData = Get(sku);
                return ProductData;

            }

            catch (System.Exception ex)
            {
                string domain = _service.DefaultURL + APIDirectory + "/add";

                string classname = "OctoPOSProduct, Method: " + Method;

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);

            }

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


        public bool Delete(Model.Product product)
        {
            throw new NotImplementedException();
        }

        public Model.Product Get(long productId)
        {
            throw new NotImplementedException();
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

        //public T Get(string productSku)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
