using System;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.CRM.App;

public class DomainsBatch
{
    [FunctionName("DomainsBatch")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
    {

        //var _connectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net";


        //QueueClient queueClient = new QueueClient(_connectionString, "3388-crm", new QueueClientOptions
        //{
        //    MessageEncoding = QueueMessageEncoding.Base64
        //});


        //var decodedString = Helpers.Decode.Base64(myQueueItem);

        //log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        //var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);

        //var company = DomainJObject["companyid"].Value<int?>();

        //var domainType = DomainJObject["domainType"].Value<string>();

        //if (company is null || company == 0)
        //{
        //    throw new Exception("Attribute companyid missing.");
        //}

        //if (string.IsNullOrEmpty(domainType))
        //{
        //    throw new Exception("Attribute domainType missing.");
        //}

      //  CustomerProcess(log);

        //   InvoiceProcess(log);

        //     OrderProcess(log);
        
        //   ProductProcess(log);


    }


    //private static void ProductProcess(ILogger log)
    //{
    //    if (domainType == Base.Helpers.DomainType.product)
    //    {
    //        Domain.Product productDomain = new((int)company);

    //        productDomain.upsert(decodedString, log).GetAwaiter().GetResult();

    //    }
    //}

    //private static void OrderProcess(ILogger log)
    //{
    //    if (domainType == Base.Helpers.DomainType.invoice)
    //    {
    //        Domain.Order orderDomain = new((int)company);

    //        orderDomain.upsert(decodedString, log).GetAwaiter().GetResult();

    //    }
    //}

    //private static void InvoiceProcess(ILogger log)
    //{
    //    if (domainType == Base.Helpers.DomainType.invoice)
    //    {
    //        Domain.Invoice invoiceDomain = new((int)company);

    //        invoiceDomain.upsert(decodedString, log).GetAwaiter().GetResult();

    //    }
    //}

    //private static void CustomerProcess(ILogger log)
    //{

  



    //    if (await theQueue.ExistsAsync())
    //    {
    //        QueueProperties properties = await theQueue.GetPropertiesAsync();

    //        if (properties.ApproximateMessagesCount > 0)
    //        {
    //            QueueMessage[] retrievedMessage = await theQueue.ReceiveMessagesAsync(1);
    //            string theMessage = retrievedMessage[0].Body.ToString();
    //            await theQueue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
    //            return theMessage;
    //        }
    //        else
    //        {
    //            Console.Write("The queue is empty. Attempt to delete it? (Y/N) ");
    //            string response = Console.ReadLine();

    //            if (response.ToUpper() == "Y")
    //            {
    //                await theQueue.DeleteIfExistsAsync();
    //                return "The queue was deleted.";
    //            }
    //            else
    //            {
    //                return "The queue was not deleted.";
    //            }
    //        }
    //    }
    //    else
    //    {
    //        return "The queue does not exist. Add a message to the command line to create the queue and store the message.";
    //    }




    //    if (domainType == Base.Helpers.DomainType.customer)
    //    {
    //        Domain.Customer Customerdomain = new((int)company);

    //        Customerdomain.upsert(decodedString, log).GetAwaiter().GetResult();

    //    }
    //}

}
