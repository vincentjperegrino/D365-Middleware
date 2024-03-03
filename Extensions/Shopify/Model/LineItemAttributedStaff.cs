using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class LineItemAttributedStaff {



    public LineItemAttributedStaff() { }


    public LineItemAttributedStaff(ShopifySharp.LineItemAttributedStaff attributedStaff) {

        id = attributedStaff.Id ?? default;
        quantity = attributedStaff.Quantity ?? default;

    }

    public string id { get; set; }
    public long quantity { get; set; }

}