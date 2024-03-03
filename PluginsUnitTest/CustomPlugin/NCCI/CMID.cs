

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginsUnitTest.CustomPlugin.NCCI
{
    [TestClass]
    public class CMID : TestBase
    {
        private readonly ITracingService _tracingService;

        public CMID()
        {
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void OnUpdateCustomerInsertToCustomer()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");
            Entity entity = new Entity("contact", new Guid("c55df164-d52b-ee11-9965-000d3a82ca70"));

            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.CreateAutoGenerationCMID();
            Domain.MainProcess(entity, _service, _tracingService);

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void OnCreateCMIDUpdateCustomersCMID()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");
            Entity entity = new Entity("kti_clubmembership", new Guid("c9624ebb-31f6-ed11-8849-000d3a82ca70"));

            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.UpdateCustomerOnCMIDCreate();

            Domain.MainProcess(entity, _service, _tracingService);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BatchUpdateCMID()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var ContactsForUpdating = GetAllCMID(_service);

            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.CreateAutoGenerationCMID();

            foreach (var contact in ContactsForUpdating)
            {
                if (contact.Id == new Guid("7d2ad31a-4cbf-ed11-83fe-000d3aa087d9"))
                {
                    continue;
                }
                Domain.Process(contact, _service, _tracingService);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BatchUpdateDuplicateCMID()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var ContactsForUpdating = GetDuplicateCMID(_service);

            var Domain = new KTI.Moo.Plugin.Custom.NCCI.Plugin.CreateAutoGenerationCMID();

            foreach (var contact in ContactsForUpdating)
            {
                if (contact.Id == new Guid("7d2ad31a-4cbf-ed11-83fe-000d3aa087d9"))
                {
                    continue;
                }

                Domain.Process(contact, _service, _tracingService);
            }

            Assert.IsTrue(true);
        }

        public List<Entity> GetDuplicateCMID(IOrganizationService service)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Contact.entity_name,
                ColumnSet = KTI.Moo.Plugin.Custom.NCCI.Model.Customer.ColumnSet,
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression()
                        {
                            AttributeName = "statuscode",
                            Operator = ConditionOperator.Equal,
                            Values = { 1 }
                        },
                        new ConditionExpression
                        {
                            AttributeName = "ncci_boughtcoffee",
                            Operator = ConditionOperator.Equal,
                            Values = { true }
                        },
                        new ConditionExpression
                        {
                            AttributeName = "ncci_newclubmembershipid",
                            Operator = ConditionOperator.In,
                            Values =
                             {
                             }
                        }
                    }
                }
            };

            LinkEntity LeftJoinCMID = new LinkEntity("contact", "kti_clubmembership", "contactid", "kti_customer", JoinOperator.LeftOuter);

            LeftJoinCMID.Columns = new ColumnSet("kti_customer", "kti_clubmembershipautoid");

            LeftJoinCMID.EntityAlias = "cmid";

            query.LinkEntities.Add(LeftJoinCMID);

            var result = new EntityCollection(RetrieveAllRecords(service, query));

            var AllowedContact = result.Entities.Where(contact => !contact.Contains("cmid.kti_clubmembershipautoid")).ToList();

            return AllowedContact;
        }


        public List<Entity> GetAllCMID(IOrganizationService service)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Contact.entity_name,
                ColumnSet = KTI.Moo.Plugin.Custom.NCCI.Model.Customer.ColumnSet,
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "statuscode",
                            Operator = ConditionOperator.Equal,
                            Values = { 1 }
                        },
                        new ConditionExpression
                        {
                            AttributeName = "ncci_boughtcoffee",
                            Operator = ConditionOperator.Equal,
                            Values = { true }
                        }
                    }
                }
            };

            LinkEntity LeftJoinCMID = new LinkEntity("contact", "kti_clubmembership", "contactid", "kti_customer", JoinOperator.LeftOuter);

            LeftJoinCMID.Columns = new ColumnSet("kti_customer", "kti_clubmembershipautoid");

            LeftJoinCMID.EntityAlias = "cmid";

            query.LinkEntities.Add(LeftJoinCMID);

            var result = new EntityCollection(RetrieveAllRecords(service, query));

            var AllowedContact = result.Entities.Where(contact => !contact.Contains("cmid.kti_clubmembershipautoid")).ToList();

            return AllowedContact;
        }



        private static List<Entity> RetrieveAllRecords(IOrganizationService service, QueryExpression query)
        {
            var pageNumber = 1;
            var pagingCookie = string.Empty;
            var result = new List<Entity>();
            EntityCollection resp;
            do
            {
                if (pageNumber != 1)
                {
                    query.PageInfo.PageNumber = pageNumber;
                    query.PageInfo.PagingCookie = pagingCookie;
                }
                resp = service.RetrieveMultiple(query);
                if (resp.MoreRecords)
                {
                    pageNumber++;
                    pagingCookie = resp.PagingCookie;
                }
                //Add the result from RetrieveMultiple to the List to be returned.
                result.AddRange(resp.Entities);
            }
            while (resp.MoreRecords);

            return result;
        }





    }
}
