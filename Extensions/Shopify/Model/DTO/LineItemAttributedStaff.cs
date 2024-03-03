using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class LineItemAttributedStaff : ShopifySharp.LineItemAttributedStaff { 
    

        public LineItemAttributedStaff() { }

        public LineItemAttributedStaff(Model.LineItemAttributedStaff attributedStaff)
        {

            Id = attributedStaff.id;
            Quantity = attributedStaff.quantity;


        }
    }
}
