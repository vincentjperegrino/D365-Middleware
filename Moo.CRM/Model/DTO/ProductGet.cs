using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model.DTO
{
    public class ProductGet : Base.Model.SearchBase<Model.ProductBase>
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }

        //[JsonProperty(PropertyName = "@odata.nextLink")]
        //public string NextLink { get; set; }

        [JsonProperty(PropertyName = "value")]
        public override List<Model.ProductBase> values { get; set; }

    }
}
