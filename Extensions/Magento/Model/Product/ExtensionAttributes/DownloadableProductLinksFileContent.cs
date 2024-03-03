using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class DownloadableProductLinksFileContent
    {
        public ProductExtensionAttributes extension_attributes { get; set; }   

        public string file_data { get; set; }

        [JsonProperty("name")]
        public string file_name { get; set; }

    }
}
