
using KTI.Moo.Base.Helpers;
using System.ComponentModel.Design;

namespace KTI.Moo.CRM.App.Schedule;

public class Order : Helpers.CompanySettings
{
    [FunctionName("OrderSchedule")]
    public void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            Process(Companyid, ConnectionString,log).Wait();
        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new Exception(ex.Message);
        }

    }


    public static async Task<bool> Process(string CompanyID , string ConnectionString, ILogger log)
    {


        CRM.Domain.Order domain = new(Convert.ToInt32(CompanyID));

         await domain.SyncToOrders();


        //CRM.Domain.Order domain = new(Convert.ToInt32(CompanyID));

        //var ordersList = await domain.GetSyncToOrders();

        //var JsonSettings = new JsonSerializerSettings
        //{
        //    DefaultValueHandling = DefaultValueHandling.Ignore,
        //    NullValueHandling = NullValueHandling.Ignore
        //};

        //if(ordersList == null || ordersList.Count <= 0)
        //{
        //    return false;
        //}

        //var DispatchTask = ordersList.Select(async order =>
        //{
        //    var quename = $"{CompanyID}-crm-order-schedule";

        //    var QueueMessage = JsonConvert.SerializeObject(order, JsonSettings);

        //    await SendMessageToQueueAsync(QueueMessage, quename , ConnectionString);
        //    log.LogInformation($"Order trigger for replication to {quename} {order.name}");
        //});

        //await Task.WhenAll(DispatchTask);

        return true;
    }

    public static async Task<bool> SendMessageToQueueAsync(string Json, string QueueName, string ConnectionString)
    {

        QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync(Json);

        return true;
    }
}
