
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ItemAppliedTaxes
    {
        [JsonProperty("applied_taxes")]
        public List<AppliedTaxes> applied_taxes { get; set; }

        [JsonProperty("associated_item_id")]
        public int associated_item_id { get; set; }

        [JsonProperty("item_id")]
        public int item_id { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        
    }
}
