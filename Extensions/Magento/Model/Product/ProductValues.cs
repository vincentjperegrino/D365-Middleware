using Newtonsoft.Json;
using System.Collections.Generic;
namespace KTI.Moo.Extensions.Magento.Model
{
    public class ProductValues
    {
        [JsonProperty("option_type_id")]
        public int option_type_id { get; set; }

        public decimal price { get; set; }

        public string price_type { get; set; }

        public string sku { get; set; }


        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int sort_order { get; set; }

        public string title { get; set; }


    }
}
