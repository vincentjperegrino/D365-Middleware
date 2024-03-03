using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.OctoPOS.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace KTI.Moo.Operation.OctoPOS.Domain.Reports;

public class Customer : KTI.Moo.Operation.Core.Domain.Reports.ICustomer<KTI.Moo.Extensions.OctoPOS.Model.Customer, KTI.Moo.CRM.Model.CustomerBase>
{
    private readonly string _connectionString;
    private readonly bool _IsProduction;
    private readonly string _StoreCode;
    private readonly string _moo_webhookURL;
    private readonly string _client_webhookURL;
    private readonly string _crm_view_link;
    private readonly INotification _notificationDomain;
    private readonly KTI.Moo.Extensions.Core.Domain.ISearch<Extensions.OctoPOS.Model.DTO.Customers.Search, KTI.Moo.Extensions.OctoPOS.Model.Customer> _extensionsSearchDomain;
    private readonly KTI.Moo.Base.Domain.ISearch<CRM.Model.DTO.CustomerSearch, KTI.Moo.CRM.Model.CustomerBase> _crmSearchDomain;
    private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagementDomain;

    public Customer(string connectionString,
                          bool IsProduction,
                          string StoreCode,
                          string client_webhookURL,
                          string moo_webhookURL,
                          string crm_view_link,
                          INotification notificationDomain,
                          Extensions.Core.Domain.ISearch<Extensions.OctoPOS.Model.DTO.Customers.Search, Extensions.OctoPOS.Model.Customer> extensionsSearchDomain,
                          Base.Domain.ISearch<CRM.Model.DTO.CustomerSearch, CustomerBase> crmSearchDomain,
                          Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagementDomain)
    {
        _connectionString = connectionString;
        _IsProduction = IsProduction;
        _StoreCode = StoreCode;
        _client_webhookURL = client_webhookURL;
        _moo_webhookURL = moo_webhookURL;
        _crm_view_link = crm_view_link;
        _notificationDomain = notificationDomain;
        _extensionsSearchDomain = extensionsSearchDomain;
        _crmSearchDomain = crmSearchDomain;
        _channelManagementDomain = channelManagementDomain;
    }


    //public bool MissingMobileProcess(DateTime StartDate, DateTime EndDate, ILogger log)
    //{
    //    var CustomerListExtensions = GetListFromExtention(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.Email) && string.IsNullOrWhiteSpace(customer.HandPhone)).ToList();

    //    var CustomerListCRM = GetListFromCRM(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.emailaddress1) && customer.kti_socialchannelorigin != CRM.Helper.ChannelOrigin.OptionSet_lazada && !string.IsNullOrWhiteSpace(customer.mobilephone)).ToList();

    //   // var CommonEmails = CustomerListCRM.Where(crmCustomer => CustomerListExtensions.Any(Customer => Customer.Email.ToLower() == crmCustomer.emailaddress1.ToLower())).ToList();

    //    var CommonEmails = CustomerListExtensions.Where(ExtensionCustomer => CustomerListCRM.Any(Customer => Customer.emailaddress1.ToLower() == ExtensionCustomer.Email.ToLower())).ToList();

    //    var missingMobiles = CommonEmails.Select(customers =>
    //    {
    //        var Mobile = CustomerListCRM.Where(customer => customer.emailaddress1 == customers.Email).Select(customer => customer.mobilephone.FormatPhoneNumber()).FirstOrDefault();

    //        customers.HandPhone = Mobile+"'";

    //        return customers;


    //    }).ToList();

    //    return true;
    //}


    public bool Process(DateTime StartDate, DateTime EndDate, ILogger log)
    {
        var CustomerListExtensions = GetListFromExtention(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.Email)).ToList();
        var CustomerListCRM = GetListFromCRM(StartDate, EndDate).Where(customer => !string.IsNullOrWhiteSpace(customer.emailaddress1) && customer.kti_socialchannelorigin != CRM.Helper.ChannelOrigin.OptionSet_lazada).ToList();

        var CommonEmails = CustomerListCRM.Where(crmCustomer => CustomerListExtensions.Any(Customer => Customer.Email.ToLower() == crmCustomer.emailaddress1.ToLower())).ToList();

        var Channel = "Octopus";
        var DomainName = "Customer";
        var Production = _IsProduction ? "Production" : "Test";
        var SummaryTitle = $"{Production}: CRM to {Channel} - {DomainName} Report";
        var SummaryLink = _crm_view_link;
        var SummaryMessage = GetSummaryMessage(Channel, DomainName, SummaryLink, StartDate, EndDate, CommonEmails.Count, CustomerListCRM.Count, log);

        var IsSuccessSendingSummaryMessage = SendMessage(SummaryTitle, SummaryMessage, log);

        if (IsSuccessSendingSummaryMessage)
        {
            log.LogInformation($"Success sending summary message");

            var MissingCustomers = CustomerListCRM.Where(crmCustomer => !CustomerListExtensions.Any(Customer => Customer.Email.ToLower() == crmCustomer.emailaddress1.ToLower())).ToList();

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

    public List<Extensions.OctoPOS.Model.Customer> GetListFromExtention(DateTime StartDate, DateTime EndDate)
    {
        return _extensionsSearchDomain.GetAll(StartDate, EndDate);
    }

    public bool Notify(string WebhookUrl, string Title, string Message, ILogger log)
    {
        return _notificationDomain.Notify(WebhookUrl, Title, Message, log);
    }

    public bool SendToRetryQueue(List<CustomerBase> CRM_Model, ILogger log)
    {
        if (CRM_Model is null || CRM_Model.Count <= 0)
        {
            return false;
        }

        var channel = _channelManagementDomain.Get(_StoreCode);
        var ChannelObject = (JObject)JToken.FromObject(channel);


        foreach (var customer in CRM_Model)
        {
            var JSONcustomer = JsonConvert.SerializeObject(customer);
            var fordomainobject = JsonConvert.DeserializeObject<JObject>(JSONcustomer);
            fordomainobject.Add(KTI.Moo.Base.Helpers.ChannelMangement.saleschannelConfigDTOname, ChannelObject);
            var QueueMessage = JsonConvert.SerializeObject(fordomainobject);

            var quename = KTI.Moo.CRM.Helper.ChannelOrigin.getquename(channel.kti_channelorigin);

            SendToChannelAppQueue(channel.kti_moocompanyid,
                                  channel.kti_storecode,
                                  quename,
                                  customer.domainType,
                                  QueueMessage);
        }

        return true;

    }

    private bool SendToChannelAppQueue(string companyid, string storecode, string channelorigin, string domainType, string QueueMessage)
    {
        var queuename = $"{companyid}-{channelorigin}-{storecode}-channelapp-{domainType}-dispatcher";

        return SendToRetryQueue(QueueMessage, queuename);
    }

    private bool SendToRetryQueue(string message, string queuename)
    {
        var queueClient = new QueueClient(_connectionString, queuename, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();

        queueClient.SendMessage(message);

        return true;
    }

    public bool SendToRetryQueue(List<Extensions.OctoPOS.Model.Customer> Extension_Model, ILogger log)
    {

        throw new NotImplementedException();
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

    private static string GetTable(List<CustomerBase> CommonEmails, int currentcount)
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
            TableRecord += $@"<tr>
                             <td>{currentcount}</td>
                             <td>{customer.emailaddress1}</td>
                             <td>{customer.mobilephone}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(customer.createdon)}</td>
                             <td>{DateTimeHelper.UTC_to_PHT(customer.modifiedon)}</td>
                             </tr>";
            currentcount++;
        }

        return HTML.TableBorder(TableHeader + TableRecord);
    }

    private string GetSummaryMessage(string Channel, string DomainName, string SummaryLink, DateTime StartDate, DateTime EndDate, int MatchExtensionCustomerCount, int CRMCustomerCount, ILogger log)
    {
        var PHTstart = DateTimeHelper.UTC_to_PHT(StartDate);
        var PHTend = DateTimeHelper.UTC_to_PHT(EndDate);
        var DateRange = $"{PHTstart:yyyy MMM, dd hh:mm tt} - {PHTend:yyyy MMM, dd hh:mm tt}";

        if (PHTstart.ToString("yyyy MMM, dd") == PHTend.ToString("yyyy MMM, dd"))
        {
            DateRange = $"{PHTstart:yyyy MMM, dd hh:mm tt} - {PHTend:hh:mm tt}";
        }

        if (MatchExtensionCustomerCount == CRMCustomerCount)
        {
            log.LogInformation($"{Channel} and CRM {DomainName} is matched");
        }

        return @$"Report for {DateRange}<br/>
                  <ul>
                  <li>CRM {DomainName}/s: <strong>{CRMCustomerCount}</strong></li>
                  <li>CRM {DomainName}/s match in {Channel}: <strong>{MatchExtensionCustomerCount}</strong></li>
                  </ul>
                 {SummaryLink}";
    }

    private bool HandleMissingCustomers(List<CustomerBase> MissingCustomers, ILogger log)
    {
        if (MissingCustomers is null || MissingCustomers.Count <= 0)
        {
            return false;
        }

        var count = 1;
        var pagenumber = 0;
        var maxPageCount = 50;
        var currentList = new List<CustomerBase>();
        foreach (var customer in MissingCustomers)
        {
            var missingTitle = "<strong>Customer/s Repushed to Octopus</strong>";
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
                currentList = new List<CustomerBase>();
                pagenumber++;
            }
            count++;
        }

        return false;
    }

}
