using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Lazada.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Operation.Lazada.Domain.Reports;

public class Order : KTI.Moo.Operation.Core.Domain.Reports.IOrder<KTI.Moo.Extensions.Lazada.Model.OrderHeader, KTI.Moo.CRM.Model.OrderBase>
{
    private readonly string _connectionString;
    private readonly bool _IsProduction;
    private readonly string _repushedQueuename;
    private readonly string _moo_webhookURL;
    private readonly string _client_webhookURL;
    private readonly string _crm_view_link;
    private readonly string _lazadaSellerID;
    private readonly INotification _notificationDomain;
    private readonly KTI.Moo.Extensions.Core.Domain.ISearch<KTI.Moo.Extensions.Lazada.Model.DTO.OrderSearch, KTI.Moo.Extensions.Lazada.Model.OrderHeader> _extensionsSearchDomain;
    private readonly KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> _crmSearchDomain;
    private readonly KTI.Moo.Base.Domain.ISearch<KTI.Moo.CRM.Model.DTO.CustomerSearch, KTI.Moo.CRM.Model.CustomerBase> _crmCustomerSearchDomain;

    public Order(string connectionString,
                      bool IsProduction,
                      string repushedQueuename,
                      string client_webhookURL,
                      string moo_webhookURL,
                      string crm_view_link,
                      string lazadaSellerID,
                      INotification notificationDomain,
                      Extensions.Core.Domain.ISearch<Extensions.Lazada.Model.DTO.OrderSearch, OrderHeader> extensionsSearchDomain,
                      Base.Domain.ISearch<CRM.Model.DTO.Orders.Search, OrderBase> crmSearchDomain,
                      Base.Domain.ISearch<CRM.Model.DTO.CustomerSearch, CustomerBase> crmCustomerSearchDomain
                     )
    {
        _connectionString = connectionString;
        _IsProduction = IsProduction;
        _repushedQueuename = repushedQueuename;
        _client_webhookURL = client_webhookURL;
        _moo_webhookURL = moo_webhookURL;
        _crm_view_link = crm_view_link;
        _lazadaSellerID = lazadaSellerID;
        _notificationDomain = notificationDomain;
        _extensionsSearchDomain = extensionsSearchDomain;
        _crmSearchDomain = crmSearchDomain;
        _crmCustomerSearchDomain = crmCustomerSearchDomain;
    }


    public bool Process(DateTime StartDate, DateTime EndDate, ILogger log)
    {
        var OrderListExtensions = GetListFromExtention(StartDate, EndDate).Where(order => !order.laz_Statuses.Contains("unpaid")).ToList();
        var OrderListCRM = GetListFromCRM(StartDate, EndDate).Where(order => order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_lazada).ToList();

        var CustomerListCRM = GetListFromCustomerCRM(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.mobilephone)).ToList();

        var CustomerOrderListExtensions = OrderListExtensions.Select(order => order.laz_customer.mobilephone.FormatPhoneNumber()).Distinct().ToList();

        var CommonCustomers = CustomerOrderListExtensions.Where(mobile => CustomerListCRM.Any(crmCustomer => crmCustomer.mobilephone.FormatPhoneNumber() == mobile)).ToList();

        var CommonOrders = OrderListExtensions.Where(order => OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == order.kti_sourceid)).ToList();

        var Channel = "Lazada";
        var DomainName = "Order";
        var Production = _IsProduction ? "Production" : "Test";
        var Summarytitle = $"{Production}: {Channel} to CRM - {DomainName} Report";
        var SummaryLink = _crm_view_link;
        var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonOrders.Count, OrderListExtensions.Count, CustomerOrderListExtensions.Count, CommonCustomers.Count, log);

        var IsSuccessSendingSummaryMessage = SendMessage(Summarytitle, SummaryMessage, log);

        if (IsSuccessSendingSummaryMessage)
        {
            log.LogInformation($"Success sending summary message");

            var MissingOrders = OrderListExtensions.Where(order => !OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == order.kti_sourceid)).ToList();

            var IsSuccessSendingToRetryQueue = SendToRetryQueue(MissingOrders, log);

            if (IsSuccessSendingToRetryQueue)
            {
                log.LogInformation($"Success sending to retry queue");

                var IsSucessSendingMissingMessage = HandleSendingMissingOrders(MissingOrders, log);

                if (IsSucessSendingMissingMessage)
                {
                    log.LogInformation($"Success sending missing message");

                    return IsSuccessSendingToRetryQueue;
                }

            }
        }

        log.LogInformation($"No Missing {DomainName}/s");

        return true;
    }


    public List<OrderBase> GetListFromCRM(DateTime StartDate, DateTime EndDate)
    {
        return _crmSearchDomain.GetAll(StartDate, EndDate);
    }

    public List<CustomerBase> GetListFromCustomerCRM(DateTime StartDate, DateTime EndDate)
    {
        return _crmCustomerSearchDomain.GetAll(StartDate, EndDate);
    }

    public List<OrderHeader> GetListFromExtention(DateTime StartDate, DateTime EndDate)
    {
        StartDate = DateTimeHelper.UTC_to_PHT(StartDate);
        EndDate = DateTimeHelper.UTC_to_PHT(EndDate);
        return _extensionsSearchDomain.GetAll(StartDate, EndDate);
    }

    public bool Notify(string WebhookUrl, string Title, string Message, ILogger log)
    {
        return _notificationDomain.Notify(WebhookUrl, Title, Message, log);
    }

    public bool SendToRetryQueue(List<OrderBase> CRM_Model, ILogger log)
    {
        throw new NotImplementedException();
    }

    public bool SendToRetryQueue(List<OrderHeader> Extension_Model, ILogger log)
    {
        if (Extension_Model is null || Extension_Model.Count <= 0)
        {
            return false;
        }

        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Base.Helpers.JSONSerializerHelper.DontIgnoreResolver()
        };

        foreach (var order in Extension_Model)
        {

            var DTO = new KTI.Moo.Extensions.Lazada.Model.Queue.Message()
            {
                SellerId = _lazadaSellerID,
                Data = new Dictionary<string, dynamic>()
                {
                    { "order_status", order.laz_Statuses.First() },
                    { "trade_order_id", order.kti_sourceid },
                    { "buyer_id", $"{order.kti_sourceid}_{order.laz_customer.firstname}"},
                }
            };

            var json = JsonConvert.SerializeObject(DTO, Formatting.None, JsonSettings);

            SendToRetryQueue(json);
        }

        return true;
    }

    private bool SendToRetryQueue(string message)
    {
        var queueClient = new QueueClient(_connectionString, _repushedQueuename, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();

        queueClient.SendMessage(message);

        return true;
    }

    private bool SendMessage(string title, string message, ILogger log)
    {
        log.LogInformation($"{title}. {message}");

        if (string.IsNullOrWhiteSpace(_client_webhookURL))
        {
            log.LogInformation($"No webhook url for notification");
            return false;
        }

        Notify(_client_webhookURL, title, message, log);

        if (!string.IsNullOrWhiteSpace(_moo_webhookURL) && _client_webhookURL != _moo_webhookURL)
        {
            Notify(_moo_webhookURL, title, message, log);
        }

        return true;
    }

    private static string GetTable(List<Extensions.Lazada.Model.OrderHeader> Extension_ListModel, int currentCount)
    {
        var TableHeader = $@"<tr>
                             <th>No.</th>
                             <th>Order ID</th>
                             <th>Created On(PHT)</th>
                             <th>Modified On(PHT)</th>
                             </tr>";

        var TableRecord = "";
        foreach (var order in Extension_ListModel)
        {
            TableRecord += $@"<tr>
                             <td>{currentCount}</td>
                             <td>{order.kti_sourceid}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(order.laz_CreatedOn)}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(order.laz_UpdatedOn)}</td>
                             </tr>";
            currentCount++;
        }

        return HTML.TableBorder(TableHeader + TableRecord);
    }

    private string GetSummaryMessage(string Channel, string DomainName, string Summarylink, DateTime StartDate, DateTime EndDate, int MatchExtensionOrderCount, int ExtensionOrderCount, int ExtensionCustomerCount, int MatchExtensionCustomerCount, ILogger log)
    {
        var PHTstart = DateTimeHelper.UTC_to_PHT(StartDate);
        var PHTend = DateTimeHelper.UTC_to_PHT(EndDate);
        var DateRange = $"{PHTstart:yyyy MMM, dd hh:mm tt} - {PHTend:yyyy MMM, dd hh:mm tt}";

        if (PHTstart.ToString("yyyy MMM, dd") == PHTend.ToString("yyyy MMM, dd"))
        {
            DateRange = $"{PHTstart:yyyy MMM, dd hh:mm tt} - {PHTend:hh:mm tt}";
        }

        if (MatchExtensionOrderCount == ExtensionOrderCount)
        {
            log.LogInformation($"{Channel} and CRM {DomainName} is matched");
        }

        return @$"Report for {DateRange}<br/>
                  <ul>
                  <li>{Channel} {DomainName}/s: <strong>{ExtensionOrderCount}</strong></li>
                  <li>{Channel} {DomainName}/s match in CRM: <strong>{MatchExtensionOrderCount}</strong></li>
                  <li>{Channel} Customer/s: <strong>{ExtensionCustomerCount}</strong></li>
                  <li>{Channel} Customer/s match in CRM: <strong>{MatchExtensionCustomerCount}</strong></li>
                  </ul>
                  {Summarylink}";
    }

    private bool HandleSendingMissingOrders(List<Extensions.Lazada.Model.OrderHeader> MissingOrders, ILogger log)
    {
        if (MissingOrders is null || MissingOrders.Count <= 0)
        {
            return false;
        }

        var count = 1;
        var pagenumber = 0;
        var maxPageCount = 50;
        var currentList = new List<Extensions.Lazada.Model.OrderHeader>();

        foreach (var order in MissingOrders)
        {
            var missingTitle = "<strong>Orders/s Repushed to CRM</strong>";
            currentList.Add(order);

            if (count == MissingOrders.Count || count % maxPageCount == 0)
            {
                var currentcount = (pagenumber * maxPageCount) + 1;

                var missingmessage = GetTable(currentList, currentcount);
                SendMessage(missingTitle, missingmessage, log);
                currentList = new List<Extensions.Lazada.Model.OrderHeader>();
                pagenumber++;
            }
            count++;
        }

        return true;

    }


}
