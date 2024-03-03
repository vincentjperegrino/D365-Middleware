using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class PresentmentMoney : ShopifySharp.Price {



        public PresentmentMoney() { }


        public PresentmentMoney(Model.PresentmentMoney presentmentMoney) {



            CurrencyCode = presentmentMoney.currency_code;
            Amount = presentmentMoney.amount;

        }



    }
}
