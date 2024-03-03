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
    public class Account : ICustomer
    {

        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly int _channelOrigin;
        private readonly string _sourceID;

        public Account(IOrganizationService service, ITracingService tracingService, int channelOrigin, string sourceID = "")
        {
            _service = service;
            _tracingService = tracingService;
            _sourceID = sourceID;
            _channelOrigin = channelOrigin;
        }

        public Account(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Account(IOrganizationService service)
        {
            _service = service;
        }


        public bool Create(Entity account)
        {
            try
            {
                account.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Account.entity_name;

                _service.Create(account);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }


        public bool Update(Entity account, Guid ExistingID)
        {
            try
            {
                account.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Account.entity_name;
                account.Id = ExistingID;

                _service.Update(account);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool Upsert(Entity account)
        {
            var sourceid = (string)account[CustomAPI.Helper.EntityHelper.Customer.kti_sourceid];
            var channelid = (string)account[CustomAPI.Helper.EntityHelper.Customer.kti_socialchannelorigin];

            var existingAccount = Get(sourceid, channelid);

            if (existingAccount is null)
            {
                return Create(account);
            }

            return Update(account, existingAccount.Id);
        }

        public Entity Get(string sourceid, string channelid)
        {
            if (sourceid == null || channelid == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Account.entity_name;
            qeEntity.ColumnSet = Core.Model.AccountBase.columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.kti_sourceid, ConditionOperator.Equal, sourceid);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.kti_socialchannelorigin, ConditionOperator.Equal, channelid);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                returnCustomer.Entities.First();
            }

            return null;
        }

        public Entity GetAccountEntityByCustomerIDChannelOrigin(string customerID)
        {
            if (Guid.TryParse(customerID, out Guid _customerID))
                return _service.Retrieve(Core.Helper.EntityHelper.CRM.Account.entity_name, _customerID, new ColumnSet(true));

            return null;
        }

        public CRM_Plugin.Models.Customer.Account GetAccountByCustomerIDChannelOrigin(string customerID, int channelOrigin)
        {
            QueryExpression qeAccount = new QueryExpression();

            qeAccount.EntityName = Core.Helper.EntityHelper.CRM.Account.entity_name;
            qeAccount.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Account.entity_id);

            var accountFilter = new FilterExpression(LogicalOperator.And);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.kti_sourceid, ConditionOperator.Equal, customerID);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.kti_socialchannelorigin, ConditionOperator.Equal, channelOrigin);
            qeAccount.Criteria.AddFilter(accountFilter);

            EntityCollection colAccount = _service.RetrieveMultiple(qeAccount);

            if (colAccount.Entities.Count > 0)
            {
                return colAccount.Entities.Select(i => new CRM_Plugin.Models.Customer.Account(i)).First();
            }

            return null;
        }

        public Entity GetAccountEntityByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            QueryExpression qeAccount = new QueryExpression();

            qeAccount.EntityName = Core.Helper.EntityHelper.CRM.Account.entity_name;
            qeAccount.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Account.entity_id);

            var accountFilter = new FilterExpression(LogicalOperator.And);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.name, ConditionOperator.Like, fullName);

            var accountInformationFilter = new FilterExpression(LogicalOperator.Or);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.emailaddress1, ConditionOperator.Like, emailAddress);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.telephone1, ConditionOperator.Like, contactNumber);

            accountFilter.AddFilter(accountInformationFilter);

            qeAccount.Criteria.AddFilter(accountFilter);

            EntityCollection colAccount = _service.RetrieveMultiple(qeAccount);

            if (colAccount.Entities.Count > 0)
            {
                return colAccount.Entities.First();
            }

            return null;
        }

        public CRM_Plugin.Models.Customer.Account GetAccountByNameEmailContact(string fullName, string emailAddress, string contactNumber)
        {
            QueryExpression qeAccount = new QueryExpression();

            qeAccount.EntityName = Core.Helper.EntityHelper.CRM.Account.entity_name;
            qeAccount.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Account.entity_id);

            var accountFilter = new FilterExpression(LogicalOperator.And);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.name, ConditionOperator.Like, fullName);

            var accountInformationFilter = new FilterExpression(LogicalOperator.Or);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.emailaddress1, ConditionOperator.Like, emailAddress);
            accountFilter.AddCondition(Core.Helper.EntityHelper.CRM.Account.telephone1, ConditionOperator.Like, contactNumber);

            accountFilter.AddFilter(accountInformationFilter);

            qeAccount.Criteria.AddFilter(accountFilter);

            EntityCollection colAccount = _service.RetrieveMultiple(qeAccount);

            if (colAccount.Entities.Count > 0)
            {
                return colAccount.Entities.Select(i => new CRM_Plugin.Models.Customer.Account(i)).First();
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
