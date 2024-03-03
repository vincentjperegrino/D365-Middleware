using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Model;

namespace KTI.Moo.Extensions.Domain.NCCI.OctoPOS;

public class Invoice : IInvoice<KTI.Moo.Extensions.OctoPOS.Model.Invoice, KTI.Moo.Extensions.OctoPOS.Model.InvoiceItem>
{
    private readonly IInvoice<Extensions.OctoPOS.Model.Invoice, Extensions.OctoPOS.Model.InvoiceItem> _invoiceDomain;

    public Invoice(IInvoice<Extensions.OctoPOS.Model.Invoice, InvoiceItem> invoiceDomain)
    {
        _invoiceDomain = invoiceDomain;
    }

    public Extensions.OctoPOS.Model.Invoice Add(Extensions.OctoPOS.Model.Invoice invoice)
    {
        return _invoiceDomain.Add(invoice);
    }

    public Extensions.OctoPOS.Model.Invoice Add(string FromDispatcherQueue)
    {
        return _invoiceDomain.Add(FromDispatcherQueue);
    }

    public Extensions.OctoPOS.Model.Invoice Get(long invoiceID)
    {
        var invoice = _invoiceDomain.Get(invoiceID);

        var TaxInvoice = TaxCalculation.UpdateTax(invoice);

        return TaxInvoice;
    }

    public Extensions.OctoPOS.Model.Invoice Get(string invoiceID)
    {
        var invoice = _invoiceDomain.Get(invoiceID);

        var TaxInvoice = TaxCalculation.UpdateTax(invoice);

        return TaxInvoice;
    }

    public Extensions.OctoPOS.Model.Invoice GetByField(string FieldName, string FieldValue)
    {
        var invoice = _invoiceDomain.GetByField(FieldName, FieldValue);

        var TaxInvoice = TaxCalculation.UpdateTax(invoice);

        return TaxInvoice;
    }

    public IEnumerable<InvoiceItem> GetItemList(long invoiceID)
    {
        return _invoiceDomain.GetItemList(invoiceID);
    }

    public bool IsForDispatch(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }

    public bool IsForReceiver(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }

    public Extensions.OctoPOS.Model.Invoice Update(Extensions.OctoPOS.Model.Invoice invoice)
    {
        throw new NotImplementedException();
    }

    public Extensions.OctoPOS.Model.Invoice Update(string FromDispatcherQueue, string Invoiceid)
    {
        throw new NotImplementedException();
    }

    public Extensions.OctoPOS.Model.Invoice Upsert(Extensions.OctoPOS.Model.Invoice invoice)
    {
        throw new NotImplementedException();
    }

    public Extensions.OctoPOS.Model.Invoice Upsert(string FromDispatcherQueue)
    {
        throw new NotImplementedException();
    }
}
