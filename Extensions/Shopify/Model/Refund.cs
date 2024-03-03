using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Refund { 

    public Refund() { } 

    public Refund(ShopifySharp.Refund refund) {

        id = refund.Id ?? default;
        order_id = refund.OrderId ?? default;
        created_at = refund.CreatedAt ?? default;
        notify = refund.Notify ?? default;
        note = refund.Note;
        user_id = refund.UserId ?? default;
        processed_at = refund.ProcessedAt ?? default;
        refund_line_items = refund.RefundLineItems?.Select(refundLineItems => new RefundLineItem(refundLineItems)).ToList();
        transactions = refund.Transactions?.Select(transactions => new Transaction(transactions)).ToList();
        order_adjustments = refund.OrderAdjustments?.Select(orderAdjustments => new RefundOrderAdjustment(orderAdjustments)).ToList();

    }   

    public long id { get; set; }
    public long order_id { get; set; }
    public DateTimeOffset created_at { get; set; }
    public string note { get; set; }
    public bool notify { get; set; }
    public long user_id { get; set; }
    public DateTimeOffset processed_at { get; set; }
    public List<RefundLineItem> refund_line_items { get; set; }
    public List<Transaction> transactions { get; set; }
    public List<RefundOrderAdjustment> order_adjustments { get; set; }


}
