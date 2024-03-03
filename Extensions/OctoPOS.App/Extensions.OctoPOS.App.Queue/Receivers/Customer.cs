using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Queues;
using KTI.Moo.Base.Domain;
using KTI.Moo.CRM.Model.ChannelManagement;
using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly;

namespace KTI.Moo.Extensions.OctoPOS.App.Queue.Receivers;

public class Customer : CompanySettings
{
    private readonly IDistributedCache _cache;

    public Customer(IDistributedCache cache)
    {
        _cache = cache;
    }

    [FunctionName("Octopos_PollCustomer")]
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
            log.LogInformation($"C# Customer Queue trigger function processed at " + OctoPOS.Helper.DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
            process();

        });

    }

    private bool process()
    {

        //var ChannelConfig = _channelManagement.Get(StoreCode);

        //var ConfigOctopos = new OctoPOS.Service.Config()
        //{
        //    companyid = Companyid,
        //    defaultURL = ChannelConfig.kti_defaulturl,
        //    password = ChannelConfig.kti_password,
        //    redisConnectionString = System.Environment.GetEnvironmentVariable("config_redisConnectionString"), // For refactor to insert dependecy of cache
        //    username = ChannelConfig.kti_username,
        //    apiAuth = ChannelConfig.kti_appkey
        //};


        config = ConfigHelper.Get();

        OctoPOS.Domain.Customer CustomerDomain = new(config, _cache);

        List<OctoPOS.Model.Customer> ForQueueList = new();

        var DateStart = GetRecentlyAddedUpdatedDate();
        var DateEnd = GetDateNowPlusOffSet();

        var customerListWithListProperty = CustomerDomain.GetSearchListByDate(DateStart, DateEnd , 1);

        if (InvalidCustomerList(customerListWithListProperty))
        {
            return false;
        }

        var ForAddingToForQueueList = customerListWithListProperty.values;

        ForQueueList = ForAddingToForQueueList;

        if (MoreThanOnePage(customerListWithListProperty))
        {
            ForQueueList = LoopTheRemainingPages(ForQueueList, CustomerDomain, customerListWithListProperty.total_pages, DateStart, DateEnd);
        }


        if (ValidQueue(ForQueueList))
        {
            return InsertToReceiverQueue(ForQueueList);
        }


        return false;

    }

    private static DateTime GetDateNowPlusOffSet()
    {
        var Offset30mins = 5;
        return OctoPOS.Helper.DateTimeHelper.PHTnow().AddMinutes(Offset30mins);
    }

    private List<OctoPOS.Model.Customer> LoopTheRemainingPages(List<OctoPOS.Model.Customer> CurrentList, OctoPOS.Domain.Customer CustomerDomain, int TotalPages, DateTime DateStart, DateTime DateEnd)
    {
        var startInSecondPage = 2;

        for (int pagenumberIteration = startInSecondPage; pagenumberIteration <= TotalPages; pagenumberIteration++)
        {
            var customerListPerPage = CustomerDomain.GetListByDate(DateStart, DateEnd, pagenumberIteration);

            if (ValidCustomerList(customerListPerPage))
            {
                var ForAddingToForQueueList = customerListPerPage;
                CurrentList.AddRange(ForAddingToForQueueList);
            }
        }

        return CurrentList;

    }
    private static bool InsertToReceiverQueue(List<OctoPOS.Model.Customer> ForQueueList)
    {
        // Get the connection string from app settings
        // string connectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");


        var DTOlist = ForQueueList.Select(customer =>
        {
            return new OctoPOS.Model.DTO.ChannelApps()
            {
                customer = customer
            };

        }).ToList();


        QueueClient queueClient = new QueueClient(ConnectionString, CustomerQueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        // Create the queue if it doesn't already exist
        queueClient.CreateIfNotExistsAsync().Wait();

        foreach (var customer in DTOlist)
        {
            var Compress = GetJson(customer).ToBrotliAsync().GetAwaiter().GetResult().Result.Value;
            queueClient.SendMessage(Compress);
        }

        return false;

    }

    private static bool ValidQueue(List<OctoPOS.Model.Customer> ForQueueList)
    {
        return ForQueueList.Count > 0;
    }

    private static bool ValidCustomerList(List<OctoPOS.Model.Customer> customerList)
    {
        return customerList is not null && customerList.Count > 0;
    }

    private static bool MoreThanOnePage(OctoPOS.Model.DTO.Customers.Search customerListWithListProperty)
    {
        return customerListWithListProperty.total_pages > 1;
    }

    private static bool InvalidCustomerList(OctoPOS.Model.DTO.Customers.Search customerListWithListProperty)
    {
        return customerListWithListProperty.values is null || customerListWithListProperty.values.Count <= 0;
    }

    private static DateTime GetRecentlyAddedUpdatedDate()
    {
        var DateNow = OctoPOS.Helper.DateTimeHelper.PHTnow();

        var RecentlyAddedUpdatedCustomerInThePastMinutes = -30;

        var DateForCheckingIfRecentlyAddedUpdated = DateNow.AddMinutes(RecentlyAddedUpdatedCustomerInThePastMinutes);

        return DateForCheckingIfRecentlyAddedUpdated;
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
