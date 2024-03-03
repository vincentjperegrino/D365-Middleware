using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;


namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class CustomAPI : TestBase
    {

        private readonly CRM_Plugin.CustomAPI.Function.GetAllProducts _Domain;

        public CustomAPI()
        {
            _service = connectToCRM();
            _Domain = new CRM_Plugin.CustomAPI.Function.GetAllProducts(_service);
        }

        [TestMethod]
        public void ProductGetAll()
        {
           
            var response = _Domain.Process();

            Assert.IsInstanceOfType(response, typeof(EntityCollection));
        }
    }
}
