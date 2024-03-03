using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.App.Queue.Dispatchers;

public class UpsertCustomer : CompanySettings
{

    //  private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagement;
    private readonly IDistributedCache _cache;
    public UpsertCustomer(IDistributedCache cache)
    {
        _cache = cache;
    }

    //public UpsertCustomer(Base.Domain.IChannelManagement<CRM.Model.ChannelManagement.SalesChannel> channelManagement, IDistributedCache cache)
    //{

    //    _channelManagement = channelManagement;
    //    _cache = cache;
    //}

    [Singleton(Mode = SingletonMode.Listener)]
    [FunctionName("Upsert-Customer-Octopos")]
    public void Run([QueueTrigger("%CompanyID%-octopos-%StoreCode%-extension-customer-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
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

            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var CompanyID = Convert.ToInt32(Companyid);

            Process(myQueueItem, config, log);

        });



        //try
        //{
        //    //var customer = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Customer>(myQueueItem);

        //    var CompanyID = Convert.ToInt32(Companyid);

        //    Process(myQueueItem, config, log);
        //}
        //catch (System.Exception ex)
        //{
        //    log.LogInformation(ex.Message);
        //    throw new System.Exception(ex.Message);
        //}
    }
    public bool Process(string FromDispatcherQueue, Config config, ILogger log)
    {
        KTI.Moo.Extensions.OctoPOS.Domain.Customer OctoposCustomerDomain = new(config, _cache);

        var CustomerData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var contactid = "";

        if (CustomerData.ContainsKey("contactid"))
        {
            contactid = CustomerData["contactid"].Value<string>();
            CustomerData.Remove("contactid");
        }

        var customercode = "";

        if (CustomerData.ContainsKey("Email"))
        {
            var Email = CustomerData["Email"].Value<string>();
            var ExsistingCustomer = OctoposCustomerDomain.GetByEmail(Email);

            if (ExsistingCustomer is not null && !string.IsNullOrWhiteSpace(ExsistingCustomer.CustomerCode))
            {
                customercode = ExsistingCustomer.CustomerCode;
            }

        }

        if (string.IsNullOrWhiteSpace(customercode) && CustomerData.ContainsKey("HandPhone") && (CustomerData["HandPhone"].Value<string>()).Length > 9)
        {
            var HandPhone = CustomerData["HandPhone"].Value<string>();
            var ExsistingCustomer = OctoposCustomerDomain.GetByPhone(HandPhone);

            if (ExsistingCustomer is not null && !string.IsNullOrWhiteSpace(ExsistingCustomer.CustomerCode))
            {
                customercode = ExsistingCustomer.CustomerCode;
            }

        }

        if (!string.IsNullOrWhiteSpace(customercode))
        {
            CustomerData["CustomerCode"] = customercode;

        }

        //var MembershipCode = "";

        //if (CustomerData.ContainsKey("MembershipCode"))
        //{
        //    MembershipCode = CustomerData["MembershipCode"].Value<string>();
        //}


        var MessageQueue = JsonConvert.SerializeObject(CustomerData);

        var OctoposCustomer = OctoposCustomerDomain.Upsert(MessageQueue);

        var Json = JsonConvert.SerializeObject(OctoposCustomer);

        log.LogInformation(Json);

        //if (MembershipCode == OctoposCustomer.MembershipCode)
        //{
        //    return true;
        //}


        //var UpsertCustomer = new Dictionary<string, string>();

        //UpsertCustomer.Add("MembershipCode", OctoposCustomer.MembershipCode);

        //var settings = new JsonSerializerSettings
        //{
        //    NullValueHandling = NullValueHandling.Ignore,
        //    DefaultValueHandling = DefaultValueHandling.Ignore
        //};

        //var Json = JsonConvert.SerializeObject(UpsertCustomer, Formatting.None, settings);

        //int companyID = Convert.ToInt32(Companyid);

        //CRM.Domain.Customer customer = new(companyID);

        ////TODO: Put in another queue
        //customer.upsert(Json, contactid, log).Wait();

        return true;
    }
}
