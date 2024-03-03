using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.SAP.Model;

public class AddressExtension
{

    [JsonProperty("ShipToStreet")]
    public string ShipToStreet { get; set; }

    [JsonProperty("ShipToStreetNo")]
    public string ShipToStreetNo { get; set; }

    [JsonProperty("ShipToBlock")]
    public string ShipToBlock { get; set; }

    [JsonProperty("ShipToBuilding")]
    public string ShipToBuilding { get; set; }

    [JsonProperty("ShipToCity")]
    public string ShipToCity { get; set; }

    [JsonProperty("ShipToZipCode")]
    public string ShipToZipCode { get; set; }

    [JsonProperty("ShipToCounty")]
    public string ShipToCounty { get; set; }

    [JsonProperty("ShipToState")]
    public string ShipToState { get; set; }

    [JsonProperty("ShipToCountry")]
    public string ShipToCountry { get; set; }

    [JsonProperty("ShipToAddressType")]
    public string ShipToAddressType { get; set; }

    [JsonProperty("BillToStreet")]
    public string BillToStreet { get; set; }

    [JsonProperty("BillToStreetNo")]
    public string BillToStreetNo { get; set; }

    [JsonProperty("BillToBlock")]
    public string BillToBlock { get; set; }

    [JsonProperty("BillToBuilding")]
    public string BillToBuilding { get; set; }

    [JsonProperty("BillToCity")]
    public string BillToCity { get; set; }

    [JsonProperty("BillToZipCode")]
    public string BillToZipCode { get; set; }

    [JsonProperty("BillToCounty")]
    public string BillToCounty { get; set; }

    [JsonProperty("BillToState")]
    public string BillToState { get; set; }

    [JsonProperty("BillToCountry")]
    public string BillToCountry { get; set; }

    [JsonProperty("BillToAddressType")]
    public string BillToAddressType { get; set; }

    [JsonProperty("ShipToGlobalLocationNumber")]
    public string ShipToGlobalLocationNumber { get; set; }

    [JsonProperty("BillToGlobalLocationNumber")]
    public string BillToGlobalLocationNumber { get; set; }

    [JsonProperty("ShipToAddress2")]
    public string ShipToAddress2 { get; set; }

    [JsonProperty("ShipToAddress3")]
    public string ShipToAddress3 { get; set; }

    [JsonProperty("BillToAddress2")]
    public string BillToAddress2 { get; set; }

    [JsonProperty("BillToAddress3")]
    public string BillToAddress3 { get; set; }

    [JsonProperty("PlaceOfSupply")]
    public string PlaceOfSupply { get; set; }

    [JsonProperty("PurchasePlaceOfSupply")]
    public string PurchasePlaceOfSupply { get; set; }
}
