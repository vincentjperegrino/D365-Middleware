using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;

namespace PluginsTest_UnitTest
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
        public void OrderReplicateProd()
        {
            int compayid = 3389;
            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            Entity entity = new Entity("salesorder", new Guid("8eab836a-46a7-ed11-aad1-002248593fc2"));

            var response = _Domain.mainProcess(entity, _service, _tracingService, compayid);

            Assert.IsTrue(response);
        }
    }
}
