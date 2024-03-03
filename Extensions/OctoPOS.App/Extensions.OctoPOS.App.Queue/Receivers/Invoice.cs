using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.App.Queue.Receivers;

public class Invoice : CompanySettings
{
    // private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;
    private readonly IDistributedCache _cache;

    //public Invoice(Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> channelManagement, IDistributedCache cache)
    //{
    //    _channelManagement = channelManagement;
    //    _cache = cache;
    //}

    public Invoice(IDistributedCache cache)
    {
        _cache = cache;
    }

    [FunctionName("Octopos_PollInvoice")]
    public void Run([TimerTrigger("0 */20 * * * *")] TimerInfo myTimer, ILogger log)
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
            log.LogInformation($"C# Invoice Queue trigger function processed at " + OctoPOS.Helper.DateTimeHelper.PHTnow().ToString("yyyy MMM, dd t"));
            process(log);

        });

    }

    private bool process(ILogger log)
    {
        //var ChannelConfig = _channelManagement.Get(StoreCode);

        //var Config = new OctoPOS.Service.Config()
        //{
        //    companyid = Companyid,
        //    defaultURL = ChannelConfig.kti_defaulturl,
        //    password = ChannelConfig.kti_password,
        //    redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
        //    username = ChannelConfig.kti_username,
        //    apiAuth = ChannelConfig.kti_appkey
        //};

        config = ConfigHelper.Get();
        OctoPOS.Domain.Invoice InvoiceDomain = new KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain.Invoice(config, _cache);

        var InvoiceAddedForTheLastHour = GetInvoiceListIntheLastHour(InvoiceDomain);
        log.LogInformation("Invoice Count {0}", InvoiceAddedForTheLastHour.Count);

        if (ValidInvoice(InvoiceAddedForTheLastHour))
        {
            return AddToQueueUpsertToCRM(InvoiceAddedForTheLastHour);
        }

        return false;

    }



    private static bool ValidInvoice(List<OctoPOS.Model.DTO.ChannelApps> InvoiceAddedForTheLastHour)
    {
        return InvoiceAddedForTheLastHour is not null && InvoiceAddedForTheLastHour.Count > 0;
    }

    private bool AddToQueueUpsertToCRM(List<OctoPOS.Model.DTO.ChannelApps> InvoiceAddedForTheLastHour)
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

    private void InsertToQueueCRM(List<OctoPOS.Model.DTO.ChannelApps> InvoiceAddedForTheLastHour, QueueClient queueClient)
    {
        foreach (var invoice in InvoiceAddedForTheLastHour)
        {
            var Compress = GetJson(invoice).ToBrotliAsync().GetAwaiter().GetResult().Result.Value;
            queueClient.SendMessage(Compress);
        }
    }

    private List<OctoPOS.Model.DTO.ChannelApps> GetInvoiceListIntheLastHour(OctoPOS.Domain.Invoice invoiceDomain)
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


        var Invoicelist = new List<Model.Invoice>();

        var NotFinish = true;
        var currentPage = 1;


        while (NotFinish)
        {
            var SearchResult = invoiceDomain.SearchInvoiceListWithdetails(startDate, endDate, currentPage);

            if (ValidSearch(SearchResult))
            {
                break;
            }

            Invoicelist.AddRange(SearchResult.values);

            if (IsFinish(SearchResult, currentPage))
            {
                //Finish/Complete
                NotFinish = false;
            }

            currentPage++;
        }

        return GetDTOList(Invoicelist);
    }

    private static bool IsFinish(Model.DTO.Invoices.Search SearchResult, int currentPage)
    {
        return SearchResult.total_pages <= currentPage;
    }

    private static bool ValidSearch(Model.DTO.Invoices.Search SearchResult)
    {
        return SearchResult.values is null || SearchResult.values.Count <= 0;
    }


    private List<Model.DTO.ChannelApps> GetDTOList(List<Model.Invoice> invoiceList)
    {

        if (invoiceList is null || invoiceList.Count <= 0)
        {
            return new List<Model.DTO.ChannelApps>();

        }


        OctoPOS.Domain.Customer CustomerDomain = new(config, _cache);

        //var SearchInvoiceList = InvoiceDomain.GetSearchInvoiceByOrderIDList(orderList);
        //var InvoiceList = SearchInvoiceList.items;

        var returnList = invoiceList.Select(invoice =>
        {
            var CustomerModels = CustomerDomain.Get(invoice.CustomerCode);

            invoice.CustomerDetails = CustomerModels;

            return new OctoPOS.Model.DTO.ChannelApps()
            {
                customer = CustomerModels,
                invoice = invoice
            };


        }).ToList();

        return returnList;
    }

    private string GetJson(object models)
    {

        var JsonSettings = new JsonSerializerSettings
        {

            ContractResolver = new Core.Helper.JSONSerializer.DontIgnoreResolver()
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return json;
    }
}
