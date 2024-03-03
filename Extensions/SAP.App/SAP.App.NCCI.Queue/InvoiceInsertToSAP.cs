using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTIdomain = Domain;

namespace KTI.Moo.Extensions.SAP.App.NCCI.Queue;

public class InvoiceInsertToSAP
{
    [FunctionName("InvoiceInsertToSAP")]
    public void Run([QueueTrigger("sapinvoice1", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        try
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var InvoiceData = JsonConvert.DeserializeObject<KTI.Moo.Extensions.SAP.Model.Invoice>(myQueueItem);
            var InvoieId = InvoiceData.invoicenumber;
            InvoiceData.invoicenumber = default;


            CRM.Domain.Invoice invoice = new(3389);


            KTI.Moo.Extensions.SAP.Service.Config SAPconfig = new()
            {
                companyid = "3389",
                defaultURL = "https://192.168.109.220:30030/b1s/v1",
                username = "manager",
                password = "n0v@rod",
                companyDB = "SBOLIVE_NOVATEUR3",
                redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
            };

            KTI.Moo.Extensions.SAP.Domain.Invoice InvoiceDomain = new(SAPconfig);

            var FromSAPdata = InvoiceDomain.Upsert(InvoiceData);


            if (InvoiceData.DocEntry > 0 && InvoiceData.DocNum > 0)
            {
                return;
            }


 

            var UpsertInvoice = new Dictionary<string, int>();

            UpsertInvoice.Add("kti_sapdocentry", FromSAPdata.DocEntry);
            UpsertInvoice.Add("kti_sapdocnum", FromSAPdata.DocNum);


            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            }; 

            var Json = JsonConvert.SerializeObject(UpsertInvoice, Formatting.None, settings);

            invoice.upsert(Json, InvoieId, log).Wait();

        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }

    }



}
