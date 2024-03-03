using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;


namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class Product : TestBase
    {
        private readonly CRM_Plugin.ReplicateProduct _Domain;
        private readonly ITracingService _tracingService;

        public Product()
        {
            _service = connectToCRM();
            _Domain = new CRM_Plugin.ReplicateProduct();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void ProductReplicate()
        {
            Entity entity = new Entity("product", new Guid("f0dbc4db-a4dd-ec11-bb3d-6045bd1bfea1"));

            var response = _Domain.ProcessProduct(entity, _service, _tracingService);

            Assert.IsTrue(response);
        }






    }
}
