using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class DiscountApplications : ShopifySharp.DiscountApplication { 
    
    


        public DiscountApplications() { }

        public DiscountApplications(Model.DiscountApplications discountApplications) {

            Type = discountApplications.type;
            Code = discountApplications.code;
            Title = discountApplications.title;
            Description = discountApplications.description;
            Value = discountApplications.value;
            ValueType = discountApplications.value_type;
            AllocationMethod = discountApplications.allocation_method;
            TargetSelection = discountApplications.target_selection;
            TargetType = discountApplications.target_type;


        }

    }
}
