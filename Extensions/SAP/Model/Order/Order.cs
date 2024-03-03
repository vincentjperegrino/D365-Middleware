
namespace KTI.Moo.Extensions.SAP.Model;

public class Order : Core.Model.OrderBase
{

    [JsonProperty("DocEntry")]
    public int DocEntry { get; set; }

    [JsonProperty("DocNum")]
    public int DocNum { get; set; }

    [JsonProperty("Series")]
    public int Series { get; set; } 

    [JsonProperty("PaymentGroupCode")]
    public int PaymentGroupCode { get; set; }

    [JsonIgnore]
    public override string kti_sourceid { get => DocNum.ToString(); }

    [JsonProperty("DocType")]
    public string DocType { get; set; } = Helper.DocType.Items;

    [JsonProperty("DocDate")]
    public string DocDate { get; set; }

    [JsonProperty("DocDueDate")]
    public string DocDueDate { get; set; }

    [JsonProperty("CardCode")]
    public override string customerid { get; set; }

    [JsonProperty("Comments")]
    public override string description { get; set; }

    [JsonProperty("U_Channel")]
    public string Channel { get; set; }    
    
    [JsonProperty("NumAtCard")]
    public string NumAtCard { get; set; }

    [JsonProperty("U_WebOrderNo")]
    public string WebOrderNo { get; set; } 
    
    [JsonProperty("U_SORemarks")]
    public string SORemarks { get; set; }


    [JsonProperty("DocumentLines")]
    public List<OrderItem> OrderItems { get; set; }

    [JsonProperty("AddressExtension")]
    public AddressExtension Address { get; set; }

}


