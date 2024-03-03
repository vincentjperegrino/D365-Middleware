
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace KTI.Moo.Extensions.Lazada.Domain;

public class OrderStatus : Core.Domain.IOrderstatus<Model.OrderStatus>
{
    private readonly Core.Domain.IFulfillment<Model.PackageOrder, Model.OrderItempackage> _fulfillmentDomain;
    private readonly Order _orderDomain;

    public OrderStatus(Service.Queue.Config config, IDistributedCache cache)
    {
        _fulfillmentDomain = new Fulfillment(config, cache);
        _orderDomain = new(config, cache);
    }    
    
    
    public OrderStatus(Service.Queue.Config config, ClientTokens clientTokens)
    {
        _fulfillmentDomain = new Fulfillment(config, clientTokens);
        _orderDomain = new(config, clientTokens);
    }

    public OrderStatus(Core.Domain.IFulfillment<Model.PackageOrder, Model.OrderItempackage> fulfillmentDomain)
    {
        _fulfillmentDomain = fulfillmentDomain;
    }
    public Model.OrderStatus Get(string OrderID)
    {
        if (!long.TryParse(OrderID, out var salesorderid))
        {
            return new Model.OrderStatus();
        }

        var Order = _orderDomain.Get(salesorderid);


        var orderStatus = new Model.OrderStatus()
        {
            kti_sourcesalesorderid = Order.kti_sourceid,
            kti_sourcesalesorderitemids = Order.order_items.Select(item => item.kti_sourceid).ToList(),
            orderstatus = SupportedConvertStatus(Order.laz_Statuses)

        };

        return orderStatus;

    }

    private static string SupportedConvertStatus(IEnumerable<string> lazadaStatuses)
    {

        string[] OrderPacked = { "packed", "repacked" };
        string[] ForDispatch = { "ready_to_ship_pending", "ready_to_ship" };
        string[] CancelOder = { "canceled", "cancelled" };

        string[] Pending = { "pending" };

        if (lazadaStatuses.Intersect(CancelOder).Any())
        {
            return Moo.Base.Helpers.OrderStatus.CancelOrderName;
        }

        if (lazadaStatuses.Intersect(ForDispatch).Any())
        {
            return Moo.Base.Helpers.OrderStatus.ForDispatchName;
        }

        if (lazadaStatuses.Intersect(OrderPacked).Any())
        {
            return Moo.Base.Helpers.OrderStatus.OrderPackedName;
        }

        if (lazadaStatuses.Intersect(Pending).Any())
        {
            return Moo.Base.Helpers.OrderStatus.CheckoutName;
        }

        return default;
    }



    public Model.OrderStatus CancelOder(Model.OrderStatus OrderStatus)
    {
        foreach (var item in OrderStatus.kti_sourcesalesorderitemids)
        {
            _orderDomain.SetCancelled(item, OrderStatus.laz_cancelReason, OrderStatus.cancelreason);
        }

        return new Model.OrderStatus()
        {
            successful = true,
        };

    }

    public Model.OrderStatus OrderPacked(Model.OrderStatus OrderStatus)
    {
        var ShipmentProvider = _fulfillmentDomain.GetShipmentProvider(OrderStatus.kti_sourcesalesorderid, OrderStatus.kti_sourcesalesorderitemids);

        var response = _fulfillmentDomain.Pack(OrderStatus.kti_sourcesalesorderid, OrderStatus.kti_sourcesalesorderitemids, ShipmentProvider);

        var Get1stOrderItem = response.order_item_list.FirstOrDefault();

        return new Model.OrderStatus()
        {
            kti_sourcesalesorderid = OrderStatus.kti_sourcesalesorderid,
            kti_sourcesalesorderitemids = OrderStatus.kti_sourcesalesorderitemids,
            packageid = Get1stOrderItem.package_id,
            shipment_provider = Get1stOrderItem.shipment_provider,
            tracking_number = Get1stOrderItem.tracking_number,
            successful = Get1stOrderItem.item_err_code == "0"
        };
    }

    public Model.OrderStatus ForDispatch(Model.OrderStatus OrderStatus)
    {
        var response = _fulfillmentDomain.ReadyToShip(OrderStatus.packageid);

        return new Model.OrderStatus()
        {
            successful = response
        };
    }

    public Model.OrderStatus OrderPrepared(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus Checkout(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus DeliveredToCustomer(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus DriverNearby(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus ForPayment(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus InCart(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus Incomplete(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus OrderReleased(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus ReceivedByDelivery(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }

    public Model.OrderStatus RequestCancelOrder(Model.OrderStatus OrderStatus)
    {
        throw new System.NotImplementedException();
    }


}
