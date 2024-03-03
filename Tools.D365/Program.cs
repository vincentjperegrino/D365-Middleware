using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace PowerPlatform.Dataverse.CodeSamples
{
    class Program : Tools.D365.Base.LazadaBase
    {
        private static KTI.Moo.Extensions.Lazada.Domain.Category categoryDomain;
        private static IDistributedCache _cache;
        private static Microsoft.Identity.Client.AuthenticationResult token;
        /// <summary>
        /// Constructor. Loads the application configuration settings from a JSON file.
        /// </summary>
        Program()
        {
            Tools.D365.Authentication authentication = new();

            token = authentication.GetToken();

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            categoryDomain = new(CongfigTest, _cache);
        }

        static void Main(string[] args)
        {
            Program program = new();

            var response = categoryDomain.Get("en_US");

            foreach (var category in response.Children)
            {
                Guid category1CRMID = Guid.NewGuid();

                category1CRMID = AddParentProduct(category);

                if (category.IsLeaf == false)
                {
                    foreach (var category1 in category.Children)
                    {
                        Guid category2CRMID = Guid.NewGuid();

                        category2CRMID = AddChildParentProduct(category1, category1CRMID);

                        if (category1.IsLeaf == false)
                        {
                            foreach (var category2 in category1.Children)
                            {
                                Guid category3CRMID = Guid.NewGuid();

                                category3CRMID = AddChildParentProduct(category2, category2CRMID);

                                if (category2.IsLeaf == false)
                                {
                                    foreach (var category3 in category2.Children)
                                    {
                                        Guid category4CRMID = Guid.NewGuid();

                                        category4CRMID = AddChildParentProduct(category3, category3CRMID);

                                        if (category3.IsLeaf == false)
                                        {
                                            foreach (var category4 in category3.Children)
                                            {
                                                Guid category5CRMID = Guid.NewGuid();

                                                category5CRMID = AddChildParentProduct(category4, category4CRMID);

                                                if (category4.IsLeaf == false)
                                                {
                                                    foreach (var category5 in category4.Children)
                                                    {
                                                        Guid category6CRMID = Guid.NewGuid();

                                                        category6CRMID = AddChildParentProduct(category5, category5CRMID);

                                                        if (category5.IsLeaf == false)
                                                        {
                                                            foreach (var category6 in category5.Children)
                                                            {
                                                                Guid category7CRMID = Guid.NewGuid();

                                                                category7CRMID = AddChildParentProduct(category6, category6CRMID);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static Guid AddParentProduct(KTI.Moo.Extensions.Lazada.Model.Category? category)
        {
            Domain.Models.Items.Products productCategory = new()
            {
                productnumber = $"laz_{category.CategoryId}",
                productstructure = 3,
                name = category.Name,
                kti_socialchannel = "959080006"
            };

            return InsertToCRM(productCategory).GetAwaiter().GetResult();
        }

        static Guid AddChildParentProduct(KTI.Moo.Extensions.Lazada.Model.Category? category, Guid parentCategory)
        {
            if(parentCategory != new Guid())
            {
                Domain.Models.Items.Products productCategory = new()
                {
                    productnumber = $"laz_{category.CategoryId}",
                    productstructure = 3,
                    name = category.Name,
                    parentproductid = $"/products({parentCategory})",
                    kti_socialchannel = "959080006"
                };
                return InsertToCRM(productCategory).GetAwaiter().GetResult();
            }

            return new Guid();
        }

        static async Task<Guid> InsertToCRM(Domain.Models.Items.Products productCategory)
        {
            string url = $"https://ktisalessandbox.api.crm5.dynamics.com/api/data/v9.2/products(productnumber='{productCategory.productnumber}')";
            JsonSerializer _jsonWriter = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var client = new HttpClient();

            // OData related headers
            client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            client.DefaultRequestHeaders.Add("Prefer", "return=representation, odata.include-annotations=\"*\"");

            // Passing AccessToken in Authentication header
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");

            var jsonString = JsonConvert.SerializeObject(productCategory,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var response = await client.PatchAsync(url, 
                new StringContent(jsonString,
                            UnicodeEncoding.UTF8,
                            "application/json"));

            var productResponse = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                //var productID = new Guid(response.Headers.GetValues("OData-EntityId").FirstOrDefault());

                if(productResponse.Contains("productid"))
                {
                    dynamic data = JObject.Parse(productResponse);

                    return Guid.Parse((string)data.productid);
                }
            }

            return new Guid();
        }
    }
}
