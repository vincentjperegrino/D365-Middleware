using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.FO.Model.DTO.ChannelManagement;

public class GetByLazadaSellerIDParameters
{
    [JsonProperty(PropertyName = "kti_lazadasellerid")]
    public string kti_lazadasellerid { get; set; }
}
