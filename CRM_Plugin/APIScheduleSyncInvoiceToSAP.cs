using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class APIScheduleSyncInvoiceToSAP : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = servicefactory.CreateOrganizationService(context.UserId);

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

        public bool process(IOrganizationService service, ITracingService tracingService , int companyid)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "invoice";
            qeEntity.ColumnSet = new ColumnSet(true);
            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("modifiedon", ConditionOperator.LastXHours,1);
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
