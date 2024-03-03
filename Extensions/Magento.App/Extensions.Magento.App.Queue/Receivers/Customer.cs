using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Extensions.Magento.App.Queue.Receivers;

public class Customer : CompanySettings
{

    private readonly Magento.Domain.Customer _CustomerDomain;
    private readonly IDistributedCache _cache;

    public Customer(IDistributedCache cache)
    {
        _CustomerDomain = new(config , cache);
        _cache = cache;
    }

    //Uses UTC
    //[TimerTrigger("0 */10 * * * *")] every 10 minutes
    //[TimerTrigger("0 */10 0-2,14-23 * * *")] every 10 minutes but outside store hours.
    //[TimerTrigger("0 */5 3-13 * * *")] every 5 minutes during store hours. 11am to 9pm
    [FunctionName("Magento_PollCustomer")]
    public void Run([TimerTrigger("0 */1 3-13 * * *")] TimerInfo storeHours, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Customer Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));

            //_ = int.TryParse(Environment.GetEnvironmentVariable("pastminutescoveredCustomer"), out int RecentlyAddedOrderInThePastMinutes);

            //if (RecentlyAddedOrderInThePastMinutes == 0)
            //{
            // 
            //    RecentlyAddedOrderInThePastMinutes = defaultminutes;
            //}

            //_ = int.TryParse(Environment.GetEnvironmentVariable("offsetminutes"), out int addOffsetMinutes);

            //if (addOffsetMinutes == 0)
            //{
            //    var defaultOffsetminutes = -5;
            //    addOffsetMinutes = defaultOffsetminutes;
            //}


            ///For Testing
            //RecentlyAddedOrderInThePastMinutes = -3;
            //addOffsetMinutes = 0;
            //var startDate = DateTime.UtcNow.AddHours(RecentlyAddedOrderInThePastMinutes);
            ///For Testing
            var defaultminutes = -5;
            var defaultOffsetminutes = 1;

            var startDate = DateTime.UtcNow.AddMinutes(defaultminutes);
            var endDate = DateTime.UtcNow.AddMinutes(defaultOffsetminutes);

            process(_CustomerDomain, startDate, endDate, log);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }

    public static bool process(Magento.Domain.Customer customerDomain, DateTime startdate, DateTime enddate, ILogger log)
    {
        //var config = ConfigHelper.Get();

        var CustomerListIntheLastMinutes = GetCustomerListIntheLastMinutes(customerDomain, startdate, enddate);

        log.LogInformation("Customer Count {0}", CustomerListIntheLastMinutes.Count);

        if (ValidCustomer(CustomerListIntheLastMinutes))
        {
            return AddToQueueUpsertToCRM(CustomerListIntheLastMinutes);
        }

        return false;
    }

    private static bool ValidCustomer(List<Magento.Model.DTO.ChannelApps> CustomerAddedForTheMinutes)
    {
        return CustomerAddedForTheMinutes is not null && CustomerAddedForTheMinutes.Count > 0;
    }

    private static bool AddToQueueUpsertToCRM(List<Magento.Model.DTO.ChannelApps> CustomerListAddedIntheLastMinutes)
    {

        // string connectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

        QueueClient queueClient = new(ConnectionString, CustomerQueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        // Create the queue if it doesn't already exist
        queueClient.CreateIfNotExistsAsync().Wait();

        InsertToQueueCRM(CustomerListAddedIntheLastMinutes, queueClient);

        return true;
    }
    private static void InsertToQueueCRM(List<Magento.Model.DTO.ChannelApps> CustomerListAddedIntheLastMinutes, QueueClient queueClient)
    {
        foreach (var Customer in CustomerListAddedIntheLastMinutes)
        {
            var Compress = GetJson(Customer).ToBrotliAsync().GetAwaiter().GetResult().Result.Value;

            queueClient.SendMessage(Compress);
        }
    }

    private static List<Magento.Model.DTO.ChannelApps> GetCustomerListIntheLastMinutes(Magento.Domain.Customer CustomerDomain, DateTime startDate, DateTime endDate)
    {


        _ = int.TryParse(Environment.GetEnvironmentVariable("pageSize"), out int pageSize);

        if (pageSize == 0)
        {
            var defaultpageSize = 100;
            pageSize = defaultpageSize;
        }

        var CustomerList = new List<Magento.Model.Customer>();

        var NotFinish = true;
        var currentPage = 1;

        while (NotFinish)
        {
            var SearchResult = CustomerDomain.GetSearchCustomers(startDate, endDate, pagesize: pageSize, currentPage: currentPage);

            if (ValidSearch(SearchResult))
            {
                break;
            }

            CustomerList.AddRange(SearchResult.values);

            var currentItemCountCovered = pageSize * currentPage;

            if (IsFinish(SearchResult, currentItemCountCovered))
            {
                //Finish/Complete
                NotFinish = false;
            }

            currentPage++;
        }

        var returnList = GetChannelAppsDTO(CustomerList);

        return returnList;
    }


    private static List<Magento.Model.DTO.ChannelApps> GetChannelAppsDTO(List<Magento.Model.Customer> CustomerAddedForTheLastMinute)
    {

        var DTOlist = CustomerAddedForTheLastMinute.Select(customer =>
        {
            return new Magento.Model.DTO.ChannelApps()
            {
                customer = customer
            };

        }).ToList();

        return DTOlist;

    }

    private static bool IsFinish(Model.DTO.Customers.Search SearchResult, int currentItemCountCovered)
    {
        return SearchResult.total_count <= currentItemCountCovered;
    }

    private static bool ValidSearch(Model.DTO.Customers.Search SearchResult)
    {
        return SearchResult.values is null || SearchResult.values.Count <= 0;
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
