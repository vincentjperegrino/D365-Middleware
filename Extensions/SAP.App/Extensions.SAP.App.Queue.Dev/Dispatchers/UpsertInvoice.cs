using System;
using KTI.Moo.Extensions.Core.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.Extensions.SAP.App.Queue.Dev.Dispatchers;

public class UpsertInvoice : CompanySettings
{
    private readonly Core.Domain.IInvoice<Model.Invoice, Model.InvoiceItem> _invoiceDomain;

    public UpsertInvoice(Core.Domain.IInvoice<Model.Invoice, Model.InvoiceItem> invoiceDomain)
    {
        _invoiceDomain = invoiceDomain;
    }

    [FunctionName("UpsertInvoice")]
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-extension-invoice-dispatcher", Connection = "AzureQueueConnectionString")]string myQueueItem, ILogger log)
    {
        try
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            if (!_invoiceDomain.IsForDispatch(myQueueItem))
            {
                log.LogInformation($"Queue not for dispatch");
                return;
            }

            var InvoiceData = JsonConvert.DeserializeObject<JObject>(myQueueItem);

            var salesorderdetailid = "";

            if (InvoiceData.ContainsKey("salesinvoiceid"))
            {
                salesorderdetailid = InvoiceData["salesinvoiceid"].Value<string>();
                InvoiceData.Remove("salesinvoiceid");
            }

            var DocEntry = 0;

            if (InvoiceData.ContainsKey("DocEntry"))
            {
                DocEntry = InvoiceData["DocEntry"].Value<int>();
            }

            var ORNo = "";

            if (InvoiceData.ContainsKey("U_ORNo"))
            {
                ORNo = InvoiceData["U_ORNo"].Value<string>();
            }


            var CompanyID = Convert.ToInt32(Companyid);

            CRM.Domain.Invoice invoice = new(CompanyID);


            var ExsistingOrder = _invoiceDomain.GetByField("U_ORNo", ORNo);

            if (ExsistingOrder.DocEntry > 0 && DocEntry == 0)
            {
                InvoiceData.Add("DocEntry", ExsistingOrder.DocEntry);
            }


            var MessageQueue = JsonConvert.SerializeObject(InvoiceData);


            var FromSAPdata = _invoiceDomain.Upsert(MessageQueue);


            //if for update in Channel Apps
            if (DocEntry > 0 || string.IsNullOrWhiteSpace(salesorderdetailid))
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



            //TODO: Put in another queue
            invoice.upsert(Json, salesorderdetailid, log).Wait();

        }


        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }
}
