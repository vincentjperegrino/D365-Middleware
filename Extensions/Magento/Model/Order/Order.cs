using KTI.Moo.Extensions.Core.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Order : OrderBase
    {

        [JsonIgnore]
        public override int companyid { get; set; }

        [JsonIgnore]
        public override string kti_sourceid { get => order_id.ToString(); }

        [JsonProperty("items")]
        public List<OrderItem> order_items { get; set; }


        [StringLength(300)]
        public override string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.name))
                {
                    return order_id.ToString();
                }

                return base.name;
            }
            set => base.name = value;
        }

        [StringLength(80)]
        public override string billto_city
        {
            get => billing_address.address_city;
            set => billing_address.address_city = value;
        }

        [StringLength(150)]
        public override string billto_contactname
        {
            get => billing_address.address_primarycontactname;
            set => billing_address.address_primarycontactname = value;
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
        public override string billto_stateorprovince { get => billing_address.region_id; }

        [StringLength(50)]
        public override string billto_telephone
        {
            get => billing_address.telephone;
            set => billing_address.telephone = value;
        }

        [StringLength(2000)]
        [JsonProperty("shipping_description")]
        public override string description { get; set; }


        [JsonProperty("base_discount_amount")]
        public decimal base_discount_amount { get; set; }

        //[JsonProperty("discount_amount")]
        //public decimal discount_amount { get; set; }

        [Range(0, 1000000000000)]
        [JsonProperty("discount_amount")]
        //[JsonProperty("base_discount_amount")]
        public override decimal discountamount { get; set; }

        [JsonProperty("base_shipping_amount")]
        public decimal base_shipping_amount { get; set; }

        //[JsonProperty("shipping_amount")]
        //public decimal shipping_amount { get; set; }

        [JsonProperty("shipping_amount")]
        // [JsonProperty("base_shipping_amount")]
        public override decimal freightamount { get; set; }

        [StringLength(80)]
        public override string shipto_city
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city;
                }
                return null;

            }

        }

        [StringLength(150)]

        public override string shipto_contactname
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    var adress = extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address;
                    return adress.address_primarycontactname;
                }
                return null;


            }



        }

        [StringLength(80)]

        public override string shipto_country
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                      .address_country;
                }
                return null;

            }


        }

        [StringLength(250)]

        public override string shipto_line1
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                        .address_line1;
                }
                return null;

            }


        }


        [StringLength(250)]

        public override string shipto_line2
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                    .address_line2;
                }
                return null;

            }


        }


        [StringLength(250)]

        public override string shipto_line3
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                    .address_line3;
                }
                return null;
            }


        }


        [StringLength(200)]

        public override string shipto_name
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                   .address_type;
                }
                return null;

            }

        }


        [StringLength(20)]
        public override string shipto_postalcode
        {
            get
            {

                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                    .address_postalcode;
                }
                return null;

            }

        }

        [StringLength(50)]
        public override string shipto_telephone
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                    .telephone;
                }
                return null;

            }


        }

        [StringLength(50)]
        public override string shipto_stateorprovince
        {
            get
            {
                if (extension_attributes.shipping_assignments.Count == 1)
                {
                    if (extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
                    {
                        return null;
                    }

                    if (string.IsNullOrWhiteSpace(extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address.address_city))
                    {
                        return null;
                    }

                    return extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address
                    .region_id;
                }

                return null;

            }


        }
        [JsonProperty("status")]
        public override string order_status { get; set; }

        [JsonProperty("adjustment_negative")]
        public decimal adjustment_negative { get; set; }

        [JsonProperty("adjustment_positive")]
        public decimal adjustment_positive { get; set; }

        [JsonProperty("applied_rule_ids")]
        public string applied_rule_ids { get; set; }

        [JsonProperty("base_adjustment_negative")]
        public decimal base_adjustment_negative { get; set; }

        [JsonProperty("base_adjustment_positive")]
        public decimal base_adjustment_positive { get; set; }

        [JsonProperty("base_currency_code")]
        public string base_currency_code { get; set; }


        [JsonProperty("base_discount_canceled")]
        public decimal base_discount_canceled { get; set; }

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

        [JsonProperty("base_grand_total")]
        public decimal base_grand_total { get; set; }


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

        [JsonProperty("base_subtotal")]
        public decimal base_subtotal { get; set; }

        [JsonProperty("base_subtotal_canceled")]
        public decimal base_subtotal_canceled { get; set; }

        [JsonProperty("base_subtotal_incl_tax")]
        public decimal base_subtotal_including_tax { get; set; }

        [JsonProperty("base_subtotal_invoiced")]
        public decimal base_subtotal_invoiced { get; set; }

        [JsonProperty("base_subtotal_refunded")]
        public decimal base_subtotal_refunded { get; set; }

        [JsonProperty("base_tax_amount")]
        public decimal base_tax_amount { get; set; }

        [JsonProperty("base_tax_canceled")]
        public decimal base_tax_canceled { get; set; }

        [JsonProperty("base_tax_invoiced")]
        public decimal base_tax_invoiced { get; set; }

        [JsonProperty("base_tax_refunded")]
        public decimal base_tax_refunded { get; set; }

        [JsonProperty("base_to_global_rate")]
        public decimal base_to_global_rate { get; set; }

        [JsonProperty("base_to_order_rate")]
        public decimal base_to_order_rate { get; set; }

        [JsonProperty("base_total_canceled")]
        public decimal base_total_canceled { get; set; }

        [JsonProperty("base_total_due")]
        public decimal base_total_due { get; set; }

        [JsonProperty("base_total_invoiced")]
        public decimal base_total_invoiced { get; set; }

        [JsonProperty("base_total_invoiced_cost")]
        public decimal base_total_invoiced_cost { get; set; }

        [JsonProperty("base_total_offline_refunded")]
        public decimal base_total_offline_refunded { get; set; }

        [JsonProperty("base_total_online_refunded")]
        public decimal base_total_online_refunded { get; set; }

        [JsonProperty("base_total_paid")]
        public decimal base_total_paid { get; set; }

        [JsonProperty("base_total_qty_ordered")]
        public decimal base_total_quantity_ordered { get; set; }

        [JsonProperty("base_total_refunded")]
        public decimal base_total_refunded { get; set; }

        [JsonProperty("billing_address")]
        public OrderAddress billing_address { get; set; }

        [JsonProperty("billing_address_id")]
        public int billing_address_id { get; set; }

        [JsonProperty("can_ship_partially")]
        public int can_ship_partially { get; set; }

        [JsonProperty("can_ship_partially_item")]
        public int can_ship_partially_item { get; set; }

        [JsonProperty("coupon_code")]
        public override string kti_couponcode { get; set; }

        [JsonProperty("customer_dob")]
        public string customer_birthDate { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }

        [JsonProperty("customer_email")]
        [StringLength(100)]
        public override string emailaddress
        {
            get
            {
                if (CustomerDetails.EmailAddress.Count > 0)
                {
                    return CustomerDetails.EmailAddress.Where(item => item.primary == true).Select(item => item.emailaddress).FirstOrDefault().ToString();
                }

                return null;
            }

            set => CustomerDetails.EmailAddress.Add(new Model.EmailAddress()
            {
                primary = true,
                emailaddress = value
            });
        }

        [JsonIgnore]
        public List<EmailAddress> EmailAddress
        {
            get => CustomerDetails.EmailAddress;
            set => CustomerDetails.EmailAddress = value;
        }

        [JsonProperty("customer_firstname")]
        public string customer_firstname
        {
            get => CustomerDetails.firstname;
            set => CustomerDetails.firstname = value;
        }

        [JsonProperty("customer_gender")]
        public int customer_gender
        {
            get => CustomerDetails.gender;
            set => CustomerDetails.gender = value;
        }

        [JsonProperty("customer_group_id")]
        public int customer_group_id
        {
            get => CustomerDetails.group_id;
            set => CustomerDetails.group_id = value;
        }


        [JsonIgnore]
        public override string customerid
        {
            get => CustomerDetails.customer_id.ToString();
            set
            {
                int valueToInt = 0;
                int.TryParse(value, out valueToInt);
                CustomerDetails.customer_id = valueToInt;
            }
        }

        [JsonProperty("customer_id")]
        public int customer_id
        {

            get => CustomerDetails.customer_id;
            set => CustomerDetails.customer_id = value;

        }


        [JsonProperty("customer_is_guest")]
        public int customer_is_guest { get; set; }

        [JsonProperty("customer_lastname")]
        public string customer_lastname
        {
            get => CustomerDetails.lastname;
            set => CustomerDetails.lastname = value;
        }

        [JsonProperty("customer_middlename")]
        public string customer_middlename
        {
            get => CustomerDetails.middlename;
            set => CustomerDetails.middlename = value;
        }

        [JsonProperty("customer_note")]
        public string customer_note { get; set; }

        [JsonProperty("customer_note_notify")]
        public int customer_note_notify { get; set; }

        [JsonProperty("customer_prefix")]
        public string customer_prefix
        {
            get => CustomerDetails.prefix;
            set => CustomerDetails.prefix = value;
        }

        [JsonProperty("customer_suffix")]
        public string customer_suffix
        {
            get => CustomerDetails.suffix;
            set => CustomerDetails.suffix = value;
        }

        [JsonProperty("customer_taxvat")]
        public string customer_taxvat
        {
            get => CustomerDetails.taxvat;
            set => CustomerDetails.taxvat = value;
        }

        [JsonProperty("discount_canceled")]
        public decimal discount_canceled { get; set; }

        [JsonProperty("discount_description")]
        public string discount_description { get; set; }

        [JsonProperty("discount_invoiced")]
        public decimal discount_invoiced { get; set; }

        [JsonProperty("discount_refunded")]
        public decimal discount_refunded { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public decimal discount_tax_compensation_amount { get; set; }

        [JsonProperty("discount_tax_compensation_invoiced")]
        public decimal discount_tax_compensation_invoiced { get; set; }

        [JsonProperty("discount_tax_compensation_refunded")]
        public decimal discount_tax_compensation_refunded { get; set; }

        [JsonProperty("edit_increment")]
        public int edit_increment { get; set; }

        [JsonProperty("email_sent")]
        public int email_sent { get; set; }

        [JsonProperty("entity_id")]
        public int order_id { get; set; }

        [JsonProperty("ext_customer_id")]
        public string ext_customer_id { get; set; }

        [JsonProperty("ext_order_id")]
        public string ext_order_id { get; set; }


        [JsonProperty("extension_attributes")]
        public OrderExtensionAttributes extension_attributes { get; set; }

        [JsonProperty("forced_shipment_with_invoice")]
        public int forced_shipment_with_invoice { get; set; }

        [JsonProperty("global_currency_code")]
        public string global_currency_code { get; set; }

        [JsonProperty("grand_total")]
        public decimal grand_total { get; set; }

        [JsonProperty("hold_before_state")]
        public string hold_before_state { get; set; }

        [JsonProperty("hold_before_status")]
        public string hold_before_status { get; set; }

        [JsonProperty("increment_id")]
        public string increment_id { get; set; }

        [JsonProperty("is_virtual")]
        public int is_virtual { get; set; }


        [JsonProperty("order_currency_code")]
        public string order_currency_code { get; set; }

        [JsonProperty("original_increment_id")]
        public string original_increment_id { get; set; }

        [JsonProperty("payment")]
        public OrderPayment order_payment { get; set; }

        [JsonProperty("payment_auth_expiration")]
        public int payment_authorization_expiration { get; set; }

        [JsonProperty("payment_authorization_amount")]
        public decimal payment_authorization_amount { get; set; }

        [JsonProperty("protect_code")]
        public string protect_code { get; set; }

        [JsonProperty("quote_address_id")]
        public int quote_address_id { get; set; }

        [JsonProperty("quote_id")]
        public int quote_id { get; set; }

        [JsonProperty("relation_child_id")]
        public string relation_child_id { get; set; }

        [JsonProperty("relation_child_real_id")]
        public string relation_child_real_id { get; set; }

        [JsonProperty("relation_parent_id")]
        public string relation_parent_id { get; set; }

        [JsonProperty("relation_parent_real_id")]
        public string relation_parent_real_id { get; set; }

        [JsonProperty("remote_ip")]
        public string remote_ip { get; set; }


        [JsonProperty("shipping_canceled")]
        public decimal shipping_canceled { get; set; }

        //[JsonProperty("shipping_description")]
        //public string shipping_description { get; set; }


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

        [JsonProperty("state")]
        public string state { get; set; }

        //[JsonProperty("status")]
        //public string status { get; set; }

        [JsonProperty("status_histories")]
        public List<OrderStatusHistory> status_histories { get; set; }

        [JsonProperty("store_currency_code")]
        public string store_currency_code { get; set; }

        [JsonProperty("store_id")]
        public int store_id { get; set; }

        [JsonProperty("store_name")]
        public string store_name { get; set; }

        [JsonProperty("store_to_base_rate")]
        public decimal store_to_base_rate { get; set; }

        [JsonProperty("store_to_order_rate")]
        public decimal store_to_order_rate { get; set; }

        [JsonProperty("subtotal")]
        public decimal subtotal { get; set; }

        [JsonProperty("subtotal_canceled")]
        public decimal subtotal_canceled { get; set; }

        [JsonProperty("subtotal_incl_tax")]
        public decimal subtotal_including_tax { get; set; }

        [JsonProperty("subtotal_invoiced")]
        public decimal subtotal_invoiced { get; set; }

        [JsonProperty("subtotal_refunded")]
        public decimal subtotal_refunded { get; set; }

        [JsonProperty("tax_amount")]
        public decimal tax_amount { get; set; }

        [JsonProperty("tax_canceled")]
        public decimal tax_canceled { get; set; }

        [JsonProperty("tax_invoiced")]
        public decimal tax_invoiced { get; set; }

        [JsonProperty("tax_refunded")]
        public decimal tax_refunded { get; set; }

        [JsonProperty("total_canceled")]
        public decimal total_canceled { get; set; }

        [JsonProperty("total_due")]
        public decimal total_due { get; set; }

        [JsonProperty("total_invoiced")]
        public decimal total_invoiced { get; set; }

        [JsonProperty("total_item_count")]
        public decimal total_item_count { get; set; }

        [JsonProperty("total_offline_refunded")]
        public decimal total_offline_refunded { get; set; }

        [JsonProperty("total_online_refunded")]
        public decimal total_online_refunded { get; set; }

        [JsonProperty("total_paid")]
        public decimal total_paid { get; set; }

        [JsonProperty("total_qty_ordered")]
        public decimal total_quantity_ordered { get; set; }

        [JsonProperty("total_refunded")]
        public decimal total_refunded { get; set; }

        [JsonProperty("updated_at")]
        public DateTime updated_at { get; set; }

        [JsonProperty("weight")]
        public decimal weight { get; set; }

        [JsonProperty("x_forwarded_for")]
        public string x_forwarded_for { get; set; }

        public virtual Customer CustomerDetails { get; set; }

        public Order()
        {
            CustomerDetails = new();
            billing_address = new();
            extension_attributes = new();
        }

    }
}
