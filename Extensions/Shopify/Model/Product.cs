using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Product : Core.Model.ProductBase
{


    public Product()
    {


    }

    public Product(ShopifySharp.Product product)
    {
        body_html = product.BodyHtml;
        created_at = product.CreatedAt?.DateTime ?? default;
        handle = product.Handle;
        id = product.Id ?? default;
        product_type = product.ProductType;
        published_at = product.PublishedAt?.DateTime ?? default;
        published_scope = product.PublishedScope;
        status = product.Status;
        tags = product.Tags;
        template_suffix = product.TemplateSuffix;
        title = product.Title;
        updated_at = product.UpdatedAt?.DateTime ?? default;
        vendor = product.Vendor;

        variants = product.Variants?.Select(variant => new ProductVariant(variant)).ToList();
        images = product.Images?.Select(image => new ProductImage(image)).ToList();
        options = product.Options?.Select(option => new ProductOption(option)).ToList();
        metafields = product.Metafields?.Select(metafield => new MetaField(metafield)).ToList();
    }


    public string body_html { get; set; }
    public DateTime created_at { get; set; }
    public string handle { get; set; }
    public long id { get; set; }

    public override string description { get => body_html; set => body_html = value; }

    public List<ProductImage> images { get; set; }
    public List<ProductOption> options { get; set; }
    public string product_type { get; set; }
    public override string parentproductid { get => product_type; set => product_type = value; }
    public DateTime published_at { get; set; }
    public string published_scope { get; set; }
    public string status { get; set; }
    public string tags { get; set; }
    public string template_suffix { get; set; }

    public string title { get; set; }
    public override string name { get => title; set => title = value; }

    public DateTime updated_at { get; set; }
    public List<ProductVariant> variants { get; set; }
    public string vendor { get; set; }
    public List<MetaField> metafields { get; set; }
}
