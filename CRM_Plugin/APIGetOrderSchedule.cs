using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class APIGetOrderSchedule : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            try
            {
                var Domain = new CustomAPI.Domain.Order(service, tracingService);
                var response = Domain.GetScheduledOrder();
                context.OutputParameters["kti_APIGetOrderScheduleResponse"] = response;
            }
            catch (Exception ex)
            {
                tracingService.Trace("APIGetOrderSchedule: {0}", ex.ToString());
                throw ex;
            }
        }
    }
}
