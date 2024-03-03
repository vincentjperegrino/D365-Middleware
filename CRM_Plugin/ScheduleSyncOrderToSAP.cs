using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel;

namespace CRM_Plugin
{
    public class ScheduleSyncOrderToSAP : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                process(service, tracingService , companyid:3388);
            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in ReplicateOrderPlugin.", ex);
            }

            catch (Exception ex)
            {
                tracingService.Trace("ReplicateOrderPlugin: {0}", ex.ToString());
                throw;
            }
        }

        public bool process(IOrganizationService service, ITracingService tracingService , int companyid)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "salesorder";
            qeEntity.ColumnSet = new ColumnSet(true);
            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("modifiedon", ConditionOperator.LastXHours, 1);
            qeEntity.Criteria.AddFilter(entityFilter);

            var OrderForSyncing = service.RetrieveMultiple(qeEntity);

            if (OrderForSyncing.Entities.Any() == false)
            {
                return false;
            }

            var ReplicateOrderPlugin = new CRM_Plugin.ReplicateOrder();

            foreach (var orders in OrderForSyncing.Entities)
            {
                ReplicateOrderPlugin.mainProcess(orders, service, tracingService, companyid);
            }

            return true;
        }

    }
}
