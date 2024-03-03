
namespace KTI.Moo.Extensions.SAP.Model;

public class Invoice : Core.Model.InvoiceBase
{

    [JsonProperty("DocEntry")]
    public int DocEntry { get; set; }

    [JsonProperty("DocNum")]
    public int DocNum { get; set; }

    [JsonIgnore]
    public override string kti_sourceid { get => DocNum.ToString(); }
    [JsonIgnore]
    public override string salesorderid { get; set; }

    [JsonProperty("DocType")]
    public string DocType { get; set; } = Helper.DocType.Items;

    [JsonProperty("DocDate")]
    public string DocDate { get; set; }

    [JsonProperty("DocDueDate")]
    public string DocDueDate { get; set; }

    [JsonProperty("CardCode")]
    public override string customerid { get; set; }

    [JsonProperty("Series")]
    public int Series { get; set; }

    [JsonProperty("Comments")]
    public override string description { get; set; }

    [JsonProperty("U_Channel")]
    public string Channel { get; set; }

    [JsonProperty("U_ORNo")]
    public string ORNo { get; set; }

    [JsonProperty("PaymentGroupCode")]
    public int PaymentGroupCode { get; set; }

    [JsonProperty("AddressExtension")]
    public AddressExtension Address { get; set; }

    [JsonProperty("DocumentLines")]
    public List<InvoiceItem> InvoiceItems { get; set; }
}
