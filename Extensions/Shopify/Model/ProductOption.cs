using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class ProductOption
{
    public long id { get; set; }
    public long product_id { get; set; }
    public string name { get; set; }
    public int position { get; set; }
    public List<string> values { get; set; }

    public ProductOption()
    {

    }

    public ProductOption(ShopifySharp.ProductOption shopifyOption)
    {
        id = shopifyOption.Id ?? default;
        name = shopifyOption.Name;
        position = shopifyOption.Position ?? default;
        product_id = shopifyOption.ProductId ?? default;
        values = shopifyOption.Values.ToList();
    }
}
