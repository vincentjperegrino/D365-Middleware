using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class PriceSet : ShopifySharp.PriceSet
    
    { 
    


        public PriceSet() { }


        public PriceSet(Model.PriceSet priceSet) {

            

            
            if (priceSet.shop_money != null)
            {
                ShopMoney = new Model.DTO.ShopMoney(priceSet.shop_money);
            }


            if (priceSet.presentment_money != null)
            {
                PresentmentMoney = new Model.DTO.PresentmentMoney(priceSet.presentment_money);
            }


        }
    }
}
