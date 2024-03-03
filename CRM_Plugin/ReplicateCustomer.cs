using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Xrm.Sdk.Query;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class ReplicateCustomer : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
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
                    CustomerProcess(entity, service, tracingService, companyid: 3388);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateCustomerPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateCustomerPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }

        public void CustomerProcess(Entity entity, IOrganizationService service, ITracingService tracingService, int companyid)
        {
            Entity eCustomer = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

            if (eCustomer.Contains("statecode") && ((OptionSetValue)eCustomer["statecode"]).Value == 0)
            {
                var customer = new Models.Customer.Customer(eCustomer, service);

                tracingService.Trace(JsonConvert.SerializeObject(customer));

                customer.importsequencenumber = companyid;

                Process(customer, tracingService);

            }
        }


        public void Process(Models.Customer.Customer customer, ITracingService tracingService)
        {
            MagentoProcess(customer, tracingService);
        }

        private void MagentoProcess(Models.Customer.Customer customer, ITracingService tracingService)
        {
            MainProcess(customer, "Magento", tracingService);
        }

        private void MainProcess(Models.Customer.Customer customer, string ExtenstionName, ITracingService tracingService)
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

                //if (MessageName == "Create")
                //{
                var response = Create(httpClient, customer, settings).GetAwaiter().GetResult();

                tracingService.Trace(response);

                //}
                //else if (MessageName == "Update")
                //{
                //    var response = Update(httpClient, customer, settings).GetAwaiter().GetResult();

                //    tracingService.Trace(response);
                //}

            }


        }



        public static async Task<string> Create(HttpClient httpClient, Models.Customer.Customer customer, JsonSerializerSettings settings)
        {
            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(customer, Formatting.Indented, settings);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.replicatecustomer_path, content);

            return await result.Content.ReadAsStringAsync();
        }

        public static async Task<string> Update(HttpClient httpClient, Models.Customer.Customer customer, JsonSerializerSettings settings)
        {
            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(customer, Formatting.Indented, settings);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.replicatecustomer_path + "/update", content);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
