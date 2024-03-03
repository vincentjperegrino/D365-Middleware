using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class CurrencyExchangeAdjustment : ShopifySharp.CurrencyExchangeAdjustment 
    { 
    

        public CurrencyExchangeAdjustment() { }


        public CurrencyExchangeAdjustment(Model.CurrencyExchangeAdjustment currencyExchangeAdjustment) {

            Adjustment = currencyExchangeAdjustment.adjustment;
            OriginalAmount = currencyExchangeAdjustment.original_amount;
            FinalAmount = currencyExchangeAdjustment.final_amount;
            Currency = currencyExchangeAdjustment.currency;

        }


    }
}
