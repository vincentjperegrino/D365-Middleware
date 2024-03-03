namespace KTI.Moo.FO.Model.DTO.ChannelManagement;

public class GetAllProducts
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    [JsonProperty(PropertyName = "kti_product_channelmanagementlistresponse")]
    public string kti_channelmanagementlistresponse { get; set; }



}
