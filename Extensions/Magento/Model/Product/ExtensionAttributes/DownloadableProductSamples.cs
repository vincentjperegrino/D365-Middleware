using KTI.Moo.Extensions.Magento.Model.Base;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class DownloadableProductSamples : SampleProductBase
    {
        public ProductExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("id")]
        public int downloadable_product_samples_id { get; set; }

  

    }
}
