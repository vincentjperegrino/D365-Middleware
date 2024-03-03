using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Refund : ShopifySharp.Refund { 
    
    

        public Refund() { }

        public Refund(Model.Refund refund) 
        {
            OrderId = refund.order_id;
            Id = refund.id;
            if (refund.created_at != default)
            {
                CreatedAt = refund.created_at;
            }
            Notify = refund.notify;
            //if (refund.shipping)
            OrderAdjustments = refund.order_adjustments?.Select(orderAdjustments => new RefundOrderAdjustment(orderAdjustments));
            if (refund.processed_at != default)
            {
                ProcessedAt = refund.processed_at;
            }
            Note = refund.note;
            //DiscrepancyReason = refund.discrepany_reason;
            RefundLineItems = refund.refund_line_items?.Select(refundLineItem => new RefundLineItem(refundLineItem));
            Transactions = refund.transactions?.Select(transaction => new Transaction(transaction));
            UserId = refund.user_id;
            //Duties = refund.refund_duty







        }



    }
}
