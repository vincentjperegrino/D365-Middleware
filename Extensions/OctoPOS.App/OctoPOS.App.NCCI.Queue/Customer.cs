using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Azure.Storage.Queues;

namespace KTI.Moo.Extensions.OctoPOS.App.NCCI.Queue;

public class Customer : CompanySettings
{
    [FunctionName("OctoPOS_QueueCustomer_3388")]
    public void Run([TimerTrigger("0 */20 * * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Customer Queue trigger function processed at " + OctoPOS.Helper.DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));
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

        //var config = ConfigHelper.Get();

        OctoPOS.Domain.Customer CustomerDomain = null;

        List<OctoPOS.Model.Customer> ForQueueList = new();

        var DateStart = GetRecentlyAddedUpdatedDate();
        var DateEnd = GetDateNowPlusOffSet();

        var customerListWithListProperty = CustomerDomain.GetSearchListByDate(DateStart, DateEnd);

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
            return TriggerCustomerQueueInDistributor(ForQueueList);
        }


        return false;

    }

    private static DateTime GetDateNowPlusOffSet()
    {
        var Offset30mins = 30;
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
    private static bool TriggerCustomerQueueInDistributor(List<OctoPOS.Model.Customer> ForQueueList)
    {
        // Get the connection string from app settings
        // string connectionString = Environment.GetEnvironmentVariable("AzureQueueConnectionString");

        QueueClient queueClient = new QueueClient(ConnectionString, CustomerQueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        // Create the queue if it doesn't already exist
        queueClient.CreateIfNotExistsAsync().Wait();

        foreach (var customer in ForQueueList)
        {
            queueClient.SendMessage(GetJson(customer));
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

    private static bool MoreThanOnePage(OctoPOS.Model.DTO.Customer.Search customerListWithListProperty)
    {
        return customerListWithListProperty.total_pages > 1;
    }

    private static bool InvalidCustomerList(OctoPOS.Model.DTO.Customer.Search customerListWithListProperty)
    {
        return customerListWithListProperty.values is null || customerListWithListProperty.values.Count <= 0;
    }

    private static DateTime GetRecentlyAddedUpdatedDate()
    {
        var DateNow = OctoPOS.Helper.DateTimeHelper.PHTnow();

        var RecentlyAddedUpdatedCustomerInThePastMinutes = -60;

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
