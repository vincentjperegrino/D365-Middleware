using KTI.Moo.Extensions.Shopify.Model;
using KTI.Moo.Extensions.Shopify.Service;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Domain;

public class Order : KTI.Moo.Extensions.Core.Domain.IOrder<Model.Order, Model.OrderItem>
{

    private readonly OrderService _service;



    public Order(Config config)
    {
        _service = new(config.defaultURL, config.admintoken);

    }
    public Model.Order Add(Model.Order order)
    {
        throw new NotImplementedException();
    }

    public Model.Order Add(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }

    public bool CancelOrder(Model.Order FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }

    public Model.Order Get(long id)
    {
        try
        {
            var OrderDTO = _service.GetAsync(id).GetAwaiter().GetResult();

            if (OrderDTO is null)
            {
                return new Model.Order();
            }

            var Order = new Model.Order(OrderDTO);

            return Order;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by OrderId. {ex.Message}");
        }
    }

    public Model.Order Get(string id)
    {
        throw new NotImplementedException();
    }

    public Model.Order GetByField(string FieldName, string FieldValue)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OrderItem> GetItems(long id)
    {
        throw new NotImplementedException();
    }

    public bool IsForDispatch(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }

    public bool IsForReceiver(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }

    public Model.Order Update(Model.Order order)
    {
        throw new NotImplementedException();
    }

    public Model.Order Update(string FromDispatcherQueue, string Orderid)
    {
        throw new NotImplementedException();
    }

    public Model.Order Upsert(Model.Order order)
    {
        throw new NotImplementedException();
    }

    public Model.Order Upsert(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }
}
