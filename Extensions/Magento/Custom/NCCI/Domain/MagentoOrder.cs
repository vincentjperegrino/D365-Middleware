using KTI.Moo.Extensions.Magento.Model.DTO.Orders;
using KTI.Moo.Extensions.Magento.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Custom.NCCI.Domain;

public class Order : Magento.Domain.Order
{
    public Order(Config config) : base(config)
    {
    }

    public Order(Config config, IDistributedCache cache) : base(config, cache)
    {
    }
    public Order(Config config, IDistributedCache cache, ILogger log) : base(config, cache, log)
    {
    }

    public Order(IMagentoService service) : base(service)
    {
    }

    public Order(string defaultDomain, string redisConnectionString, string username, string password) : base(defaultDomain, redisConnectionString, username, password)
    {
    }

    public override Model.Order Get(long orderid)
    {
        var OrderModel = base.Get(orderid);

        return UpdateTax(OrderModel);
    }

    public override Search GetSearchOrders(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
    {
        var SearchInBase = base.GetSearchOrders(dateFrom, dateTo, pagesize, currentPage);

        if (SearchInBase.values is null || SearchInBase.values.Count <= 0)
        {
            return new Search();
        }

        //Update Orders
        SearchInBase.values = SearchInBase.values.Where(orders => orders.order_payment.method == "cashondelivery" || orders.order_payment.method == "phoenix_cashondelivery" || orders.order_status == "processing" || orders.order_status == "complete").Select(orders => UpdateTax(orders)).ToList();

        return SearchInBase;
    }

    private Model.Order UpdateTax(Model.Order order)
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
