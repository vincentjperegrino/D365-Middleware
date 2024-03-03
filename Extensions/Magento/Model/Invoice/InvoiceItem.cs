using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class InvoiceItem : InvoiceItemBase
    {

        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("tax_amount")]
        public override decimal tax { get; set; }


        [JsonProperty("price")]
        public override decimal priceperunit { get; set; }


        //[JsonProperty("price")]
        //public override decimal baseamount { get; set; }

        [JsonIgnore]
        public override string kti_sourceid
        {
            get => order_item_id.ToString();
        }

        [JsonProperty("order_item_id")]
        public int order_item_id { get; set; }

        [JsonIgnore]
        public override string productid
        {
            get => product_id.ToString();
        }

        [JsonProperty("product_id")]
        public int product_id { get; set; }

        [JsonProperty("qty")]
        public override decimal quantity { get; set; }

        [JsonProperty("name")]
        [StringLength(500)]
        public override string productname { get; set; }

        [JsonIgnore]
        public override string invoicedetailid
        {
            get => invoiceItemID.ToString();
        }

        [JsonProperty("entity_id")]
        public int invoiceItemID { get; set; }

        [JsonIgnore]
        public override string invoiceid
        {
            get => invoice_id.ToString();
        }

        [JsonProperty("parent_id")]
        public int invoice_id { get; set; }


        [JsonProperty("base_discount_tax_compensation_amount")]
        public decimal base_discount_tax_compensation_amount { get; set; }

        [JsonProperty("base_price")]
        public decimal base_price { get; set; }

        [JsonProperty("base_price_incl_tax")]
        public decimal base_price_including_tax { get; set; }

        [JsonProperty("base_row_total")]
        public decimal base_row_total { get; set; }

        [JsonProperty("base_row_total_incl_tax")]
        public decimal base_row_total_including_tax { get; set; }

        [JsonProperty("base_tax_amount")]
        public decimal base_tax_amount { get; set; }


        [JsonProperty("discount_tax_compensation_amount")]
        public decimal discount_tax_compensation_amount { get; set; }


        [JsonProperty("price_incl_tax")]
        public decimal price_including_tax { get; set; }

        [JsonProperty("row_total")]
        public decimal row_total { get; set; }

        [JsonProperty("row_total_incl_tax")]
        public decimal row_total_including_tax { get; set; }



    }
}
