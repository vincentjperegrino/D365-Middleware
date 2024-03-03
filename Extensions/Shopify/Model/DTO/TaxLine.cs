using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class TaxLine : ShopifySharp.TaxLine { 
    

        public TaxLine() { }

        public TaxLine(Model.TaxLine taxLine) {


            Price = taxLine.price;
            Rate = taxLine.rate;
            Title = taxLine.title;

            if (taxLine.price_set != null)
            { 

                PriceSet = new Model.DTO.PriceSet(taxLine.price_set);
            
            
            }





        }  


    
    }
}
