using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Order : KTI.Moo.Extensions.Core.Model.OrderBase

{

    public Order()
    {


    }

    public Order(ShopifySharp.Order order)
    {
        app_id = order.AppId ?? default;
        if (order.BillingAddress != null)
        {
            billing_address = new Model.Address(order.BillingAddress);
        }
        browser_ip = order.BrowserIp;
        buyer_accepts_marketing = order.BuyerAcceptsMarketing ?? default;
        cancel_reason = order.CancelReason;
        cancelled_at = order.CancelledAt ?? default;
        cart_token = order.CartToken;
        checkout_token = order.CheckoutToken;
        checkout_id = order.CheckoutId ?? default;
        if (order.ClientDetails != null)
        {
            client_details = new Model.ClientDetails(order.ClientDetails);
        
        }

        closed_at = order.ClosedAt?.DateTime ?? default;
        //company = order.Company; //cant map

        //if (order.Company != null)
        //{
        //    company = new Model.Company(order.Company);

        //}
        //confirmation_number = order.confirmation; //no confirmation_number


        created_at = order.CreatedAt?.DateTime ?? default;
        currency = order.Currency;
        if (order.CurrentTotalAdditionalFeesSet != null)
        {
            current_total_additional_fees_set = new Model.PriceSet(order.CurrentTotalAdditionalFeesSet);
        }
        
        current_total_discounts = order.CurrentTotalDiscounts ?? default;
        if (order.CurrentTotalDiscountsSet != null)
        {
            current_total_discounts_set = new Model.PriceSet(order.CurrentTotalDiscountsSet);

        }


        if (order.CurrentTotalDutiesSet != null)
        {
            current_total_duties_set = new Model.PriceSet(order.CurrentTotalDutiesSet);

        }

        current_total_price = order.CurrentTotalPrice ?? default;


        if (order.CurrentTotalPriceSet != null)
        {
            current_total_price_set = new Model.PriceSet(order.CurrentTotalPriceSet);

        }


        current_subtotal_price = order.CurrentSubtotalPrice ?? default;
 
        if (order.CurrentSubtotalPriceSet != null)
        {
            current_subtotal_price_set = new Model.PriceSet(order.CurrentSubtotalPriceSet);
        }


        current_total_tax = order.CurrentTotalTax ?? default;

        if (order.CurrentTotalTaxSet != null)
        {
            current_total_tax_set = new Model.PriceSet(order.CurrentTotalTaxSet);
        }

        if (order.Customer != null)
        {
            customer = new Model.Customer(order.Customer);
        
        }

        customer_locale = order.CustomerLocale;

        //discount_applications = order.DiscountApplications;
        discount_applications = order.DiscountApplications?.Select(discountApplication => new DiscountApplications(discountApplication)).ToList();


        discount_codes = order.DiscountCodes?.Select(discount_codes => new DiscountCode(discount_codes)).ToList();

        email = order.Email;
        estimated_taxes = order.EstimatedTaxes ?? default;
        financial_status = order.FinancialStatus;

        fulfillments = order.Fulfillments?.Select(fulfillments => new Fulfillments(fulfillments)).ToList();

        fulfillment_status = order.FulfillmentStatus;

        id = order.Id ?? default;
        // gateway = order //no gateway
        landing_site = order.LandingSite;

        line_items = order.LineItems?.Select(lineItem => new LineItem(lineItem)).ToList();

        location_id = order.LocationId ?? default;
        //merchant_of_record_app_id = order.AppId ?? default; // not available

        Name = order.Name;
        note = order.Note;
        note_attributes = order.NoteAttributes?.Select(noteAttribute => new NoteAttribute(noteAttribute)).ToList();

        number = order.Number ?? default;
        order_number = order.OrderNumber ?? default;

        if (order.OriginalTotalAdditionalFeesSet != null) {
            
            original_total_additional_fees_set = new Model.PriceSet(order.OriginalTotalAdditionalFeesSet);
        }

        if (order.OriginalTotalDutiesSet != null) {

            original_total_duties_set = new Model.PriceSet(order.OriginalTotalDutiesSet);
        }

        //if (order.PaymentDetails) //not avaialble

        if (order.PaymentTerms != null)
        { 
            payment_terms = new Model.PaymentTerms(order.PaymentTerms);
        }

        payment_gateway_names = order.PaymentGatewayNames?.ToList();
        phone = order.Phone;
        po_number = order.PoNumber;

        presentment_currency = order.PresentmentCurrency;
        //processing_method = order.ProcessingMethod; // Deprecated
        processed_at = order.ProcessedAt?.DateTime ?? default;
        
        referring_site = order.ReferringSite;
        refunds = order.Refunds?.Select(refunds => new Refund(refunds)).ToList();

        if (order.ShippingAddress != null)
        {

            shipping_address = new Model.Address(order.ShippingAddress);
        }

        shipping_lines = order.ShippingLines?.Select(shippingLine => new ShippingLine(shippingLine)).ToList();

        source_name = order.SourceName;
        //source_identifier = order.SourceIden // not available

        subtotal_price = order.SubtotalPrice ?? default;
        if (order.SubtotalPriceSet != null) {

            subtotal_price_set = new Model.PriceSet(order.SubtotalPriceSet);
        }

        //source_url = order.SourceUrl // not available
        tags = order.Tags;
        tax_lines = order.TaxLines?.Select(taxLine => new TaxLine(taxLine)).ToList();
        taxes_included = order.TaxesIncluded ?? default;
        test = order.Test ?? default;
        token = order.Token;
        total_discounts = order.TotalDiscounts ?? default;
        if (order.TotalDiscountsSet != null) {

            total_discounts_set = new Model.PriceSet(order.TotalDiscountsSet);
        }

        total_line_items_price = order.TotalLineItemsPrice ?? default;
        if (order.TotalLineItemsPriceSet != null) {
            total_line_items_price_set = new Model.PriceSet(order.TotalLineItemsPriceSet);
        }

        total_outstanding = order.TotalOutstanding;
        total_price = order.TotalPrice ?? default;
        if (order.TotalPriceSet != null) {

            total_price_set = new Model.PriceSet(order.TotalPriceSet);
        }

        if (order.TotalShippingPriceSet != null) {

            total_shipping_price_set = new Model.PriceSet(order.TotalShippingPriceSet);
        }


        total_tax = order.TotalTax ?? default;
        if (order.TotalTaxSet != null) {

            total_tax_set = new Model.PriceSet(order.TotalTaxSet);
        }

        total_tip_received = order.TotalTipReceived ?? default;
        total_weight = order.TotalWeight ?? default;
        updated_at = order.UpdatedAt?.DateTime ?? default;
        user_id = order.UserId ?? default;
        order_status_url = order.OrderStatusUrl;

        confirmed = order.Confirmed ?? default;
        device_id = order.DeviceId ?? default;
        if (order.Company != null)
        {

            company = new Model.Company(order.Company);
        }



    }
    public long app_id { get; set; }
    public string browser_ip { get; set; }
    public bool buyer_accepts_marketing { get; set; }
    public string cancel_reason { get; set; }
    public DateTimeOffset cancelled_at { get; set; }
    public string cart_token { get; set; }
    public string checkout_token { get; set; }
    public long checkout_id { get; set; }
    public DateTimeOffset closed_at { get; set; }
    public Company company { get; set; }
    public bool confirmed { get; set; }
    public string confirmation_number { get; set; }
    public DateTimeOffset created_at { get; set; }
    public string currency { get; set; }
    public decimal current_total_discounts { get; set; }
    public PriceSet current_total_discounts_set { get; set; }
    public decimal current_total_price { get; set; }
    public decimal current_subtotal_price { get; set; }
    public decimal current_total_tax { get; set; }
    public string customer_locale { get; set; }
    public string email { get; set; }
    public bool estimated_taxes { get; set; }
    public string fulfillment_status { get; set; }
    public string gateway { get; set; }
    public long id { get; set; }
    public string landing_site { get; set; }
    public long location_id { get; set; }
  // public int merchant_of_record_app_id { get; set; }
    public string Name { get; set; }
    public string note { get; set; }
    public int number { get; set; }
    public int order_number { get; set; }
    public List<string> payment_gateway_names { get; set; }
    public string phone { get; set; }
    public string po_number { get; set; }
    public string presentment_currency { get; set; }
    public DateTimeOffset processed_at { get; set; }
    //public string processing_method { get; set; } //deprecated
    public string referring_site { get; set; }
    public string source_name { get; set; }
    //public string source_identifier { get; set; } // not available
    public decimal subtotal_price { get; set; }
    public string tags { get; set; }
    public bool taxes_included { get; set; }
    public bool test { get; set; }
    public string token { get; set; }
    public decimal total_discounts { get; set; }
    public decimal total_line_items_price { get; set; }
    public string total_outstanding { get; set; }
    public decimal total_price { get; set; }
    public decimal total_tax { get; set; }
    public decimal total_tip_received { get; set; }
    public long total_weight { get; set; }
    public DateTime updated_at { get; set; }
    public long user_id { get; set; }

    public Address billing_address { get; set; }
    public ClientDetails client_details { get; set; }

    public PriceSet current_total_additional_fees_set { get; set; }

    public PriceSet current_total_duties_set { get; set; }
    public PriceSet current_total_price_set { get; set; }

    public PriceSet current_subtotal_price_set { get; set; }

    public PriceSet current_total_tax_set { get; set; }

    public Customer customer { get; set; }
    public List<DiscountApplications> discount_applications { get; set; }
    public List<DiscountCode> discount_codes { get; set; }

    public string financial_status { get; set; }

    public List<Fulfillments> fulfillments { get; set; }

    public List<LineItem> line_items { get; set; }

    public List<NoteAttribute> note_attributes { get; set; }

    public PriceSet original_total_additional_fees_set { get; set; }

    public PriceSet original_total_duties_set { get; set; }

    //public PaymentDetails payment_details { get; set; } //not avaialble

    public PaymentTerms payment_terms { get; set; }

    public List<Refund> refunds { get; set; }

    public Address shipping_address { get; set; }

    public List<ShippingLine> shipping_lines { get; set; }

    //public string source_url { get; set; } // not available

    public PriceSet subtotal_price_set { get; set; }

    public List<TaxLine> tax_lines { get; set; }

    public PriceSet total_discounts_set { get; set; }

    public PriceSet total_line_items_price_set { get; set; }

    public PriceSet total_price_set { get; set; }

    public PriceSet total_shipping_price_set { get; set; }

    public PriceSet total_tax_set { get; set; }
    public string order_status_url { get; set; }
    public long device_id { get; set; }



}



//public Company company { get; set; }

