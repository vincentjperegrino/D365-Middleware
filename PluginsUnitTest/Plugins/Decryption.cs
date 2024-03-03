using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class Decryption : TestBase
    {

        private readonly CRM_Plugin.SalesChannel.Domain.SalesChannel _Domain;
        private readonly ITracingService _tracingService;

        public Decryption() 
        {
            _service = connectToCRM();
            _tracingService = Mock.Of<ITracingService>();
            _Domain = new CRM_Plugin.SalesChannel.Domain.SalesChannel(_service, _tracingService);
        }


        [TestMethod]
        public void Decrypt()
        { 

            var result = _Domain.DecryptKey("6OIw1V2/oRUTERiyRbZQ3g==");

            Assert.IsInstanceOfType(result, typeof(string));
        }
    }
}
