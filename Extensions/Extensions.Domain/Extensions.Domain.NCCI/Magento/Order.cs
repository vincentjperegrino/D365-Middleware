using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Model.DTO.Orders;
using System.Reflection;

namespace KTI.Moo.Extensions.Domain.NCCI.Magento;
public class Order : IOrder<Extensions.Magento.Model.Order, Extensions.Magento.Model.OrderItem>
{
    private readonly IOrder<Extensions.Magento.Model.Order, Extensions.Magento.Model.OrderItem> _orderDomain;

    public Order(IOrder<Extensions.Magento.Model.Order, OrderItem> orderDomain)
    {
        _orderDomain = orderDomain;
    }

    public Extensions.Magento.Model.Order Add(Extensions.Magento.Model.Order order)
    {
        return _orderDomain.Add(order);
    }

    public Extensions.Magento.Model.Order Add(string FromDispatcherQueue)
    {
        return _orderDomain.Add(FromDispatcherQueue);
    }

    public bool CancelOrder(Extensions.Magento.Model.Order FromDispatcherQueue)
    {
        return _orderDomain.CancelOrder(FromDispatcherQueue);
    }

    public Extensions.Magento.Model.Order Get(long id)
    {
        var order = _orderDomain.Get(id);

        var TaxOrder = TaxCalculation.UpdateTax(order);

        return TaxOrder;

    }

    public Extensions.Magento.Model.Order Get(string id)
    {
        var order = _orderDomain.Get(id);

        var TaxOrder = TaxCalculation.UpdateTax(order);

        return TaxOrder;
    }

    public Extensions.Magento.Model.Order GetByField(string FieldName, string FieldValue)
    {
        var order = _orderDomain.GetByField(FieldName, FieldValue);
        var TaxOrder = TaxCalculation.UpdateTax(order);

        return TaxOrder;
    }

    public IEnumerable<OrderItem> GetItems(long id)
    {
        return _orderDomain.GetItems(id);
    }

    public bool IsForDispatch(string FromDispatcherQueue)
    {
        return _orderDomain.IsForDispatch(FromDispatcherQueue);
    }

    public bool IsForReceiver(string FromDispatcherQueue)
    {
        return _orderDomain.IsForReceiver(FromDispatcherQueue);
    }

    public Extensions.Magento.Model.Order Update(Extensions.Magento.Model.Order order)
    {
        return _orderDomain.Update(order);
    }

    public Extensions.Magento.Model.Order Update(string FromDispatcherQueue, string Orderid)
    {
        return _orderDomain.Update(FromDispatcherQueue, Orderid);
    }

    public Extensions.Magento.Model.Order Upsert(Extensions.Magento.Model.Order order)
    {
        return _orderDomain.Upsert(order);
    }

    public Extensions.Magento.Model.Order Upsert(string FromDispatcherQueue)
    {
        return _orderDomain.Upsert(FromDispatcherQueue);
    }
}
