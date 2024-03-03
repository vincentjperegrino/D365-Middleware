using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginsUnitTest.Plugins.ChannelManagement
{
    [TestClass]
    public class UpsertRedisCacheChannelManagement : TestBase
    {

        private readonly CRM_Plugin.UpsertRedisCacheChannelManagement _Domain;
        private readonly ITracingService _tracingService;

        public UpsertRedisCacheChannelManagement()
        {
          
            _Domain = new CRM_Plugin.UpsertRedisCacheChannelManagement();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void UpsertRedisCacheChannelManagement_Success()
        {
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = new Entity("kti_saleschannel", new Guid("745f74df-ed48-ed11-bba2-000d3a8561a6"));

            var response = _Domain.mainProcess(entity , _service , _tracingService);
            Assert.IsTrue(response);
        }



    }
}
 