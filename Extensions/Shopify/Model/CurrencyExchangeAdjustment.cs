using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class CurrencyExchangeAdjustment {



    public CurrencyExchangeAdjustment() { }

    public CurrencyExchangeAdjustment(ShopifySharp.CurrencyExchangeAdjustment currencyExchangeAdjustment) {

        adjustment = currencyExchangeAdjustment.Adjustment ?? default;
        original_amount = currencyExchangeAdjustment.OriginalAmount ?? default;
        final_amount = currencyExchangeAdjustment.FinalAmount ?? default;
        currency = currencyExchangeAdjustment.Currency;
        

    
    
    }



    public decimal adjustment { get; set; }
    public decimal original_amount { get; set; }
    public decimal final_amount { get; set; }
    public string currency { get; set; }

}