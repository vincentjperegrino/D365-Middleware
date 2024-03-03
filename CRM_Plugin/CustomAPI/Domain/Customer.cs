using CRM_Plugin.Core.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class Customer : ICustomer
    {

        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly CustomAPI.Domain.Contact contactDomain;
        private readonly CustomAPI.Domain.Account accountDomain;
        private readonly int _channelOrigin = 0;
        private readonly string _sourceID = "";

        public Customer(IOrganizationService service, ITracingService tracingService, int channelOrigin, string sourceID = "")
        {
            _service = service;
            _tracingService = tracingService;
            _sourceID = sourceID;
            _channelOrigin = channelOrigin;

            contactDomain = new CustomAPI.Domain.Contact(_service, _tracingService);
            accountDomain = new CustomAPI.Domain.Account(_service, _tracingService);
        }

        public Customer(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;

            contactDomain = new CustomAPI.Domain.Contact(_service, _tracingService);
            accountDomain = new CustomAPI.Domain.Account(_service, _tracingService);
        }

        public Customer(IOrganizationService service)
        {
            _service = service;
        }


        public bool Create(Entity customer)
        {
            try
            {
                customer.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Customer.entity_name;

                var MappedCustomer = MapCustomFields(customer);

                _service.Create(MappedCustomer);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }
        private string createGetID(Entity customer)
        {
            try
            {
                customer.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Customer.entity_name;

                return _service.Create(customer).ToString();
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }


        public bool Update(Entity customer, Guid ExistingID)
        {
            try
            {
                customer.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Customer.entity_name;
                customer.Id = ExistingID;

                var MappedCustomer = MapCustomFields(customer);

                _service.Update(MappedCustomer);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool Upsert(Entity customer)
        {
            var sourceid = (string)customer[CustomAPI.Helper.EntityHelper.Customer.kti_sourceid];
            var channelid = (string)customer[CustomAPI.Helper.EntityHelper.Customer.kti_socialchannelorigin];

            var ExistingCustomer = Get(sourceid, channelid);

            if (ExistingCustomer is null)
            {
                return Create(customer);
            }

            return Update(customer, ExistingCustomer.Id);
        }



        public Entity Get(string sourceid, string channelid)
        {

            if (sourceid == null || channelid == null)
            {

                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
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

        private EntityReference GetBranchCode(string Branch)
        {
            if (string.IsNullOrWhiteSpace(Branch))
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.Branch.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.Branch.kti_branchcode);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.Branch.kti_branchcode, ConditionOperator.Equal, Branch);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnBranch = _service.RetrieveMultiple(qeEntity);

            if (returnBranch.Entities.Any())
            {
                var BranchEntity = returnBranch.Entities.First();

                return new EntityReference(Core.Helper.EntityHelper.Branch.entity_name, BranchEntity.Id);
            }

            return null;
        }



        private Entity MapCustomFields(Entity customer)
        {
            if (customer.Contains(CustomAPI.Helper.EntityHelper.Customer.ncci_customerjoinedbranch))
            {
                var branchcode = GetBranchCode((string)customer[CustomAPI.Helper.EntityHelper.Customer.ncci_customerjoinedbranch]);

                if (branchcode != null)
                {
                    customer[CustomAPI.Helper.EntityHelper.Customer.ncci_customerjoinedbranch] = branchcode;
                }
            }

            return customer;
        }

        public Entity GetCustomerEntityByCustomerID(string sourceID)
        {
            var contact = contactDomain.GetContactEntityByCustomerID(sourceID);

            if (Guid.Empty == contact.Id)
            {
                var accountDomain = new CustomAPI.Domain.Account(_service, _tracingService);

                return accountDomain.GetAccountEntityByCustomerIDChannelOrigin(sourceID);
            }
            else if(Guid.Empty != contact.Id)
            {
                return contact;
            }

            return null;
        }

        public Entity GetCustomerEntityBySourceIDChannelOrigin(string sourceID, int channelOrigin)
        {

            var contact = contactDomain.GetContactEntityByCustomerIDChannelOrigin(sourceID, channelOrigin);

            if (Guid.Empty == contact.Id)
            {
                var accountDomain = new CustomAPI.Domain.Account(_service, _tracingService);

                return accountDomain.GetAccountEntityByCustomerIDChannelOrigin(sourceID);
            }
            else
            {
                return contact;
            }

            return null;
        }

        public string GetCustomerIDBySourceIDChannelOrigin(string sourceID, int channelOrigin)
        {

            var contact = contactDomain.GetContactByCustomerIDChannelOrigin(sourceID, channelOrigin);

            if (String.IsNullOrEmpty(contact.contactid))
            {
                var accountDomain = new CustomAPI.Domain.Account(_service, _tracingService);

                var account = accountDomain.GetAccountByCustomerIDChannelOrigin(sourceID, channelOrigin);

                return account.accountid;
            }
            else
            {
                return contact.contactid;
            }

            return "";
        }

        public EntityReference GetCustomerEntityByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            var contact = contactDomain.GetContactEntityByNameEmailContact(fullName, emailAddress, contactNumber);

            if (Guid.Empty == contact.Id)
            {
                var account = accountDomain.GetAccountEntityByNameEmailContact(fullName, emailAddress, contactNumber);

                if (Guid.Empty != account.Id)
                    return new EntityReference(account.LogicalName, account.Id);
            }
            else
            {
                return new EntityReference(contact.LogicalName, contact.Id);
            }

            return new EntityReference(CRM_Plugin.Core.Helper.EntityHelper.CRM.Customer.entity_name, Guid.Parse(createGetID(CreateContactByNameEmailContact(fullName, emailAddress, contactNumber))));
        }

        public string GetCustomerIDByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            var contact = contactDomain.GetContactByNameEmailContact(fullName, emailAddress, contactNumber);

            if (String.IsNullOrEmpty(contact.contactid))
            {
                var account = accountDomain.GetAccountByNameEmailContact(fullName, emailAddress, contactNumber);

                if(!String.IsNullOrEmpty(account.accountid))
                    return account.accountid;
            } 
            else if (!String.IsNullOrEmpty(contact.contactid))
            {
                return contact.contactid;
            }

            return createGetID(CreateContactByNameEmailContact(fullName, emailAddress, contactNumber));
        }

        private Entity CreateContactByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            Entity contact = new Entity();

            var name = CustomAPI.Helper.NameWrapper.SplitName(fullName, ' ');

            contact[Core.Helper.EntityHelper.CRM.Contact.firstname] = name.FirstName;
            contact[Core.Helper.EntityHelper.CRM.Contact.lastname] = name.LastName;
            contact[Core.Helper.EntityHelper.CRM.Contact.emailaddress1] = emailAddress;
            contact[Core.Helper.EntityHelper.CRM.Contact.telephone2] = contactNumber;

            if(_channelOrigin != 0)
                contact[Core.Helper.EntityHelper.CRM.Contact.kti_socialchannelorigin] = new OptionSetValue(_channelOrigin);

            if (!String.IsNullOrEmpty(_sourceID))
                contact[Core.Helper.EntityHelper.CRM.Contact.kti_sourceid] = _sourceID;

            return contact;
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
