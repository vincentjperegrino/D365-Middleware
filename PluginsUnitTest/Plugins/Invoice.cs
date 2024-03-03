using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Workflow.Runtime.Tracking;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class Invoice : TestBase
    {
        private readonly CRM_Plugin.ReplicateInvoice _Domain;
        private readonly ITracingService _tracingService;

        public Invoice()
        {
            
            _Domain = new CRM_Plugin.ReplicateInvoice();
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void Replicate()
        {
            int companyid = 3388;
            _service = connectToCRM(companyid);
            Entity entity = new Entity("invoice", new Guid("4339a84a-974e-ed11-bba3-000d3a8562a7"));

            var response = _Domain.mainProcess(entity, _service, _tracingService , companyid);

            Assert.IsTrue(response);

        }


    }
}
