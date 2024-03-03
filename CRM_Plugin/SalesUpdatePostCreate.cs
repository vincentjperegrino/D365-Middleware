using Microsoft.Xrm.Sdk;
using System;
using CRM_Plugin.Function;
using ParameterModel = CRM_Plugin.Models.DTO;
using CRM_Plugin.Domain;

namespace CRM_Plugin
{
    public class SalesUpdatePostCreate : IPlugin
    {
        ITracingService tracingService;
        IPluginExecutionContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = servicefactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity target = (Entity)context.InputParameters["Target"];

                Entity entity = service.Retrieve(target.LogicalName, target.Id, ParameterModel.SalesChannelDTOParameters.columnSet);
                var salesChannel = TransformEntity(entity);

                try
                {
                    if (!string.IsNullOrEmpty(salesChannel.password))
                    {
                        var salesChannelDomain = new SalesChannel.Domain.SalesChannel(service, tracingService);
                        var Function = new SalesChannelHashingAndEncryption(salesChannelDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppPassword(salesChannel);
                    }

                    if (!string.IsNullOrEmpty(salesChannel.appKey))
                    {
                        var salesChannelDomain = new SalesChannel.Domain.SalesChannel(service, tracingService);
                        var Function = new SalesChannelHashingAndEncryption(salesChannelDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppKey(salesChannel);
                    }

                    if (!string.IsNullOrEmpty(salesChannel.appSecret))
                    {
                        var salesChannelDomain = new SalesChannel.Domain.SalesChannel(service, tracingService);
                        var Function = new SalesChannelHashingAndEncryption(salesChannelDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppKey(salesChannel);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error on sales channel creation: " + ex.Message);
                }
            }
        }

        private Models.ChannelManagement.SalesChannel TransformEntity(Entity entity)
        {
            var parameter = new ParameterModel.SalesChannelDTOParameters(entity);
            var salesChannel = new Models.ChannelManagement.SalesChannel()
            {
                salesChannel = parameter.salesChannel,
                password = parameter.password,
                appKey = parameter.appKey,
                appSecret = parameter.appSecret,
                passwordFlag = parameter.passwordFlag,
                appKeyFlag = parameter.appKeyFlag,
                appSecretFlag = parameter.appSecretFlag,
            };

            return salesChannel;
        }
    }
}
