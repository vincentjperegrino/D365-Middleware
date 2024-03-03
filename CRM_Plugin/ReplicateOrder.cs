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
using System.Collections.Generic;
using System.Linq;

namespace CRM_Plugin
{
    public class ReplicateOrder : IPlugin
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
                    tracingService.Trace("ReplicateOrderPlugin: {0}", ex.ToString());
                    throw;
                }

            }
        }


        public bool mainProcess(Entity entity, IOrganizationService service, ITracingService tracingService, int companyid)
        {

            Entity eOrder = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

            var ERCustomer = (EntityReference)eOrder["customerid"];

            var Customer = service.Retrieve(ERCustomer.LogicalName, ERCustomer.Id, new ColumnSet("kti_sourceid"));

            ReplicateCustomer customerPlugin = new ReplicateCustomer();

            customerPlugin.CustomerProcess(Customer, service, tracingService, companyid);

            var orders = new Models.Sales.OrderAndDetails()
            {
                order = new Models.Sales.Order()
                {
                    name = eOrder.Contains("name") ? (string)eOrder["name"] : default,
                    companyid = companyid,
                    emailaddress = eOrder.Contains("emailaddress") ? (string)eOrder["emailaddress"] : default,
                    salesorderid = entity.Id.ToString(),
                    customerid = (string)Customer["kti_sourceid"],
                    datefulfilled = (DateTime)eOrder["createdon"],
                    discountamount = eOrder.Contains("discountamount") ? ((Money)eOrder["discountamount"]).Value : default,
                    discountpercentage = eOrder.Contains("discountpercentage") ? (decimal)eOrder["discountpercentage"] : default,
                    freightamount = eOrder.Contains("freightamount") ? ((Money)eOrder["freightamount"]).Value : default,
                    kti_orderstatus = eOrder.Contains("kti_orderstatus") ? ((OptionSetValue)eOrder["kti_orderstatus"]).Value : default,
                    description = eOrder.Contains("description") ? (string)eOrder["description"] : default,
                    kti_sapdocnum = eOrder.Contains("kti_sapdocnum") ? (int)eOrder["kti_sapdocnum"] : default,
                    kti_sapdocentry = eOrder.Contains("kti_sapdocentry") ? (int)eOrder["kti_sapdocentry"] : default,
                    kti_socialchannelorigin = eOrder.Contains("kti_socialchannelorigin") ? ((OptionSetValue)eOrder["kti_socialchannelorigin"]).Value : default,
                    kti_paymenttermscode = eOrder.Contains("kti_paymenttermscode") ? ((OptionSetValue)eOrder["kti_paymenttermscode"]).Value : default,
                    statuscode = eOrder.Contains("statuscode") ? ((OptionSetValue)eOrder["statuscode"]).Value : default,
                    statecode = eOrder.Contains("statecode") ? ((OptionSetValue)eOrder["statecode"]).Value : default,
                    billto_name = eOrder.Contains("billto_name") ? (string)eOrder["billto_name"] : default,
                    billto_line1 = eOrder.Contains("billto_line1") ? (string)eOrder["billto_line1"] : default,
                    billto_line2 = eOrder.Contains("billto_line2") ? (string)eOrder["billto_line2"] : default,
                    billto_line3 = eOrder.Contains("billto_line3") ? (string)eOrder["billto_line3"] : default,
                    billto_postalcode = eOrder.Contains("billto_postalcode") ? (string)eOrder["billto_postalcode"] : default,
                    billto_stateorprovince = eOrder.Contains("billto_stateorprovince") ? (string)eOrder["billto_stateorprovince"] : default,
                    billto_city = eOrder.Contains("billto_city") ? (string)eOrder["billto_city"] : default,
                    billto_country = eOrder.Contains("billto_country") ? (string)eOrder["billto_country"] : default,
                    billto_telephone = eOrder.Contains("billto_telephone") ? (string)eOrder["billto_telephone"] : default,
                    billto_fax = eOrder.Contains("billto_fax") ? (string)eOrder["billto_fax"] : default,
                    billto_contactName = eOrder.Contains("billto_contactname") ? (string)eOrder["billto_contactname"] : default,
                    shipto_city = eOrder.Contains("shipto_city") ? (string)eOrder["shipto_city"] : default,
                    shipto_contactName = eOrder.Contains("shipto_contactname") ? (string)eOrder["shipto_contactname"] : default,
                    shipto_country = eOrder.Contains("shipto_country") ? (string)eOrder["shipto_country"] : default,
                    shipto_fax = eOrder.Contains("shipto_fax") ? (string)eOrder["shipto_fax"] : default,
                    shipto_line1 = eOrder.Contains("shipto_line1") ? (string)eOrder["shipto_line1"] : default,
                    shipto_line2 = eOrder.Contains("shipto_line2") ? (string)eOrder["shipto_line2"] : default,
                    shipto_line3 = eOrder.Contains("shipto_line3") ? (string)eOrder["shipto_line3"] : default,
                    shipto_name = eOrder.Contains("shipto_name") ? (string)eOrder["shipto_name"] : default,
                    shipto_postalcode = eOrder.Contains("shipto_postalcode") ? (string)eOrder["shipto_postalcode"] : default,
                    shipto_stateorprovince = eOrder.Contains("shipto_stateorprovince") ? (string)eOrder["shipto_stateorprovince"] : default,
                    shipto_telephone = eOrder.Contains("shipto_telephone") ? (string)eOrder["shipto_telephone"] : default,

                },

            };

            var qeEntity = new QueryExpression();
            qeEntity.EntityName = "salesorderdetail";
            qeEntity.ColumnSet = new ColumnSet(true);
            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("salesorderid", ConditionOperator.Equal, entity.Id);
            qeEntity.Criteria.AddFilter(entityFilter);

            var SalesOrderDetailEntityCollection = service.RetrieveMultiple(qeEntity);


            if (SalesOrderDetailEntityCollection.Entities.Any() == false)
            {
                return false;
            }

            var hasnoproduct = false;

            orders.orderdetails = SalesOrderDetailEntityCollection.Entities.Select(details =>
            {
                var returnModel = new Models.Sales.OrderDetails();
                if (details.Contains("productid"))
                {
                    var ERProduct = (EntityReference)details["productid"];
                    var product = service.Retrieve(ERProduct.LogicalName, ERProduct.Id, new ColumnSet(true));
                    returnModel.kti_lineitemnumber = details.Contains("kti_lineitemnumber") ? (string)details["kti_lineitemnumber"] : default;
                    returnModel.salesorderid = entity.Id.ToString();
                    returnModel.productid = (string)product["productnumber"];
                    returnModel.quantity = (decimal)details["quantity"];
                    returnModel.priceperunit = ((Money)details["priceperunit"]).Value;
                    returnModel.manualdiscountamount = details.Contains("manualdiscountamount") ? ((Money)details["manualdiscountamount"]).Value : default;
                    returnModel.tax = details.Contains("tax") ? ((Money)details["tax"]).Value : default;
                    returnModel.parentbundleid = details.Contains("parentbundleid") ? ((Guid)details["parentbundleid"]).ToString() : default;
                    return returnModel;
                }

                hasnoproduct = true;
                return returnModel;

            }).ToList();


            if (hasnoproduct)
            {
                return false;
            }

            Process(orders, tracingService);

            return true;

        }


        public void Process(Models.Sales.OrderAndDetails Order, ITracingService tracingService)
        {
            MagentoProcess(Order, tracingService);
        }

        private void MagentoProcess(Models.Sales.OrderAndDetails Order, ITracingService tracingService)
        {
            MainProcess(Order, "Magento", tracingService);
        }

        private void MainProcess(Models.Sales.OrderAndDetails Order, string ExtenstionName, ITracingService tracingService)
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
                var result = httpClient.PostAsync(Moo.Config.replicateorder_path, content).GetAwaiter().GetResult();

                var resultMessage = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                tracingService.Trace(resultMessage);


            }


        }

        //public static async Task<string> Create(HttpClient httpClient, Models.Customer.Customer customer, JsonSerializerSettings settings)
        //{

        //}

        //public static async Task<string> Update(HttpClient httpClient, Models.Customer.Customer customer, JsonSerializerSettings settings)
        //{
        //    //Add this line for TLS complaience
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //    var jsonObject = JsonConvert.SerializeObject(customer, Formatting.Indented, settings);
        //    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

        //    var result = await httpClient.PostAsync(Moo.Config.replicateorder_path, content);

        //    return await result.Content.ReadAsStringAsync();
        //}

    }
}