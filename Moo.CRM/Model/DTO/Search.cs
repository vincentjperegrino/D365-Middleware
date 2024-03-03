using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.DTO;

public class Search<T> : SearchBase<T> where T : class
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    [JsonProperty(PropertyName = "@odata.nextLink")]
    public string NextLink { get; set; }

    [JsonProperty(PropertyName = "value")]
    public override List<T> values { get; set; }

}
