using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginsUnitTest.Plugins.Lazada
{
    [TestClass]
    public class OrderStatus : TestBase
    {
        private readonly CRM_Plugin.Lazada.Plugins.ReplicateOrderStatus_Lazada _Domain;
        private readonly ITracingService _tracingService;

        public OrderStatus()
        {

            _Domain = new CRM_Plugin.Lazada.Plugins.ReplicateOrderStatus_Lazada();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void OrderReplicateStatusTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = new Entity("salesorder", new Guid("7f0760b7-117d-ed11-81ad-000d3a8562a7"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }


        [TestMethod]
        public void OrderReplicateStatusModelTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = new Entity("salesorder", new Guid("dbb73bc6-a88d-ed11-81ad-000d3a8562a7"));

            var MainQuery = new QueryExpression
            {
                //Select salesorderid, kti_sourceid, kti_sourceitemid, kti_orderstatus, kti_socialchannelorigin
                ColumnSet = new ColumnSet("salesorderid", "kti_sourceid", "kti_orderstatus", "kti_socialchannelorigin"),
                //From salesorder
                EntityName = entity.LogicalName
            };

            //Inner Join
            var InnerJoinSalesChannel = new LinkEntity("salesorder", "kti_saleschannel", "kti_socialchannel", "kti_saleschannelid", JoinOperator.Inner)
            {
                Columns = new ColumnSet("kti_saleschannelcode"), //Select StoreChannels.kti_saleschannelcode
                EntityAlias = "StoreChannels"
            };

            MainQuery.LinkEntities.Add(InnerJoinSalesChannel);

            //Where
            var MainFilter = new FilterExpression(LogicalOperator.And);
            MainFilter.AddCondition("salesorderid", ConditionOperator.Equal, entity.Id);
            MainQuery.Criteria.AddFilter(MainFilter);

            var ResultOrders = _service.RetrieveMultiple(MainQuery);
            var Top1Order = ResultOrders.Entities.FirstOrDefault();

            var orderstatusDomain = new CRM_Plugin.Lazada.Domain.OrderStatus(_service, tracingService, compayid);

            var orderstatusmodel = orderstatusDomain.OrderPacked(Top1Order);

            var APIResponse = CallAPI(orderstatusmodel, "OrderStatus");

            var response = JsonConvert.DeserializeObject<CRM_Plugin.Lazada.Model.OrderStatus>(APIResponse);

            Assert.IsInstanceOfType(response, typeof(CRM_Plugin.Lazada.Model.OrderStatus));
        }

        private string CallAPI(object Order, string ExtenstionName)
        {
            string accessToken = CRM_Plugin.Authenticate.AccessToken.Generate(3388).GetAwaiter().GetResult();

            using (HttpClient httpClient = new HttpClient())
            {
                var settings = new JsonSerializerSettings();

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                httpClient.BaseAddress = new Uri(CRM_Plugin.Moo.Config.baseurl);
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
                var result = httpClient.PostAsync("/api/Lazada/OrderStatus/OrderPacked", content).GetAwaiter().GetResult();


                var response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return response;

            }

        }




    }
}
