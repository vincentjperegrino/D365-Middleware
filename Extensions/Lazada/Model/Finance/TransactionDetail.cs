using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class TransactionDetail
    {

        [JsonProperty("order_no")]
        public long order_no { get; set; }

        [JsonProperty("transaction_date")]
        public DateTime? transaction_date { get; set; }

        [JsonProperty("amount")]
        public decimal amount { get; set; }

        [JsonProperty("paid_status")]
        public string paid_status { get; set; }

        [JsonProperty("shipping_provider")]
        public string shipping_provider { get; set; }

        [JsonProperty("WHT_included_in_amount")]
        public string WHT_included_in_amount { get; set; }

        [JsonProperty("lazada_sku")]
        public string lazada_sku { get; set; }

        [JsonProperty("transaction_type")]
        public string transaction_type { get; set; }


        [JsonProperty("orderItem_no")]
        public long orderItem_no { get; set; }

        [JsonProperty("orderItem_status")]
        public string orderItem_status { get; set; }

        public long reference { get; set; }

        //Transaction type ID.
        public int fee_type { get; set; }

        [JsonProperty("fee_name")]
        public string fee_name { get; set; }

        [JsonProperty("shipping_speed")]
        public string shipping_speed { get; set; }

        [JsonProperty("WHT_amount")]
        public decimal WHT_amount { get; set; }

        [JsonProperty("transaction_number")]
        public string transaction_number { get; set; }

        [JsonProperty("seller_sku")]
        public string seller_sku { get; set; }

        [JsonProperty("statement")]
        public string statement { get; set; }

        [JsonProperty("details")]
        public string details { get; set; }

        [JsonProperty("VAT_in_amount")]
        public decimal VAT_in_amount { get; set; }

        [JsonProperty("shipment_type")]
        public string shipment_type { get; set; }

        [JsonProperty("comment")]
        public string comment { get; set; }

        [JsonProperty("payment_ref_id")]
        public string payment_ref_id { get; set; }

    }
}
