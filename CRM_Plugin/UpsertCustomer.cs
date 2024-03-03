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
    public class UpsertCustomer : IPlugin
    {
   
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = servicefactory.CreateOrganizationService(context.UserId);

            string erroron = "";
            try
            {
                erroron = "Error on parameters: ";
                var customParameters = context.InputParameters;
                var EntityName = "Customer";

                if (Checker_Context_Input(customParameters))
                {
                  
                    var DomainCustomer = new CustomAPI.Function.UpsertCustomer(service,tracingService);

                    //erroron = "Error on process: ";
                    string outputMessage = DomainCustomer.Process(customParameters);

                    context.OutputParameters["DTO_Response"] = outputMessage;
                }
            }

            catch (Exception ex)
            {
                tracingService.Trace(erroron + ex.Message);
                throw new Exception(erroron + ex.Message);
            }
        }


        public bool Checker_Context_Input(ParameterCollection customParameters)
        {

            //if (!customParameters.Contains(DTO_EntityHelper.Product.productid))
            //{
            //    throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.productid);
            //}


            return true;
        }
    }
}
