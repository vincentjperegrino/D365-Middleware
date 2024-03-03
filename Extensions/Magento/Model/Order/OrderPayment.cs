
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class OrderPayment
    {
        [JsonProperty("account_status")]
        public object account_status { get; set; }

        [JsonProperty("additional_information")]
        public string[] additional_information { get; set; }

        [JsonProperty("amount_ordered")]
        public decimal amount_ordered { get; set; }

        [JsonProperty("base_amount_ordered")]
        public decimal base_amount_ordered { get; set; }

        [JsonProperty("base_shipping_amount")]
        public decimal base_shipping_amount { get; set; }

        [JsonProperty("cc_exp_year")]
        public string cc_exp_year { get; set; }

        [JsonProperty("cc_last4")]
        public object cc_last4 { get; set; }

        [JsonProperty("cc_ss_start_month")]
        public string cc_ss_start_month { get; set; }

        [JsonProperty("cc_ss_start_year")]
        public string cc_ss_start_year { get; set; }

        [JsonProperty("entity_id")]
        public int entity_id { get; set; }

        [JsonProperty("method")]
        public string method { get; set; }

        [JsonProperty("parent_id")]
        public int parent_id { get; set; }

        [JsonProperty("shipping_amount")]
        public decimal shipping_amount { get; set; }

    }
}

