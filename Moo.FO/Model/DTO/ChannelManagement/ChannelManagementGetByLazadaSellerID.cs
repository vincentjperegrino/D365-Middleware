using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.FO.Model.DTO.ChannelManagement;

public class GetByLazadaSellerID
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    [JsonProperty(PropertyName = "kti_getchannelmanagementbylazadasellerid_response")]
    public string kti_channelmanagementresponse { get; set; }
}
