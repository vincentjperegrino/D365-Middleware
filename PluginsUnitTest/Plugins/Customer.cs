using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class Customer : TestBase
    {
        private readonly ITracingService _tracingService;

        private readonly CRM_Plugin.ReplicateCustomer _Domain;

        public Customer()
        {
         
            _Domain = new CRM_Plugin.ReplicateCustomer();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void Replicate()
        {
            int companyid = 3389;
            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            Entity entity = new Entity("contact", new Guid("b403cbd8-5361-ed11-9562-002248593ae0"));

            _Domain.CustomerProcess(entity, _service, _tracingService, companyid);

            Assert.IsTrue(true);
        }



    }
}
