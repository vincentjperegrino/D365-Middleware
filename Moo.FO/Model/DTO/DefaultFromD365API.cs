using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.FO.Model.DTO;

public class DefaultFromD365API<T>
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context { get; set; }

    public List<T> value { get; set; }

}
