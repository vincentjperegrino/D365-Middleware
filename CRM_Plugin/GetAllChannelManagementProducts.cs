using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class GetAllChannelManagementProducts : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var channelManagementDomain = new CustomAPI.Domain.ChannelManagementInventory(service);
                var response = JsonConvert.SerializeObject(channelManagementDomain.GetChannelList());
                context.OutputParameters["kti_product_channelmanagementlistresponse"] = response;

            }
            catch (Exception ex)
            {
                tracingService.Trace("GetAllChannelManagementProducts: {0}", ex.ToString());
                throw ex;
            }

        }


    }
}
