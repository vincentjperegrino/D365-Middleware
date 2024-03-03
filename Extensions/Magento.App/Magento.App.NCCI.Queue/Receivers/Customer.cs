namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Receivers;

public class Customer : CompanySettings
{
    [FunctionName("Magento_QueueCustomer")]
    public void Run([TimerTrigger("5 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Customer Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
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
        Magento.Domain.Customer CustomerDomain = new(config);

        var CustomerListIntheLastMinutes = GetCustomerListIntheLastMinutes(CustomerDomain);

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

    private List<Magento.Model.DTO.ChannelApps> GetCustomerListIntheLastMinutes(Magento.Domain.Customer CustomerDomain)
    {
        _ = int.TryParse(Environment.GetEnvironmentVariable("pastminutescovered"), out int RecentlyAddedOrderInThePastMinutes);

        if (RecentlyAddedOrderInThePastMinutes == 0)
        {
            var defaultminutes = -25;
            RecentlyAddedOrderInThePastMinutes = defaultminutes;
        }

        _ = int.TryParse(Environment.GetEnvironmentVariable("offsetminutes"), out int addOffsetMinutes);

        if (addOffsetMinutes == 0)
        {
            var defaultOffsetminutes = -5;
            addOffsetMinutes = defaultOffsetminutes;
        }


        ///For Testing
        //RecentlyAddedOrderInThePastMinutes = -3;
        //addOffsetMinutes = 0;
        //var startDate = DateTime.UtcNow.AddHours(RecentlyAddedOrderInThePastMinutes);
        ///For Testing

         var startDate = DateTime.UtcNow.AddMinutes(RecentlyAddedOrderInThePastMinutes);
        var endDate = DateTime.UtcNow.AddMinutes(addOffsetMinutes);

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


    private List<Magento.Model.DTO.ChannelApps> GetChannelAppsDTO(List<Magento.Model.Customer> CustomerAddedForTheLastMinute)
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
