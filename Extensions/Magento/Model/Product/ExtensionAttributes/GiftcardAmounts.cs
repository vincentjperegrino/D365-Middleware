using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class GiftcardAmounts
    {
        [JsonProperty("attribute_id")]
        public int attribute_id { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        public decimal value { get; set; }

        public int website_id { get; set; }

        public decimal website_value { get; set; }

    }
}
