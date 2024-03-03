using Azure.Storage.Queues;
using Domain;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Magento.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KTI.Moo.Operation.Magento.Domain.Reports;

public class Customer : KTI.Moo.Operation.Core.Domain.Reports.ICustomer<KTI.Moo.Extensions.Magento.Model.Customer, KTI.Moo.CRM.Model.CustomerBase>
{

    private readonly string _connectionString;
    private readonly bool _IsProduction;
    private readonly string _repushedQueuename;
    private readonly string _moo_webhookURL;
    private readonly string _client_webhookURL;
    private readonly string _crm_view_link;
    private readonly INotification _notificationDomain;
    private readonly KTI.Moo.Extensions.Core.Domain.ISearch<Extensions.Magento.Model.DTO.Customers.Search, KTI.Moo.Extensions.Magento.Model.Customer> _extensionsSearchDomain;
    private readonly KTI.Moo.Base.Domain.ISearch<CRM.Model.DTO.CustomerSearch, KTI.Moo.CRM.Model.CustomerBase> _crmSearchDomain;

    public Customer(string connectionString,
                          bool IsProduction,
                          string repushedQueuename,
                          string client_webhookURL,
                          string moo_webhookURL,
                          string crm_view_link,
                          INotification notificationDomain,
                          Extensions.Core.Domain.ISearch<Extensions.Magento.Model.DTO.Customers.Search, Extensions.Magento.Model.Customer> extensionsSearchDomain,
                          Base.Domain.ISearch<CRM.Model.DTO.CustomerSearch, CustomerBase> crmSearchDomain)
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
    }


    public bool Process(DateTime StartDate, DateTime EndDate, ILogger log)
    {
        var CustomerListExtensions = GetListFromExtention(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.email)).ToList();
        var CustomerListCRM = GetListFromCRM(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.emailaddress1)).ToList();

        var CommonEmails = CustomerListExtensions.Where(customer => CustomerListCRM.Any(crmCustomer => crmCustomer.emailaddress1.ToLower() == customer.email.ToLower())).ToList();

        var Channel = "Magento";
        var DomainName = "Customer";
        var Production = _IsProduction ? "Production" : "Test";
        var SummaryTitle = $"{Production}: {Channel} to CRM - {DomainName} Report";
        var SummaryLink = _crm_view_link;
        var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonEmails.Count, CustomerListExtensions.Count, log);

        var IsSuccessSendingSummaryMessage = SendMessage(SummaryTitle, SummaryMessage, log);

        if (IsSuccessSendingSummaryMessage)
        {
            log.LogInformation($"Success sending summary message");

            var MissingCustomers = CustomerListExtensions.Where(customer => !CustomerListCRM.Any(crmCustomer => crmCustomer.emailaddress1.ToLower() == customer.email.ToLower())).ToList();

            var IsSuccessSendingToRetryQueue = SendToRetryQueue(MissingCustomers, log);

            if (IsSuccessSendingToRetryQueue)
            {
                log.LogInformation($"Success sending to retry queue");

                var IsSucessSendingMissingMessage = HandleMissingCustomers(MissingCustomers, log);

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

    public List<CustomerBase> GetListFromCRM(DateTime StartDate, DateTime EndDate)
    {
        return _crmSearchDomain.GetAll(StartDate, EndDate);
    }

    public List<Extensions.Magento.Model.Customer> GetListFromExtention(DateTime StartDate, DateTime EndDate)
    {
        return _extensionsSearchDomain.GetAll(StartDate, EndDate);
    }

    public bool Notify(string WebhookUrl, string Title, string Message, ILogger log)
    {
        return _notificationDomain.Notify(WebhookUrl, Title, Message, log);
    }

    public bool SendToRetryQueue(List<CustomerBase> CRM_Model, ILogger log)
    {
        throw new NotImplementedException();
    }

    public bool SendToRetryQueue(List<Extensions.Magento.Model.Customer> Extension_Model, ILogger log)
    {
        if (Extension_Model is null || Extension_Model.Count <= 0)
        {
            return false;
        }

        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Base.Helpers.JSONSerializerHelper.DontIgnoreResolver()
        };

        foreach (var customer in Extension_Model)
        {
            var DTO = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                customer = customer
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

    private static string GetTable(List<Extensions.Magento.Model.Customer> CommonEmails, int currentcount)
    {
        var TableHeader = $@"<tr>
                             <th>No.</th>
                             <th>Email Address</th>
                             <th>Mobile Number</th>
                             <th>Created On(PHT)</th>
                             <th>Modified On(PHT)</th>
                             </tr>";

        var TableRecord = "";
        foreach (var customer in CommonEmails)
        {
            var mobile = "";

            if (customer.custom_attributes is not null && customer.custom_attributes.Any(custom => custom.attribute_code == "mobile_number"))
            {
                mobile = Convert.ToString(customer.custom_attributes.Where(custom => custom.attribute_code == "mobile_number").FirstOrDefault().value);
                mobile = mobile.FormatPhoneNumber();
            }

            TableRecord += $@"<tr>
                             <td>{currentcount}</td>
                             <td>{customer.email.ToLower()}</td>
                             <td>{mobile}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(customer.created_at)}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(customer.updated_at)}</td>
                             </tr>";
            currentcount++;
        }

        return HTML.TableBorder(TableHeader + TableRecord);
    }

    private string GetSummaryMessage(string Channel, string DomainName, string SummaryLink, DateTime StartDate, DateTime EndDate, int MatchExtensionCustomerCount, int ExtensionCustomerCount, ILogger log)
    {
        var PHTstart = DateTimeHelper.UTC_to_PHT(StartDate);
        var PHTend = DateTimeHelper.UTC_to_PHT(EndDate);
        var DateRange = $"{PHTstart:yyyy MMM, dd hh:mm tt} - {PHTend:yyyy MMM, dd hh:mm tt}";

        if (PHTstart.ToString("yyyy MMM, dd") == PHTend.ToString("yyyy MMM, dd"))
        {
            DateRange = $"{PHTstart:yyyy MMM, dd hh:mm tt} - {PHTend:hh:mm tt}";
        }

        if (MatchExtensionCustomerCount == ExtensionCustomerCount)
        {
            log.LogInformation($"{Channel} and CRM {DomainName} is matched");
        }

        return @$"Report for {DateRange}<br/>
                  <ul>
                  <li>{Channel} {DomainName}/s: <strong>{ExtensionCustomerCount}</strong></li>
                  <li>{Channel} {DomainName}/s match in CRM: <strong>{MatchExtensionCustomerCount}</strong></li>
                  </ul>
                 {SummaryLink}";
    }

    private bool HandleMissingCustomers(List<Extensions.Magento.Model.Customer> MissingCustomers, ILogger log)
    {
        if (MissingCustomers is null || MissingCustomers.Count <= 0)
        {
            return false;
        }

        var count = 1;
        var pagenumber = 0;
        var maxPageCount = 50;
        var currentList = new List<Extensions.Magento.Model.Customer>();
        foreach (var customer in MissingCustomers)
        {
            var missingTitle = "<strong>Customer/s Repushed to CRM</strong>";
            currentList.Add(customer);

            if (count == MissingCustomers.Count || count % maxPageCount == 0)
            {
                var currentcount = (pagenumber * maxPageCount) + 1;

                if (count == maxPageCount) //first page
                {
                    currentcount = 1;
                }

                var missingmessage = GetTable(currentList, currentcount);
                SendMessage(missingTitle, missingmessage, log);
                currentList = new List<Extensions.Magento.Model.Customer>();
                pagenumber++;
            }
            count++;
        }

        return false;
    }
}
