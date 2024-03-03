using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class ShopMoney : ShopifySharp.Price
    
    {



        public ShopMoney() { 
        
        }

        public ShopMoney(Model.ShopMoney shopMoney)
        {
            CurrencyCode = shopMoney.currency_code;
            Amount = shopMoney.amount;

        }


    }
}
