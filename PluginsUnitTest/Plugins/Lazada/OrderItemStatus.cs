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

namespace PluginsUnitTest.Plugins.Lazada
{
    [TestClass]
    public class OrderItemStatus : TestBase
    {
        private readonly CRM_Plugin.Lazada.Plugins.ReplicateOrderItemStatus_Lazada _Domain;
        private readonly ITracingService _tracingService;

        public OrderItemStatus()
        {
            _Domain = new CRM_Plugin.Lazada.Plugins.ReplicateOrderItemStatus_Lazada();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void OrderItemReplicateStatusTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = new Entity("salesorderdetail", new Guid("be7c1cbc-117d-ed11-81ad-000d3a8561a6"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void ForDispatchedTest()
        {
            int compayid = 3388;
    
            var orderstatus = new CRM_Plugin.Lazada.Model.OrderStatus()
            {
                companyid = compayid,
                kti_storecode = "lazadatestncci",
                kti_sourcesalesorderitemids = new List<string>()
                {
                    "613707800355852"
                },
                cancelreason = "canceled",
                laz_cancelReason = 15,
               
            };

            var response = CallAPI(orderstatus, "orderstatus");

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
                var result = httpClient.PostAsync("/api/Lazada/OrderStatus/CancelOrder", content).GetAwaiter().GetResult();


                var response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;

            }

        }
    }
}
