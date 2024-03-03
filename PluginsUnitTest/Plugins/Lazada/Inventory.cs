using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace PluginsUnitTest.Plugins.Lazada
{
    [TestClass]
    public class Inventory
    {



        [TestMethod]
        public void ForDispatchedTest()
        {
            string compayid = "3388";
            var kti_storecode = "lazadatestncci";
            var qtyonhand = "40";
            var product = "COF";


            var parameters = new Dictionary<string, string>
            {
                {"companyid",compayid },
                {"kti_storecode",kti_storecode },
                {"product",product },
                {"qtyonhand",qtyonhand },
                {"warehouse","WELFO1" },
                {"unit","PC" },

            };


            var response = CallAPI(parameters, "inventory");

            Assert.IsInstanceOfType(response, typeof(string));
        }


        private string CallAPI(object Order, string ExtenstionName)
        {
            string accessToken = CRM_Plugin.Authenticate.AccessToken.Generate(3388).GetAwaiter().GetResult();

            using (HttpClient httpClient = new HttpClient())
            {
                var settings = new JsonSerializerSettings();

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                httpClient.BaseAddress = new Uri("https://localhost:44346");
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("OC-Api-App-Key", CRM_Plugin.Moo.Config.company_33388_occapikey);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", CRM_Plugin.Moo.Config.company_33388_subkey);
                httpClient.DefaultRequestHeaders.Add("Instance-Type", ExtenstionName);
                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var jsonObject = JsonConvert.SerializeObject(Order, Formatting.Indented, settings);


                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync("/api/Lazada/Inventory/UpdateStock", content).GetAwaiter().GetResult();

                var response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;

            }

        }

    }
}
