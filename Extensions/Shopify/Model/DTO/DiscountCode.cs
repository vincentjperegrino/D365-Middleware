using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class DiscountCode : ShopifySharp.DiscountCode {



        public DiscountCode() { }


        public DiscountCode(Model.DiscountCode discountCode) {

            Amount = discountCode.amount;
            Code = discountCode.code;
            Type = discountCode.type;


        }

    }
}
