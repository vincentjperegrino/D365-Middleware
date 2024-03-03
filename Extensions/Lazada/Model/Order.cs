using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using KTI.Moo.Extensions.Core.Model;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class OrderHeader : OrderBase
    {
        public OrderHeader()
        {
            this.kti_socialchannelorigin = 959080006; // "LAZADA"
        }

        public override int kti_socialchannelorigin { get; set; } = 959080006;
        /// <summary>
        /// Lazada Seller ID. To be set externally.
        /// </summary>
        public override string moosourcesystem { get; set; }

        public Customer laz_customer { get; set; }
        // public string laz_CustomerLastName { get; set; }
        // public string laz_CustomerFirstName { get; set; }
        public DateTime laz_CreatedOn { get; set; }
        public DateTime laz_UpdatedOn { get; set; }
        public int laz_ItemCount { get; set; }
        public string laz_VoucherCode { get; set; }
        public string laz_DeliveryInfo { get; set; }
        public string laz_ExtraAttributes { get; set; }
        public string laz_PaymentMethod { get; set; }
        public string laz_Remarks { get; set; }
        public decimal laz_Voucher { get; set; }
        public string laz_NationalRegistrationNumber { get; set; }
        // public DateTime? laz_PromisedShippingTimes { get; set; }
        public string laz_WarehouseCode { get; set; }
        // public decimal ShippingFee { get; set; } // freightamount
        public decimal laz_ShippingFeeOriginal { get; set; }
        public decimal laz_ShippingFeeDiscountSeller { get; set; }
        public decimal laz_ShippingFeeDiscountPlatform { get; set; }
        // public string laz_Status { get; set; }
        public string[] laz_Statuses { get; set; }

        public List<OrderItem> order_items { get; set; }



        public OrderHeader(string json)
        {
            var orderJson = JsonDocument.Parse(json).RootElement;
            var orderId = orderJson.GetProperty("order_id").ToString();

            this.kti_sourceid = orderId;
            this.kti_socialchannelorigin = 959080006;

            JsonElement billto = orderJson.GetProperty("address_billing");
            this.billto_contactname = billto.GetProperty("first_name").GetString();
            this.billto_city = billto.GetProperty("city").GetString();
            this.billto_country = billto.GetProperty("country").GetString();
            this.billto_line1 = billto.GetProperty("address1").GetString();
            this.billto_stateorprovince = billto.GetProperty("address3").GetString();
            this.billto_postalcode = billto.GetProperty("post_code").GetString();
            var billphone1 = billto.GetProperty("phone").GetString();
            var billphone2 = billto.GetProperty("phone2").GetString();
            this.billto_telephone = string.IsNullOrWhiteSpace(billphone1) ? string.IsNullOrWhiteSpace(billphone2) ? "" : billphone2 : billphone1;

            JsonElement shipto = orderJson.GetProperty("address_shipping");
            this.shipto_contactname = shipto.GetProperty("first_name").ToString();
            this.shipto_city = shipto.GetProperty("city").ToString();
            this.shipto_country = shipto.GetProperty("country").ToString();
            this.shipto_line1 = shipto.GetProperty("address1").ToString();
            this.shipto_stateorprovince = shipto.GetProperty("address3").GetString();
            this.shipto_postalcode = shipto.GetProperty("post_code").GetString();
            var shipphone1 = shipto.GetProperty("phone").GetString();
            var shipphone2 = shipto.GetProperty("phone2").GetString();
            this.shipto_telephone = string.IsNullOrWhiteSpace(shipphone1) ? string.IsNullOrWhiteSpace(shipphone2) ? "" : shipphone2 : shipphone1;

            this.totalamount = decimal.Parse(orderJson.GetProperty("price").GetString());
            this.freightamount = orderJson.GetProperty("shipping_fee").GetDecimal();
            this.totalamountlessfreight = this.totalamount - this.freightamount;

            this.kti_giftwrap = orderJson.GetProperty("gift_option").GetBoolean();
            this.kti_gifttagmessage = orderJson.GetProperty("gift_message").GetString();

            this.laz_Statuses = orderJson.GetProperty("statuses").EnumerateArray().Select(s => s.GetString()).ToArray();
            this.kti_orderstatus = Domain.Order.ConvertStatus(this.laz_Statuses);

            this.laz_customer = new()
            {
                lastname = orderJson.GetProperty("customer_last_name").GetString(),
                firstname = orderJson.GetProperty("customer_first_name").GetString(),
                mobilephone = this.billto_telephone,
                country = this.billto_country,
                address = new()
                {
                    new()
                    {
                       default_billing = true,
                       first_name = billto.GetProperty("first_name").GetString(),
                       last_name = billto.GetProperty("last_name").GetString(),
                       address_line1 = this.billto_line1,
                       address_line2 = this.billto_line2,
                       address_line3 = this.billto_line3,
                       address_city = this.billto_city,
                       address_country = this.billto_country,
                       address_stateorprovince = this.billto_stateorprovince,
                       address_postalcode = this.billto_postalcode,
                       telephone = new()
                       {
                           new()
                           {
                               primary = true,
                               telephone = billto_telephone
                           }
                       }
                    },
                    new()
                    {
                       default_shipping = true,
                       first_name = shipto.GetProperty("first_name").GetString(),
                       last_name = shipto.GetProperty("last_name").GetString(),
                       address_line1 = this.shipto_line1,
                       address_line2 = this.shipto_line2,
                       address_line3 = this.shipto_line2,
                       address_city = this.shipto_city,
                       address_country = this.shipto_country,
                       address_stateorprovince = this.shipto_stateorprovince,
                       address_postalcode = this.shipto_postalcode,
                       telephone = new()
                       {
                           new()
                           {
                               primary = true,
                               telephone = shipto_telephone
                           }
                       }

                    }
                }


            };




            this.laz_ItemCount = orderJson.GetProperty("items_count").GetInt32();
            this.laz_CreatedOn = DateTime.ParseExact(orderJson.GetProperty("created_at").GetString(), "yyyy-MM-dd HH:mm:ss zz00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            this.laz_UpdatedOn = DateTime.ParseExact(orderJson.GetProperty("updated_at").GetString(), "yyyy-MM-dd HH:mm:ss zz00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            this.laz_VoucherCode = orderJson.GetProperty("voucher_code").GetString();
            this.laz_DeliveryInfo = orderJson.GetProperty("delivery_info").GetString();
            this.laz_ExtraAttributes = orderJson.GetProperty("extra_attributes").GetString();
            this.laz_PaymentMethod = orderJson.GetProperty("payment_method").GetString();
            this.laz_Remarks = orderJson.GetProperty("remarks").GetString();
            this.laz_Voucher = orderJson.GetProperty("voucher").GetDecimal();
            this.laz_NationalRegistrationNumber = orderJson.GetProperty("national_registration_number").GetString();
            this.laz_WarehouseCode = orderJson.GetProperty("warehouse_code").GetString();
            this.laz_ShippingFeeOriginal = orderJson.GetProperty("shipping_fee_original").GetDecimal();
            this.laz_ShippingFeeDiscountSeller = orderJson.GetProperty("shipping_fee_discount_seller").GetDecimal();
            this.laz_ShippingFeeDiscountPlatform = orderJson.GetProperty("shipping_fee_discount_platform").GetDecimal();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class OrderItem : OrderItemBase
    {
        public OrderItem()
        {
            this.kti_socialchannelorigin = 959080006; // "LAZADA"
            this.ispriceoverridden = true;

        }
        // transaction details can be get 
        public List<TransactionDetail> item_transaction_details { get; set; }

        // Used for matching the Transaction orderItem_no that needs long type
        public long laz_OrderItemID { get; set; }


        // Source_id used in CRM as order_item_id
        public override string kti_sourceid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(base.kti_sourceid))
                {

                    return laz_OrderItemID.ToString();
                }

                return base.kti_sourceid;
            }
            set => base.kti_sourceid = value;
        }

        public override int kti_socialchannelorigin { get; set; } = 959080006;
        /// <summary>
        /// Lazada Seller ID. To be set externally.
        /// </summary>
        public override string moosourcesystem { get; set; }
        public override bool ispriceoverridden { get; set; } = true;


        public PickupStore laz_PickupStoreInfo { get; set; }
        public DateTime? laz_PromisedShippingTime { get; set; }
        public long? laz_PurchaseOrderNumber { get; set; }
        public string laz_Image { get; set; }
        public decimal laz_Tax { get; set; }
        public decimal laz_RetailPrice { get; set; }
        public decimal laz_ItemPrice { get; set; }
        public decimal laz_DailyDiscount { get; set; }
        public string laz_Status { get; set; }
        public string laz_CancelReturnInitiator { get; set; }
        public decimal laz_VoucherPlatform { get; set; }
        public decimal laz_VoucherSeller { get; set; }
        public string laz_OrderType { get; set; }
        public string laz_StagePayStatus { get; set; }
        public string laz_WarehouseCode { get; set; }
        public decimal laz_VoucherSellerLpi { get; set; }
        public decimal laz_VoucherPlatformLpi { get; set; }
        public long laz_BuyerId { get; set; }
        public decimal laz_ShippingFeeOriginal { get; set; }
        public decimal laz_ShippingFeeDiscountSeller { get; set; }
        public decimal laz_ShippingFeeDiscountPlatform { get; set; }
        public string laz_VoucherCodeSeller { get; set; }
        public string laz_VoucherCodePlatform { get; set; }
        public bool laz_DeliveryOptionSof { get; set; }
        public bool laz_IsFbl { get; set; }
        public bool laz_IsReroute { get; set; }
        public string laz_Reason { get; set; }
        public string laz_DigitalDeliveryInfo { get; set; }
        public decimal laz_VoucherAmount { get; set; }
        public string laz_ReturnStatus { get; set; }
        public string laz_ShippingType { get; set; }
        public string laz_ShipmentProvider { get; set; }
        public string laz_Variation { get; set; }
        public DateTime laz_CreatedAt { get; set; }
        public long? laz_Invoice { get; set; }
        public decimal laz_ShippingAmount { get; set; }
        public string laz_Currency { get; set; }
        public string laz_OrderFlag { get; set; }
        public string laz_ShopId { get; set; }
        public decimal laz_WalletCredits { get; set; }
        public DateTime laz_UpdatedAt { get; set; }
        public bool laz_IsDigital { get; set; }
        public string laz_PackageId { get; set; }
        public string laz_TrackingCode { get; set; }
        public decimal laz_ShippingServiceCost { get; set; }
        public string laz_ExtraAttributes { get; set; }
        public decimal laz_PaidPrice { get; set; }
        public string laz_ShippingProviderType { get; set; }
        public string laz_ProductUrl { get; set; }
        public string laz_ShopSku { get; set; }
        public string laz_ReasonDetail { get; set; }
        public long? laz_PurchaseOrderId { get; set; }
        public long laz_SkuId { get; set; }
        public long laz_ProductId { get; set; }

        public OrderItem(string json)
        {
            var itemJson = JsonDocument.Parse(json).RootElement;

            //this.kti_sourceid = itemJson.GetProperty("order_item_id").ToString();
            long _OrderItemID;
            long.TryParse(itemJson.GetProperty("order_item_id").ToString(), out _OrderItemID);

            this.laz_OrderItemID = _OrderItemID;

            this.kti_socialchannelorigin = 959080006;
            this.ispriceoverridden = true;

            this.productid = itemJson.GetProperty("sku").ToString();
            this.productdescription = itemJson.GetProperty("name").ToString();
            this.priceperunit = itemJson.GetProperty("item_price").GetDecimal();
            this.laz_ItemPrice = this.priceperunit;
            this.quantity = 1;
            if (itemJson.TryGetProperty("sla_time_stamp", out var sla) && !string.IsNullOrWhiteSpace(sla.GetString()))
                this.requestdeliveryby = DateTime.ParseExact(sla.GetString(), "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);


            this.laz_Status = itemJson.GetProperty("status").GetString();
            this.kti_orderstatus = Domain.Order.ConvertStatus(new string[] { this.laz_Status });

            var psi = itemJson.GetProperty("pick_up_store_info");
            this.laz_PickupStoreInfo = psi.ToString() == "{}" ? null : new(itemJson.GetProperty("pick_up_store_info").ToString());
            var poi = itemJson.GetProperty("purchase_order_id").ToString();
            this.laz_PurchaseOrderId = string.IsNullOrWhiteSpace(poi) ? null : long.Parse(poi);
            var pon = itemJson.GetProperty("purchase_order_number").ToString();
            this.laz_PurchaseOrderNumber = string.IsNullOrWhiteSpace(pon) ? null : long.Parse(pon);
            this.laz_Image = itemJson.GetProperty("product_main_image").ToString();
            this.laz_Tax = decimal.Parse(itemJson.GetProperty("tax_amount").ToString());
            this.laz_CancelReturnInitiator = itemJson.GetProperty("cancel_return_initiator").ToString();
            this.laz_VoucherPlatform = decimal.Parse(itemJson.GetProperty("voucher_platform").ToString());
            this.laz_VoucherSeller = decimal.Parse(itemJson.GetProperty("voucher_seller").ToString());
            this.laz_OrderType = itemJson.GetProperty("order_type").ToString();
            this.laz_StagePayStatus = itemJson.GetProperty("stage_pay_status").ToString();
            this.laz_WarehouseCode = itemJson.GetProperty("warehouse_code").ToString();
            this.laz_VoucherSellerLpi = decimal.Parse(itemJson.GetProperty("voucher_seller_lpi").ToString());
            this.laz_VoucherPlatformLpi = decimal.Parse(itemJson.GetProperty("voucher_platform_lpi").ToString());
            this.laz_BuyerId = long.Parse(itemJson.GetProperty("buyer_id").ToString());
            this.laz_ShippingFeeOriginal = decimal.Parse(itemJson.GetProperty("shipping_fee_original").ToString());
            this.laz_ShippingFeeDiscountSeller = decimal.Parse(itemJson.GetProperty("shipping_fee_discount_seller").ToString());
            this.laz_ShippingFeeDiscountPlatform = decimal.Parse(itemJson.GetProperty("shipping_fee_discount_platform").ToString());
            this.laz_VoucherCodeSeller = itemJson.GetProperty("voucher_code_seller").ToString();
            this.laz_VoucherCodePlatform = itemJson.GetProperty("voucher_code_platform").ToString();
            this.laz_DeliveryOptionSof = itemJson.GetProperty("delivery_option_sof").ToString().Equals("1");
            this.laz_IsFbl = itemJson.GetProperty("is_fbl").ToString().Equals("1");
            this.laz_IsReroute = itemJson.GetProperty("is_reroute").ToString().Equals("1");
            this.laz_Reason = itemJson.GetProperty("reason").ToString();
            this.laz_DigitalDeliveryInfo = itemJson.GetProperty("digital_delivery_info").ToString();
            this.laz_VoucherAmount = decimal.Parse(itemJson.GetProperty("voucher_amount").ToString());
            this.laz_ReturnStatus = itemJson.GetProperty("return_status").ToString();
            this.laz_ShippingType = itemJson.GetProperty("shipping_type").ToString();
            this.laz_ShipmentProvider = itemJson.GetProperty("shipment_provider").ToString();
            this.laz_Variation = itemJson.GetProperty("variation").ToString();
            this.laz_CreatedAt = DateTime.ParseExact(itemJson.GetProperty("created_at").ToString(), "yyyy-MM-dd HH:mm:ss zz00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            this.laz_UpdatedAt = DateTime.ParseExact(itemJson.GetProperty("updated_at").ToString(), "yyyy-MM-dd HH:mm:ss zz00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var pst = itemJson.GetProperty("promised_shipping_time").GetString();
            this.laz_PromisedShippingTime = string.IsNullOrWhiteSpace(pst) ? null : DateTime.ParseExact(pst, "yyyy-MM-dd HH:mm:ss zz00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var inv = itemJson.GetProperty("invoice_number").ToString();
            this.laz_Invoice = string.IsNullOrWhiteSpace(inv) ? null : long.Parse(inv);
            this.laz_ShippingAmount = decimal.Parse(itemJson.GetProperty("shipping_amount").ToString());
            this.laz_Currency = itemJson.GetProperty("currency").ToString();
            this.laz_OrderFlag = itemJson.GetProperty("order_flag").ToString();
            this.laz_ShopId = itemJson.GetProperty("shop_id").ToString();
            this.laz_WalletCredits = decimal.Parse(itemJson.GetProperty("wallet_credits").ToString());
            this.laz_IsDigital = itemJson.GetProperty("is_digital").ToString().Equals("1");
            this.laz_PackageId = itemJson.GetProperty("package_id").ToString();
            this.laz_TrackingCode = itemJson.GetProperty("tracking_code").ToString();
            this.laz_ShippingServiceCost = decimal.Parse(itemJson.GetProperty("shipping_service_cost").ToString());
            this.laz_ExtraAttributes = itemJson.GetProperty("extra_attributes").ToString();
            this.laz_PaidPrice = decimal.Parse(itemJson.GetProperty("paid_price").ToString());
            this.laz_ShippingProviderType = itemJson.GetProperty("shipping_provider_type").ToString();
            this.laz_ProductUrl = itemJson.GetProperty("product_detail_url").ToString();
            this.laz_ShopSku = itemJson.GetProperty("shop_sku").ToString();
            this.laz_ReasonDetail = itemJson.GetProperty("reason_detail").ToString();
            this.laz_SkuId = long.Parse(itemJson.GetProperty("sku_id").ToString());
            this.laz_ProductId = long.Parse(itemJson.GetProperty("product_id").ToString());
        }
    }

    public record OrderItems
    {
        [JsonPropertyName("order_number")]
        public long OrderNumber { get; set; }
        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }
        [JsonPropertyName("order_items")]
        public List<OrderItem> Items { get; set; }
    }

    public record PickupStore
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string StoreCode { get; set; }
        public List<string> OpenHours { get; set; }

        public PickupStore(string json)
        {
            JsonElement pickupJson = JsonDocument.Parse(json).RootElement;
            this.Name = pickupJson.GetProperty("pick_up_store_name").GetString();
            this.Address = pickupJson.GetProperty("pick_up_store_address").GetString();
            this.StoreCode = pickupJson.GetProperty("pick_up_store_code").GetString();
            this.OpenHours = pickupJson.GetProperty("pick_up_store_open_hour").EnumerateArray().Select(i => i.GetString()).ToList();
        }
    }

    //public record Address
    //{
    //    public string Address1 { get; set; }
    //    public string Address2 { get; set; }
    //    public string Address3 { get; set; }
    //    public string Address4 { get; set; }
    //    public string Address5 { get; set; }
    //    public string City { get; set; }
    //    public string Country { get; set; }
    //    [JsonPropertyName("post_code")]
    //    public string PostCode { get; set; }
    //    [JsonPropertyName("last_name")]
    //    public string LastName { get; set; }
    //    [JsonPropertyName("first_name")]
    //    public string FirstName { get; set; }
    //    public string Phone { get; set; }
    //    public string Phone2 { get; set; }
    //}

    public enum CancelReason
    {
        SourcingDelay = 10,
        SystemError = 11,
        IncompleteShippingAddress = 13,
        OutOfDeliveryArea = 14,
        OutOfStock = 15,
        CustomerUnreachable = 16,
        DuplicateOrder = 17,
        PricingError = 21
    }
}
