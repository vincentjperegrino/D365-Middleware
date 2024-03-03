using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ShippingTotal
    {
        [JsonProperty("base_shipping_amount")]
        public decimal base_shipping_amount { get; set; }

        [JsonProperty("base_shipping_canceled")]
        public decimal base_shipping_canceled { get; set; }

        [JsonProperty("base_shipping_discount_amount")]
        public decimal base_shipping_discount_amount { get; set; }

        [JsonProperty("base_shipping_discount_tax_compensation_amnt")]
        public decimal base_shipping_discount_tax_compensation_amount { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public decimal base_shipping_including_tax { get; set; }

        [JsonProperty("base_shipping_invoiced")]
        public decimal base_shipping_invoiced { get; set; }

        [JsonProperty("base_shipping_refunded")]
        public decimal base_shipping_refunded { get; set; }
        
        [JsonProperty("base_shipping_tax_amount")]
        public decimal base_shipping_tax_amount { get; set; }

        [JsonProperty("base_shipping_tax_refunded")]
        public decimal base_shipping_tax_refunded { get; set; }
        
        [JsonProperty("shipping_amount")]
        public decimal shipping_amount { get; set; }

        [JsonProperty("shipping_canceled")]
        public decimal shipping_canceled { get; set; }
        
        [JsonProperty("shipping_discount_amount")]
        public decimal shipping_discount_amount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public decimal shipping_discount_tax_compensation_amount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public decimal shipping_including_tax { get; set; }

        [JsonProperty("shipping_invoiced")]
        public decimal shipping_invoiced { get; set; }

        [JsonProperty("shipping_refunded")]
        public decimal shipping_refunded { get; set; }
        
        [JsonProperty("shipping_tax_amount")]
        public decimal shipping_tax_amount { get; set; }

        [JsonProperty("shipping_tax_refunded")]
        public decimal shipping_tax_refunded { get; set; }
        


    }
}
