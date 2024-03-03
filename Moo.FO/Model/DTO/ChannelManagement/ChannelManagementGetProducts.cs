namespace KTI.Moo.FO.Model.DTO.ChannelManagement;

public class GetProducts
{
    [JsonProperty("@odata.context")]
    public string context { get; set; }

    [JsonProperty("kti_product_channelmanagementresponse")]
    public string kti_channelmanagementresponse { get; set; }
}
