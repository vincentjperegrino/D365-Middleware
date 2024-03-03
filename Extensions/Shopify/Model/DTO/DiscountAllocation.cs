using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class DiscountAllocation : ShopifySharp.DiscountAllocation {



        public DiscountAllocation() { 
        
        
        }

        public DiscountAllocation(Model.DiscountAllocation discountAllocation) {

            Amount = discountAllocation.amount;
            DiscountApplicationIndex = discountAllocation.discount_application_index;

            if (discountAllocation.amount_set != null)

            {
                AmountSet = new Model.DTO.PriceSet(discountAllocation.amount_set);
            
            }



        }
    
    }
}
