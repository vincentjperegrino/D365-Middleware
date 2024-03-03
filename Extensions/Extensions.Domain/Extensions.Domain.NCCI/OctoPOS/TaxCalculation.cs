namespace KTI.Moo.Extensions.Domain.NCCI.OctoPOS;

internal static class TaxCalculation
{
    internal static KTI.Moo.Extensions.OctoPOS.Model.Invoice UpdateTax(KTI.Moo.Extensions.OctoPOS.Model.Invoice invoice)
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
