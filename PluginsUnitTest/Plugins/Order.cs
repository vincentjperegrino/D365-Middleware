using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class Order : TestBase
    {
        private readonly CRM_Plugin.ReplicateOrder _Domain;
        private readonly ITracingService _tracingService;
 
        public Order()
        {
            _Domain = new CRM_Plugin.ReplicateOrder();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void OrderReplicateTest_KTIDEV()
        {
            int compayid = 3388;
            _service = connectToCRM("https://ktisalessandbox.crm5.dynamics.com");

            Entity entity = new Entity("salesorder", new Guid("33613d94-755e-ed11-9562-000d3a856d22"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void OrderReplicateTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            Entity entity = new Entity("salesorder", new Guid("f45d397b-c5e4-ed11-8847-002248ecf784"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void OrderBatchReplicateTest()
        {
            int compayid = 3388;
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            var Domain = new CRM_Plugin.APIScheduleSyncOrderToSAP();

            var response = Domain.process(_service, _tracingService, compayid);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void OrderReplicateProd()
        {
            int compayid = 3389;
            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            Entity entity = new Entity("salesorder", new Guid("79f77c89-3c35-ee11-bdf4-000d3aa087d9"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void OrderBatchReplicateProd()
        {
            int compayid = 3389;
            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            var Domain = new CRM_Plugin.APIScheduleSyncOrderToSAP();

            var response = Domain.process(_service, _tracingService, compayid);

            Assert.IsTrue(response);
        }


        [TestMethod]
        public void GetOrderBatchDev()
        {
            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            var Domain = new CRM_Plugin.CustomAPI.Domain.Order(_service, _tracingService);

            var response = Domain.GetScheduledOrder();

            Assert.IsInstanceOfType(response, typeof(EntityCollection));
        }

    }
}
