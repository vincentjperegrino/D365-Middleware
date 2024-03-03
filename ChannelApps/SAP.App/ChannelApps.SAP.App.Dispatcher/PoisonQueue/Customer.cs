using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using KTI.Moo.Base.Domain.Queue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Polly;

namespace KTI.Moo.ChannelApps.SAP.App.Dispatcher.PoisonQueue;

public class Customer : CompanySettings
{
    private readonly IPoisonNotification _PoisonNotification;

    public Customer(IPoisonNotification poisonNotification)
    {
        _PoisonNotification = poisonNotification;
    }

    [FunctionName("Customer_Poison_To_Main")]
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-channelapp-customer-dispatcher-poison", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var webhookUrl = Environment.GetEnvironmentVariable("TeamsNotificationWebHook");
        var Title = $"{Companyid}>NCCI>{StoreCode}>ChannelApps>SAP>Customer>Error Alert";
        var Message = myQueueItem;

        _PoisonNotification.Notify(webhookUrl, Title, Message, log);
    }
}