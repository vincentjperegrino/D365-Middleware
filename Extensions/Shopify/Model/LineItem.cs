using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class LineItem
{ 

    public LineItem() { }

    public LineItem(ShopifySharp.LineItem lineItem) {


        attributed_staffs = lineItem.AttributedStaffs?.Select(attributedStaff => new LineItemAttributedStaff(attributedStaff)).ToList();
        fulfillable_quantity = lineItem.FulfillableQuantity ?? default;
        fulfillment_service = lineItem.FulfillmentService;
        fulfillment_status = lineItem.FulfillmentStatus;
        grams = lineItem.Grams ?? default;
        id = lineItem.Id ?? default;
        price = lineItem.Price ?? default;
        product_id = lineItem.ProductId ?? default;
        quantity = lineItem.Quantity ?? default;  
        requires_shipping = lineItem.RequiresShipping ?? default;
        sku = lineItem.SKU;
        title = lineItem.Title;
        variant_id = lineItem.VariantId ?? default;
        variant_title = lineItem.VariantTitle;
        vendor = lineItem.Vendor;
        name = lineItem.Name;
        gift_card = lineItem.GiftCard ?? default;
        if (lineItem.PriceSet != null)
        {
            price_set = new Model.PriceSet(lineItem.PriceSet);

        }

        properties = lineItem.Properties?.Select(properties => new Properties(properties)).ToList();
        taxable = lineItem.Taxable ?? default;
        taxlines = lineItem.TaxLines?.Select(taxLine => new TaxLine(taxLine)).ToList();
        total_discount = lineItem.TotalDiscount ?? default;

        if (lineItem.TotalDiscountSet != null)
        {
            total_discount_set = new Model.PriceSet(lineItem.TotalDiscountSet);
        
        }

        discount_allocations = lineItem.DiscountAllocations?.Select(discountAllocation => new DiscountAllocation(discountAllocation)).ToList();


        duties = lineItem.Duties?.Select(lineItemDuty => new LineItemDuty(lineItemDuty)).ToList();






    }

    public List<LineItemAttributedStaff> attributed_staffs { get; set; }
    public int fulfillable_quantity { get; set; }
    public string fulfillment_service { get; set; }
    public string fulfillment_status { get; set; }
    public long grams { get; set; }
    public long id { get; set; }
    public decimal price { get; set; }
    public long product_id { get; set; }
    public int quantity { get; set; }
    public bool requires_shipping { get; set; }
    public string sku { get; set; }
    public string title { get; set; }
    public long variant_id { get; set; }
    public string variant_title { get; set; }
    public string vendor { get; set; }
    public string name { get; set; }
    public bool gift_card { get; set; }
    public PriceSet price_set { get; set; }
    public List<Properties> properties { get; set; }
    public bool taxable { get; set; }
    public List<TaxLine> taxlines { get; set; }
    public decimal total_discount { get; set; }

    public PriceSet total_discount_set { get; set; }

    public List<DiscountAllocation> discount_allocations { get; set; }
    //public OriginLocation origin_location { get; set; } // not avaialable

    public List<LineItemDuty> duties { get; set; }
}