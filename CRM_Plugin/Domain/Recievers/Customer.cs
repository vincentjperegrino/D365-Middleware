using CRM_Plugin.Core.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Domain.Recievers
{
    public class Customer : ICustomer
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public Customer(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;

        }
        public Entity Get(Guid ExistingID)
        {
            return _service.Retrieve(Core.Helper.EntityHelper.SalesOrder.entity_name, ExistingID, new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid));
        }

        public Entity Get(string sourceid, OptionSetValue channel)
        {
            if (sourceid == null || channel == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid, ConditionOperator.Equal, sourceid);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin, ConditionOperator.Equal, channel.Value);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                returnCustomer.Entities.First();
            }

            return null;
        }

        public List<Entity> Get(string sourceid, OptionSetValue channel, string firstname, string lastname, string email, string mobile)
        {
            if (sourceid == null || channel == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid, Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin);


            //(kti_sourceid = sourceid AND kti_socialchannelorigin = channel.Value)
            var sourceId_AND_Channel_Filter = new FilterExpression(LogicalOperator.And);
            sourceId_AND_Channel_Filter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid, ConditionOperator.Equal, sourceid);
            sourceId_AND_Channel_Filter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin, ConditionOperator.Equal, channel.Value);


            //(firstname = firstname AND lastname = lastname)
            var NameFilter = new FilterExpression(LogicalOperator.And);
            NameFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.firstname, ConditionOperator.Equal, firstname);
            NameFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.lastname, ConditionOperator.Equal, lastname);

            //(emailaddress1 = email OR mobilephone = mobile)
            var Email_MobileFilter = new FilterExpression(LogicalOperator.Or);
            Email_MobileFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.emailaddress1, ConditionOperator.Equal, email);
            Email_MobileFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.mobilephone, ConditionOperator.Equal, mobile);

            // ((firstname = firstname AND lastname = lastname) AND (emailaddress1 = email OR mobilephone = mobile))
            var Name_Email_MobileFilter = new FilterExpression(LogicalOperator.And);
            Name_Email_MobileFilter.AddFilter(NameFilter);
            Name_Email_MobileFilter.AddFilter(Email_MobileFilter);


            //( (kti_sourceid = sourceid AND kti_socialchannelorigin = channel.Value) OR ((firstname = firstname AND lastname = lastname) AND (emailaddress1 = email OR mobilephone = mobile)) )
            var Filter = new FilterExpression(LogicalOperator.Or);
            Filter.AddFilter(sourceId_AND_Channel_Filter);
            Filter.AddFilter(Name_Email_MobileFilter);


            qeEntity.Criteria.AddFilter(Filter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                return returnCustomer.Entities.ToList();
            }

            return new List<Entity>();
        }


        public bool Create(Entity customer)
        {
            CreateWithGuid(customer);
            return true;
        }

        public Guid CreateWithGuid(Entity customer)
        {
            customer.LogicalName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            return _service.Create(customer);
        }

        public bool Update(Entity customer, Guid ExistingID)
        {
            customer.LogicalName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            customer.Id = ExistingID;
            _service.Update(customer);
            return true;
        }

        public bool Upsert(Entity customer)
        {
            var Erroron = "";
            try
            {
                Erroron = Core.Helper.EntityHelper.CRM.Customer.kti_sourceid;
                var sourceid = (string)customer[Core.Helper.EntityHelper.CRM.Customer.kti_sourceid];
                Erroron = Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin;
                var channel = (OptionSetValue)customer[Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin];


                Erroron = "Get(sourceid, channel)";
                var existingCustomer = Get(sourceid, channel);

                if (existingCustomer == null)
                {
                    Erroron = "create(customer)";
                    return Create(customer);
                }

                Erroron = "update(customer, existingCustomer.Id)";
                return Update(customer, existingCustomer.Id);

            }
            catch (Exception ex)
            {
                throw new Exception($"Customer Error on : {Erroron}. {ex}");
            }

        }

        public Guid UpsertWithGuid(Entity customer)
        {
            var Erroron = "";
            try
            {
                var existingCustomer = new Entity();

                if (customer.Contains(Core.Helper.EntityHelper.CRM.Customer.entity_id))
                {
                    Erroron = "UpdateExistingCustomer(customer, existingCustomer)";
                    return UpdateExistingCustomer(customer, existingCustomer);
                }

                Erroron = Core.Helper.EntityHelper.CRM.Customer.kti_sourceid;
                var sourceid = (string)customer[Core.Helper.EntityHelper.CRM.Customer.kti_sourceid];

                Erroron = Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin;
                var channel = (OptionSetValue)customer[Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin];


                var ListOfCustomer = GetPosibleExistingCustomers(customer);

                if (ListOfCustomer is null || ListOfCustomer.Count <= 0)
                {
                    existingCustomer = null;
                    return UpsertEntity(existingCustomer, customer);
                }

                if (ListOfCustomer.Any(customersFromList => customersFromList.Contains(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid) && customersFromList.Contains(Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin)
                                    && (string)customersFromList[Core.Helper.EntityHelper.CRM.Customer.kti_sourceid] == sourceid
                                    && ((OptionSetValue)customersFromList[Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin]).Value == channel.Value))
                {

                    existingCustomer = ListOfCustomer.Where(customersFromList => customersFromList.Contains(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid) && customersFromList.Contains(Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin)
                                                       && (string)customersFromList[Core.Helper.EntityHelper.CRM.Customer.kti_sourceid] == sourceid
                                                       && ((OptionSetValue)customersFromList[Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin]).Value == channel.Value).FirstOrDefault();

                    return UpsertEntity(existingCustomer, customer);
                }

                existingCustomer = ListOfCustomer.FirstOrDefault();

                return UpsertEntity(existingCustomer, customer);

            }
            catch (Exception ex)
            {
                throw new Exception($"Customer Error on : {Erroron}. {ex}");

            }
        }

        private List<Entity> GetPosibleExistingCustomers(Entity customer)
        {
            var Erroron = "";
            try
            {

                Erroron = Core.Helper.EntityHelper.CRM.Customer.kti_sourceid;
                var sourceid = (string)customer[Core.Helper.EntityHelper.CRM.Customer.kti_sourceid];
                Erroron = Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin;
                var channel = (OptionSetValue)customer[Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin];

                Erroron = Core.Helper.EntityHelper.CRM.Customer.firstname;
                var firstname = customer.Contains(Core.Helper.EntityHelper.CRM.Customer.firstname) ? (string)customer[Core.Helper.EntityHelper.CRM.Customer.firstname] : "";
                Erroron = Core.Helper.EntityHelper.CRM.Customer.lastname;
                var lastname = customer.Contains(Core.Helper.EntityHelper.CRM.Customer.lastname) ? (string)customer[Core.Helper.EntityHelper.CRM.Customer.lastname] : "";

                Erroron = Core.Helper.EntityHelper.CRM.Customer.emailaddress1;
                var emailaddress1 = customer.Contains(Core.Helper.EntityHelper.CRM.Customer.emailaddress1) ? (string)customer[Core.Helper.EntityHelper.CRM.Customer.emailaddress1] : "";
                Erroron = Core.Helper.EntityHelper.CRM.Customer.mobilephone;
                var mobilephone = customer.Contains(Core.Helper.EntityHelper.CRM.Customer.mobilephone) ? (string)customer[Core.Helper.EntityHelper.CRM.Customer.mobilephone] : "";

                return Get(sourceid, channel, firstname, lastname, emailaddress1, mobilephone);
            }
            catch (Exception ex)
            {
                throw new Exception($"Customer Error on : {Erroron}. {ex}");

            }

        }


        private Guid UpdateExistingCustomer(Entity customer, Entity existingCustomer)
        {

            var contactid = new Guid(customer[Core.Helper.EntityHelper.CRM.Customer.entity_id].ToString());
            existingCustomer.Id = contactid;
            customer.Attributes.Remove(Core.Helper.EntityHelper.CRM.Customer.entity_id);
            return UpsertEntity(existingCustomer, customer);
        }

        private Guid UpsertEntity(Entity existingCustomer, Entity customer)
        {
            var Erroron = "";
            try
            {
                if (existingCustomer == null)
                {
                    Erroron = "CreateWithGuid(customer)";
                    return CreateWithGuid(customer);
                }

                if (customer.Contains("kti_socialchannelorigin"))
                {
                    customer.Attributes.Remove("kti_socialchannelorigin");
                }

                if (customer.Contains("kti_sourceid"))
                {
                    customer.Attributes.Remove("kti_sourceid");
                }

                Erroron = "update(order, existingCustomer.Id)";
                Update(customer, existingCustomer.Id);
                return existingCustomer.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Customer Error on : {Erroron}. {ex}");

            }
        }

        public Entity GetByMobile(string Mobile)
        {
            if (Mobile == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.mobilephone, ConditionOperator.Equal, Mobile);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                returnCustomer.Entities.First();
            }

            return null;
        }

        public Entity GetByEmail(string Email)
        {
            if (Email == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.emailaddress1, ConditionOperator.Equal, Email);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                returnCustomer.Entities.First();
            }

            return null;
        }
    }
}
