using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.OctoPOS.Model;
using KTI.Moo.Operation.Core.Domain.Reports;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Operation.OctoPOS.Domain.Reports;
public class InvoiceToOrder : IInvoiceToOrderReport<KTI.Moo.Extensions.OctoPOS.Model.Invoice, KTI.Moo.CRM.Model.OrderBase>
{


    private readonly string _connectionString;
    private readonly bool _IsProduction;
    private readonly string _repushedQueuename;
    private readonly string _moo_webhookURL;
    private readonly string _client_webhookURL;
    private readonly string _crm_view_link;
    private readonly INotification _notificationDomain;
    private readonly KTI.Moo.Extensions.Core.Domain.ISearch<Extensions.OctoPOS.Model.DTO.Invoices.Search, KTI.Moo.Extensions.OctoPOS.Model.Invoice> _extensionsSearchDomain;
    private readonly KTI.Moo.Base.Domain.ISearch<CRM.Model.DTO.Orders.Search, KTI.Moo.CRM.Model.OrderBase> _crmSearchDomain;

    public InvoiceToOrder(string connectionString,
                          bool isProduction,
                          string repushedQueuename,
                          string moo_webhookURL,
                          string client_webhookURL,
                          string crm_view_link,
                          INotification notificationDomain,
                          Extensions.Core.Domain.ISearch<Extensions.OctoPOS.Model.DTO.Invoices.Search, Invoice> extensionsSearchDomain,
                          Base.Domain.ISearch<CRM.Model.DTO.Orders.Search, OrderBase> crmSearchDomain)
    {
        _connectionString = connectionString;
        _IsProduction = isProduction;
        _repushedQueuename = repushedQueuename;
        _moo_webhookURL = moo_webhookURL;
        _client_webhookURL = client_webhookURL;
        _crm_view_link = crm_view_link;
        _notificationDomain = notificationDomain;
        _extensionsSearchDomain = extensionsSearchDomain;
        _crmSearchDomain = crmSearchDomain;

    }


    //public bool MigrationOfMissingOrderProcess(DateTime StartDate, DateTime EndDate, ILogger log)
    //{
    //    var InvoiceListExtensions = GetListFromExtention(StartDate, EndDate);
    //    var OrderListCRM = GetListFromCRM(StartDate, EndDate).Where(order => order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_octopos).ToList();
    //    var CommonOrders = InvoiceListExtensions.Where(invoice => OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == invoice.kti_sourceid)).ToList();


    //    var Channel = "Octopus";
    //    var DomainName = "Order";
    //    var Production = _IsProduction ? "Production" : "Test";
    //    var Summarytitle = $"{Production}: {Channel} to CRM - {DomainName} Report";
    //    var SummaryLink = _crm_view_link;
    //    var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonOrders.Count, InvoiceListExtensions.Count, log);

    //    var IsSuccessSendingSummaryMessage = SendMessage(Summarytitle, SummaryMessage, log);

    //    if (IsSuccessSendingSummaryMessage)
    //    {
    //        log.LogInformation($"Success sending summary message");

    //        var MissingInvoice = InvoiceListExtensions.Where(invoice => !OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == invoice.kti_sourceid)).ToList();

    //        MissingInvoice = MissingInvoice.Select(order =>
    //        {
    //            if (string.IsNullOrWhiteSpace(order.CustomerCode))
    //            {
    //                order.CustomerDetails.CustomerCode = "BTQ-SALE";
    //                order.CustomerName = "BTQ-SALE BTQ-SALE";
    //                order.CustomerDetails.companyid = 3389;
    //                order.CustomerDetails.Email = "btq-sale@eleos.com";
    //            }

    //            if (order.InvoiceItems is null || order.InvoiceItems.Count <= 0)
    //            {
    //                int DecimalPlaces = 8;
    //                decimal TaxComputation = (decimal)1.12;

    //                order.InvoiceItems = new List<InvoiceItem>()
    //                {
    //                    new()
    //                    {
    //                        productid = "LegacySKU",
    //                        quantity =  1,
    //                        priceperunit =  Math.Round(order.totalamount / TaxComputation, DecimalPlaces),
    //                        tax = order.totaltax
    //                    }
    //                };
    //            }


    //            if (order.InvoiceItems is not null && order.InvoiceItems.Count > 0 && order.InvoiceItems.Any(item => string.IsNullOrWhiteSpace(item.productid)))
    //            {

    //                order.InvoiceItems = order.InvoiceItems.Select(item =>
    //                {
    //                    if (string.IsNullOrWhiteSpace(item.productid))
    //                    {
    //                        item.productid = "LegacySKU";
    //                    }

    //                    return item;

    //                }).ToList();
    //            }


    //            if (order.InvoiceItems is not null && order.InvoiceItems.Count > 0 && order.InvoiceItems.Any(item => item.manualdiscountamount < 0))
    //            {

    //                order.InvoiceItems = order.InvoiceItems.Select(item =>
    //                {
    //                    if (item.manualdiscountamount < 0)
    //                    {
    //                        item.manualdiscountamount = 0;
    //                    }

    //                    return item;

    //                }).ToList();

    //            }

    //            return order;

    //        }).ToList();



    //        var IsSuccessSendingToRetryQueue = SendToRetryQueue(MissingInvoice, log);

    //        if (IsSuccessSendingToRetryQueue)
    //        {
    //            log.LogInformation($"Success sending to retry queue");

    //            var IsSucessSendingMissingMessage = HandleSendingMissingInvoice(MissingInvoice, log);

    //            if (IsSucessSendingMissingMessage)
    //            {
    //                log.LogInformation($"Success sending missing message");

    //                return IsSuccessSendingToRetryQueue;
    //            }

    //        }
    //    }

    //    log.LogInformation($"No Missing {DomainName}/s");

    //    return true;
    //}

    public bool ProcessTest(DateTime StartDate, DateTime EndDate, ILogger log)
    {

        log.LogInformation($"Getting invoice from extension");
        var InvoiceListExtensions = GetListFromExtention(StartDate, EndDate).Skip(100).Take(100).ToList();
        log.LogInformation($"Getting order from CRM");
        var OrderListCRM = GetListFromCRM(StartDate, EndDate).Where(order => order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_octopos).ToList();

        var CommonOrders = InvoiceListExtensions.Where(invoice => OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == invoice.kti_sourceid)).ToList();


        var Channel = "Octopus";
        var DomainName = "Order";
        var Production = _IsProduction ? "Production" : "Test";
        var Summarytitle = $"{Production}: {Channel} to CRM - {DomainName} Report";
        var SummaryLink = _crm_view_link;
        var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonOrders.Count, InvoiceListExtensions.Count, log);

        var IsSuccessSendingSummaryMessage = true; //SendMessage(Summarytitle, SummaryMessage, log);

        if (IsSuccessSendingSummaryMessage)
        {
            log.LogInformation($"Success sending summary message");

            var MissingInvoice = InvoiceListExtensions.Where(invoice => !OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == invoice.kti_sourceid)).ToList();

            var IsSuccessSendingToRetryQueue = SendToRetryQueue(MissingInvoice, log);


            return true;

            //if (IsSuccessSendingToRetryQueue)
            //{
            //    log.LogInformation($"Success sending to retry queue");

            //    var IsSucessSendingMissingMessage = HandleSendingMissingInvoice(MissingInvoice, log);

            //    if (IsSucessSendingMissingMessage)
            //    {
            //        log.LogInformation($"Success sending missing message");

            //        return IsSuccessSendingToRetryQueue;
            //    }

            //}
        }

        log.LogInformation($"No Missing {DomainName}/s");

        return true;
    }

    public bool Process(DateTime StartDate, DateTime EndDate, ILogger log)
    {

        log.LogInformation($"Getting invoice from extension");
        var InvoiceListExtensions = GetListFromExtention(StartDate, EndDate);
        log.LogInformation($"Getting order from CRM");
        var OrderListCRM = GetListFromCRM(StartDate, EndDate).Where(order => order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_octopos).ToList();

        var CommonOrders = InvoiceListExtensions.Where(invoice => OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == invoice.kti_sourceid)).ToList();

        var Channel = "Octopus";
        var DomainName = "Order";
        var Production = _IsProduction ? "Production" : "Test";
        var Summarytitle = $"{Production}: {Channel} to CRM - {DomainName} Report";
        var SummaryLink = _crm_view_link;
        var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonOrders.Count, InvoiceListExtensions.Count, log);

        var IsSuccessSendingSummaryMessage = SendMessage(Summarytitle, SummaryMessage, log);

        if (IsSuccessSendingSummaryMessage)
        {
            log.LogInformation($"Success sending summary message");

            var MissingInvoice = InvoiceListExtensions.Where(invoice => !OrderListCRM.Any(crmOrder => crmOrder.kti_sourceid == invoice.kti_sourceid)).ToList();

            var IsSuccessSendingToRetryQueue = SendToRetryQueue(MissingInvoice, log);

            if (IsSuccessSendingToRetryQueue)
            {
                log.LogInformation($"Success sending to retry queue");

                var IsSucessSendingMissingMessage = HandleSendingMissingInvoice(MissingInvoice, log);

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

    public List<Invoice> GetListFromExtention(DateTime StartDate, DateTime EndDate)
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

    public bool SendToRetryQueue(List<Invoice> Extension_Model, ILogger log)
    {
        if (Extension_Model is null || Extension_Model.Count <= 0)
        {
            return false;
        }

        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Base.Helpers.JSONSerializerHelper.DontIgnoreResolver()
        };

        foreach (var invoice in Extension_Model)
        {

            var lastname = string.Empty;
            var firstname = string.Empty;

            if (!string.IsNullOrWhiteSpace(invoice.CustomerName))
            {
                var fullname = invoice.CustomerName.Split(' ').ToList();

                if (fullname.Count == 1)
                {
                    lastname = fullname.FirstOrDefault();
                }

                if (fullname.Count > 1)
                {
                    lastname = fullname.LastOrDefault();
                    var lastindex = fullname.Count - 1;
                    fullname.RemoveAt(lastindex);

                    firstname = string.Join(" ", fullname.ToArray());
                }

            }

            //var invoice = OctoPOSInvoice.Get("RB005S030863");

            // var CustomerModels = OctoPOSCustomer.Get(invoice.CustomerCode);

            //  invoice.CustomerDetails = CustomerModels;
            invoice.CustomerDetails.firstname = firstname;
            invoice.CustomerDetails.lastname = lastname;
            invoice.CustomerDetails.companyid = invoice.companyid;

            var DTO = new KTI.Moo.Extensions.OctoPOS.Model.DTO.ChannelApps()
            {
                invoice = invoice,
                customer = invoice.CustomerDetails
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

    private static string GetTable(List<Extensions.OctoPOS.Model.Invoice> Extension_ListModel, int currentCount)
    {
        var TableHeader = $@"<tr>
                             <th>No.</th>
                             <th>Order Number</th>
                             <th>Customer ID</th>
                             <th>Invoice Date</th>
                             <th>Created On(PHT)</th>
                             </tr>";

        var TableRecord = "";
        foreach (var invoice in Extension_ListModel)
        {
            TableRecord += $@"<tr>
                             <td>{currentCount}</td>
                             <td>{invoice.invoicenumber}</td>
                             <td>{(string.IsNullOrWhiteSpace(invoice.CustomerCode) ? "-" : invoice.CustomerCode)}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(invoice.InvoiceDate)}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(invoice.CreatedDateTime)}</td>
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

    private bool HandleSendingMissingInvoice(List<Extensions.OctoPOS.Model.Invoice> MissingInvoices, ILogger log)
    {
        if (MissingInvoices is null || MissingInvoices.Count <= 0)
        {
            return false;
        }

        var count = 1;
        var pagenumber = 0;
        var maxPageCount = 50;
        var currentList = new List<Extensions.OctoPOS.Model.Invoice>();

        foreach (var invoice in MissingInvoices)
        {
            var missingTitle = "<strong>Orders/s Repushed to CRM</strong>";
            currentList.Add(invoice);

            if (count == MissingInvoices.Count || count % maxPageCount == 0)
            {
                var currentcount = (pagenumber * maxPageCount) + 1;

                var missingmessage = GetTable(currentList, currentcount);
                SendMessage(missingTitle, missingmessage, log);
                currentList = new List<Extensions.OctoPOS.Model.Invoice>();
                pagenumber++;
            }
            count++;
        }

        return true;

    }
}
