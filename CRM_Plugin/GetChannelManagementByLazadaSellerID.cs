﻿using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class GetChannelManagementByLazadaSellerID : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("kti_lazadasellerid"))
                {
                    var channelManagementDomain = new CustomAPI.Domain.ChannelManagement(service);
                    var response = JsonConvert.SerializeObject(channelManagementDomain.GetbyLazadaSellerID((string)context.InputParameters["kti_lazadasellerid"]));

                    context.OutputParameters["kti_getchannelmanagementbylazadasellerid_response"] = response;
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("GetChannelManagementByLazadaSellerID: {0}", ex.ToString());
                throw ex;
            }

        }
    }
}
