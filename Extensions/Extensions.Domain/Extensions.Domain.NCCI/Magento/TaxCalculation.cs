namespace KTI.Moo.Extensions.Domain.NCCI.Magento;
internal static class TaxCalculation
{
    internal static Extensions.Magento.Model.Order UpdateTax(Extensions.Magento.Model.Order order)
    {
        int DecimalPlaces = 8;
        decimal TaxComputation = (decimal)1.12;

        decimal addFreightAmountTaxPerItemInCRM = 0;

        if (order.freightamount > 0)
        {
            var originalFreightamount = order.freightamount;
            order.freightamount = Math.Round(originalFreightamount / TaxComputation, DecimalPlaces);
            var taxofFreightamount = originalFreightamount - order.freightamount;
            var itemCount = order.order_items.Count();

            if (itemCount > 0)
            {
                addFreightAmountTaxPerItemInCRM = taxofFreightamount / itemCount;
            }
        }

        order.order_items.Select(orderitems =>
        {
            orderitems.priceperunit = Math.Round(orderitems.priceperunit / TaxComputation, DecimalPlaces);
            orderitems.tax = Math.Round(orderitems.price_including_tax - orderitems.priceperunit, DecimalPlaces) * (decimal)orderitems.quantity;
            orderitems.tax = orderitems.tax + addFreightAmountTaxPerItemInCRM;
            return orderitems;

        }).ToList();

        return order;
    }

}
