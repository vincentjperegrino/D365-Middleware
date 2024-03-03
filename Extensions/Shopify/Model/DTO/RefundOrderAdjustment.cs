using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class RefundOrderAdjustment :ShopifySharp.RefundOrderAdjustment { 
    

       public RefundOrderAdjustment() { 
        
        
        
        }

        public RefundOrderAdjustment(Model.RefundOrderAdjustment refundOrderAdjustment)
        {
            OrderId = refundOrderAdjustment.order_id;
            RefundId = refundOrderAdjustment.refund_id;
            Amount = refundOrderAdjustment.amount;
            TaxAmount = refundOrderAdjustment.tax_amount;
            Kind = refundOrderAdjustment.kind;
            Reason = refundOrderAdjustment.reason;

            if (refundOrderAdjustment.amount_set != null) {

                AmountSet = new Model.DTO.PriceSet(refundOrderAdjustment.amount_set);

            }

            if (refundOrderAdjustment.tax_amount_set != null) {

                TaxAmountSet = new Model.DTO.PriceSet(refundOrderAdjustment.tax_amount_set);
            
            }







        }

    }
}
