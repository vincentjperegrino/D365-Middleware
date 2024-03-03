using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.SAP.App.Queue.Dev.Dispatchers;

public class UpsertCustomer : CompanySettings
{
    [FunctionName("UpsertCustomer")]
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-extension-customer-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var CustomerData = JsonConvert.DeserializeObject<JObject>(myQueueItem);

            //KTI.Moo.Extensions.SAP.Service.Config SAPconfig = new()
            //{
            //    companyid = "3389",
            //    defaultURL = "https://192.168.109.220:30030/b1s/v1",
            //    username = "manager",
            //    password = "n0v@rod",
            //    CompanyDB = "SBOLIVE_NOVATEUR3",
            //    redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
            //};

            //KTI.Moo.Extensions.SAP.Service.Config SAPconfig = new()
            //{
            //    companyid = "3388",
            //    defaultURL = "https://192.168.109.14:30030/b1s/v1",
            //    username = "manager",
            //    password = "1234",
            //    companyDB = "SBO_03042022",
            //    redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
            //};


            var contactid = "";

            if (CustomerData.ContainsKey("contactid"))
            {
                contactid = CustomerData["contactid"].Value<string>();
                CustomerData.Remove("contactid");
            }

            var sapbpcode = "";

            if (CustomerData.ContainsKey("CardCode"))
            {
                sapbpcode = CustomerData["CardCode"].Value<string>();
            }
             



            var MessageQueue = JsonConvert.SerializeObject(CustomerData);

            KTI.Moo.Extensions.SAP.Domain.Customer SAPCustomerDomain = new(config);

            if (string.IsNullOrWhiteSpace(sapbpcode))
            {           
                if (CustomerData.ContainsKey("EmailAddress"))
                {
                    var EmailAddress = CustomerData["EmailAddress"].Value<string>();

                    var ReturnCustomerData = SAPCustomerDomain.GetByField(FieldName: "EmailAddress", FieldValue: EmailAddress);

                    if (!string.IsNullOrWhiteSpace(ReturnCustomerData.kti_sapbpcode))
                    {
                        sapbpcode = ReturnCustomerData.kti_sapbpcode;
                    }
                }   
            }


            var FromSAPdata = SAPCustomerDomain.Upsert(MessageQueue, sapbpcode);


            //if for update in Channel Apps
            if (string.IsNullOrWhiteSpace(sapbpcode) || sapbpcode == FromSAPdata.kti_sapbpcode)
            {
                return;
            }

            var UpsertCustomer = new Dictionary<string, string>();

            UpsertCustomer.Add("kti_sapbpcode", FromSAPdata.kti_sapbpcode);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(UpsertCustomer, Formatting.None, settings);

            int companyID = Convert.ToInt32(Companyid);

            CRM.Domain.Customer customer = new(companyID);

            //TODO: Put in another queue
            customer.upsert(Json, contactid, log).Wait();


        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }
}
