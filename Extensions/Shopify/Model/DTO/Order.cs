using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Order : ShopifySharp.Order {


        public Order() { 
        
        }

        public Order(Model.Order order) {

            AppId = order.app_id;

            if (order.billing_address != null)
            {
                BillingAddress = new Model.DTO.Address(order.billing_address);
            
            }

            BrowserIp = order.browser_ip;
            BuyerAcceptsMarketing = order.buyer_accepts_marketing;
            CancelReason = order.cancel_reason;
           
            if (order.cancelled_at != default)
            {
                CancelledAt = order.cancelled_at;
            }

            CartToken = order.cart_token;
            CheckoutToken = order.checkout_token;
            CheckoutId = order.checkout_id;

            if (order.client_details != null)
            { 
                ClientDetails = new Model.DTO.ClientDetails(order.client_details);
            
            }

            if (order.closed_at != default) { 
            
                ClosedAt = order.closed_at;
            }

            Confirmed = order.confirmed;

            if (order.created_at != default)
            { 
                CreatedAt = order.created_at;
            
            }

            Currency = order.currency;

            if (order.customer != null)
            {
                
                Customer = new Model.DTO.Customer(order.customer);
            
            }

            CustomerLocale = order.customer_locale;
            DeviceId = order.device_id;

            DiscountCodes = order.discount_codes?.Select(discountCode => new DiscountCode(discountCode));
            DiscountApplications = order.discount_applications?.Select(discountApplication => new DiscountApplications(discountApplication));
           
            Email = order.email;
            FinancialStatus = order.financial_status;
            Fulfillments = order.fulfillments?.Select(fulfillment => new Fulfillments(fulfillment));
            Phone = order.phone;
            Tags = order.tags;
            LandingSite = order.landing_site;
            LineItems = order.line_items?.Select(lineItem => new LineItem(lineItem));
            LocationId = order.location_id;
            Name = order.name;
            Note = order.note;
            NoteAttributes = order.note_attributes?.Select(noteAttribute => new NoteAttribute(noteAttribute));
            Number = order.number;
            OrderNumber = order.order_number;
            OrderStatusUrl = order.order_status_url;
            PaymentGatewayNames = order.payment_gateway_names ?? Enumerable.Empty<string>();
            ProcessedAt = order.processed_at;
            ReferringSite = order.referring_site;
            Refunds = order.refunds?.Select(refund => new Refund(refund));
            if (order.shipping_address != null)
            {
                ShippingAddress = new Model.DTO.Address(order.shipping_address);
            }
            ShippingLines = order.shipping_lines?.Select(shippingLine => new ShippingLine(shippingLine));
            SourceName = order.source_name;
            SubtotalPrice = order.subtotal_price;
            TaxLines = order.tax_lines?.Select(taxLine => new TaxLine(taxLine));
            TaxesIncluded = order.taxes_included;
            Test = order.test;
            Token = order.token;
            TotalDiscounts = order.total_discounts;
            TotalLineItemsPrice = order.total_line_items_price;
            TotalTipReceived = order.total_tip_received;
            TotalPrice = order.total_price;
            TotalTax = order.total_tax;
            TotalWeight = order.total_weight;
            if (order.updated_at != default) {

                UpdatedAt = order.updated_at;
            }
            UserId = order.user_id;
            //Transactions = order.transaction
            //Metafields = order.meta
            if (order.current_total_duties_set != null) {

                CurrentTotalDutiesSet = new Model.DTO.PriceSet(order.current_subtotal_price_set);
            }
            if (order.original_total_duties_set != null)
            {
                OriginalTotalDutiesSet = new Model.DTO.PriceSet(order.original_total_duties_set);
            }
            PresentmentCurrency = order.presentment_currency;
            if (order.total_line_items_price_set != null) {

                TotalLineItemsPriceSet = new Model.DTO.PriceSet(order.total_line_items_price_set);
            }
            if (order.total_discounts_set != null) {

                TotalDiscountsSet = new Model.DTO.PriceSet(order.total_discounts_set);
            }
            if (order.total_shipping_price_set != null) {

                TotalShippingPriceSet = new Model.DTO.PriceSet(order.total_shipping_price_set);
            }
            if (order.subtotal_price_set != null) {

                SubtotalPriceSet = new Model.DTO.PriceSet(order.subtotal_price_set);
            }
            if (order.total_price_set != null)
            {
                TotalPriceSet = new Model.DTO.PriceSet(order.total_price_set);
            }
            TotalOutstanding = order.total_outstanding;
            if (order.total_tax_set != null) 
            {
                TotalTaxSet = new Model.DTO.PriceSet(order.total_tax_set);
            }
            EstimatedTaxes = order.estimated_taxes;
            CurrentSubtotalPrice = order.current_subtotal_price;
            if (order.current_subtotal_price_set != null) 
            {
                CurrentSubtotalPriceSet = new Model.DTO.PriceSet(order.current_subtotal_price_set);
            }
            CurrentTotalDiscounts = order.current_total_discounts;
            if (order.current_total_discounts_set != null)
            {
                CurrentTotalDiscountsSet = new Model.DTO.PriceSet(order.current_total_discounts_set);
            }
            CurrentTotalPrice = order.current_total_price;
            if (order.current_total_price_set != null)
            {
                CurrentTotalPriceSet = new Model.DTO.PriceSet(order.current_total_price_set);
            }
            CurrentTotalTax = order.current_total_tax;
            if (order.current_total_tax_set != null)
            {
                CurrentTotalTaxSet = new Model.DTO.PriceSet(order.current_total_tax_set);
            }
           
            if (order.payment_terms != null)
            {
                PaymentTerms = new Model.DTO.PaymentTerms(order.payment_terms);
            }
            if (order.current_total_additional_fees_set != null)
            {
                CurrentTotalAdditionalFeesSet = new Model.DTO.PriceSet(order.current_total_additional_fees_set);
            }
         
            if (order.original_total_additional_fees_set != null)
            {
                OriginalTotalAdditionalFeesSet = new Model.DTO.PriceSet(order.original_total_additional_fees_set);
            }
            PoNumber = order.po_number;
            //TaxExempt = order.tax_exempt
            if (order.company != null)
            {
                Company = new Model.DTO.Company(order.company);
            }









        }
    
    
    }
}
