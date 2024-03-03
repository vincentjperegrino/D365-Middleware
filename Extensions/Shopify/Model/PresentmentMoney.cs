using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class PresentmentMoney {

    public PresentmentMoney() { 
    
    }

    public PresentmentMoney(ShopifySharp.Price presentmentMoney) {

        amount = presentmentMoney.Amount ?? default;
        currency_code = presentmentMoney.CurrencyCode;
    }




    public decimal amount { get; set; }
    public string currency_code { get; set; }

}
