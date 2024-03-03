using Microsoft.Xrm.Sdk;
using System;
using CRM_Plugin.Function;
using ParameterModel = CRM_Plugin.Models.DTO;
using ParameterEntityHelper = CRM_Plugin.EntityHelper;
using CRM_Plugin.Domain;

namespace CRM_Plugin
{
    public class SalesChannelPostUpdate : IPlugin
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
                    if (IsPasswordChanged(target, salesChannel))
                    {
                        var salesChannelDomain = new SalesChannel.Domain.SalesChannel(service, tracingService);
                        var Function = new SalesChannelHashingAndEncryption(salesChannelDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppPassword(salesChannel);
                    }

                    if (IsAppKeyChanged(target, salesChannel))
                    {
                        var salesChannelDomain = new SalesChannel.Domain.SalesChannel(service, tracingService);
                        var Function = new SalesChannelHashingAndEncryption(salesChannelDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppKey(salesChannel);
                    }

                    if (IsAppSecretChanged(target, salesChannel))
                    {
                        var salesChannelDomain = new SalesChannel.Domain.SalesChannel(service, tracingService);
                        var Function = new SalesChannelHashingAndEncryption(salesChannelDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppSecret(salesChannel);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Error on sales channel update: " + ex.Message);
                }
            }
        }

        private bool IsPasswordChanged(Entity target, Models.ChannelManagement.SalesChannel salesChannel)
        {

            if (target.Contains(ParameterEntityHelper.SalesChannelParameter.password))
            {
                if (!string.IsNullOrEmpty(salesChannel.password)
                    && string.IsNullOrEmpty(salesChannel.passwordFlag)
                    || !salesChannel.password.Equals(salesChannel.passwordFlag))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAppKeyChanged(Entity target, Models.ChannelManagement.SalesChannel salesChannel)
        {
            if (target.Contains(ParameterEntityHelper.SalesChannelParameter.appKey))
            {
                if (!string.IsNullOrEmpty(salesChannel.appKey)
                     && string.IsNullOrEmpty(salesChannel.appKeyFlag)
                     || !salesChannel.appKey.Equals(salesChannel.appKeyFlag))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAppSecretChanged(Entity target, Models.ChannelManagement.SalesChannel salesChannel)
        {
            if (target.Contains(ParameterEntityHelper.SalesChannelParameter.appSecret))
            {
                if (!string.IsNullOrEmpty(salesChannel.appSecret)
                     && string.IsNullOrEmpty(salesChannel.appSecretFlag)
                     || !salesChannel.appSecret.Equals(salesChannel.appSecretFlag))
                {
                    return true;
                }
            }

            return false;
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
