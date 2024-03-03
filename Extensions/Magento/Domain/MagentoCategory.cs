using Azure;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Domain
{
    public class Category : ICategory<Model.Category>
    {

        private IMagentoService _service { get; init; }
        public const string APIDirectory = "/categories";



        public Category(string defaultDomain, string redisConnectionString, string username, string password)
        {

            this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);

        }

        public Category(IMagentoService service)
        {
            this._service = service;
        }
        public Category(Config config, IDistributedCache cache)
        {
            this._service = new MagentoService(config, cache);
        }



        public Model.Category Add(Model.Category categoryModel)
        {
            try
            {
                var path = APIDirectory;

                var method = "POST";

                var isAuthenticated = true;

                Model.DTO.Category.Add CategoryAddModel = new();

                CategoryAddModel.category = categoryModel;

                var stringContent = GetContent(CategoryAddModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore

                };

                var ReturnCategoryData = JsonConvert.DeserializeObject<Model.Category>(response, settings);

                if (ReturnCategoryData.CategoryId == 0)
                {
                    throw new System.Exception(response);
                }

                return ReturnCategoryData;

            }
            catch (System.Exception ex)
            {
                string domain = _service.DefaultURL + APIDirectory;

                string classname = "MagentoCategory, Method: Add";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }

        }

        public List<Model.Category> Get()
        {
            try
            {

                var path = APIDirectory;

                var isAuthenticated = true;
                var method = "GET";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnCategoryData = JsonConvert.DeserializeObject<Model.Category>(response, settings);

                return ReturnCategoryData.children_data;

            }
            catch
            {
                return new List<Model.Category>();
            }

        }


        public Model.Category Get(int categoryID)
        {
            if (categoryID <= 0)
            {
                throw new ArgumentException("Invalid categoryID", nameof(categoryID));
            }

            try
            {
                var path = APIDirectory + "/" + categoryID;

                var isAuthenticated = true;
                var method = "GET";


                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnCategoryData = JsonConvert.DeserializeObject<Model.Category>(response, settings);

                return ReturnCategoryData;
            }
            catch
            {
                return new Model.Category();
            }
        }

        public Model.Category Update(Model.Category categoryModel)
        {
            if (categoryModel is null)
            {
                throw new ArgumentNullException(nameof(categoryModel));
            }

            try
            {
                var path = APIDirectory + "/" + categoryModel.CategoryId;

                var isAuthenticated = true;
                var method = "PUT";

                Model.DTO.Category.Update CategoryUpdateModel = new();

                CategoryUpdateModel.category = categoryModel;

                var stringContent = GetContent(CategoryUpdateModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);


                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore

                };

                var ReturnCategoryData = JsonConvert.DeserializeObject<Model.Category>(response, settings);

                if (ReturnCategoryData.CategoryId == 0)
                {

                    throw new System.Exception(response);

                }


                return ReturnCategoryData;

            }
            catch (System.Exception ex)
            {

                var domain = _service.DefaultURL + APIDirectory + "/" + categoryModel.CategoryId;

                var classname = "MagentoCategory, Method: Update";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }

        }

        public bool Delete(int categoryID)
        {

            if (categoryID <= 0)
            {
                throw new ArgumentException("Invalid categoryID", nameof(categoryID));
            }


            try
            {
                var path = APIDirectory + "/" + categoryID;
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

                var domain = _service.DefaultURL + APIDirectory + "/" + categoryID;

                var classname = "MagentoCategory, Method: Delete";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
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

    }
}
