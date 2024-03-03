using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class APIReplicateOrder : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            try
            {

                if (context.InputParameters.Contains("kti_APIReplicateOrderID") && context.InputParameters.Contains("kti_APIReplicateOrderCompanyID"))
                {
                    var Entity = new Entity("salesorder", (Guid)context.InputParameters["kti_APIReplicateOrderID"]);
                    var CompanyID = (int)context.InputParameters["kti_APIReplicateOrderCompanyID"];
                    var Domain = new ReplicateOrder();
                    var response = Domain.mainProcess(Entity, service, tracingService, companyid: CompanyID);

                    context.OutputParameters["kti_APIReplicateOrderResponse"] = response;
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("APIReplicateOrder: {0}", ex.ToString());
                throw ex;
            }
        }


    }
}
