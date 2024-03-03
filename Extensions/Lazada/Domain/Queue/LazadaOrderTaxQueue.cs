using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Model.DTO;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Domain.Queue;

public class OrderTax : Core.Domain.Queue.IOrderTax<Model.OrderHeader>
{

    private readonly Domain.Order _orderDomain;
    private readonly Service.Queue.Config _config;

    public OrderTax(Service.Queue.Config config, IDistributedCache cache)
    {
        _orderDomain = new(config, cache);
        _config = config;
    }

    public OrderTax(Service.Queue.Config config, ClientTokens clientTokens)
    {
        _orderDomain = new(config, clientTokens);
        _config = config;
    }

    public OrderHeader GetTax_Exclusive(string orderid)
    {
        var IsStringLong = long.TryParse(orderid, out var longorderid);

        if (IsStringLong)
        {

            var header = _orderDomain.GetHeader(longorderid);
            var items = _orderDomain.GetItems(longorderid).ToList();
            var skulist = items.Select(item => item.productid).ToList();
            List<Model.Product> productPriceList = _orderDomain.GetProductPriceFromLazada(skulist);
            var transactions = _orderDomain.GetTransactionDetailsFromFinance(longorderid, header.laz_CreatedOn);

            var sellerId = $"lazada_{_config.Region}_{_config.SellerId}";
            // header.emailaddress = items[0].laz_DigitalDeliveryInfo;
            //header.totallineitemamount = items.Sum(i => i.laz_PaidPrice);
            //header.moosourcesystem = sellerId;

            if (!string.IsNullOrWhiteSpace(header.laz_VoucherCode))
            {
                header.laz_VoucherCode = _orderDomain.GetPromoCodeFromSellerVoucher(header.laz_VoucherCode);
            }


            header.kti_channelurl = $"{_config.BaseSourceUrl}/{longorderid}/{sellerId}";

            int DecimalPlaces = 8;
            decimal TaxComputation = (decimal)1.12;

            decimal addFreightAmountTaxPerItemInCRM = 0;

            if (header.freightamount > 0)
            {
                var originalFreightamount = header.freightamount;
                header.freightamount = Math.Round(originalFreightamount / TaxComputation, DecimalPlaces);
                var taxofFreightamount = originalFreightamount - header.freightamount;
                var itemCount = items.Count();

                if (itemCount > 0)
                {
                    addFreightAmountTaxPerItemInCRM = taxofFreightamount / itemCount;
                }
            }

            foreach (var item in items)
            {
                var PriceInOrder = item.laz_ItemPrice;
                var originalPrice = productPriceList.Where(productprice => productprice.productid == item.productid).Select(productprice => productprice.price).FirstOrDefault();
                decimal PromoDiscount = 0;

                if (PriceInOrder != originalPrice)
                {
                    item.laz_DailyDiscount = originalPrice - PriceInOrder;
                }
                item.laz_RetailPrice = originalPrice;
                item.item_transaction_details = transactions.Where(transaction => transaction.orderItem_no == item.laz_OrderItemID).ToList();

                item.moosourcesystem = header.moosourcesystem;
                item.shipto_city = header.shipto_city;
                item.shipto_contactname = header.shipto_contactname;
                item.shipto_country = header.shipto_country;
                item.shipto_line1 = header.shipto_line1;
                item.shipto_line2 = header.shipto_line2;
                item.shipto_line3 = header.shipto_line3;
                item.shipto_name = header.shipto_name;
                item.shipto_postalcode = header.shipto_postalcode;
                item.shipto_stateorprovince = header.shipto_stateorprovince;
                item.shipto_telephone = header.shipto_telephone;

                item.laz_ItemPrice = Math.Round(item.laz_ItemPrice / TaxComputation, DecimalPlaces);
                item.manualdiscountamount = item.laz_VoucherSeller + item.laz_DailyDiscount;
                item.priceperunit = Math.Round(originalPrice / TaxComputation, DecimalPlaces);
                item.tax = Math.Round(originalPrice - item.priceperunit, DecimalPlaces) * (decimal)item.quantity;
                item.tax += addFreightAmountTaxPerItemInCRM;

            }

            header.order_items = items;

            return header;

        }

        return new OrderHeader();
    }

    public OrderHeader GetTax_Inclusive(string orderid)
    {

        var IsStringLong = long.TryParse(orderid, out var longorderid);

        if (IsStringLong)
        {
            return _orderDomain.Get(longorderid);
        }

        return new OrderHeader();

    }
}
