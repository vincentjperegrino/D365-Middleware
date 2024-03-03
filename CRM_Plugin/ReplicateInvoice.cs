using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
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
    public class ReplicateInvoice : IPlugin
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

                    mainProcess(entity, service, tracingService , comapanyid: 3388);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in ReplicateInvoicePlugin.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("ReplicateInvoicePlugin: {0}", ex.ToString());
                    throw;
                }

            }
        }

        public bool mainProcess(Entity entity, IOrganizationService service, ITracingService tracingService , int comapanyid)
        {

            Entity eInvoice = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));

            var ERCustomer = (EntityReference)eInvoice["customerid"];

            var Customer = service.Retrieve(ERCustomer.LogicalName, ERCustomer.Id, new ColumnSet("kti_sourceid"));

            ReplicateCustomer customerPlugin = new ReplicateCustomer();

            customerPlugin.CustomerProcess(Customer, service, tracingService, comapanyid);

            if (eInvoice.Contains("statecode") && ((OptionSetValue)eInvoice["statecode"]).Value == 0)
            {
                var invoices = new Models.Sales.InvoiceAndDetails()
                {
                    invoiceHeader = new Models.Sales.InvoiceHeader()
                    {
                        name = eInvoice.Contains("name") ? (string)eInvoice["name"] : default,
                        companyid = comapanyid,
                        invoicenumber = entity.Id.ToString(),
                        salesorderid = eInvoice.Contains("salesorderid") ? ((EntityReference)eInvoice["salesorderid"]).Name : default,
                        emailaddress = eInvoice.Contains("emailaddress") ? (string)eInvoice["emailaddress"] : default,
                        customerid = (string)Customer["kti_sourceid"],
                        kti_invoicedate = eInvoice.Contains("kti_invoicedate") ? (DateTime)eInvoice["kti_invoicedate"] : default,
                        description = eInvoice.Contains("description") ? (string)eInvoice["description"] : default,
                        kti_sapdocnum = eInvoice.Contains("kti_sapdocnum") ? (int)eInvoice["kti_sapdocnum"] : default,
                        kti_sapdocentry = eInvoice.Contains("kti_sapdocentry") ? (int)eInvoice["kti_sapdocentry"] : default,
                        kti_socialchannelorigin = eInvoice.Contains("kti_socialchannelorigin") ? ((OptionSetValue)eInvoice["kti_socialchannelorigin"]).Value : default,
                        kti_paymenttermscode = eInvoice.Contains("kti_paymenttermscode") ? ((OptionSetValue)eInvoice["kti_paymenttermscode"]).Value : default,
                        statuscode = eInvoice.Contains("statuscode") ? ((OptionSetValue)eInvoice["statuscode"]).Value : default,
                        statecode = eInvoice.Contains("statecode") ? ((OptionSetValue)eInvoice["statecode"]).Value : default,
                        billto_name = eInvoice.Contains("billto_name") ? (string)eInvoice["billto_name"] : default,
                        billto_line1 = eInvoice.Contains("billto_line1") ? (string)eInvoice["billto_line1"] : default,
                        billto_line2 = eInvoice.Contains("billto_line2") ? (string)eInvoice["billto_line2"] : default,
                        billto_line3 = eInvoice.Contains("billto_line3") ? (string)eInvoice["billto_line3"] : default,
                        billto_postalcode = eInvoice.Contains("billto_postalcode") ? (string)eInvoice["billto_postalcode"] : default,
                        billto_stateorprovince = eInvoice.Contains("billto_stateorprovince") ? (string)eInvoice["billto_stateorprovince"] : default,
                        billto_city = eInvoice.Contains("billto_city") ? (string)eInvoice["billto_city"] : default,
                        billto_country = eInvoice.Contains("billto_country") ? (string)eInvoice["billto_country"] : default,
                        billto_telephone = eInvoice.Contains("billto_telephone") ? (string)eInvoice["billto_telephone"] : default,
                        billto_fax = eInvoice.Contains("billto_fax") ? (string)eInvoice["billto_fax"] : default,
                        billto_contactName = eInvoice.Contains("billto_contactname") ? (string)eInvoice["billto_contactname"] : default,
                        shipto_city = eInvoice.Contains("shipto_city") ? (string)eInvoice["shipto_city"] : default,
                        shipto_contactName = eInvoice.Contains("shipto_contactname") ? (string)eInvoice["shipto_contactname"] : default,
                        shipto_country = eInvoice.Contains("shipto_country") ? (string)eInvoice["shipto_country"] : default,
                        shipto_fax = eInvoice.Contains("shipto_fax") ? (string)eInvoice["shipto_fax"] : default,
                        shipto_line1 = eInvoice.Contains("shipto_line1") ? (string)eInvoice["shipto_line1"] : default,
                        shipto_line2 = eInvoice.Contains("shipto_line2") ? (string)eInvoice["shipto_line2"] : default,
                        shipto_line3 = eInvoice.Contains("shipto_line3") ? (string)eInvoice["shipto_line3"] : default,
                        shipto_name = eInvoice.Contains("shipto_name") ? (string)eInvoice["shipto_name"] : default,
                        shipto_postalcode = eInvoice.Contains("shipto_postalcode") ? (string)eInvoice["shipto_postalcode"] : default,
                        shipto_stateorprovince = eInvoice.Contains("shipto_stateorprovince") ? (string)eInvoice["shipto_stateorprovince"] : default,
                        shipto_telephone = eInvoice.Contains("shipto_telephone") ? (string)eInvoice["shipto_telephone"] : default,
                    },


                };

                var qeEntity = new QueryExpression();
                qeEntity.EntityName = "invoicedetail";
                qeEntity.ColumnSet = new ColumnSet(true);
                var entityFilter = new FilterExpression(LogicalOperator.And);
                entityFilter.AddCondition("invoiceid", ConditionOperator.Equal, entity.Id);
                qeEntity.Criteria.AddFilter(entityFilter);

                var InvoiceDetailEntityCollection = service.RetrieveMultiple(qeEntity);


                if (InvoiceDetailEntityCollection.Entities.Any() == false)
                {
                    return false;
                }


                invoices.invoiceDetails = InvoiceDetailEntityCollection.Entities.Select(details =>
                {

                    var ERProduct = (EntityReference)details["productid"];

                    var product = service.Retrieve(ERProduct.LogicalName, ERProduct.Id, new ColumnSet(true));

                    var returnModel = new Models.Sales.InvoiceDetails();
                    returnModel.kti_lineitemnumber = details.Contains("kti_lineitemnumber") ? (string)details["kti_lineitemnumber"] : default;
                    returnModel.invoiceid = entity.Id.ToString();
                    returnModel.productid = (string)product["productnumber"];
                    returnModel.quantity = (decimal)details["quantity"];
                    returnModel.priceperunit = ((Money)details["priceperunit"]).Value;
                    returnModel.manualdiscountamount = details.Contains("manualdiscountamount") ? ((Money)details["manualdiscountamount"]).Value : default;
                    returnModel.tax = details.Contains("tax") ? ((Money)details["tax"]).Value : default;


                    return returnModel;

                }).ToList();

                Process(invoices, tracingService);

                return true;
            }

            return false;
        }


        public void Process(Models.Sales.InvoiceAndDetails Invoice, ITracingService tracingService)
        {
            MagentoProcess(Invoice, tracingService);
        }

        private void MagentoProcess(Models.Sales.InvoiceAndDetails Invoice, ITracingService tracingService)
        {
            MainProcess(Invoice, "Magento", tracingService);
        }

        private void MainProcess(Models.Sales.InvoiceAndDetails Invoice, string ExtenstionName, ITracingService tracingService)
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
                var jsonObject = JsonConvert.SerializeObject(Invoice, Formatting.Indented, settings);

                if (tracingService != null)
                {
                    tracingService.Trace(jsonObject);
                }

                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var result = httpClient.PostAsync(Moo.Config.replicateinvoice_path, content).GetAwaiter().GetResult();

                var resultMessage = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (tracingService != null)
                {
                    tracingService.Trace(resultMessage);
                }

            }


        }

    }
}
