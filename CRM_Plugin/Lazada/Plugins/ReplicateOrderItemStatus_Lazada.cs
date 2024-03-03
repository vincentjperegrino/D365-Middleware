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
    public class ReplicateOrderItemStatus_Lazada : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

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
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateOrderItemStatusPlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateCustomerPlugin: {0}", ex.ToString());
                    throw;
                }

            }
        }

        public bool mainProcess(Entity orderItem, IOrganizationService service, ITracingService tracingService, int companyid)
        {
            var MainQuery = new QueryExpression
            {
                //Select salesorderdetailid, salesorder, kti_orderstatus
                ColumnSet = new ColumnSet("salesorderdetailid", "salesorderid", "kti_orderstatus"),
                //From salesorderdetail     
                EntityName = orderItem.LogicalName
            };

            //Inner Join salesorder as OrderHeader on MainQuery.salesorder = OrderHeader.salesorderid
            //Select OrderHeader.salesorderid, OrderHeader.kti_socialchannel, OrderHeader.kti_socialchannelorigin
            var InnerJoin_SalesOrder = new LinkEntity("salesorderdetail", "salesorder", "salesorderid", "salesorderid", JoinOperator.Inner)
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

            // Where MainQuery.salesorderdetailid = orderItem.Id
            var MainFilter = new FilterExpression(LogicalOperator.And);
            MainFilter.AddCondition("salesorderdetailid", ConditionOperator.Equal, orderItem.Id);
            MainQuery.Criteria.AddFilter(MainFilter);

            var ResultOrderItems = service.RetrieveMultiple(MainQuery);

            if (ResultOrderItems.Entities.Any())
            {
                var TOP1OrderItem = ResultOrderItems.Entities.FirstOrDefault();

                if (Channel_Lazada(TOP1OrderItem))
                {
                    if (OrderStatus_CancelOrder(TOP1OrderItem))
                    {
                        var orderstatusDomain = new Lazada.Domain.OrderStatus(service, tracingService, companyid);

                        var orderstatus = orderstatusDomain.CancelOder(TOP1OrderItem);

                        MainProcess(orderstatus, "OrderStatus", tracingService);
                        return true;
                    }

                }

            }

            return false;
        }

        private static bool OrderStatus_CancelOrder(Entity Order)
        {
            return Order.Contains("kti_orderstatus") && ((OptionSetValue)Order["kti_orderstatus"]).Value == Helpers.OrderStatus.CancelOrder;
        }

        private static bool Channel_Lazada(Entity Order)
        {
            return Order.Contains("OrderHeader.kti_socialchannelorigin") && ((OptionSetValue)((AliasedValue)Order["OrderHeader.kti_socialchannelorigin"]).Value).Value == Helpers.ChannelOrigin.OptionSet_lazada;
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
