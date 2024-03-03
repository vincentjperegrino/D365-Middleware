using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Product : ShopifySharp.Product
    {
        public Product()
        {

        }

        public Product(Model.Product product)
        {

            Title = product.title;
            BodyHtml = product.body_html;

            if (product.created_at != default)
            {
                CreatedAt = product.created_at;
            }

            if (product.updated_at != default)
            {
                UpdatedAt = product.updated_at;
            }

            if (product.published_at != default)
            {
                PublishedAt = product.published_at;
            }

            Vendor = product.vendor;
            ProductType = product.product_type;
            Handle = product.handle;
            TemplateSuffix = product.template_suffix;
            PublishedScope = product.published_scope;
            Tags = product.tags;
            Status = product.status;

            Variants = product.variants?.Select(variant => new Model.DTO.ProductVariant(variant));
            Options = product.options?.Select(option => new Model.DTO.ProductOption(option));
            Images = product.images?.Select(image => new Model.DTO.ProductImage(image));
            Metafields = product.metafields?.Select(metafield => new Model.DTO.MetaField(metafield));

        }

    }
}
