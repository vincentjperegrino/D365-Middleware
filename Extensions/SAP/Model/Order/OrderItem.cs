namespace KTI.Moo.Extensions.SAP.Model;

public class OrderItem : Core.Model.OrderItemBase
{
    [JsonProperty("LineNum", DefaultValueHandling = DefaultValueHandling.Include)]
    public int LineNum { get; set; }

    [JsonIgnore]
    public override string kti_lineitemnumber { get => LineNum.ToString(); }

    [JsonProperty("ItemCode")]
    public override string productid { get; set; }

    [JsonProperty("ItemDescription")]
    public override string description { get; set; }

    [JsonProperty("WarehouseCode")]
    public string WarehouseCode { get; set; } 

    [JsonProperty("Quantity")]
    public override decimal quantity { get; set; }

    [JsonProperty("Price")]
    public override decimal baseamount { get; set; }

    [JsonProperty("DiscountPercent")]
    public decimal DiscountPercent { get; set; }

    [JsonProperty("UnitPrice", DefaultValueHandling = DefaultValueHandling.Include)]
    public override decimal priceperunit { get; set; }

    [JsonIgnore]
    public override int linetype { get; set; }
}
