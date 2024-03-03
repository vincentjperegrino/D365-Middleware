using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Domain
{
    public interface IOrderStatus<T> where T : Model.OrderStatusBase
    {
        T InCart(Entity Order);
        T ForPayment(Entity Order);
        T Checkout(Entity Order);
        T OrderPrepared(Entity Order);
        T OrderPacked(Entity Order);
        T ForDispatch(Entity Shipment);
        T OrderReleased(Entity Order);
        T ReceivedByDelivery(Entity Order);
        T DriverNearby(Entity Order);
        T DeliveredToCustomer(Entity Order);
        T CancelOder(Entity Order);
        T RequestCancelOrder(Entity Order);
        T Incomplete(Entity Order);
    }
}
