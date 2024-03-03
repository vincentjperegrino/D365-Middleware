namespace KTI.Moo.CRM.Model.DTO.ChannelManagement;

public class GetAll
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    [JsonProperty(PropertyName = "kti_channelmanagementlistresponse")]
    public string kti_channelmanagementlistresponse { get; set; }

}
