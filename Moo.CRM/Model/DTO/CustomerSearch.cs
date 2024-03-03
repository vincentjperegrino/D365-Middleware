using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.DTO;

public class CustomerSearch : Base.Model.SearchBase<Model.CustomerBase> 
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    [JsonProperty(PropertyName = "@odata.nextLink")]
    public string NextLink { get; set; }

    [JsonProperty(PropertyName = "value")]
    public override List<Model.CustomerBase> values { get; set; }

}


