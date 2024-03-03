using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginsUnitTest.CustomPlugin.NCCI
{
    [TestClass]
    public class LastCoffePurchaceDateUpdateOnContacts : TestBase
    {
        private readonly ITracingService _tracingService;

        public LastCoffePurchaceDateUpdateOnContacts()
        {
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void GetSalesOrderDetail_NonCoffeeResult()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.LastCoffeePurchaseDateUpdateOnContactsBatch();
            var result = Domain.GetSalesOrderDetail(_service);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSalesOrderDetail_Process()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");
            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.LastCoffeePurchaseDateUpdateOnContactsBatch();

            var result = Domain.MainProcess(_service, _tracingService);

            Assert.IsTrue(result);
        }



        [TestMethod]
        public void GetSalesOrderDetails_Process()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");
            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.LastCoffeePurchaseDateUpdateOnContacts();

            var salesorderdetail = new Entity("salesorderdetail", new Guid("bdd6e24e-f41f-ee11-9966-002248ecf212"));

            var result = Domain.GetSalesOrderDetail(salesorderdetail, _service);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSalesOrderDetails_MainProcess()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");
            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.LastCoffeePurchaseDateUpdateOnContacts();

            var salesorderdetail = new Entity("salesorderdetail", new Guid("bdd6e24e-f41f-ee11-9966-002248ecf212"));

            var result = Domain.MainProcess(salesorderdetail, _service, tracingService);

            Assert.IsInstanceOfType(result, typeof(bool));

        }
    }
}
