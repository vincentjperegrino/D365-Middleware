namespace KTI.Moo.FO.Model.DTO.ChannelManagement;

public class Get
{
    [JsonProperty(PropertyName = "@odata.context")]
     public string context { get; set; }

    [JsonProperty(PropertyName = "kti_channelmanagementresponse")]
    public string kti_channelmanagementresponse { get; set; }

}
