
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{

    public class ProductExtensionAttributes
    {
        
        [JsonProperty("category_links")]
        public List<ProductCategory> category { get; set; }

        [JsonProperty("stock_item")]
        public StockItem stock_item { get; set; }

        public int[] website_ids { get; set; }

        public VideoContent video_content { get; set; }

        public string vertex_flex_field { get; set; }

        [JsonProperty("qty")]
        public decimal quantity { get; set; }

        public decimal percentage_value { get; set; }

        public int website_id { get; set; }

        public int[] configurable_product_links { get; set; }

        public List<ConfigurableProductOptions> configurable_product_options { get; set; }

        public List<DownloadableProductLinks> downloadable_product_links { get; set; }

        public List<DownloadableProductSamples> downloadable_product_samples { get; set; }

        public List<GiftcardAmounts> giftcard_amounts { get; set; }

        public VertextCommodityCode vertex_commodity_code { get; set; }


    }
}
