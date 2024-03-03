using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using CRM_Plugin.Core.Domain;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class Contact : ICustomer
    {

        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly int _channelOrigin;
        private readonly string _sourceID;

        public Contact(IOrganizationService service, ITracingService tracingService, int channelOrigin, string sourceID = "")
        {
            _service = service;
            _tracingService = tracingService;
            _sourceID = sourceID;
            _channelOrigin = channelOrigin;
        }

        public Contact(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Contact(IOrganizationService service)
        {
            _service = service;
        }


        public bool Create(Entity contact)
        {
            try
            {
                contact.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Contact.entity_name;

                _service.Create(contact);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }


        public bool Update(Entity contact, Guid ExistingID)
        {
            try
            {
                contact.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Contact.entity_name;
                contact.Id = ExistingID;

                _service.Update(contact);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool Upsert(Entity contact)
        {
            var sourceid = (string)contact[CustomAPI.Helper.EntityHelper.Customer.kti_sourceid];
            var channelid = (string)contact[CustomAPI.Helper.EntityHelper.Customer.kti_socialchannelorigin];

            var existingContact = Get(sourceid, channelid);

            if (existingContact is null)
            {
                return Create(contact);
            }

            return Update(contact, existingContact.Id);
        }

        public Entity Get(string sourceid, string channelid)
        {
            if (sourceid == null || channelid == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Contact.entity_name;
            qeEntity.ColumnSet = Core.Model.CustomerBase.columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid, ConditionOperator.Equal, sourceid);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_socialchannelorigin, ConditionOperator.Equal, channelid);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                returnCustomer.Entities.First();
            }

            return null;
        }

        public Entity GetContactEntityByCustomerID(string customerID)
        {
            if(Guid.TryParse(customerID, out Guid _customerGUID))
                return _service.Retrieve(Core.Helper.EntityHelper.CRM.Contact.entity_name, _customerGUID, new ColumnSet(true));

            return null;
        }

        public Entity GetContactEntityByCustomerIDChannelOrigin(string customerID, int channelOrigin)
        {
            QueryExpression qeContact = new QueryExpression();

            qeContact.EntityName = Core.Helper.EntityHelper.CRM.Contact.entity_name;
            qeContact.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Contact.contactid);

            var contactFilter = new FilterExpression(LogicalOperator.And);
            contactFilter.AddCondition(Core.Helper.EntityHelper.CRM.Contact.kti_sourceid, ConditionOperator.Equal, customerID);
            contactFilter.AddCondition(Core.Helper.EntityHelper.CRM.Contact.kti_socialchannelorigin, ConditionOperator.Equal, channelOrigin);
            qeContact.Criteria.AddFilter(contactFilter);

            EntityCollection colContact = _service.RetrieveMultiple(qeContact);

            if (colContact.Entities.Count > 0)
            {
                return colContact.Entities.First();
            }

            return null;
        }

        public CRM_Plugin.Models.Customer.Customer GetContactByCustomerIDChannelOrigin(string customerID, int channelOrigin)
        {
            QueryExpression qeContact = new QueryExpression();

            qeContact.EntityName = Core.Helper.EntityHelper.CRM.Contact.entity_name;
            qeContact.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Contact.contactid);

            var contactFilter = new FilterExpression(LogicalOperator.And);
            contactFilter.AddCondition(Core.Helper.EntityHelper.CRM.Contact.kti_sourceid, ConditionOperator.Equal, customerID);
            contactFilter.AddCondition(Core.Helper.EntityHelper.CRM.Contact.kti_socialchannelorigin, ConditionOperator.Equal, channelOrigin);
            qeContact.Criteria.AddFilter(contactFilter);

            EntityCollection colContact = _service.RetrieveMultiple(qeContact);

            if (colContact.Entities.Count > 0)
            {
                return colContact.Entities.Select(i => new CRM_Plugin.Models.Customer.Customer(i)).First();
            }

            return null;
        }

        public Entity GetContactEntityByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            QueryExpression qeContact = new QueryExpression();

            qeContact.EntityName = Core.Helper.EntityHelper.CRM.Contact.entityimage;
            qeContact.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Contact.contactid);

            var contactFilter = new FilterExpression(LogicalOperator.And);
            contactFilter.AddCondition("fullname", ConditionOperator.Like, fullName);

            var contactInformationFilter = new FilterExpression(LogicalOperator.Or);
            contactFilter.AddCondition("emailaddress1", ConditionOperator.Like, emailAddress);
            contactFilter.AddCondition("telephone2", ConditionOperator.Like, contactNumber);

            contactFilter.AddFilter(contactInformationFilter);

            qeContact.Criteria.AddFilter(contactFilter);

            EntityCollection colContact = _service.RetrieveMultiple(qeContact);

            if (colContact.Entities.Count > 0)
            {
                return colContact.Entities.First();
            }

            return null;
        }

        public CRM_Plugin.Models.Customer.Customer GetContactByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            QueryExpression qeContact = new QueryExpression();

            qeContact.EntityName = Core.Helper.EntityHelper.CRM.Contact.entityimage;
            qeContact.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Contact.contactid);

            var contactFilter = new FilterExpression(LogicalOperator.And);
            contactFilter.AddCondition("fullname", ConditionOperator.Like, fullName);

            var contactInformationFilter = new FilterExpression(LogicalOperator.Or);
            contactFilter.AddCondition("emailaddress1", ConditionOperator.Like, emailAddress);
            contactFilter.AddCondition("telephone2", ConditionOperator.Like, contactNumber);

            contactFilter.AddFilter(contactInformationFilter);

            qeContact.Criteria.AddFilter(contactFilter);

            EntityCollection colContact = _service.RetrieveMultiple(qeContact);

            if (colContact.Entities.Count > 0)
            {
                return colContact.Entities.Select(i => new CRM_Plugin.Models.Customer.Customer(i)).First();
            }

            return null;
        }

        public Entity Get(Guid ExistingID)
        {
            throw new NotImplementedException();
        }

        public Entity GetByMobile(string Mobile)
        {
            throw new NotImplementedException();
        }

        public Entity GetByEmail(string Email)
        {
            throw new NotImplementedException();
        }

        public Guid UpsertWithGuid(Entity customer)
        {
            throw new NotImplementedException();
        }

        public Guid CreateWithGuid(Entity customer)
        {
            throw new NotImplementedException();
        }

        public Entity Get(string sourceid, OptionSetValue channelid)
        {
            throw new NotImplementedException();
        }
    }
}
