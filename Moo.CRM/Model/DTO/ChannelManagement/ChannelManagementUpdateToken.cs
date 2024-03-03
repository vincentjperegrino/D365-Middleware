using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.DTO.ChannelManagement;

public class UpdateToken
{
    [JsonProperty("@odata.context")]
    public string context { get; set; }

    [JsonProperty("kti_updatechannelmanagementtokens_response")]
    public bool kti_channelmanagementresponse { get; set; }
}
