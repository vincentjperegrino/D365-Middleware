using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.Domain
{
    public class Employee : CRM_Plugin.Core.Domain.IEmployee
    {
        IOrganizationService _service;
        ITracingService _tracingService;

        public Employee(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }
        public bool upsert(Entity order)
        {

            return false;
        }

        public bool create(Entity order)
        {

            return false;
        }

        public bool update(Entity order, Guid guid)
        {

            return false;
        }

        public Entity GetEmployeeByStaffCode(string staffCode)
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.Employee.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_employeeid", ConditionOperator.Equal, staffCode);

            qeEntity.Criteria.AddFilter(entityFilter);

            return _service.RetrieveMultiple(qeEntity).Entities.FirstOrDefault();
        }
    }
}
