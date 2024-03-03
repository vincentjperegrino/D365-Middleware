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
    public class ShipmentStatus : TestBase
    {
        private readonly CRM_Plugin.Lazada.Plugins.ReplicateShipmentStatus_Lazada _Domain;
        private readonly ITracingService _tracingService;

        public ShipmentStatus()
        {

            _Domain = new CRM_Plugin.Lazada.Plugins.ReplicateShipmentStatus_Lazada();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void ShipmentStatusReplicateStatusTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = new Entity("kti_shipment", new Guid("b03ac99e-1d70-4dc9-aa3f-b0d16929db33"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }


        [TestMethod]
        public void PrintWayBillTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            var orderstatus = new CRM_Plugin.Lazada.Model.OrderStatus()
            {
                companyid = compayid,
                kti_storecode = "lazadatestncci",
                packageid = "FP039611831708090"

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
                var result = httpClient.PostAsync("/api/Lazada/Order/PrintAirWayBill", content).GetAwaiter().GetResult();

                var response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;

            }

        }
    }
}
