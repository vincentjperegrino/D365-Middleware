using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class ReplicatePromoCoupon : IPlugin
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
                    Entity ePromoCoupon = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

                    if (ePromoCoupon.Contains("statecode") && ((OptionSetValue)ePromoCoupon["statecode"]).Value == 0)
                    {
                        var promoCoupon = new Models.Promo.Promo(ePromoCoupon, service);
                        tracingService.Trace(JsonConvert.SerializeObject(promoCoupon));
                        promoCoupon.importsequencenumber = 3388;

                        Process(context.MessageName, promoCoupon, tracingService, service, ePromoCoupon);

                    }

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in ReplicatePromoPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateCustomerPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }

        public void Process(string MessageName, Models.Promo.Promo promoCode, ITracingService tracingService, IOrganizationService service, Entity ForProcessEntity)
        {

            MainProcess(MessageName, promoCode, "Magento", tracingService);
            //  MagentoProcess(MessageName, promoCode, tracingService, service, ForProcessEntity);

        }
        private void MagentoProcess(string MessageName, Models.Promo.Promo promoCode, ITracingService tracingService, IOrganizationService service, Entity ForProcessEntity)
        {
            MainProcess(MessageName, promoCode, "Magento", tracingService);
        }

        private void MainProcess(string MessageName, Models.Promo.Promo promoCode, string ExtenstionName, ITracingService tracingService)
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

                if (MessageName == "Create")
                {
                    var response = Create(httpClient, promoCode, settings).GetAwaiter().GetResult();

                    tracingService.Trace(response);

                }

                else if (MessageName == "Update")
                {
                    var response = Update(httpClient, promoCode, settings).GetAwaiter().GetResult();

                    tracingService.Trace(response);

       
                }

            }

        }



        public static async Task<string> Create(HttpClient httpClient, Models.Promo.Promo promoCode, JsonSerializerSettings settings)
        {

            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(promoCode, Formatting.Indented, settings);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.replicatepromocoupon_path, content);

            return await result.Content.ReadAsStringAsync();
        }

        public static async Task<string> Update(HttpClient httpClient, Models.Promo.Promo promoCode, JsonSerializerSettings settings)
        {
            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(promoCode, Formatting.Indented, settings);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.replicatepromocoupon_path + "/update", content);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
