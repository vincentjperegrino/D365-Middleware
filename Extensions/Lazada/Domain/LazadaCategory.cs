using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Lazada.Domain
{
    public class Category : Core.Domain.ICategory<Lazada.Model.Category>
    {
        private Service.ILazopService _service { get; init; }
        private Model.Category _root;

        public Category(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public Category(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public Category(Service.ILazopService service)
        {
            this._service = service;
        }
        public Category(Service.Queue.Config config, IDistributedCache cache)
        {
            this._service = new LazopService(config, cache);
        }

        public Category(Service.Queue.Config config, ClientTokens clientTokens)
        {
            this._service = new LazopService(config, clientTokens);
        }


        public List<Model.Category> Get()
        {
            return Get("en_US").Children;
        }

        public Model.Category GetAsTree()
        {
            return Get("en_US");
        }

        public Model.Category Get(string languageCode)
        {
            var parameters = new Dictionary<string, string>
            {
                {"language_code", languageCode}
            };

            var response = JsonDocument.Parse(_service.ApiCall("/category/tree/get", parameters, "GET")).RootElement.GetProperty("data");

            _root = new() { Name = "_root", Children = response.EnumerateArray().Select(c => ParseJson(c)).ToList() };

            return _root;
        }

        public List<CategorySuggestion> GetSuggestions(string productName)
        {
            var parameters = new Dictionary<string, string>
            {
                {"product_name", productName}
            };

            var response = this._service.AuthenticatedApiCall("/product/category/suggestion/get", parameters, "GET");

            return JsonSerializer.Deserialize<List<CategorySuggestion>>(JsonDocument.Parse(response).RootElement.GetProperty("data").GetProperty("categorySuggestions").ToString());
        }

        private Model.Category ParseJson(JsonElement json)
        {
            Model.Category node = new()
            {
                Name = json.GetProperty("name").ToString(),
                Var = json.GetProperty("var").GetBoolean(),
                IsLeaf = json.GetProperty("leaf").GetBoolean(),
                Children = new(),
                CategoryId = json.GetProperty("category_id").GetInt32(),
            };


            JsonElement cJson;
            if (json.TryGetProperty("children", out cJson))
            {
                var childrenJson = cJson.EnumerateArray().ToList();
                if (childrenJson.Count > 0)
                {
                    foreach (JsonElement child in childrenJson)
                    {
                        node.Children.Add(ParseJson(child));
                    }
                }
            }
            return node;
        }

        public Model.Category Add(Model.Category categoryModel)
        {
            throw new NotImplementedException();
        }

        public Model.Category Get(int categoryID)
        {
            throw new NotImplementedException();
        }

        public Model.Category Update(Model.Category categoryModel)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int categoryID)
        {
            throw new NotImplementedException();
        }
    }
}
