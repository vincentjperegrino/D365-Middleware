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
    public class UpsertUnitGroup : IPlugin
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
                if (context.InputParameters.Contains("unitgroup"))
                {
                    Entity unitGroup = (Entity)context.InputParameters["unitgroup"];

                    EntityCollection ecUnitGroup = Helper.GetCRMEntity("uomschedule", service);

                    if (ecUnitGroup.Entities.Count > 0 && unitGroup.Contains("name"))
                    {
                        string unitGroupIdx = (string)unitGroup["name"];

                        var eUnitGroup = Helper.GetEntityByStringAttribute(unitGroupIdx, ecUnitGroup, "name");
                        
                        if (eUnitGroup.Id != Guid.Empty)
                        {
                            var eUpdate = new Entity(eUnitGroup.LogicalName);

                            eUpdate.Id = eUnitGroup.Id;

                            eUpdate["name"] = (string)unitGroup["name"];
                            eUpdate["baseuomname"] = (string)unitGroup["baseuomname"];

                            service.Update(eUpdate);

                            message = $"{EntityName}{Messages.EntityUpdated}";
                        }
                        else
                        {
                            var eCreate = new Entity("uomschedule");

                            eCreate["name"] = (string)unitGroup["name"];
                            eCreate["baseuomname"] = (string)unitGroup["baseuomname"];

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
