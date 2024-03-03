using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Custom.NCCI.Domain;

public class Invoice : OctoPOS.Domain.Invoice
{
    public Invoice(IOctoPOSService service) : base(service)
    {
    }

    public Invoice(Config config, IDistributedCache cache) : base(config, cache)
    {
    }


    public override Model.Invoice Get(string invoicenumber)
    {
        var invoicemodel = base.Get(invoicenumber);

        return UpdateTax(invoicemodel);
    }

    public override Model.DTO.Invoices.Search SearchInvoiceListWithdetails(DateTime startdate, DateTime enddate, int Pagenumber)
    {
        var SearchInBase = base.SearchInvoiceListWithdetails(startdate, enddate, Pagenumber);

        if(SearchInBase.values is null || SearchInBase.values.Count <= 0)
        {
            return new Model.DTO.Invoices.Search();
        }

        //Update Orders
        SearchInBase.values = SearchInBase.values.Select(invoice => UpdateTax(invoice)).ToList();

        return SearchInBase;
    }

    private Model.Invoice UpdateTax(Model.Invoice invoice)
    {
        int DecimalPlaces = 8;
        decimal TaxComputation = (decimal)1.12;

        decimal addFreightAmountTaxPerItemInCRM = 0;

        if (invoice.freightamount > 0)
        {
            var originalFreightamount = invoice.freightamount;
            invoice.freightamount = Math.Round(originalFreightamount / TaxComputation, DecimalPlaces);
            var taxofFreightamount = originalFreightamount - invoice.freightamount;
            var itemCount = invoice.InvoiceItems.Count();

            if (itemCount > 0)
            {
                addFreightAmountTaxPerItemInCRM = taxofFreightamount / itemCount;
            }
        }

        invoice.InvoiceItems.Select(invoiceitems =>
        {
            invoiceitems.priceperunit = Math.Round(invoiceitems.RetailSalesPrice / TaxComputation, DecimalPlaces);
            //   invoiceitems.tax = Math.Round(invoiceitems.RetailSalesPrice - invoiceitems.priceperunit, DecimalPlaces) * (decimal)invoiceitems.quantity;
            invoiceitems.tax = invoiceitems.tax + addFreightAmountTaxPerItemInCRM;
            invoiceitems.manualdiscountamount = Math.Round(invoiceitems.manualdiscountamount / TaxComputation, DecimalPlaces);
            return invoiceitems;

        }).ToList();

        return invoice;
    }
}
