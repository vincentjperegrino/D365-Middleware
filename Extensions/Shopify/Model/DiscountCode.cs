using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class DiscountCode
{
    public DiscountCode() { 
    
    }

    public DiscountCode(ShopifySharp.DiscountCode discountCode)
    {

        code = discountCode.Code;
        amount = discountCode.Amount;
        type = discountCode.Type;



    }

    public string code { get; set; }
    public string amount { get; set; }
    public string type { get; set; }
    //public string applicable { get; set; }
    //public string description { get; set; }
    //public string value { get; set; }

}
