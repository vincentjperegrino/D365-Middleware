
namespace KTI.Moo.Extensions.SAP.Model;
public class InvoiceItem : Core.Model.InvoiceItemBase
{
    [JsonProperty("LineNum")]
    public int LineNum { get; set; }

    [JsonIgnore]
    public override string kti_lineitemnumber { get => LineNum.ToString(); }

    [JsonProperty("ItemCode")]
    public override string productid { get; set; }

    [JsonProperty("ItemDescription")]
    public override string description { get; set; }

    [JsonProperty("Quantity")]
    public override decimal quantity { get; set; }

    [JsonProperty("Price")]
    public override decimal baseamount { get; set; }

    [JsonProperty("DiscountPercent")]
    public decimal DiscountPercent { get; set; }

    [JsonProperty("UnitPrice")]
    public override decimal priceperunit { get; set; }


}
