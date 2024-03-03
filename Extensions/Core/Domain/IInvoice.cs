using KTI.Moo.Extensions.Core.Model;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IInvoice<T, K> where T : InvoiceBase where K : InvoiceItemBase
    {
        T Add(T invoice);

        T Update(T invoice);

        T Upsert(T invoice);

        T Get(long invoiceID);

        T Get(string invoiceID);

        IEnumerable<K> GetItemList(long invoiceID);

        T Add(string FromDispatcherQueue);

        T Update(string FromDispatcherQueue, string Invoiceid);

        T Upsert(string FromDispatcherQueue);

        T GetByField(string FieldName, string FieldValue);

        bool IsForDispatch(string FromDispatcherQueue);
        bool IsForReceiver(string FromDispatcherQueue);
    }
}
