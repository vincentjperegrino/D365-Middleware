using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class MediaGallery
    {
        [JsonProperty("id")]
        public int media_gallery_id { get; set; }

        public Content content { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool disabled { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        public string file { get; set; }

        public string label { get; set; }

        public string media_type { get; set; }

        public int position { get; set; }

        public string[] types { get; set; }


    }
}
