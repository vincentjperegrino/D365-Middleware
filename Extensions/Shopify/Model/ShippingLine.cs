using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class ShippingLine { 

    public ShippingLine() { }

    public ShippingLine(ShopifySharp.ShippingLine shippingLine) {

        code = shippingLine.Code;
        price = shippingLine.Price ?? default;
        if (shippingLine.PriceSet != null)
        {
            price_set = new Model.PriceSet(shippingLine.PriceSet);
        
        }
        discounted_price = shippingLine.DiscountedPrice ?? default;

        if (shippingLine.DiscountedPriceSet != null) {

            discounted_price_set = new Model.PriceSet(shippingLine.DiscountedPriceSet);
        }

        source = shippingLine.Source;
        title = shippingLine.Title;
        tax_lines = shippingLine.TaxLines?.Select(taxLine => new TaxLine(taxLine)).ToList();
        carrier_identifier = shippingLine.CarrierIdentifier;
        //requested_fulfillment_service_id = shippingLine.Full // not available

    }

    public string code { get; set; }
    public decimal price { get; set; } 
    public PriceSet price_set { get; set; }
    public decimal discounted_price { get; set; }
    public PriceSet discounted_price_set { get; set; }
    public string source { get; set; }
    public string title { get; set; }
    public List<TaxLine> tax_lines { get; set; }
    public string carrier_identifier { get; set; }
    //public string requested_fulfillment_service_id { get; set; }



}