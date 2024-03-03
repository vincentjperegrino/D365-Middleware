using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class ProductVariant : Core.Model.ProductVariantBase
{
    //public string barcode { get; set; }
    //public decimal compare_at_price { get; set; }
    //public DateTime created_at { get; set; }
    //public string fulfillment_service { get; set; }
    //public long grams { get; set; }
    //public decimal weight { get; set; }
    //public string weight_unit { get; set; }
    //public long id { get; set; }
    //public long inventory_item_id { get; set; }
    //public string inventory_management { get; set; }
    //public string inventory_policy { get; set; }
    //public long inventory_quantity { get; set; }
    //public string option1 { get; set; }
    //public int position { get; set; }
    //public double price { get; set; }
    //public long product_id { get; set; }
    //public bool requires_shipping { get; set; }
    //public string sku { get; set; }
    //public bool taxable { get; set; }
    //public string title { get; set; }
    //public DateTime updated_at { get; set; }



    public ProductVariant()
    {

    }

    public ProductVariant(ShopifySharp.ProductVariant shopifyVariant)
    {
        barcode = shopifyVariant.Barcode;
        compare_at_price = shopifyVariant.CompareAtPrice ?? default;
        created_at = shopifyVariant.CreatedAt?.DateTime ?? default;
        fulfillment_service = shopifyVariant.FulfillmentService;
        grams = shopifyVariant.Grams ?? default;
        weight = shopifyVariant.Weight ?? default;
        weight_unit = shopifyVariant.WeightUnit;
        id = shopifyVariant.Id ?? default;
        inventory_item_id = shopifyVariant.InventoryItemId ?? default;
        inventory_management = shopifyVariant.InventoryManagement;
        inventory_policy = shopifyVariant.InventoryPolicy;
        inventory_quantity = shopifyVariant.InventoryQuantity ?? default;
        option1 = shopifyVariant.Option1;
        position = shopifyVariant.Position ?? default;
        price = shopifyVariant.Price ?? default;
        product_id = shopifyVariant.ProductId ?? default;
        requires_shipping = shopifyVariant.RequiresShipping ?? default;
        sku = shopifyVariant.SKU;
        taxable = shopifyVariant.Taxable ?? default;
        title = shopifyVariant.Title;
        updated_at = shopifyVariant.UpdatedAt?.DateTime ?? default;
        metafields = shopifyVariant.Metafields?.Select(metafield => new MetaField(metafield)).ToList();
    }

    public List<MetaField> metafields { get; set; }


}
