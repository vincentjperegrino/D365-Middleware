using Newtonsoft.Json;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class RefundLineItem {

    public RefundLineItem() { }

    public RefundLineItem(ShopifySharp.RefundLineItem refundLineItem) {

        if (refundLineItem.LineItem != null)
        { 
            lineitem = new Model.LineItem(refundLineItem.LineItem);
        
        }
        line_item_id = refundLineItem.LineItemId ?? default;
        quantity = refundLineItem.Quantity ?? default;
        total_tax = refundLineItem.TotalTax ?? default;
        sub_total = refundLineItem.SubTotal ?? default;

        if (refundLineItem.SubTotalTaxSet != null)
        {

            sub_total_tax_set = new Model.PriceSet(refundLineItem.SubTotalTaxSet);
        }

        if (refundLineItem.TotalTaxSet != null)
        {

            total_tax_set = new Model.PriceSet(refundLineItem.TotalTaxSet);
        }
        restock_type = refundLineItem.RestockType;
        location_id = refundLineItem.LocationId ?? default;


    }


    public LineItem lineitem { get; set; }
    public long line_item_id { get; set; }
    public int quantity { get; set; }
    public decimal total_tax { get; set; }

    public decimal sub_total { get; set; }

    public PriceSet sub_total_tax_set { get; set; }

    public PriceSet total_tax_set { get; set; }

    public string restock_type { get; set; }

    public long location_id { get; set; }


}