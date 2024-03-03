using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class UpdateChannelManagementToken : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("kti_updatechannelmanagementtokens_parameters"))
                {
                    var ParameterModel = JsonConvert.DeserializeObject<CustomAPI.Model.DTO.ChannelManagement.SalesChannel>((string)context.InputParameters["kti_updatechannelmanagementtokens_parameters"]);
                    var channelManagementDomain = new CustomAPI.Domain.ChannelManagement(service);
                    var response = channelManagementDomain.UpdateToken(ParameterModel);
                    context.OutputParameters["kti_updatechannelmanagementtokens_response"] = response;
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("UpdateChannelManagementToken: {0}", ex.ToString());
                throw ex;
            }

        }



    }
}
