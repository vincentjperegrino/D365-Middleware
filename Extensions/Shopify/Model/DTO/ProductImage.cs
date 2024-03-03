using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO;

public class ProductImage : ShopifySharp.ProductImage
{
    public ProductImage()
    {

    }

    public ProductImage(Model.ProductImage productImage)
    {
        ProductId = productImage.product_id;
        Position = productImage.position;

        if (productImage.created_at != default)
        {
            CreatedAt = productImage.created_at;
        }

        if (productImage.updated_at != default)
        {
            UpdatedAt = productImage.updated_at;
        }

        Src = productImage.src;
        VariantIds = productImage.variant_ids;
        Height = productImage.height;
        Width = productImage.width;
        Metafields = productImage.metafields?.Select(metafield => new Model.DTO.MetaField(metafield));
    }
}
