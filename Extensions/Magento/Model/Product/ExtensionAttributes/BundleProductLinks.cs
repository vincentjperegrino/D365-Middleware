using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class BundleProductLinks
    {
        public int can_change_quantity { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("id")]
        public int bundle_product_links_id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_default { get; set; }

        [JsonProperty("option_id")]
        public int option_id { get; set; }

        public int position { get; set; }

        public decimal price { get; set; }

        public int price_type { get; set; }

        [JsonProperty("qty")]
        public string quantity { get; set; }

        public string sku { get; set; }

    }
}
