using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.SAP.Model.DTO.Orders;

public class Search : Core.Model.SearchBase<Model.Order>
{
    [JsonProperty("odata.metadata")]
    public string metadata { get; set; }    

    [JsonProperty("odata.nextLink")]
    public string nextLink { get; set; }

    [JsonProperty("value")]
    public override List<Model.Order> values { get; set; }
}
