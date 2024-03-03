using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginsUnitTest.Plugins
{
    [TestClass]
    public class Order_WithCustomer : TestBase
    {

        private readonly CRM_Plugin.UpsertOrder_WithCustomer _Domain;
        private readonly ITracingService _tracingService;

        public Order_WithCustomer()
        {
            _Domain = new CRM_Plugin.UpsertOrder_WithCustomer();
            _tracingService = Mock.Of<ITracingService>();
        }


        [TestMethod]
        public void OrderReplicateTest_KTIDEV()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            var kti_UpsertOrder_CustomerParameters = _service.Retrieve("contact", new Guid("8de7feb3-c200-ed11-82e6-000d3a81912a"), new ColumnSet("kti_sourceid"));
            var kti_UpsertOrder_OrderParameters = _service.Retrieve("salesorder", new Guid("e3949a2d-c300-ed11-82e6-000d3a81912a"), new ColumnSet("kti_sourceid", "kti_socialchannelorigin"));
            var kti_UpsertOrder_OrderLineParameters = new EntityCollection();

            kti_UpsertOrder_OrderParameters["kti_branchassigned"] = "TJ001";
            kti_UpsertOrder_OrderParameters["kti_socialchannelorigin"] = ((OptionSetValue)kti_UpsertOrder_OrderParameters["kti_socialchannelorigin"]).Value;

            var Line1 = new Entity();
            Line1["productid"] = "BUN-1234";
            Line1["quantity"] = 1;
            Line1["priceperunit"] = (decimal)1000.00;
            Line1["kti_lineitemnumber"] = "1";
            Line1["ispriceoverridden"] = true;
            Line1["kti_sourceid"] = kti_UpsertOrder_OrderParameters["kti_sourceid"];
            Line1["kti_socialchannelorigin"] = kti_UpsertOrder_OrderParameters["kti_socialchannelorigin"];

            kti_UpsertOrder_OrderLineParameters.Entities.Add(Line1);


            var Parameters = new ParameterCollection
            {
                { "kti_UpsertOrder_CustomerParameters", kti_UpsertOrder_CustomerParameters },
                { "kti_UpsertOrder_OrderParameters", kti_UpsertOrder_OrderParameters },
                { "kti_UpsertOrder_OrderLineParameters", kti_UpsertOrder_OrderLineParameters }
            };

            var response = _Domain.Process(_tracingService, Parameters, _service);

            Assert.IsTrue(response);
        }


        [TestMethod]
        public void OrderGetItem()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            var OrderLineDomain = new CRM_Plugin.Domain.Recievers.OrderLine(_service, _tracingService);

            var ExistingItems = OrderLineDomain.GetOrderItemList(new Guid("ee152adf-e5df-ed11-a7c7-000d3aa087d9"));

            var WithLineItemList = ExistingItems.Entities.Where(existing => !existing.Contains("parentbundleid")).ToList();

            Assert.IsTrue(true);
        }       
        
        
        [TestMethod]
        public void OrderGet()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            var OrderLineDomain = new CRM_Plugin.Domain.Recievers.Order(_service, _tracingService);

            var ExistingItems = OrderLineDomain.Get(new Guid("ee152adf-e5df-ed11-a7c7-000d3aa087d9"));
           
            Assert.IsNotNull(ExistingItems);
        }



        [TestMethod]
        public void CustomerGet()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");
            var Domain = new CRM_Plugin.Domain.Recievers.Customer(_service, _tracingService);

            string sourceid = "500131881834";
            OptionSetValue channel = new OptionSetValue(959080006);
            string firstname = "Dongkyun";
            string lastname = "Son";
            string email = "";
            string mobile = "6309190968084";

            var response = Domain.Get(sourceid, channel, firstname, lastname, email, mobile);



            Assert.IsInstanceOfType(response, typeof(List<Entity>));
        }


    }
}
