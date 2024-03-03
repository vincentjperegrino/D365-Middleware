using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class BundleProductOption
    {

        public ProductExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("option_id")]
        public int option_id { get; set; }

        public int position { get; set; }

        [JsonProperty("product_links")]
        public List<BundleProductLinks> bundle_product_links { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool required { get; set; }

        public string sku { get; set; }

        public string title { get; set; }

        public string type { get; set; }

    }
}
