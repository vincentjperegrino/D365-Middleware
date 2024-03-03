using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KTIdomain = Domain;
namespace KTI.Moo.Extensions.SAP.App.NCCI.Queue;

public class OrderInsertToSAP
{
    [FunctionName("OrderInsertToSAP")]
    public void Run([QueueTrigger("saporder", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        try
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var OrderData = JsonConvert.DeserializeObject<KTI.Moo.Extensions.SAP.Model.Order>(myQueueItem);

            string connectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");



            var salesorderdetailid = OrderData.salesorderid;
            var statuscode = OrderData.statuscode;

            OrderData.salesorderid = default;
            OrderData.statuscode = default;
            OrderData.Address.BillToState = default;
            OrderData.Address.ShipToState = default;

            CRM.Domain.Order order = new(3389);

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

            KTI.Moo.Extensions.SAP.Domain.Order OrderDomain = new(SAPconfig);

            var ExsistingOrder = OrderDomain.GetByField("U_WebOrderNo", OrderData.WebOrderNo);

            if (ExsistingOrder.DocEntry > 0)
            {
                OrderData.DocEntry = ExsistingOrder.DocEntry;
                OrderData.DocNum = ExsistingOrder.DocNum;
            }

            var FromSAPdata = OrderDomain.Upsert(OrderData);

            if (OrderData.DocEntry > 0 && OrderData.DocNum > 0)
            {
                return;
            }

            var UpsertOrder = new Dictionary<string, int>();

            UpsertOrder.Add("kti_sapdocentry", FromSAPdata.DocEntry);
            UpsertOrder.Add("kti_sapdocnum", FromSAPdata.DocNum);

            //if (statuscode == 2)//if equals to pending
            //{
            //    var changetoProcessing = 959080000;
            //    UpsertOrder.Add("statuscode", changetoProcessing);
            //}


            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(UpsertOrder, Formatting.None, settings);

            order.upsert(Json, salesorderdetailid, log).Wait();

        }


        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }


    }
}
