using System;
using System.ComponentModel.Design;
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.ChannelApps.Lazada.App.Receiver.Store;

public class Order : CompanySettings
{
    private readonly IOrderForReceiver _OrderDomain;

    public Order(IOrderForReceiver OrderDomain)
    {
        _OrderDomain = OrderDomain;
    }

    [FunctionName("Lazada_Order_Receiver")]
    public void Run([QueueTrigger("%CompanyID%-lazada-%StoreCode%-channelapp-order-receiver", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var decodedString = ChannelApps.Core.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();
        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        _OrderDomain.WithCustomerProcess(decodedString);
    }



}
