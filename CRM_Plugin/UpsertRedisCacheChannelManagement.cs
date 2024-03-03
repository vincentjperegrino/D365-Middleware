using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Net;

namespace CRM_Plugin
{
    public class UpsertRedisCacheChannelManagement : IPlugin
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
                    mainProcess(entity, service, tracingService);

                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in Upsert to RedisCache for channel management.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("UpsertRedisCacheChannelManagement: {0}", ex.ToString());
                    throw;
                }

            }
        }


        public bool mainProcess(Entity entity, IOrganizationService service, ITracingService tracingService)
        {

            var RetrievedChannelManagement = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

            var ChannelManagement = new CustomAPI.Model.DTO.ChannelManagement.SalesChannel(RetrievedChannelManagement);

            var OptionSet_lazada = 959_080_006; 

            if (ChannelManagement.kti_channelorigin != OptionSet_lazada)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(ChannelManagement.kti_sellerid))
            {
                return false;
            }


            var key = $"lazada_ph_{ChannelManagement.kti_sellerid}_channelmanagement";
            var value = JsonConvert.SerializeObject(ChannelManagement);

            var storeconfig = new Dictionary<string, string>();

            storeconfig.Add("key", key);
            storeconfig.Add("value", value);

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
                httpClient.DefaultRequestHeaders.Add("Instance-Type", "ChannelManagement");

                //if (MessageName == "Create")
                //{
                var response = Create(httpClient, storeconfig, settings).GetAwaiter().GetResult();

                tracingService.Trace(response);

                //}
                //else if (MessageName == "Update")
                //{
                //    var response = Update(httpClient, customer, settings).GetAwaiter().GetResult();

                //    tracingService.Trace(response);
                //}

            }

            return true;

        }


        public static async Task<string> Create(HttpClient httpClient, Dictionary<string, string> storeconfig, JsonSerializerSettings settings)
        {
            //Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var jsonObject = JsonConvert.SerializeObject(storeconfig, Formatting.Indented, settings);

            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Moo.Config.rediscacheStoreConfig, content);

            return await result.Content.ReadAsStringAsync();
        }


    }
}
