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
    public class ScheduleSyncInvoiceToSAP : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                process(service, tracingService , companyid: 3388);
            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in ReplicateInvoicePlugin.", ex);
            }

            catch (Exception ex)
            {
                tracingService.Trace("ReplicateInvoicePlugin: {0}", ex.ToString());
                throw;
            }
        }

        public bool process(IOrganizationService service, ITracingService tracingService ,int companyid)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "invoice";
            qeEntity.ColumnSet = new ColumnSet(true);
            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("modifiedon", ConditionOperator.LastXHours, 2);
            qeEntity.Criteria.AddFilter(entityFilter);

            var InvoiceForSyncing = service.RetrieveMultiple(qeEntity);

            if (InvoiceForSyncing.Entities.Any() == false)
            {
                return false;
            }

            var ReplicateInvoicePlugin = new CRM_Plugin.ReplicateInvoice();

            foreach (var invoice in InvoiceForSyncing.Entities)
            {
                ReplicateInvoicePlugin.mainProcess(invoice, service, tracingService, companyid);
            }

            return true;
        }


    }
}
