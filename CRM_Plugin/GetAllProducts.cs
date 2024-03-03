using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class GetAllProducts : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
   
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var getAllProducts = new CustomAPI.Function.GetAllProducts(service);

                context.OutputParameters["kti_GetAllProducts_Response"] = getAllProducts.Process();

            }
            catch (Exception ex)
            {
                tracingService.Trace("GetAllProducts: {0}", ex.ToString());
                throw ex;
            }

        }
    }
}
