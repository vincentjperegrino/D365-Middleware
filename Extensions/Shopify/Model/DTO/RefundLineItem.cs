using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class RefundLineItem : ShopifySharp.RefundLineItem {

        public RefundLineItem() { 
        
        }

        public RefundLineItem(Model.RefundLineItem refundLineItem) {

            if (refundLineItem.lineitem != null) {

                LineItem = new Model.DTO.LineItem(refundLineItem.lineitem);

            }
            LineItemId = refundLineItem.line_item_id;
            TotalTax = refundLineItem.total_tax;
            SubTotal = refundLineItem.sub_total;
            if (refundLineItem.sub_total_tax_set != null)
            {
                SubTotalTaxSet = new Model.DTO.PriceSet(refundLineItem.sub_total_tax_set);

            }
            if (refundLineItem.total_tax_set != null)
            {

                TotalTaxSet = new Model.DTO.PriceSet(refundLineItem.total_tax_set);
            }

            RestockType = refundLineItem.restock_type;
            LocationId = refundLineItem.location_id;




        }


    }
}
