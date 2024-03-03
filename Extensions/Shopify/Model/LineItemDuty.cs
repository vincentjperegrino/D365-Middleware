using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class LineItemDuty {

    public LineItemDuty() { }

    public LineItemDuty(ShopifySharp.LineItemDuty lineItemDuty) {

        id = lineItemDuty.Id ?? default;
        harmonized_system_code = lineItemDuty.HarmonizedSystemCode;
        country_code_of_origin = lineItemDuty.CountryCodeOfOrigin;
        if (lineItemDuty.PriceSet != null)
        {
            price_set = new Model.PriceSet(lineItemDuty.PriceSet);
        
        }

        taxLines = lineItemDuty.TaxLines?.Select(taxLine => new TaxLine(taxLine) ).ToList();

        admin_graphql_api_id = lineItemDuty.AdminGraphQLAPIId;



    }


    public long id { get; set; }
    public string harmonized_system_code { get; set; }
    public string country_code_of_origin { get; set; }
    public PriceSet price_set { get; set; }

    public List<TaxLine> taxLines { get; set; }

    public string admin_graphql_api_id { get; set; }

}