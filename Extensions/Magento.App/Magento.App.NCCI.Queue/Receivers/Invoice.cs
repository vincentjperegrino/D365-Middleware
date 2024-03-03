namespace KTI.Moo.Extensions.Magento.App.NCCI.Queue.Receivers;

public class Invoice : CompanySettings
{

    //for test
    private Invoice(Config _config, string _connectionstring)
    {
        config = _config;
        Companyid = _config.companyid;
        ConnectionString = _connectionstring;
        CustomerQueueName = $"{_config.companyid}{Magento.Helper.QueueName.Customer}";
        InvoiceQueueName = $"{_config.companyid}{Magento.Helper.QueueName.Invoice}";
        OrderQueueName = $"{_config.companyid}{Magento.Helper.QueueName.Order}";
    }

    public Invoice()
    {

    }

    [FunctionName("Magento_QueueInvoice")]
    public void Run([TimerTrigger("0 */20 * * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Order Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd t"));

            process();

        }

        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }


    //private bool Testprocess()
    //{
    //    //var config = ConfigHelper.Get();
    //    Magento.Domain.Invoice InvoiceDomain = new(config);
    //    Magento.Domain.Order OrderDomain = new(config);


    //    var Invoices = InvoiceDomain.Get(1);

    //    var InvoiceAddedForTheLastHour = new List<Magento.Model.Invoice>()
    //    {
    //        Invoices
    //    };

    //    var DTOlist = InvoiceAddedForTheLastHour.Select(invoice =>
    //    {
    //        var OrderModel = OrderDomain.Get(invoice.order_id);

    //        //add customer to Invoice
    //        invoice.CustomerDetails.customer_id = OrderModel.CustomerDetails.customer_id;

    //        return new Magento.Model.DTO.ChannelApps()
    //        {
    //            order = OrderModel,
    //            customer = OrderModel.CustomerDetails,
    //            invoice = invoice
    //        };

    //    }).ToList();


    //    if (ValidInvoice(DTOlist))
    //    {
    //        return AddToQueueUpsertToCRM(DTOlist);
    //    }

    //    return false;

    //}


    private bool process()
    {
        //var config = ConfigHelper.Get();
        Magento.Domain.Invoice InvoiceDomain = new(config);

        var InvoiceAddedForTheLastHour = GetInvoiceListIntheLastHour(InvoiceDomain);

        if (ValidInvoice(InvoiceAddedForTheLastHour))
        {
            return AddToQueueUpsertToCRM(InvoiceAddedForTheLastHour);
        }

        return false;
    }
    private static bool ValidInvoice(List<Magento.Model.DTO.ChannelApps> InvoiceAddedForTheLastHour)
    {
        return InvoiceAddedForTheLastHour is not null && InvoiceAddedForTheLastHour.Count > 0;
    }

    private static bool AddToQueueUpsertToCRM(List<Magento.Model.DTO.ChannelApps> InvoiceAddedForTheLastHour)
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
    private static void InsertToQueueCRM(List<Magento.Model.DTO.ChannelApps> InvoiceAddedForTheLastHour, QueueClient queueClient)
    {
        foreach (var Invoice in InvoiceAddedForTheLastHour)
        {
            var Compress = GetJson(Invoice).ToBrotliAsync().GetAwaiter().GetResult().Result.Value;

            queueClient.SendMessage(Compress);
        }
    }

    private List<Magento.Model.DTO.ChannelApps> GetInvoiceListIntheLastHour(Magento.Domain.Invoice InvoiceDomain)
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
            var defaultOffsetminutes = 0;
            addOffsetMinutes = defaultOffsetminutes;
        }

        /// for testing
        //RecentlyAddedOrderInThePastMinutes = 5;
        //addOffsetMinutes = 0;

        /// for testing

        //var startDate = DateTime.UtcNow.AddHours(RecentlyAddedOrderInThePastMinutes);

        var startDate = DateTime.UtcNow.AddMinutes(RecentlyAddedOrderInThePastMinutes);
        var endDate = DateTime.UtcNow.AddMinutes(addOffsetMinutes);

        _ = int.TryParse(Environment.GetEnvironmentVariable("pageSize"), out int pageSize);

        if (pageSize == 0)
        {
            var defaultpageSize = 100;
            pageSize = defaultpageSize;
        }



        var InvoiceList = new List<Magento.Model.Invoice>();

        var NotFinish = true;
        var currentPage = 1;

        while (NotFinish)
        {
            var SearchResult = InvoiceDomain.GetSearchInvoice(startDate, endDate, pagesize: pageSize, currentPage: currentPage);

            if (ValidSearch(SearchResult))
            { 
                break;
            }

            InvoiceList.AddRange(SearchResult.values);

            var currentItemCountCovered = pageSize * currentPage;

            if (IsFinish(SearchResult, currentItemCountCovered))
            {
                //Finish/Complete
                NotFinish = false;
            }

            currentPage++;
        }

        var returnList = GetDTOList(InvoiceList);

        return returnList;
    }


    private List<Magento.Model.DTO.ChannelApps> GetDTOList(List<Magento.Model.Invoice> InvoiceAddedForTheLastHour)
    {

        //var config = ConfigHelper.Get();
        Magento.Domain.Order OrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(config);
        Magento.Domain.Customer CustomerDomain = new(config);


        var DTOlist = InvoiceAddedForTheLastHour.Select(invoice =>
         {
             var OrderModel = OrderDomain.Get(invoice.order_id);

             var CustomerModel = CustomerDomain.Get(OrderModel.customer_id);

             //add customer to Invoice
             invoice.customerid = OrderModel.customerid;

             return new Magento.Model.DTO.ChannelApps()
             {
                 order = OrderModel,
                 customer = CustomerModel,
                 invoice = invoice
             };

         }).ToList();

        return DTOlist;

    }

    private static bool IsFinish(Model.DTO.Invoices.Search SearchResult, int currentItemCountCovered)
    {
        return SearchResult.total_count <= currentItemCountCovered;
    }

    private static bool ValidSearch(Model.DTO.Invoices.Search SearchResult)
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
