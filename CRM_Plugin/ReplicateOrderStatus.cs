﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class ReplicateOrderStatus : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    mainProcess(entity, service, tracingService, companyid: 3388);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateOrderPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateCustomerPlugin: {0}", ex.ToString());
                    throw;
                }

            }
        }

        public bool mainProcess(Entity entity, IOrganizationService service, ITracingService tracingService, int companyid)
        {
            Entity eOrder = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

            var orders = new Models.Sales.SalesOrderStatus(eOrder, service, companyid);

            MainProcess(orders, "OrderStatus", tracingService);
            return true;
        }

        private void MainProcess(Models.Sales.SalesOrderStatus Order, string ExtenstionName, ITracingService tracingService)
        {
            string accessToken = Authenticate.AccessToken.Generate(3388).GetAwaiter().GetResult();

            using (HttpClient httpClient = new HttpClient())
            {
                var settings = new JsonSerializerSettings();

                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                httpClient.BaseAddress = new Uri(Moo.Config.baseurl);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("OC-Api-App-Key", Moo.Config.company_33388_occapikey);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Moo.Config.company_33388_subkey);
                httpClient.DefaultRequestHeaders.Add("Instance-Type", ExtenstionName);
                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var jsonObject = JsonConvert.SerializeObject(Order, Formatting.Indented, settings);

                tracingService.Trace(jsonObject);


                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync(Moo.Config.replicateorderstatus_path, content).GetAwaiter().GetResult();

                var resultMessage = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();


                tracingService.Trace(resultMessage);


            }


        }







    }
}
