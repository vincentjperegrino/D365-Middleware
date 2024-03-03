using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using KTI.Moo.Extensions.Lazada.Domain;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class ReverseOrder
    {
        [JsonPropertyName("reverse_order_lines")]
        public List<ReverseOrderItem> Items { get; set; }
        [JsonPropertyName("reverse_order_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long Id { get; set; }
        [JsonPropertyName("request_type")]
        public string RequestType { get; set; }
        [JsonPropertyName("is_rtm")]
        public bool IsRTM { get; set; }
        [JsonPropertyName("shipping_type")]
        public string ShippingType { get; set; }
        [JsonPropertyName("trade_order_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public string TradeOrderId { get; set; }
    }

    public class ReverseOrderItem
    {
        [JsonPropertyName("product")]
        public ReverseOrderProduct Product { get; set; }
        [JsonPropertyName("reverse_status")]
        public string ReverseStatus { get; set; }
        [JsonPropertyName("is_need_refund")]
        public bool ShouldRefund { get; set; }
        [JsonPropertyName("trade_order_line_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long TradeOrderLineId { get; set; }
        [JsonPropertyName("reverse_order_line_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long Id { get; set; }
        [JsonPropertyName("ofc_status")]
        public string OFCStatus { get; set; }
        [JsonPropertyName("buyer_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long BuyerId { get; set; }
    }

    public class ReverseOrderProduct
    {
        [JsonPropertyName("product_sku")]
        public string Sku { get; set; }
        [JsonPropertyName("product_id")]
        public string Id { get; set; }
    }

    public class CancellationEligibility
    {
        [JsonPropertyName("tip_content")]
        public string TipContent { get; set; }
        [JsonPropertyName("tip_type")]
        public string TipType { get; set; }
        [JsonPropertyName("reason_options")]
        public IEnumerable<CancellationEligibilityReason> ReasonOptions { get; set; }
    }

    public class CancellationReason
    {
        [JsonPropertyName("text")]
        public virtual string Name { get; set; }
        [JsonPropertyName("reason_id")]
        public virtual string Id { get; set; }
    }

    public class CancellationEligibilityReason : CancellationReason
    {
        [JsonPropertyName("reason_name")]
        public override string Name { get; set; }
        [JsonPropertyName("reason_id")]
        public override string Id { get; set; }
    }

    public class ReverseOrderDetail
    {
        [JsonPropertyName("reverse_order_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long Id { get; set; }
        [JsonPropertyName("request_type")]
        public string RequestType { get; set; }
        [JsonPropertyName("is_rtm")]
        public bool IsRTM { get; set; }
        [JsonPropertyName("shipping_type")]
        public string ShippingType { get; set; }
        [JsonPropertyName("trade_order_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long TradeOrderId { get; set; }
        [JsonPropertyName("reverseOrderLineDTOList")]
        public List<ReverseOrderDetailLine> LineItems { get; set; }
    }

    public class ReverseOrderDetailLine
    {
        [JsonPropertyName("reverse_status")]
        public string Status { get; set; }
        [JsonPropertyName("is_need_refund")]
        public bool ShouldRefund { get; set; }
        [JsonPropertyName("trade_order_line_id")]
        public long TradeOrderLineId { get; set; }
        [JsonPropertyName("ofc_status")]
        public string OfcStatus { get; set; }
        [JsonPropertyName("productDTO")]
        public ReverseOrderProduct Product { get; set; }
        [JsonPropertyName("reverse_order_line_id")]
        public long ReverseOrderLineId { get; set; }
        [JsonPropertyName("buyer")]
        public ReverseOrderBuyer Buyer { get; set; }
    }

    public class ReverseOrderBuyer
    {
        [JsonPropertyName("buyer_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public string Id { get; set; }
    }

    public class ReverseOrderCancelTip
    {
        [JsonPropertyName("tip_content")]
        public string Content { get; set; }
        [JsonPropertyName("tip_type")]
        public string Type { get; set; }
    }

    public class ReverseOrderLine
    {
        [JsonPropertyName("order_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long OrderId { get; set; }
        [JsonPropertyName("order_line_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long OrderLineId { get; set; }
        [JsonPropertyName("reverse_order_line_id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long ReverseOrderLineId { get; set; }
        [JsonPropertyName("seller_sku")]
        public string SellerSku { get; set; }
        [JsonPropertyName("paid_price")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal PaidPrice { get; set; }
        [JsonPropertyName("refund_amount")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal RefundAmount { get; set; }
        [JsonPropertyName("is_cancel")]
        public bool IsCancelled { get; set; }
        [JsonPropertyName("reason_id")]
        public string ReasonId { get; set; }
        [JsonPropertyName("reason_name")]
        public string ReasonName { get; set; }
        [JsonPropertyName("reason_type")]
        public string ReasonType { get; set; }
        [JsonPropertyName("reason_source")]
        public string ReasonSource { get; set; }
        [JsonPropertyName("reason_desc")]
        public string ReasonDescription { get; set; }
        [JsonPropertyName("apply_reason")]
        public string ApplyReason { get; set; }
    }

    public class ReverseOrderUpdate
    {
        [JsonPropertyName("reason_info")]
        public List<CancellationEligibilityReason> ReasonInfo { get; set; }
        public long ReverseOrderId { get; set; }
        public decimal TotalRefund { get; set; }
        public List<ReverseOrderLine> ReverseOrderLines { get; set; }
    }

    public class ReverseOrderCommunicationHistory
    {
        [JsonPropertyName("list")]
        public List<ReverseOrderCommunicationList> List { get; set; }
        [JsonPropertyName("page_info")]
        public ReverseOrderCommunicationPageInfo PageInfo { get; set; }
    }
    public class ReverseOrderCommunicationList
    {
        public DateTime Time { get; set; }
        [JsonPropertyName("time")]
        public ulong Timestamp
        {
            get => Timestamp;
            set
            {
                Timestamp = value;
                Time = DateTime.UnixEpoch.AddMilliseconds(value);
            }
        }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("picture")]
        public List<string> Picture { get; set; }
    }
    public class ReverseOrderCommunicationPageInfo
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("current_page_number")]
        public int CurrentPage { get; set; }
    }
}
