using System;
using KTI.Moo.Extensions.Core.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Polly;

namespace KTI.Moo.Extensions.SAP.App.Queue.Dispatchers
{
    public class UpsertOrder : CompanySettings
    {
        private readonly Core.Domain.IOrder<Model.Order, Model.OrderItem> _orderdomain;

        public UpsertOrder(IOrder<Model.Order, Model.OrderItem> orderdomain)
        {
            _orderdomain = orderdomain;
        }

        [Singleton(Mode = SingletonMode.Listener)]
        [FunctionName("UpsertOrder")]
        public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-extension-order-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {

            var MaxNumberOfRetry = 5;

            var retryPolicy = Policy.Handle<System.Exception>().WaitAndRetry(
            MaxNumberOfRetry, // number of retries
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
            (exception, timeSpan, retryCount, context) =>
            {
                log.LogInformation(exception.Message);
            });

            retryPolicy.Execute(() =>
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
                process(myQueueItem, log);
                return;
            });


            //try
            //{
            //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            //    process(myQueueItem, log);
            //}


            //catch (System.Exception ex)
            //{

            //    log.LogInformation(ex.Message);
            //    throw new System.Exception(ex.Message);
            //}


        }



        private bool process(string myQueueItem, ILogger log)
        {

            if (!_orderdomain.IsForDispatch(myQueueItem))
            {
                log.LogInformation($"Queue not for dispatch");
                return false;
            }

            var OrderData = JsonConvert.DeserializeObject<JObject>(myQueueItem);

            var salesorderdetailid = "";

            if (OrderData.ContainsKey("salesorderid"))
            {
                salesorderdetailid = OrderData["salesorderid"].Value<string>();
                OrderData.Remove("salesorderid");
            }

            var DocEntry = 0;

            if (OrderData.ContainsKey("DocEntry"))
            {
                DocEntry = OrderData["DocEntry"].Value<int>();
            }

            var WebOrderNo = "";

            if (OrderData.ContainsKey("U_WebOrderNo"))
            {
                WebOrderNo = OrderData["U_WebOrderNo"].Value<string>();
            }


            var CompanyID = Convert.ToInt32(Companyid);

            CRM.Domain.Order order = new(CompanyID);

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
            //    companyid = "3389",
            //    defaultURL = "https://192.168.109.14:30030/b1s/v1",
            //    username = "manager",
            //    password = "1234",
            //    companyDB = "SBO_03042022",
            //    redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
            //};

            //    KTI.Moo.Extensions.SAP.Domain.Order OrderDomain = new(config);



            var ExsistingOrder = _orderdomain.GetByField("U_WebOrderNo", WebOrderNo);

            if (ExsistingOrder.DocEntry > 0 && DocEntry == 0)
            {
                OrderData.Add("DocEntry", ExsistingOrder.DocEntry);
            }


            var MessageQueue = JsonConvert.SerializeObject(OrderData);


            var FromSAPdata = _orderdomain.Upsert(MessageQueue);


            //if for update in Channel Apps
            if (string.IsNullOrWhiteSpace(salesorderdetailid))
            {
                return true;
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

            //TODO: Put in another queue
            order.upsert(Json, salesorderdetailid, log).Wait();
            return true;
        }
    }
}
