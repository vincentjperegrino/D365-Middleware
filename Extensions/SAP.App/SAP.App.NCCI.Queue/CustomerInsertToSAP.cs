using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.SAP.App.NCCI.Queue;

public class CustomerInsertToSAP
{
    [FunctionName("InsertToSAP")]
    public void Run([QueueTrigger("sapcustomer", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var CustomerData = JsonConvert.DeserializeObject<KTI.Moo.Extensions.SAP.Model.Customer>(myQueueItem);

            KTI.Moo.Extensions.SAP.Service.Config SAPconfig = new()
            {
                companyid = "3389",
                defaultURL = "https://192.168.109.220:30030/b1s/v1",
                username = "manager",
                password = "n0v@rod",
                companyDB = "SBOLIVE_NOVATEUR3",
                redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
            };

            //KTI.Moo.Extensions.SAP.Service.Config SAPconfig = new()
            //{
            //    companyid = "3389",
            //    defaultURL = "https://192.168.109.14:30030/b1s/v1",
            //    username = "manager",
            //    password = "1234",
            //    CompanyDB = "SBO_03042022",
            //    redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
            //};

            KTI.Moo.Extensions.SAP.Domain.Customer SAPCustomerDomain = new(SAPconfig);
            SAPCustomerDomain.UpsertbyID(CustomerData);

        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }


    }
}
