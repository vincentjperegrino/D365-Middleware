using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class LineItem : ShopifySharp.LineItem { 
    
    

        public LineItem() { }

        public LineItem(Model.LineItem lineItem) {



            FulfillableQuantity = lineItem.fulfillable_quantity;
            FulfillmentService = lineItem.fulfillment_service;
            FulfillmentStatus = lineItem.fulfillment_status;
            Grams = lineItem.grams;
            Price = lineItem.price;
            ProductId = lineItem.product_id;
            Quantity = lineItem.quantity;
            RequiresShipping = lineItem.requires_shipping;
            SKU = lineItem.sku;
            Title = lineItem.title;
            VariantId = lineItem.variant_id;
            VariantTitle = lineItem.variant_title;
            Name = lineItem.name;
            Vendor = lineItem.vendor;
            GiftCard = lineItem.gift_card;
            Taxable = lineItem.taxable;

            TaxLines = lineItem.taxlines?.Select(taxLine => new TaxLine(taxLine));

            //TipPaymentGateway = lineItem.

            //TipPaymentMethod = lineItem

            //TipPaymentGatewaySpecified = lineItem.Tip

            TotalDiscount = lineItem.total_discount;

            if (lineItem.total_discount_set != null) {
                
                TotalDiscountSet = new Model.DTO.PriceSet(lineItem.total_discount_set);
            
            }

            DiscountAllocations = lineItem.discount_allocations?.Select(discountAllocation => new DiscountAllocation(discountAllocation));

            Properties = lineItem.properties?.Select(properties => new Properties(properties));
            //VariantInventoryManagement = lineItem.variantI

            if (lineItem.price_set != null)
            {

                PriceSet = new Model.DTO.PriceSet(lineItem.price_set);

            }

            //PreTaxPrice = lineItem.PreTax

            Duties = lineItem.duties?.Select(duties => new LineItemDuty(duties));
     

            AttributedStaffs = lineItem.attributed_staffs?.Select(attributedStaff => new LineItemAttributedStaff(attributedStaff));





        } 

    }
}
