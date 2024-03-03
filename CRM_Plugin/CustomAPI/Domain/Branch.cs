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
    public class Branch
    {

        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly int _channelOrigin;
        private readonly string _branchCode;

        public Branch(IOrganizationService service, ITracingService tracingService, string branchCode)
        {
            _service = service;
            _tracingService = tracingService;
            _branchCode = branchCode;
        }

        public Branch(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Branch(IOrganizationService service)
        {
            _service = service;
        }


        public bool create(Entity branch)
        {
            try
            {
                branch.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.Branch.entity_name;

                _service.Create(branch);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }


        public bool update(Entity branch, Guid ExistingID)
        {
            try
            {
                branch.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.Branch.entity_name;
                branch.Id = ExistingID;

                _service.Update(branch);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool upsert(Entity branch)
        {
            var branchCode = (string)branch[CRM_Plugin.Core.Helper.EntityHelper.Branch.kti_branchcode];

            var existingAccount = Get(branchCode);

            if (existingAccount is null)
            {
                return create(branch);
            }

            return update(branch, existingAccount.Id);
        }

        public Entity Get(string branchCode)
        {
            if (branchCode == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Account.entity_name;
            qeEntity.ColumnSet = Core.Model.AccountBase.columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(CRM_Plugin.Core.Helper.EntityHelper.Branch.kti_branchcode, ConditionOperator.Equal, branchCode);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnBranch = _service.RetrieveMultiple(qeEntity);

            if (returnBranch.Entities.Any())
            {
                returnBranch.Entities.First();
            }

            return null;
        }

        public Models.Items.Branch GetBranchByBranchCode(string branchCode)
        {
            QueryExpression qeBranch = new QueryExpression();

            qeBranch.EntityName = Core.Helper.EntityHelper.Branch.entity_name;
            qeBranch.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.Branch.entity_id);

            var branchFilter = new FilterExpression(LogicalOperator.And);
            branchFilter.AddCondition(Core.Helper.EntityHelper.Branch.kti_branchcode, ConditionOperator.Equal, branchCode);
            qeBranch.Criteria.AddFilter(branchFilter);

            EntityCollection colBranch = _service.RetrieveMultiple(qeBranch);

            if (colBranch.Entities.Count > 0)
            {
                return colBranch.Entities.Select(i => new Models.Items.Branch(i)).First();
            }

            return null;
        }
    }
}
