
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Shipping
    {

        [JsonProperty("address")]
        public OrderAddress shipping_address { get; set; }
            
        [JsonProperty("method")]
        public string shipping_method { get; set; }

        [JsonProperty("total")]
        public ShippingTotal shipping_total { get; set; }

    }
}
