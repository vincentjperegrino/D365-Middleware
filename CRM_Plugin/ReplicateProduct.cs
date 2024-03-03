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
using System.Linq;

namespace CRM_Plugin
{
    public class ReplicateProduct : IPlugin
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
                    ProcessProduct(entity, service, tracingService);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateProductPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateProductPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }

        public bool ProcessProduct(Entity entity, IOrganizationService service, ITracingService tracingService)
        {
            Entity eProduct = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

            //Multiple Store
            var salesChannel = new SalesChannel.Domain.SalesChannel(service, tracingService);

            var product = new CRM_Plugin.Models.Items.Products(eProduct, service);

            if (!String.IsNullOrEmpty(product.kti_sku))
                return false;

            tracingService.Trace(JsonConvert.SerializeObject(product));
            product.importsequencenumber = 3388;

            Process(product, tracingService).GetAwaiter().GetResult();

            return true;
        }

        public async Task Process(Models.Items.Products product, ITracingService tracingService)
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
                httpClient.DefaultRequestHeaders.Add("Instance-Type", "CRM");

                var response = await Create(httpClient, product, settings);

                if (tracingService != null)
                {
                    tracingService.Trace(response);
                }

            }

        }


        public static async Task<string> Create(HttpClient httpClient, Models.Items.Products product, JsonSerializerSettings settings)
        {
            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(product, Formatting.Indented, settings);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.replicateproduct_path, content);

            return await result.Content.ReadAsStringAsync();
        }


    }
}
