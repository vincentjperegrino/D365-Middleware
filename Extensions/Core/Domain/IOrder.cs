using System.Collections;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IOrder<T, K> where T : Core.Model.OrderBase where K : Core.Model.OrderItemBase
    {
        T Add(T order);

        T Update(T order);

        T Upsert(T order);

        T Get(long id);

        T Get(string id);

        IEnumerable<K> GetItems(long id);

        T Add(string FromDispatcherQueue);

        T Update(string FromDispatcherQueue, string Orderid);

        T Upsert(string FromDispatcherQueue);

        T GetByField(string FieldName, string FieldValue);

        bool CancelOrder(T FromDispatcherQueue);

        bool IsForDispatch(string FromDispatcherQueue);
        bool IsForReceiver(string FromDispatcherQueue);

    }
}