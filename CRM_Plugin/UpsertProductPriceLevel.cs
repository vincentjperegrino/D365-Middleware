using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace CRM_Plugin
{
    public class UpsertProductPriceLevel : IPlugin
    {
        ITracingService tracingService;
        IPluginExecutionContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;
        string EntityName = "Unit Group";

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            try
            {
                serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                service = serviceFactory.CreateOrganizationService(context.UserId);

                Guid recordId = Guid.Empty;
                string message = string.Empty;

                //check the key is present
                if (context.InputParameters.Contains("unitofmeasurement"))
                {
                    Entity uom = (Entity)context.InputParameters["unitofmeasurement"];

                    EntityCollection ecUOM = Helper.GetCRMEntity("uom", service);

                    if (ecUOM.Entities.Count > 0 && uom.Contains("name"))
                    {
                        string uomIdx = (string)uom["name"];

                        var eUom = Helper.GetEntityByStringAttribute(uomIdx, ecUOM, "name");
                        
                        if (eUom.Id != Guid.Empty)
                        {
                            var eUpdate = new Entity(eUom.LogicalName);

                            eUpdate.Id = eUom.Id;

                            eUpdate["name"] = (string)eUom["name"];
                            eUpdate["baseuomname"] = (string)eUom["baseuomname"];

                            service.Update(eUpdate);

                            message = $"{EntityName}{Messages.EntityUpdated}";
                        }
                        else
                        {
                            var eCreate = new Entity("uomschedule");

                            eCreate["name"] = (string)eUom["name"];
                            eCreate["baseuomname"] = (string)eUom["baseuomname"];

                            message = $"{EntityName}{Messages.EntityCreated}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.InnerException != null && ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}
