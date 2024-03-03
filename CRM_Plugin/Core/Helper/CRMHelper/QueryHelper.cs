using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PickerCustomAPI.Helpers
{
    public static class QueryHelper
    {
        public static EntityCollection getEntityListByGuid(string entityName, ColumnSet columnSet, string attributeName, Guid recordId, IOrganizationService service)
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = entityName;
            qeEntity.ColumnSet = columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(attributeName, ConditionOperator.Equal, recordId);

            qeEntity.Criteria.AddFilter(entityFilter);

            return service.RetrieveMultiple(qeEntity);
        }


    }
}
