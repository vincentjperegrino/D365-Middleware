using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class ProductImage : Core.Model.ProductImageBase
{
    //public long id { get; set; }
    //public long product_id { get; set; }
    //public int position { get; set; }
    //public DateTime created_at { get; set; }
    //public DateTime updated_at { get; set; }
    //public int width { get; set; }
    //public int height { get; set; }
    //public string src { get; set; }
    //public List<long> variant_ids { get; set; }

    public List<MetaField> metafields { get; set; }

    public ProductImage()
    {

    }

    public ProductImage(ShopifySharp.ProductImage shopifyImage)
    {
        id = shopifyImage.Id ?? default;
        product_id = shopifyImage.ProductId ?? default;
        position = shopifyImage.Position ?? default;
        created_at = shopifyImage.CreatedAt?.DateTime ?? default;
        updated_at = shopifyImage.UpdatedAt?.DateTime ?? default;
        width = shopifyImage.Width ?? default;
        height = shopifyImage.Height?? default;
        src = shopifyImage.Src;
        variant_ids = shopifyImage.VariantIds?.ToList();
        metafields = shopifyImage.Metafields?.Select(metafield => new Model.MetaField(metafield)).ToList();
    }
}
