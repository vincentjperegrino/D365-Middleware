using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.SAP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Domain.NCCI.SAP
{
    public class Invoice : IInvoice<KTI.Moo.Extensions.SAP.Model.Invoice, KTI.Moo.Extensions.SAP.Model.InvoiceItem>
    {

        private readonly IInvoice<KTI.Moo.Extensions.SAP.Model.Invoice, KTI.Moo.Extensions.SAP.Model.InvoiceItem> _invoiceDomain;

        public Invoice(IInvoice<Extensions.SAP.Model.Invoice, KTI.Moo.Extensions.SAP.Model.InvoiceItem> invoiceDomain)
        {
            _invoiceDomain = invoiceDomain;
        }


        public Extensions.SAP.Model.Invoice Add(Extensions.SAP.Model.Invoice invoice)
        {
            return _invoiceDomain.Add(invoice);
        }

        public Extensions.SAP.Model.Invoice Add(string FromDispatcherQueue)
        {
            return _invoiceDomain.Add(FromDispatcherQueue);
        }

        public Extensions.SAP.Model.Invoice Get(long invoiceID)
        {
            return _invoiceDomain.Get(invoiceID);
        }

        public Extensions.SAP.Model.Invoice Get(string invoiceID)
        {
            return _invoiceDomain.Get(invoiceID);
        }

        public Extensions.SAP.Model.Invoice GetByField(string FieldName, string FieldValue)
        {
            return _invoiceDomain.GetByField(FieldName, FieldValue);
        }

        public IEnumerable<InvoiceItem> GetItemList(long invoiceID)
        {
            return _invoiceDomain.GetItemList(invoiceID);
        }

        public bool IsForDispatch(string FromDispatcherQueue)
        {
            return _invoiceDomain.IsForDispatch(FromDispatcherQueue);
        }

        public bool IsForReceiver(string FromDispatcherQueue)
        {
            return _invoiceDomain.IsForReceiver(FromDispatcherQueue);
        }

        public Extensions.SAP.Model.Invoice Update(Extensions.SAP.Model.Invoice invoice)
        {
            return _invoiceDomain.Update(invoice);
        }

        public Extensions.SAP.Model.Invoice Update(string FromDispatcherQueue, string invoiceid)
        {
            return _invoiceDomain.Update(FromDispatcherQueue , invoiceid);
        }

        public Extensions.SAP.Model.Invoice Upsert(Extensions.SAP.Model.Invoice invoice)
        {
            return _invoiceDomain.Upsert(invoice);
        }

        public Extensions.SAP.Model.Invoice Upsert(string FromDispatcherQueue)
        {
            return _invoiceDomain.Upsert(FromDispatcherQueue);
        }
    }
}
