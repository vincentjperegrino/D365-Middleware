using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain;

public interface IOrderstatus<T> where T : Model.OrderStatusBase
{
    T Get(string OrderID);
    T InCart(T OrderStatus);
    T ForPayment(T OrderStatus);
    T Checkout(T OrderStatus);
    T OrderPrepared(T OrderStatus);
    T OrderPacked(T OrderStatus);
    T ForDispatch(T OrderStatus);
    T OrderReleased(T OrderStatus);
    T ReceivedByDelivery(T OrderStatus);
    T DriverNearby(T OrderStatus);
    T DeliveredToCustomer(T OrderStatus);
    T CancelOder(T OrderStatus);
    T RequestCancelOrder(T OrderStatus);
    T Incomplete(T OrderStatus);

}
