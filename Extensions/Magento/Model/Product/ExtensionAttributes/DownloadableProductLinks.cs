using KTI.Moo.Extensions.Core.Model;
using KTI.Moo.Extensions.Magento.Model.Base;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class DownloadableProductLinks : SampleProductBase
    {
        public ProductExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("id")]
        public int downloadable_product_links_id { get; set; }

        public int is_shareable { get; set; }

        public string link_file { get; set; }

        public DownloadableProductLinksFileContent link_file_content { get; set; }

        public string link_type { get; set; }

        public string link_url { get; set; }

        public int number_of_downloads { get; set; }

        public string price { get; set; }



    }
}
