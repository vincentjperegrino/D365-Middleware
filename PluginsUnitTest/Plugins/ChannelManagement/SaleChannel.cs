using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PluginsUnitTest.Plugins.ChannelManagement
{
    [TestClass]
    public class SaleChannel : TestBase
    {

        private readonly CRM_Plugin.CustomAPI.Domain.ChannelManagement _Domain;

        public SaleChannel()
        {
            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            _Domain = new CRM_Plugin.CustomAPI.Domain.ChannelManagement(_service);
        }

        [TestMethod]
        public void GetSalesChannelByCode()
        {
            var response = _Domain.Get("octopos-prod");
            Assert.IsInstanceOfType(response, typeof(CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement.SalesChannel));
        }

        [TestMethod]
        public void GetALLSalesChannel()
        {
            var response = _Domain.GetChannelList();
            Assert.IsInstanceOfType(response, typeof(List<CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement.SalesChannel>));
        }


        [TestMethod]
        public void UpdateSalesChannelConfig()
        {
            var salesChannelConfig = JsonConvert.DeserializeObject<CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement.SalesChannel>("{\"CustomFieldList\":[],\"kti_AppKeyflag\":null,\"kti_AppSecretflag\":null,\"kti_salt\":null,\"kti_appsecret\":\"sGFdQUXMsPZ9PSU3fPk8s8pQbXoAPM5N\",\"kti_account\":\"f5c3c983-88eb-ec11-bb3c-002248174787\",\"kti_appkey\":\"105046\",\"kti_channelorigin\":959080006,\"kti_country\":\"7383cac6-cdc2-ea11-a812-000d3a579c6d\",\"kti_sellerid\":\"500203125266\",\"kti_defaulturl\":\"https://api.lazada.com/rest\",\"kti_isproduction\":false,\"kti_name\":\"Lazada Test Store\",\"kti_databasename\":null,\"kti_password\":\"qwe\",\"kti_Passwordflag\":null,\"kti_storecode\":\"lazadamoo\",\"kti_saleschannelId\":\"e4d019a1-83e7-ec11-bb3c-002248174787\",\"kti_username\":null,\"kti_warehousecode\":\"Warehouse\",\"kti_moocompanyid\":\"3387\",\"kti_azureconnectionstring\":\"DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net\",\"kti_access_token\":\"123\",\"kti_access_expiration\":\"2023-02-02T11:00:29.1677006Z\",\"kti_refresh_token\":\"1231332\",\"kti_refresh_expiration\":\"2023-02-02T11:00:29.1676998Z\",\"createdby\":null,\"createdon\":\"0001-01-01T00:00:00\",\"createdonbehalfby\":null,\"importsequencenumber\":0,\"modifiedby\":null,\"modifiedon\":\"0001-01-01T00:00:00\",\"modifiedonbehalfby\":null,\"overriddencreatedon\":\"0001-01-01T00:00:00\",\"ownerid\":null,\"owningbusinessunit\":null,\"owningteam\":null,\"owninguser\":null,\"statecode\":0,\"statuscode\":0}");

            var response = _Domain.UpdateToken(salesChannelConfig);
            Assert.IsTrue(response);
        }

    }
}
