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
    public class ReplicateProductDemo : IPlugin
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
                    ProcessProductDemo(entity, context.MessageName, service, tracingService);
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

        public bool ProcessProductDemo(Entity entityProductID, string MessageName, IOrganizationService service, ITracingService tracingService)
        {
            Entity eProduct = service.Retrieve(entityProductID.LogicalName, entityProductID.Id, new ColumnSet(true));

            if (eProduct.Contains("statecode") && eProduct.Contains("productstructure"))
            {
                if (((OptionSetValue)eProduct["statecode"]).Value == 0 && (((OptionSetValue)eProduct["productstructure"]).Value == 1))
                {
                    var product = new Models.Items.Products(eProduct, service);
                    if (tracingService != null)
                    {
                        tracingService.Trace(JsonConvert.SerializeObject(product));
                    }

                    product.importsequencenumber = 3388;

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
                        httpClient.DefaultRequestHeaders.Add("Instance-Type", "Lazada");

                        var response = Create(httpClient, product, settings).GetAwaiter().GetResult();

                        if (tracingService != null)
                        {
                            tracingService.Trace(response);

                        }
                        return true;
                    }

                }
            }

            return false;
        }

        public static async Task<string> Create(HttpClient httpClient, Models.Items.Products product, JsonSerializerSettings settings)
        {
            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(product, Formatting.Indented, settings);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.replicateproduct_path + "/lazada", content);

            return await result.Content.ReadAsStringAsync();
        }



    }
}
