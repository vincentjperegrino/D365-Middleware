using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class SyncStoreChannel : TestBase
    {
        private readonly CRM_Plugin.SyncSalesChannelProduct _Domain;
        private readonly ITracingService _tracingService;

        public SyncStoreChannel()
        {
        
            _Domain = new CRM_Plugin.SyncSalesChannelProduct();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void SyncProduct()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = _service.Retrieve("kti_saleschannel", new Guid("745f74df-ed48-ed11-bba2-000d3a8561a6"), new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            var response = _Domain.Process(entity, DateTime.Now, 0, "product", _service , _tracingService);

            Assert.IsTrue(response);
        }
    }
}
