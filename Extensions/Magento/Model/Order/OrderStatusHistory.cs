using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class OrderStatusHistory
    {
        [JsonProperty("comment")]
        public string comment { get; set; }

        [JsonProperty("created_at")]
        public string created_at { get; set; }

        [JsonProperty("entity_id")]
        public int entity_id { get; set; }

        [JsonProperty("entity_name")]
        public string entity_name { get; set; }

        [JsonProperty("is_customer_notified")]
        public int is_customer_notified { get; set; }

        [JsonProperty("is_visible_on_front")]
        public int is_visible_on_front { get; set; }

        [JsonProperty("parent_id")]
        public int parent_id { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }
    }
}
