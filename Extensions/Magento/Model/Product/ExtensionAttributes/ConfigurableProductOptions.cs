using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ConfigurableProductOptions
    {
        [JsonProperty("attribute_id")]
        public string attribute_id { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("id")]
        public int configurable_product_options_id { get; set; }


        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_use_default { get; set; }

        public string label { get; set; }

        public int position { get; set; }

        [JsonProperty("product_id")]
        public int product_id { get; set; }

        [JsonProperty("values")]
        public List<ConfigurableProductOptionsValues> values { get; set; }

    }
}
