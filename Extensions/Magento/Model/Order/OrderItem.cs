using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class OrderItem : OrderItemBase
    {


        [JsonProperty("price")]
        [Range(-922337203685477, 922337203685477)]
        public override decimal priceperunit { get; set; }


        //[JsonProperty("price")]
        //[Range(-922337203685477, 922337203685477)]
        //public override decimal baseamount { get; set; }

        [JsonProperty("description")]
        [StringLength(2000)]
        public override string description { get; set; }

        [JsonProperty("item_id")]
        [Range(0, 1000000000)]
        public int item_id { get; set; }

        //[JsonIgnore]
        //public override string kti_sourceid
        //{
        //    get => item_id.ToString();
        //}

        [JsonProperty("discount_amount")]
        [Range(0, 1000000000000)]
        public override decimal manualdiscountamount { get; set; }

        [JsonProperty("product_id")]
        public override string productid { get; set; }

        [JsonProperty("name")]
        [StringLength(500)]
        public override string productname { get; set; }

        [StringLength(500)]
        public override string productdescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.productdescription))
                {
                    return productname;
                }

                return base.productdescription;
            }
            set => base.productdescription = value;
        }

        [JsonProperty("qty_ordered")]
        [Required]
        [Range(-100000000000, 100000000000)]
        public override decimal quantity { get; set; }

        [JsonProperty("qty_canceled")]
        [Range(0, 100000000000)]
        public override decimal quantitycancelled { get; set; }

        [JsonProperty("qty_shipped")]
        [Range(-100000000000, 100000000000)]
        public override decimal quantityshipped { get; set; }

        [JsonProperty("order_id")]
        [Required]
        public override string salesorderid { get; set; }

        [JsonProperty("tax_amount")]
        [Range(-1000000000000, 1000000000000)]
        public override decimal tax { get; set; }

        [JsonProperty("additional_data")]
        public string additional_data { get; set; }

        [JsonProperty("amount_refunded")]
        public decimal amount_refunded { get; set; }

        [JsonProperty("applied_rule_ids")]
        public string applied_rule_ids { get; set; }

        [JsonProperty("base_amount_refunded")]
        public decimal base_amount_refunded { get; set; }

        [JsonProperty("base_cost")]
        public decimal base_cost { get; set; }

        [JsonProperty("base_discount_amount")]
        public decimal base_discount_amount { get; set; }

        [JsonProperty("base_discount_invoiced")]
        public decimal base_discount_invoiced { get; set; }

        [JsonProperty("base_discount_refunded")]
        public decimal base_discount_refunded { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public decimal base_discount_tax_compensation_amount { get; set; }

        [JsonProperty("base_discount_tax_compensation_invoiced")]
        public decimal base_discount_tax_compensation_invoiced { get; set; }

        [JsonProperty("base_discount_tax_compensation_refunded")]
        public decimal base_discount_tax_compensation_refunded { get; set; }

        [JsonProperty("base_original_price")]
        public decimal base_original_price { get; set; }

        [JsonProperty("base_price")]
        public decimal base_price { get; set; }

        [JsonProperty("base_price_incl_tax")]
        public decimal base_price_including_tax { get; set; }

        [JsonProperty("base_row_invoiced")]
        public decimal base_row_invoiced { get; set; }

        [JsonProperty("base_row_total")]
        public decimal base_row_total { get; set; }

        [JsonProperty("base_row_total_incl_tax")]
        public decimal base_row_total_including_tax { get; set; }

        [JsonProperty("base_tax_amount")]
        public decimal base_tax_amount { get; set; }

        [JsonProperty("base_tax_before_discount")]
        public decimal base_tax_before_discount { get; set; }

        [JsonProperty("base_tax_invoiced")]
        public decimal base_tax_invoiced { get; set; }

        [JsonProperty("base_tax_refunded")]
        public decimal base_tax_refunded { get; set; }

        [JsonProperty("base_weee_tax_applied_amount")]
        public decimal base_weee_tax_applied_amount { get; set; }

        [JsonProperty("base_weee_tax_applied_row_amnt")]
        public decimal base_weee_tax_applied_row_amnt { get; set; }

        [JsonProperty("base_weee_tax_disposition")]
        public decimal base_weee_tax_disposition { get; set; }

        [JsonProperty("base_weee_tax_row_disposition")]
        public decimal base_weee_tax_row_disposition { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }



        //[JsonProperty("discount_amount")]
        //public decimal discount_amount { get; set; }

        [JsonProperty("discount_invoiced")]
        public decimal discount_invoiced { get; set; }

        [JsonProperty("discount_percent")]
        public decimal discount_percent { get; set; }

        [JsonProperty("discount_refunded")]
        public decimal discount_refunded { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public decimal discount_tax_compensation_amount { get; set; }

        [JsonProperty("discount_tax_compensation_canceled")]
        public decimal discount_tax_compensation_canceled { get; set; }

        [JsonProperty("discount_tax_compensation_invoiced")]
        public decimal discount_tax_compensation_invoiced { get; set; }

        [JsonProperty("discount_tax_compensation_refunded")]
        public decimal discount_tax_compensation_refunded { get; set; }

        [JsonProperty("event_id")]
        public int event_id { get; set; }

        [JsonProperty("ext_order_item_id")]
        public string ext_order_item_id { get; set; }

        [JsonProperty("free_shipping")]
        public int free_shipping { get; set; }

        [JsonProperty("gw_base_price")]
        public decimal gw_base_price { get; set; }

        [JsonProperty("gw_base_price_invoiced")]
        public decimal gw_base_price_invoiced { get; set; }

        [JsonProperty("gw_base_price_refunded")]
        public decimal gw_base_price_refunded { get; set; }

        [JsonProperty("gw_base_tax_amount")]
        public decimal gw_base_tax_amount { get; set; }

        [JsonProperty("gw_base_tax_amount_invoiced")]
        public decimal gw_base_tax_amount_invoiced { get; set; }

        [JsonProperty("gw_base_tax_amount_refunded")]
        public decimal gw_base_tax_amount_refunded { get; set; }

        [JsonProperty("gw_id")]
        public decimal gw_id { get; set; }

        [JsonProperty("gw_price")]
        public decimal gw_price { get; set; }

        [JsonProperty("gw_price_invoiced")]
        public decimal gw_price_invoiced { get; set; }

        [JsonProperty("gw_price_refunded")]
        public decimal gw_price_refunded { get; set; }

        [JsonProperty("gw_tax_amount")]
        public decimal gw_tax_amount { get; set; }

        [JsonProperty("gw_tax_amount_invoiced")]
        public decimal gw_tax_amount_invoiced { get; set; }

        [JsonProperty("gw_tax_amount_refunded")]
        public decimal gw_tax_amount_refunded { get; set; }

        [JsonProperty("is_qty_decimal")]
        public int is_quantity_decimal { get; set; }

        [JsonProperty("is_virtual")]
        public int is_virtual { get; set; }

        //[JsonProperty("item_id")]
        //public int item_id { get; set; }

        [JsonProperty("locked_do_invoice")]
        public int locked_do_invoice { get; set; }

        [JsonProperty("locked_do_ship")]
        public int locked_do_ship { get; set; }

        //[JsonProperty("name")]
        //public string name { get; set; }

        [JsonProperty("no_discount")]
        public int no_discount { get; set; }

        //[JsonProperty("order_id")]
        //public int order_id { get; set; }

        [JsonProperty("original_price")]
        public decimal original_price { get; set; }

        [JsonProperty("parent_item")]
        public OrderItem parent_item { get; set; }

        [JsonProperty("parent_item_id")]
        public int parent_item_id { get; set; }

        //[JsonProperty("price")]
        //public decimal price { get; set; }

        [JsonProperty("price_incl_tax")]
        public decimal price_including_tax { get; set; }

        //[JsonProperty("product_id")]
        //public int product_id { get; set; }

        [JsonProperty("product_option")]
        public OrderProductOption order_product_option { get; set; }

        [JsonProperty("product_type")]
        public string product_type { get; set; }

        [JsonProperty("qty_backordered")]
        public decimal quantity_backordered { get; set; }

        //[JsonProperty("qty_canceled")]
        //public decimal quantity_canceled { get; set; }

        [JsonProperty("qty_invoiced")]
        public decimal quantity_invoiced { get; set; }

        //[JsonProperty("qty_ordered")]
        //public decimal quantity_ordered { get; set; }

        [JsonProperty("qty_refunded")]
        public decimal quantity_refunded { get; set; }

        [JsonProperty("qty_returned")]
        public decimal quantity_returned { get; set; }

        //[JsonProperty("qty_shipped")]
        //public decimal quantity_shipped { get; set; }

        [JsonProperty("quote_item_id")]
        public int quote_item_id { get; set; }

        [JsonProperty("row_invoiced")]
        public decimal row_invoiced { get; set; }

        [JsonProperty("row_total")]
        public decimal row_total { get; set; }

        [JsonProperty("row_total_incl_tax")]
        public decimal row_total_including_tax { get; set; }

        [JsonProperty("row_weight")]
        public decimal row_weight { get; set; }

        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("store_id")]
        public int store_id { get; set; }

        //[JsonProperty("tax_amount")]
        //public decimal tax_amount { get; set; }

        [JsonProperty("tax_before_discount")]
        public decimal tax_before_discount { get; set; }


        [JsonProperty("tax_canceled")]
        public decimal tax_canceled { get; set; }

        [JsonProperty("tax_invoiced")]
        public decimal tax_invoiced { get; set; }

        [JsonProperty("tax_percent")]
        public decimal tax_percent { get; set; }

        [JsonProperty("tax_refunded")]
        public decimal tax_refunded { get; set; }

        [JsonProperty("updated_at")]
        public DateTime updated_at { get; set; }

        [JsonProperty("weee_tax_applied")]
        public string weee_tax_applied { get; set; }

        [JsonProperty("weee_tax_applied_amount")]
        public decimal weee_tax_applied_amount { get; set; }

        [JsonProperty("weee_tax_applied_row_amount")]
        public decimal weee_tax_applied_row_amount { get; set; }

        [JsonProperty("weee_tax_disposition")]
        public decimal weee_tax_disposition { get; set; }

        [JsonProperty("weee_tax_row_disposition")]
        public decimal weee_tax_row_disposition { get; set; }

        [JsonProperty("weight")]
        public decimal weight { get; set; }


    }












}
