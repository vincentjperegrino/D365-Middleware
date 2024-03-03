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
    public class APIScheduleSyncOrderToSAP : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = servicefactory.CreateOrganizationService(context.UserId);

            try
            {
                process(service, tracingService, companyid: 3388);
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

        public bool process(IOrganizationService service, ITracingService tracingService, int companyid)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "salesorder";
            qeEntity.ColumnSet = new ColumnSet("modifiedon", "kti_sapdocnum", "kti_sapdocentry", "statecode");
            var entityFilter = new FilterExpression(LogicalOperator.And);

            var createdonDaysAllowed = 5;
            entityFilter.AddCondition("createdon", ConditionOperator.LastXDays, createdonDaysAllowed);
            var MinusRange = -30;
            var modifiedonStartDate = DateTime.UtcNow.AddMinutes(MinusRange);
            var modifiedonEndDate = DateTime.UtcNow;
            entityFilter.AddCondition("modifiedon", ConditionOperator.Between, modifiedonStartDate, modifiedonEndDate);
            var new_statecode = 0;
            entityFilter.AddCondition("statecode", ConditionOperator.Equal, new_statecode);
            entityFilter.AddCondition("kti_socialchannelorigin", ConditionOperator.In, CRM_Plugin.Helpers.ChannelOrigin.OptionSet_magento, CRM_Plugin.Helpers.ChannelOrigin.OptionSet_lazada);
            //entityFilter.AddCondition("kti_sapdocentry", ConditionOperator.Null);
            //entityFilter.AddCondition("statuscode", ConditionOperator.NotIn, 959080001, 959080002);
            qeEntity.Criteria.AddFilter(entityFilter);

            var OrderForSyncing = service.RetrieveMultiple(qeEntity);

            if (OrderForSyncing.Entities.Any() == false)
            {
                return false;
            }

            var ReplicateOrderPlugin = new CRM_Plugin.ReplicateOrder();

            if (OrderForSyncing.Entities.Any(entity => !entity.Contains("kti_sapdocentry")))
            {
                var ForAddingentityOrder = OrderForSyncing.Entities.Where(entity => !entity.Contains("kti_sapdocentry"));

                foreach (var orders in ForAddingentityOrder)
                {
                    ReplicateOrderPlugin.mainProcess(orders, service, tracingService, companyid);
                }
            }

            if (OrderForSyncing.Entities.Any(entity => entity.Contains("kti_sapdocentry")))
            {
                var ForUpdatingentityOrder = OrderForSyncing.Entities.Where(entity => entity.Contains("kti_sapdocentry"));

                foreach (var orders in ForUpdatingentityOrder)
                {
                    ReplicateOrderPlugin.mainProcess(orders, service, tracingService, companyid);
                }
            }


            return true;
        }

    }
}
