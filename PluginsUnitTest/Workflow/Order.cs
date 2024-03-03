
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;


namespace PluginsUnitTest.Workflow
{
    [TestClass]
    public class Order : TestBase
    {
        private readonly CRM_Plugin.ScheduleSyncOrderToSAP _Domain;

        public Order()
        {
            _service = connectToCRM();
            _Domain = new CRM_Plugin.ScheduleSyncOrderToSAP();
        }

        //[TestMethod]
        //public void Replicate()
        //{



        //    var response = _Domain.process(_service, null);
        //    Assert.IsInstanceOfType(response, typeof(bool));
        //}

    
    }
}
