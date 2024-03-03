using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO;

public class ProductOption : ShopifySharp.ProductOption
{
    public ProductOption()
    {

    }

    public ProductOption(Model.ProductOption productOption)
    {
        Id = productOption.id;
        ProductId = productOption.product_id;
        Name = productOption.name;
        Position = productOption.position;
        Values = productOption.values;
    }

}
