using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ProductOptions
    {
        [JsonProperty("option_id")]
        public int option_id { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        public string file_extension { get; set; }

        public int image_size_x { get; set; }

        public int image_size_y { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_require { get; set; }

        public int max_characters { get; set; }

        public decimal price { get; set; }

        public string price_type { get; set; }

        public string product_sku { get; set; }

        public string sku { get; set; }

        public int sort_order { get; set; }

        public string title { get; set; } 

        public string type { get; set; }

        public List<ProductValues> values { get; set; }

    }
}
