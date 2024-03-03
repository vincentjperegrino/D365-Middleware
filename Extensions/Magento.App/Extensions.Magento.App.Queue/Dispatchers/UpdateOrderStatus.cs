

using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Magento.App.Queue.Dispatchers;

public class UpdateOrderStatus : CompanySettings
{

    [FunctionName("Update-OrderStatus-Magento")]
    public void Run([QueueTrigger("%CompanyID%-magento-orderstatus", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

        try
        {
            var OrderData = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Core.Model.OrderBase>(myQueueItem);

            Process(OrderData, log);
        }
        catch (System.Exception ex)
        {

            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }

    }

    public bool Process(KTI.Moo.Extensions.Core.Model.OrderBase OrderData, ILogger log)
    {

        Magento.Domain.Order OrderDomain = new(config);

        if (OrderData.statuscode == 959080000)
        {
            var isValidOrderID = int.TryParse(OrderData.kti_sourceid, out var Orderid);
            if (isValidOrderID)
            {
                var MagentoOrder = OrderDomain.Get(Orderid);

                //if(MagentoOrder.order_status == "pending")
                //{
                //    log.LogInformation("Update pending to processing");
                //    return OrderDomain.UpdateStatusProcessing(Orderid);
                //}
                //log.LogInformation("Order already processing");

                return OrderDomain.UpdateStatusProcessing(Orderid);
            }
        }

        return false;
    }



}
