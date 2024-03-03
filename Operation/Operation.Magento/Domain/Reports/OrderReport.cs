using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Operation.Magento.Domain.Reports;

public class Order : KTI.Moo.Operation.Core.Domain.Reports.IOrder<KTI.Moo.Extensions.Magento.Model.Order, KTI.Moo.CRM.Model.OrderBase>
{
    private readonly string _connectionString;
    private readonly bool _IsProduction;
    private readonly string _repushedQueuename;
    private readonly string _moo_webhookURL;
    private readonly string _client_webhookURL;
    private readonly string _crm_view_link;
    private readonly INotification _notificationDomain;
    private readonly KTI.Moo.Extensions.Core.Domain.ISearch<Extensions.Magento.Model.DTO.Orders.Search, KTI.Moo.Extensions.Magento.Model.Order> _extensionsSearchDomain;
    private readonly KTI.Moo.Base.Domain.ISearch<CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> _crmSearchDomain;
    private readonly KTI.Moo.Extensions.Core.Domain.ICustomer<KTI.Moo.Extensions.Magento.Model.Customer> _customerDomain;

    public Order(string connectionString,
                      bool IsProduction,
                      string repushedQueuename,
                      string client_webhookURL,
                      string moo_webhookURL,
                      string crm_view_link,
                      INotification notificationDomain,
                      KTI.Moo.Extensions.Core.Domain.ISearch<Extensions.Magento.Model.DTO.Orders.Search, KTI.Moo.Extensions.Magento.Model.Order> extensionsSearchDomain,
                      KTI.Moo.Base.Domain.ISearch<CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> crmSearchDomain,
                      KTI.Moo.Extensions.Core.Domain.ICustomer<Extensions.Magento.Model.Customer> customerDomain)
    {
        _connectionString = connectionString;
        _IsProduction = IsProduction;
        _repushedQueuename = repushedQueuename;
        _client_webhookURL = client_webhookURL;
        _moo_webhookURL = moo_webhookURL;
        _crm_view_link = crm_view_link;
        _notificationDomain = notificationDomain;
        _extensionsSearchDomain = extensionsSearchDomain;
        _crmSearchDomain = crmSearchDomain;
        _customerDomain = customerDomain;
    }


    public bool Process(DateTime StartDate, DateTime EndDate, ILogger log)
    {
        var OrderListExtensions = GetListFromExtention(StartDate, EndDate);
        var OrderListCRM = GetListFromCRM(StartDate, EndDate).Where(order => order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_magento).ToList();

        log.LogInformation("CRM count {OrderListCRM.Count}", OrderListCRM.Count);

        var CommonOrders = OrderListExtensions.Where(order => OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == order.kti_sourceid)).ToList();

        var Channel = "Magento";
        var DomainName = "Order";
        var Production = _IsProduction ? "Production" : "Test";
        var Summarytitle = $"{Production}: {Channel} to CRM - {DomainName} Report";
        var SummaryLink = _crm_view_link;
        var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonOrders.Count, OrderListExtensions.Count, log);

        var IsSuccessSendingSummaryMessage = SendMessage(Summarytitle, SummaryMessage, log);

        if (IsSuccessSendingSummaryMessage)
        {
            log.LogInformation("Success sending summary message");

            var MissingOrders = OrderListExtensions.Where(order => !OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == order.kti_sourceid)).ToList();

            var IsSuccessSendingToRetryQueue = SendToRetryQueue(MissingOrders, log);

            if (IsSuccessSendingToRetryQueue)
            {
                log.LogInformation("Success sending to retry queue");

                var IsSucessSendingMissingMessage = HandleSendingMissingOrders(MissingOrders, log);

                if (IsSucessSendingMissingMessage)
                {
                    log.LogInformation("Success sending missing message");

                    return IsSuccessSendingToRetryQueue;
                }

            }
        }

        log.LogInformation("No Missing {DomainName}/s", DomainName);

        return true;
    }

    public List<OrderBase> GetListFromCRM(DateTime StartDate, DateTime EndDate)
    {
        return _crmSearchDomain.GetAll(StartDate, EndDate);
    }

    public List<Extensions.Magento.Model.Order> GetListFromExtention(DateTime StartDate, DateTime EndDate)
    {
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

    public bool SendToRetryQueue(List<Extensions.Magento.Model.Order> Extension_Model, ILogger log)
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
            var customerModel = _customerDomain.Get(order.customer_id);

            var DTO = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                order = order,
                customer = customerModel
            };

            var json = JsonConvert.SerializeObject(DTO, Formatting.None, JsonSettings);

            var CompressionResults = json.ToBrotliAsync().GetAwaiter().GetResult();

            var CompressionResult = CompressionResults.Result.Value;

            SendToRetryQueue(CompressionResult);
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

    private static string GetTable(List<Extensions.Magento.Model.Order> Extension_ListModel, int currentCount)
    {
        var TableHeader = $@"<tr>
                             <th>No.</th>
                             <th>Order Number</th>
                             <th>Order ID</th>
                             <th>Created On(PHT)</th>
                             <th>Modified On(PHT)</th>
                             </tr>";

        var TableRecord = "";
        foreach (var order in Extension_ListModel)
        {
            TableRecord += $@"<tr>
                             <td>{currentCount}</td>
                             <td>{order.increment_id}</td>
                             <td>{order.order_id}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(order.created_at)}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(order.updated_at)}</td>
                             </tr>";
            currentCount++;
        }

        return HTML.TableBorder(TableHeader + TableRecord);
    }

    private string GetSummaryMessage(string Channel, string DomainName, string Summarylink, DateTime StartDate, DateTime EndDate, int MatchExtensionOrderCount, int ExtensionOrderCount, ILogger log)
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
                  </ul>
                  {Summarylink}";
    }

    private bool HandleSendingMissingOrders(List<Extensions.Magento.Model.Order> MissingOrders, ILogger log)
    {
        if (MissingOrders is null || MissingOrders.Count <= 0)
        {
            return false;
        }

        var count = 1;
        var pagenumber = 0;
        var maxPageCount = 50;
        var currentList = new List<Extensions.Magento.Model.Order>();

        foreach (var order in MissingOrders)
        {
            var missingTitle = "<strong>Orders/s Repushed to CRM</strong>";
            currentList.Add(order);

            if (count == MissingOrders.Count || count % maxPageCount == 0)
            {
                var currentcount = (pagenumber * maxPageCount) + 1;

                var missingmessage = GetTable(currentList, currentcount);
                SendMessage(missingTitle, missingmessage, log);
                currentList = new List<Extensions.Magento.Model.Order>();
                pagenumber++;
            }
            count++;
        }

        return true;

    }
}
