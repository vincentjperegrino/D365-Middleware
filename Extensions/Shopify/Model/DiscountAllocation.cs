using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class DiscountAllocation
{
    public DiscountAllocation() { }

    public DiscountAllocation(ShopifySharp.DiscountAllocation discountAllocation) {

        amount = discountAllocation.Amount;
        discount_application_index = discountAllocation.DiscountApplicationIndex;

        if(discountAllocation.AmountSet != null) { 

            amount_set = new Model.PriceSet(discountAllocation.AmountSet);
        
        }

    
    }

    public string amount { get; set; }
    public long discount_application_index { get; set; }
    public PriceSet amount_set { get; set; }



}
