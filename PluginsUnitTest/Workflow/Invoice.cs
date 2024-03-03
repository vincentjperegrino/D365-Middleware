using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;

namespace PluginsUnitTest.Workflow
{
    [TestClass]
    public class Invoice : TestBase
    {
        private readonly CRM_Plugin.ScheduleSyncInvoiceToSAP _Domain;

        public Invoice()
        {
         
            _Domain = new CRM_Plugin.ScheduleSyncInvoiceToSAP();
        }

        [TestMethod]
        public void Replicate()
        {
            int companyid = 3388;
            _service = connectToCRM(companyid);
            var response = _Domain.process(_service, null , companyid);
            Assert.IsInstanceOfType(response, typeof(bool));
        }



    }
}
