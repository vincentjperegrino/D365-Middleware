using Microsoft.Xrm.Sdk;
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

namespace CRM_Plugin.Lazada.Plugins
{
    public class ReplicateOrderStatus_Lazada : IPlugin
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
                    mainProcess(entity, service, tracingService, companyid: 3387);
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateOrderStatusPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateCustomerPlugin: {0}", ex.ToString());
                    throw;
                }

            }
        }

        public bool mainProcess(Entity Order, IOrganizationService service, ITracingService tracingService, int companyid)
        {

            var MainQuery = new QueryExpression
            {
                //Select salesorderid, kti_sourceid, kti_sourceitemid, kti_orderstatus, kti_socialchannelorigin
                ColumnSet = new ColumnSet("salesorderid", "kti_sourceid", "kti_orderstatus", "kti_socialchannelorigin"),
                //From salesorder
                EntityName = Order.LogicalName
            };

            //Inner Join
            var InnerJoinSalesChannel = new LinkEntity("salesorder", "kti_saleschannel", "kti_socialchannel", "kti_saleschannelid", JoinOperator.Inner)
            {
                Columns = new ColumnSet("kti_saleschannelcode"), //Select StoreChannels.kti_saleschannelcode
                EntityAlias = "StoreChannels"
            };

            MainQuery.LinkEntities.Add(InnerJoinSalesChannel);

            //Where
            var MainFilter = new FilterExpression(LogicalOperator.And);
            MainFilter.AddCondition("salesorderid", ConditionOperator.Equal, Order.Id);
            MainQuery.Criteria.AddFilter(MainFilter);

            var ResultOrders = service.RetrieveMultiple(MainQuery);

            if (ResultOrders.Entities.Any())
            {
                var Top1Order = ResultOrders.Entities.FirstOrDefault();

                if (Channel_Lazada(Top1Order))
                {
                    if (OrderStatus_OrderPacked(Top1Order))
                    {
                        var orderstatusDomain = new Lazada.Domain.OrderStatus(service, tracingService, companyid);

                        var orderstatus = orderstatusDomain.OrderPacked(Top1Order);

                        MainProcess(orderstatus, "OrderStatus", tracingService);
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool OrderStatus_OrderPacked(Entity Order)
        {
            return Order.Contains("kti_orderstatus") && ((OptionSetValue)Order["kti_orderstatus"]).Value == Helpers.OrderStatus.OrderPacked;
        }

        private static bool Channel_Lazada(Entity Order)
        {
            return Order.Contains("kti_socialchannelorigin") && ((OptionSetValue)Order["kti_socialchannelorigin"]).Value == Helpers.ChannelOrigin.OptionSet_lazada;
        }



        private void MainProcess(object Order, string ExtenstionName, ITracingService tracingService)
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
