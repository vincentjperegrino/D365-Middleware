using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class LineItemDuty : ShopifySharp.LineItemDuty {

        public LineItemDuty() { }


        public LineItemDuty(Model.LineItemDuty lineItemDuty) {

            HarmonizedSystemCode = lineItemDuty.harmonized_system_code;
            CountryCodeOfOrigin = lineItemDuty.country_code_of_origin;

            if (lineItemDuty.price_set != null) {

                PriceSet = new Model.DTO.PriceSet(lineItemDuty.price_set);
            }

            TaxLines = lineItemDuty.taxLines?.Select(taxLine => new TaxLine(taxLine));




        }


    }


}
