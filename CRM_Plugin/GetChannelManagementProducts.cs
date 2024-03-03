using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class GetChannelManagementProducts : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("kti_product_channelcode"))
                {
                    var channelManagementDomain = new CustomAPI.Domain.ChannelManagementInventory(service);
                    var response = JsonConvert.SerializeObject(channelManagementDomain.Get((string)context.InputParameters["kti_product_channelcode"]));

                    context.OutputParameters["kti_product_channelmanagementresponse"] = response;
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("GetChannelManagementProducts: {0}", ex.ToString());
                throw ex;
            }

        }

    }
}
