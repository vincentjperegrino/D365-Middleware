using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class ShippingLine : ShopifySharp.ShippingLine { 
    

        public ShippingLine() { }

        public ShippingLine(Model.ShippingLine shippingLine) {

            CarrierIdentifier = shippingLine.carrier_identifier;
            Code = shippingLine.code;
            //Phone = shippingLine.phone
            Price = shippingLine.price;
            DiscountedPrice = shippingLine.discounted_price;
            //DiscountAllocations = shippingLine.discount
            Source = shippingLine.source;
            Title = shippingLine.title;
            TaxLines = shippingLine.tax_lines?.Select(shippingLine => new TaxLine(shippingLine));
            if (shippingLine.price_set != null) {

                PriceSet = new Model.DTO.PriceSet(shippingLine.price_set);

            }
            if (shippingLine.discounted_price_set != null) {

                DiscountedPriceSet = new Model.DTO.PriceSet(shippingLine.discounted_price_set);
            }






        }

    }
}
