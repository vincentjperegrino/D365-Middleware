using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class OrderProductOption
    {
        [JsonProperty("extension_attributes")]
        public ProductExtensionAttributes extension_attributes { get; set; }
    }
}
