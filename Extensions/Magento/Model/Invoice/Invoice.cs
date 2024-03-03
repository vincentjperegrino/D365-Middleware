using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Invoice : InvoiceBase
    {

        [JsonIgnore]
        public override int companyid { get; set; }

        [JsonProperty("order_id")]
        public int order_id { get; set; }

 
        public override string customerid
        {
            get => CustomerDetails.customer_id.ToString();
            set => CustomerDetails.customer_id = Convert.ToInt32(value);
        }


        public override string salesorderid { get => order_id.ToString(); }


        [JsonProperty("entity_id")]
        public int invoice_id { get; set; }

        [JsonIgnore]
        public override string kti_sourceid
        {
            get => this.invoice_id.ToString();
        }

        [JsonIgnore]
        public override string invoicenumber
        {
            get => this.invoice_id.ToString();
        }

        [JsonProperty("shipping_amount")]
        public override decimal freightamount { get; set; }


        [JsonProperty("grand_total")]
        public override decimal totalamount { get; set; }


        [JsonProperty("tax_amount")]
        public override decimal totaltax { get; set; }


        [JsonProperty("discount_amount")]
        public override decimal discountamount { get; set; }

        [JsonIgnore]
        public List<EmailAddress> EmailAddress
        {
            get => CustomerDetails.EmailAddress;
            set => CustomerDetails.EmailAddress = value;
        }

        [JsonProperty("shipping_address_id")]
        public int shipping_address_id { get; set; }

        public InvoiceAddress billing_address { get; set; }

        [JsonProperty("billing_address_id")]
        public int billing_address_id { get; set; }

        [StringLength(80)]
        public override string billto_city
        {
            get => billing_address.address_city;
            set => billing_address.address_city = value;
        }

        [StringLength(80)]
        public override string billto_country
        {
            get => billing_address.address_country;
            set => billing_address.address_country = value;
        }

        [StringLength(50)]
        public override string billto_fax
        {
            get => billing_address.address_fax;
            set => billing_address.address_fax = value;
        }

        [StringLength(250)]
        public override string billto_line1
        {
            get => billing_address.address_line1;
            set => billing_address.address_line1 = value;
        }

        [StringLength(250)]
        public override string billto_line2
        {
            get => billing_address.address_line2;
            set => billing_address.address_line2 = value;
        }

        [StringLength(250)]
        public override string billto_line3
        {
            get => billing_address.address_line3;
            set => billing_address.address_line3 = value;
        }

        [StringLength(200)]
        public override string billto_name
        {
            get => billing_address.address_type;
            set => billing_address.address_type = value;
        }

        [StringLength(20)]
        public override string billto_postalcode
        {
            get => billing_address.address_postalcode;
            set => billing_address.address_postalcode = value;
        }


        [StringLength(50)]
        public override string billto_telephone
        {
            get => billing_address.telephone;
            set => billing_address.telephone = value;
        }

        public Customer CustomerDetails { get; set; }

        [StringLength(100)]
        public override string emailaddress
        {
            get
            {
                if (CustomerDetails.EmailAddress.Any(email => !string.IsNullOrWhiteSpace(email.emailaddress)))
                {
                    return CustomerDetails.EmailAddress.Where(email => !string.IsNullOrWhiteSpace(email.emailaddress)).FirstOrDefault().emailaddress;
                }

                return null;
            }

            set => CustomerDetails.EmailAddress.Add(new Model.EmailAddress()
            {
                primary = true,
                emailaddress = value
            });
        }

        [JsonProperty("total_qty")]
        public decimal total_quantity { get; set; }

        [JsonProperty("base_currency_code")]
        public string base_currency_code { get; set; }

        [JsonProperty("base_discount_amount")]
        public decimal base_discount_amount { get; set; }

        [JsonProperty("base_grand_total")]
        public decimal base_grand_total { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public decimal base_discount_tax_compensation_amount { get; set; }

        [JsonProperty("base_shipping_amount")]
        public decimal base_shipping_amount { get; set; }

        [JsonProperty("base_shipping_discount_tax_compensation_amnt")]
        public decimal base_shipping_discount_tax_compensation_amount { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public decimal base_shipping_including_tax { get; set; }

        [JsonProperty("base_shipping_tax_amount")]
        public decimal base_shipping_tax_amount { get; set; }

        [JsonProperty("base_subtotal")]
        public decimal base_subtotal { get; set; }

        [JsonProperty("base_subtotal_incl_tax")]
        public decimal base_subtotal_including_tax { get; set; }

        [JsonProperty("base_tax_amount")]
        public decimal base_tax_amount { get; set; }

        [JsonProperty("base_to_global_rate")]
        public decimal base_to_global_rate { get; set; }

        [JsonProperty("base_to_order_rate")]
        public decimal base_to_order_rate { get; set; }

        [JsonProperty("can_void_flag")]
        public int can_void_flag { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }

        [JsonProperty("email_sent")]
        public int email_sent { get; set; }

        [JsonProperty("global_currency_code")]
        public string global_currency_code { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public decimal discount_tax_compensation_amount { get; set; }

        [JsonProperty("increment_id")]
        public string increment_id { get; set; }

        [JsonProperty("order_currency_code")]
        public string order_currency_code { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public decimal shipping_discount_tax_compensation_amount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public decimal shipping_including_tax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public decimal shipping_tax_amount { get; set; }

        [JsonProperty("state")]
        public int state { get; set; }

        [JsonProperty("store_currency_code")]
        public string store_currency_code { get; set; }

        [JsonProperty("store_id")]
        public int store_id { get; set; }

        [JsonProperty("store_to_base_rate")]
        public decimal store_to_base_rate { get; set; }

        [JsonProperty("store_to_order_rate")]
        public decimal store_to_order_rate { get; set; }

        [JsonProperty("subtotal")]
        public decimal subtotal { get; set; }

        [JsonProperty("subtotal_incl_tax")]
        public decimal subtotal_including_tax { get; set; }

        [JsonProperty("updated_at")]
        public DateTime updated_at { get; set; }

        [JsonProperty("items")]
        public List<InvoiceItem> invoiceItems { get; set; }

        //[JsonProperty("comments")]
        //public List<object> comments { get; set; }

        public Invoice()
        {
            CustomerDetails = new();
            billing_address = new();
        }
    }
}
