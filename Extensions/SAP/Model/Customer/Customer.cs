namespace KTI.Moo.Extensions.SAP.Model;

public class Customer : Core.Model.CustomerBase
{
    [JsonProperty("CardCode")]
    public override string kti_sapbpcode { get; set; }

    [JsonProperty("CardName")]
    public string CardName { get; set; }

    [JsonProperty("CardType")]
    public string CardType { get; set; } = Helper.Customer.CardType.cCustomer;

    [JsonProperty("Series")]
    public int Series { get; set; }

    [JsonProperty("Cellular")]
    public override string mobilephone { get; set; }

    [JsonProperty("EmailAddress")]
    public string EmailAddress { get; set; }

    [JsonProperty("U_Channel")]
    public string Channel { get; set; }

    [JsonProperty("U_MemberCode")]
    public string MemberCode { get; set; }

    [JsonProperty("UpdateDate")]
    public string UpdateDate { get; set; }

    [JsonProperty("UpdateTime")]
    public string UpdateTime { get; set; }

    [JsonProperty("CreateDate")]
    public string CreateDate { get; set; }

    [JsonProperty("CreateTime")]
    public string CreateTime { get; set; }

    [JsonProperty("BPAddresses")]
    public virtual List<Address> Addresses { get; set; }


}
