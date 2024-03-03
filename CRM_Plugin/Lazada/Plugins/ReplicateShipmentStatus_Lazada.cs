using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Lazada.Plugins
{
    public class ReplicateShipmentStatus_Lazada : IPlugin
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
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateShipmentStatusPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateCustomerPlugin: {0}", ex.ToString());
                    throw;
                }

            }
        }

        public bool mainProcess(Entity Shipment, IOrganizationService service, ITracingService tracingService, int companyid)
        {
            var MainQuery = new QueryExpression
            {
                //Select
                ColumnSet = new ColumnSet("kti_shipmentid", "kti_packageid", "kti_salesorder", "kti_shipment_status"),
                //From kti_shipment    
                EntityName = Shipment.LogicalName
            };

            //Inner Join salesorder as OrderHeader on MainQuery.salesorderid = OrderHeader.salesorderid
            //Select OrderHeader.salesorderid, OrderHeader.kti_socialchannel
            var InnerJoin_SalesOrder = new LinkEntity("kti_shipment", "salesorder", "kti_salesorder", "salesorderid", JoinOperator.Inner)
            {
                Columns = new ColumnSet("salesorderid", "kti_socialchannel", "kti_socialchannelorigin"),
                EntityAlias = "OrderHeader"
            };

            MainQuery.LinkEntities.Add(InnerJoin_SalesOrder);

            //Inner Join kti_saleschannel as StoreChannels on OrderHeader.kti_socialchannel = StoreChannels.kti_saleschannelid
            //Select StoreChannels.kti_saleschannelcode
            var InnerJoinSalesChannel = new LinkEntity("salesorder", "kti_saleschannel", "kti_socialchannel", "kti_saleschannelid", JoinOperator.Inner)
            {
                Columns = new ColumnSet("kti_saleschannelcode"),
                EntityAlias = "StoreChannels"
            };

            InnerJoin_SalesOrder.LinkEntities.Add(InnerJoinSalesChannel);

            // Where
            var MainFilter = new FilterExpression(LogicalOperator.And);
            MainFilter.AddCondition("kti_shipmentid", ConditionOperator.Equal, Shipment.Id);
            MainQuery.Criteria.AddFilter(MainFilter);

            var ResultOrderItems = service.RetrieveMultiple(MainQuery);

            if (ResultOrderItems.Entities.Any())
            {
                var TOP1Shipment = ResultOrderItems.Entities.FirstOrDefault();

                if (Channel_Lazada(TOP1Shipment))
                {
                    if (OrderStatus_ForDispatch(TOP1Shipment))
                    {
                        var orderstatusDomain = new Lazada.Domain.OrderStatus(service, tracingService, companyid);

                        var orderstatus = orderstatusDomain.ForDispatch(TOP1Shipment);

                        MainProcess(orderstatus, "OrderStatus", tracingService);
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool OrderStatus_ForDispatch(Entity Shipment)
        {
            return Shipment.Contains("kti_shipment_status") && ((OptionSetValue)Shipment["kti_shipment_status"]).Value == Helpers.ShipmentStatus.Dispatch;
        }

        private static bool Channel_Lazada(Entity Shipment)
        {
            return Shipment.Contains("OrderHeader.kti_socialchannelorigin") && ((OptionSetValue)((AliasedValue)Shipment["OrderHeader.kti_socialchannelorigin"]).Value).Value == Helpers.ChannelOrigin.OptionSet_lazada;
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
