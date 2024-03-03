using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;


namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class ProductDemo : TestBase
    {
        private readonly CRM_Plugin.ReplicateProductDemo _Domain;

        public ProductDemo()
        {
            _service = connectToCRM();
            _Domain = new CRM_Plugin.ReplicateProductDemo();
        }

        [TestMethod]
        public void OrderReplicate()
        {
            Entity entity = new Entity("product", new Guid("b46a298a-66f0-eb11-94ef-000d3a81e314"));

            var response = _Domain.ProcessProductDemo(entity, "Create", _service, null);

            Assert.IsTrue(response);
        }


    }
}
