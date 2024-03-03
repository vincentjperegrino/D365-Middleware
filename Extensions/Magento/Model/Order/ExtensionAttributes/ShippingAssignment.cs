using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ShippingAssignment
    {
        [JsonProperty("shipping")]
        public Shipping shipping { get; set; }

        [JsonProperty("items")]
        public List<OrderItem> shipping_items { get; set; }

        [JsonProperty("stock_id")]
        public int stock_id { get; set; }

    }

}
