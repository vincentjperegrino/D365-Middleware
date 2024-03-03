using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class PrdouctTierPrice
    {
        [JsonProperty("customer_group_id")]
        public int customer_group_id { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("qty")]
        public decimal quantity { get; set; }

        [JsonProperty("value")]
        public decimal value { get; set; }
    }
}
