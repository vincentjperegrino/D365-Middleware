using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class RefundOrderAdjustment { 

    public RefundOrderAdjustment() { }

    public RefundOrderAdjustment(ShopifySharp.RefundOrderAdjustment refundOrderAdjustment) {

        order_id = refundOrderAdjustment.OrderId ?? default;
        refund_id = refundOrderAdjustment.RefundId ?? default;
        amount = refundOrderAdjustment.Amount ?? default;  
        tax_amount = refundOrderAdjustment.TaxAmount ?? default;
        kind = refundOrderAdjustment.Kind;
        reason = refundOrderAdjustment.Reason;
        if (refundOrderAdjustment.AmountSet != null)
        { 
            amount_set = new Model.PriceSet(refundOrderAdjustment.AmountSet);   
        
        }

        if (refundOrderAdjustment.TaxAmountSet != null) {
            
            tax_amount_set = new Model.PriceSet(refundOrderAdjustment.TaxAmountSet);
        
        }
    }


    public long order_id { get; set; }
    public long refund_id { get; set; }
    public decimal amount { get; set; }
    public decimal tax_amount { get; set; }
    public string kind { get; set; }
    public string reason { get; set; }
    public PriceSet amount_set { get; set; }
    public PriceSet tax_amount_set { get; set; }



}
