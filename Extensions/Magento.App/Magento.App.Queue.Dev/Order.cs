using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Extensions.Magento.App.Queue.Dev;

public class Order : CompanySettings
{
    //for test
    private Order(Config _config, string _connectionstring)
    {
        config = _config;
        Companyid = _config.companyid;
        ConnectionString = _connectionstring;
        CustomerQueueName = $"{_config.companyid}{Magento.Helper.QueueName.Customer}";
        InvoiceQueueName = $"{_config.companyid}{Magento.Helper.QueueName.Invoice}";
        OrderQueueName = $"{_config.companyid}{Magento.Helper.QueueName.Order}";
    }

    public Order()
    {

    }

    [FunctionName("Dev_Magento_PollOrder")]
    public void Run([TimerTrigger("0 */25 * * * *")] TimerInfo myTimer, ILogger log)
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

    private bool process()
    {
        Magento.Domain.Order OrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(config);

        var OrderAddedForTheLastHour = GetOrderListIntheLastHour(OrderDomain);

        if (ValidOrder(OrderAddedForTheLastHour))
        {
            return AddToQueueUpsertToCRM(OrderAddedForTheLastHour);
        }

        return false;
    }

    private static bool ValidOrder(List<Magento.Model.DTO.ChannelApps> OrderAddedForTheLastHour)
    {
        return OrderAddedForTheLastHour is not null && OrderAddedForTheLastHour.Count > 0;
    }

    private static bool AddToQueueUpsertToCRM(List<Magento.Model.DTO.ChannelApps> OrderAddedForTheLastHour)
    {
        QueueClient queueClient = new(ConnectionString, OrderQueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExistsAsync().Wait();

        InsertToQueueCRM(OrderAddedForTheLastHour, queueClient);

        return true;
    }

    private static void InsertToQueueCRM(List<Magento.Model.DTO.ChannelApps> OrderAddedForTheLastHour, QueueClient queueClient)
    {
        foreach (var Order in OrderAddedForTheLastHour)
        {
            var Compress = GetJson(Order).ToBrotliAsync().GetAwaiter().GetResult().Result.Value;

            queueClient.SendMessage(Compress);
        }
    }

    private List<Magento.Model.DTO.ChannelApps> GetOrderListIntheLastHour(Magento.Domain.Order OrderDomain)
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

        return GetDTOList(orderList);
    }

    private List<Model.DTO.ChannelApps> GetDTOList(List<Model.Order> orderList)
    {

     //   Magento.Domain.Invoice InvoiceDomain = new(config);
        Magento.Domain.Customer CustomerDomain = new(config);

        //var SearchInvoiceList = InvoiceDomain.GetSearchInvoiceByOrderIDList(orderList);
        //var InvoiceList = SearchInvoiceList.items;

        var returnList = orderList.Select(order =>
        {
            var CustomerModels = CustomerDomain.Get(order.customer_id);

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
