namespace KTI.Moo.Extensions.SAP.Model;

public class Address : Core.Model.AddressBase
{

    [JsonProperty("RowNum", DefaultValueHandling = DefaultValueHandling.Include)]
    public int RowNum { get; set; }

    [JsonProperty("AddressType")]
    public string AddressType { get; set; } = Helper.Customer.AddressType.Billing;

    [JsonProperty("AddressName")]
    public string AddressName { get; set; } = Helper.Customer.DefaultAddressName.billing;

    [JsonProperty("Street")]
    public override string address_line1 { get; set; }

    [JsonProperty("ZipCode")]
    public override string address_postalcode { get; set; }

    [JsonProperty("City")]
    public override string address_city { get; set; }

    [JsonProperty("Country")]
    public override string address_country { get; set; } = "PH";



    [JsonProperty("County")]
    public string County { get; set; }

    [JsonProperty("Block")]
    public string Block { get; set; }

    [JsonProperty("State")]
    public string State { get; set; }

    [JsonProperty("BuildingFloorRoom")]
    public string BuildingFloorRoom { get; set; }

    [JsonProperty("AddressName2")]
    public string AddressName2 { get; set; }

    [JsonProperty("AddressName3")]
    public string AddressName3 { get; set; }

    [JsonProperty("TypeOfAddress")]
    public string TypeOfAddress { get; set; }

    [JsonProperty("StreetNo")]
    public string StreetNo { get; set; }

    [JsonProperty("BPCode")]
    public string BPCode { get; set; }

    [JsonProperty("GlobalLocationNumber")]
    public string GlobalLocationNumber { get; set; }

    [JsonProperty("Nationality")]
    public string Nationality { get; set; }

    [JsonProperty("TaxOffice")]
    public string TaxOffice { get; set; }

    [JsonProperty("GSTIN")]
    public string GSTIN { get; set; }

    [JsonProperty("GstType")]
    public string GstType { get; set; }

    [JsonProperty("CreateDate")]
    public string CreateDate { get; set; }

    [JsonProperty("CreateTime")]
    public string CreateTime { get; set; }

}
