using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class DiscountApplications
{
    public DiscountApplications () { 
    
    }

    public DiscountApplications (ShopifySharp.DiscountApplication discountApplication)
    {
        type = discountApplication.Type;
        title = discountApplication.Title;
        description = discountApplication.Description;
        value = discountApplication.Value;
        value_type = discountApplication.ValueType;
        allocation_method = discountApplication.AllocationMethod;
        target_selection = discountApplication.TargetSelection;
        target_type = discountApplication.TargetType;
        code = discountApplication.Code;


    }

    public string type { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string value { get; set; }
    public string value_type { get; set; }
    public string allocation_method { get; set; }
    public string target_selection { get; set; }
    public string target_type { get; set; }
    public string code { get; set; }
}
