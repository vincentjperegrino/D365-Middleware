using System;
using System.ComponentModel.Design;
using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.CRM.Domain.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Extensions.SAP.App.Queue.Dispatchers;

public class Order : CompanySettings
{
    private readonly IPoisonNotification _PoisonNotification;

    public Order(IPoisonNotification poisonNotification)
    {
        _PoisonNotification = poisonNotification;
    }

    [FunctionName("Order_Upsert_Poison")]
    public void Run([QueueTrigger("%CompanyID%-sap-%StoreCode%-extension-order-dispatcher-poison", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var webhookUrl = Environment.GetEnvironmentVariable("TeamsNotificationWebHook");
        var Title = $"{Companyid}>NCCI>{StoreCode}>Extension>SAP>Order>Error Alert";
        var Message = myQueueItem;

        _PoisonNotification.Notify(webhookUrl, Title, Message, log);
    }

}
