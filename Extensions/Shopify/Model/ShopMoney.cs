using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class ShopMoney
{ 

    public ShopMoney() { }  
    
    public ShopMoney(ShopifySharp.Price shopMoney) {

        amount = shopMoney.Amount ?? default;
        currency_code = shopMoney.CurrencyCode;

    }



    public decimal amount { get; set; }
    public string currency_code { get; set; }



}
