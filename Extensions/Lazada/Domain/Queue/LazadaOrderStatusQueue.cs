using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Lazada.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Domain.Queue;

public class LazadaOrderStatus : Core.Domain.Queue.IOrderStatusMessage
{

    private readonly Core.Domain.IOrderstatus<Model.OrderStatus> _orderstatusDomain;
    private readonly Service.Queue.Config _config;
    private readonly string _azureconnectionstring;

    public LazadaOrderStatus(Service.Queue.Config config, IDistributedCache cache, string azureconnectionstring)
    {
        _orderstatusDomain = new Domain.OrderStatus(config, cache);
        _config = config;
        _azureconnectionstring = azureconnectionstring;
    }  
    
    
    
    public LazadaOrderStatus(Service.Queue.Config config, ClientTokens clientTokens, string azureconnectionstring)
    {
        _orderstatusDomain = new Domain.OrderStatus(config, clientTokens);
        _config = config;
        _azureconnectionstring = azureconnectionstring;
    }




    public bool Process(string message, ILogger log)
    {
        var OrderStatusModel = JsonConvert.DeserializeObject<Model.OrderStatus>(message);

        var OrderStatus = OrderStatusModel.kti_orderstatus;
        var OrderStatusName = OrderStatusModel.orderstatus;

        log.LogInformation($"{OrderStatusName} process");

        if (CheckOrderStatus(OrderStatusModel.kti_sourcesalesorderid, OrderStatusName, log))
        {
            return true;
        }

        //Cancel status process 
        if (OrderStatus == Base.Helpers.OrderStatus.CancelOrder)
        {
            _orderstatusDomain.CancelOder(OrderStatusModel);

            log.LogInformation($"{OrderStatusName} process done");
            return true;
        }

        //Packed status process 
        if (OrderStatus == Base.Helpers.OrderStatus.OrderPacked)
        {
            var result = _orderstatusDomain.OrderPacked(OrderStatusModel);
            result.kti_shipmentid = OrderStatusModel.kti_shipmentid;

            log.LogInformation($"{result.packageid} for updating shipment sending to queue");

            UpdateShipmentCRM(result);

            log.LogInformation($"{OrderStatusName} process done");
            return true;
        }

        //Prepared status process 
        if (OrderStatusName == Base.Helpers.OrderStatus.ForDispatchName)
        {
            _orderstatusDomain.ForDispatch(OrderStatusModel);
            log.LogInformation($"{OrderStatusName} process done");
            return true;
        }

        log.LogInformation($"{OrderStatusName} not included in process");

        return false;
    }

    private bool UpdateShipmentCRM(Model.OrderStatus orderStatusresult)
    {
        var ShipmentDTO = new Core.Model.ShipmentBase()
        {
            kti_shipmentid = orderStatusresult.kti_shipmentid,
            kti_shipmentprovidertrackingnumber = orderStatusresult.tracking_number,
            kti_packageid = orderStatusresult.packageid
        };

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var Json = JsonConvert.SerializeObject(ShipmentDTO, Formatting.None, settings);

        return SendMessageToQueue(Json, $"{_config.companyid}-crm-callback-shipment");
    }

    private bool SendMessageToQueue(string Json, string QueueName)
    {
        var queueClient = new QueueClient(_azureconnectionstring, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();
        queueClient.SendMessage(Json);

        return true;
    }

    public bool CheckOrderStatus(string orderid, string orderstatus, ILogger log)
    {
        var Orderstatus = _orderstatusDomain.Get(orderid);

        if (Orderstatus == null)
        {
            throw new System.Exception("No Lazada Order");
        }

        if (Orderstatus.orderstatus == Moo.Base.Helpers.OrderStatus.CancelOrderName)
        {
            log.LogInformation($"{orderstatus} process done. Order {orderid} is already cancelled.");
            return true;
        }

        log.LogInformation($"{orderstatus} process done. Order {orderid} is already {orderstatus}.");
        return Orderstatus.orderstatus == orderstatus; // return true if same
    }
}
