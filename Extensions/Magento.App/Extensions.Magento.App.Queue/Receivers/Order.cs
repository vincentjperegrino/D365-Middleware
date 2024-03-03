using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly;

namespace KTI.Moo.Extensions.Magento.App.Queue.Receivers;

public class Order : CompanySettings
{

    private readonly IDistributedCache _cache;

    public Order(IDistributedCache cache)
    {
        _cache = cache;
    }

    [FunctionName("Magento_PollOrder")] //"0 */25 * * * *"
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
            log.LogInformation($"C# Order Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd t"));

            process(log);

        });

        //try
        //{
        //    log.LogInformation($"C# Order Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd t"));

        //    process(log);
        //}
        //catch (System.Exception ex)
        //{
        //    log.LogInformation(ex.Message);
        //    throw new System.Exception(ex.Message);
        //}

    }

    private bool process(ILogger log)
    {
       var _OrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(config, _cache, log);

        var OrderAddedForTheLastHour = GetOrderListIntheLastHour(_OrderDomain, log);
        log.LogInformation("Order Count {0}", OrderAddedForTheLastHour.Count);
        if (ValidOrder(OrderAddedForTheLastHour))
        {
            return AddToQueueUpsertToCRM(OrderAddedForTheLastHour, log);
        }

        return false;
    }

    private static bool ValidOrder(List<Magento.Model.DTO.ChannelApps> OrderAddedForTheLastHour)
    {
        return OrderAddedForTheLastHour is not null && OrderAddedForTheLastHour.Count > 0;
    }

    private static bool AddToQueueUpsertToCRM(List<Magento.Model.DTO.ChannelApps> OrderAddedForTheLastHour, ILogger log)
    {
        QueueClient queueClient = new(ConnectionString, OrderQueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExistsAsync().Wait();

        InsertToQueueCRM(OrderAddedForTheLastHour, queueClient, log);

        return true;
    }

    private static void InsertToQueueCRM(List<Magento.Model.DTO.ChannelApps> OrderAddedForTheLastHour, QueueClient queueClient, ILogger log)
    {
        foreach (var Order in OrderAddedForTheLastHour)
        {
            var Compress = GetJson(Order).ToBrotliAsync().GetAwaiter().GetResult().Result.Value;

            queueClient.SendMessage(Compress);

            log.LogInformation("Inserted to queue Order id {0}, Order number {1}, Modified date {2} , Created date {3}  , Customer ID {4}", Order.order.order_id, Order.order.increment_id, Order.order.updated_at, Order.order.created_at, Order.order.customerid);
        }
    }

    private List<Magento.Model.DTO.ChannelApps> GetOrderListIntheLastHour(Magento.Domain.Order OrderDomain, ILogger log)
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
            var defaultOffsetminutes = 20;
            addOffsetMinutes = defaultOffsetminutes;
        }

        ///For testing
        //RecentlyAddedOrderInThePastMinutes = -3;
        //addOffsetMinutes = 0;
        ///For testing
        ///

        //var startDate = DateTime.UtcNow.AddHours(RecentlyAddedOrderInThePastMinutes);

        var startDate = DateTime.UtcNow.AddMinutes(RecentlyAddedOrderInThePastMinutes);
        var endDate = DateTime.UtcNow.AddMinutes(addOffsetMinutes);

        _ = int.TryParse(Environment.GetEnvironmentVariable("pageSize"), out int pageSize);

        if (pageSize == 0)
        {
            var defaultpageSize = 100;
            pageSize = defaultpageSize;
        }

        var orderList = new List<Magento.Model.Order>();

        var NotFinish = true;
        var currentPage = 1;

        while (NotFinish)
        {
            var SearchResult = OrderDomain.GetSearchOrders(startDate, endDate, pagesize: pageSize, currentPage: currentPage);

            if (ValidSearch(SearchResult))
            {
                break;
            }

            orderList.AddRange(SearchResult.values);

            var currentItemCountCovered = pageSize * currentPage;

            if (IsFinish(SearchResult, currentItemCountCovered))
            {
                //Finish/Complete
                NotFinish = false;
            }

            currentPage++;
        }

        return GetDTOList(orderList, log);
    }

    private List<Model.DTO.ChannelApps> GetDTOList(List<Model.Order> orderList, ILogger log)
    {

        //   Magento.Domain.Invoice InvoiceDomain = new(config);
        //var SearchInvoiceList = InvoiceDomain.GetSearchInvoiceByOrderIDList(orderList);
        //var InvoiceList = SearchInvoiceList.items;

        Magento.Domain.Customer _CustomerDomain = new(config, _cache, log);


        var returnList = orderList.Select(order =>
        {
            Model.Customer CustomerModels = GetCustomer(log, order, _CustomerDomain);

            //var ValidInvoice = InvoiceList.Where(invoices => invoices.order_id == order.order_id).ToList();
            //order.CustomerDetails.companyid = order.companyid;

            //if (ValidInvoice.Any())
            //{

            //    var invoiceFOrDto = ValidInvoice.FirstOrDefault();

            //    invoiceFOrDto.CustomerDetails = CustomerModels;

            //    //return with invoice
            //    return new Magento.Model.DTO.ChannelApps()
            //    {
            //        customer = CustomerModels,
            //        order = order,
            //     //   invoice = ValidInvoice.FirstOrDefault()
            //    };

            //}

            // return without invoice
            return new Magento.Model.DTO.ChannelApps()
            {
                customer = CustomerModels,
                order = order,
            };


        }).ToList();

        return returnList;
    }

    private static Model.Customer GetCustomer(ILogger log, Model.Order order, Domain.Customer _CustomerDomain, int retry = 0)
    {
        var maxretry = 3;
        
        var CustomerModels = _CustomerDomain.Get(order.customer_id, log);

        if (CustomerModels is null || CustomerModels.customer_id == 0)
        {
            log.LogInformation("No customer data from Customer ID {0}. Order ID {1}, Order Number {2}", CustomerModels.customer_id, order.order_id, order.increment_id);

            if(retry >= maxretry)
            {
                return new Model.Customer();
            }

            retry++;
            return GetCustomer(log, order, _CustomerDomain, retry);
        }

        return CustomerModels;
    }

    private static bool IsFinish(Model.DTO.Orders.Search SearchResult, int currentItemCountCovered)
    {
        return SearchResult.total_count <= currentItemCountCovered;
    }

    private static bool ValidSearch(Model.DTO.Orders.Search SearchResult)
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
