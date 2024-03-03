using System.Collections.Generic;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Extensions.OctoPOS.App.NCCI.Queue;

public class Invoice : CompanySettings
{
    [FunctionName("OctoPOS_QueueInvoice_3388")]
    public void Run([TimerTrigger("0 */20 * * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Invoice Queue trigger function processed at " + OctoPOS.Helper.DateTimeHelper.PHTnow().ToString("yyyy MMM, dd t"));

            process();

        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }

    }

    private bool process()
    {
        //var config = ConfigHelper.Get();
        OctoPOS.Domain.Invoice InvoiceDomain = null;

        var InvoiceAddedForTheLastHour = GetInvoiceListIntheLastHour(InvoiceDomain);

        if (ValidInvoice(InvoiceAddedForTheLastHour))
        {
            return AddToQueueUpsertToCRM(InvoiceAddedForTheLastHour);
        }

        return false;

    }



    private static bool ValidInvoice(List<OctoPOS.Model.Invoice> InvoiceAddedForTheLastHour)
    {
        return InvoiceAddedForTheLastHour is not null && InvoiceAddedForTheLastHour.Count > 0;
    }

    private static bool AddToQueueUpsertToCRM(List<OctoPOS.Model.Invoice> InvoiceAddedForTheLastHour)
    {

       // string connectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

        QueueClient queueClient = new(ConnectionString, InvoiceQueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        // Create the queue if it doesn't already exist
        queueClient.CreateIfNotExistsAsync().Wait();

        InsertToQueueCRM(InvoiceAddedForTheLastHour, queueClient);

        return true;
    }

    private static void InsertToQueueCRM(List<OctoPOS.Model.Invoice> InvoiceAddedForTheLastHour, QueueClient queueClient)
    {
        foreach (var invoice in InvoiceAddedForTheLastHour)
        {
            queueClient.SendMessage(GetJson(invoice));
        }
    }

    private List<OctoPOS.Model.Invoice> GetInvoiceListIntheLastHour(OctoPOS.Domain.Invoice invoiceDomain)
    {
        _ = int.TryParse(Environment.GetEnvironmentVariable("pastminutescovered"), out int RecentlyAddedInvoiceInThePastMinutes);

        if (RecentlyAddedInvoiceInThePastMinutes == 0)
        {
            var defaultminutes = -25;
            RecentlyAddedInvoiceInThePastMinutes = defaultminutes;
        }

        _ = int.TryParse(Environment.GetEnvironmentVariable("offsetminutes"), out int addOffsetMinutes);

        if (addOffsetMinutes == 0)
        {
            var defaultOffsetminutes = -5;
            addOffsetMinutes = defaultOffsetminutes;
        }


        var startDate = OctoPOS.Helper.DateTimeHelper.PHTnow().AddMinutes(RecentlyAddedInvoiceInThePastMinutes);

        var endDate = OctoPOS.Helper.DateTimeHelper.PHTnow().AddMinutes(addOffsetMinutes);//10 mins offset

        var returnList = invoiceDomain.SearchInvoiceList(startDate, endDate);

        return returnList;
    }

    private static string GetJson(object models)
    {

        var JsonSettings = new JsonSerializerSettings
        {
           
            ContractResolver = new Core.Helper.JSONSerializer.DontIgnoreResolver()
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return json;
    }

}
