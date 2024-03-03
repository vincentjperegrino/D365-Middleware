using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO;

public class ProductVariant : ShopifySharp.ProductVariant
{
    public ProductVariant()
    {

    }

    public ProductVariant(Model.ProductVariant productVariant)
    {
        Barcode = productVariant.barcode;
        CompareAtPrice = productVariant.compare_at_price;

        if (productVariant.created_at != default)
        {
            CreatedAt = productVariant.created_at;
        }

        if (productVariant.updated_at != default)
        {
            UpdatedAt = productVariant.updated_at;
        }

        FulfillmentService = productVariant.fulfillment_service;
        Grams = productVariant.grams;
        Weight = productVariant.weight;
        WeightUnit = productVariant.weight_unit;
        Id = productVariant.id;
        InventoryItemId = productVariant.inventory_item_id;
        InventoryManagement = productVariant.inventory_management;
        InventoryPolicy = productVariant.inventory_policy;
        InventoryQuantity = productVariant.inventory_quantity;
        Option1 = productVariant.option1;
        Position = productVariant.position;
        Price = productVariant.price;
        ProductId = productVariant.product_id;
        RequiresShipping = productVariant.requires_shipping;
        SKU = productVariant.sku;
        Taxable = productVariant.taxable;
        Title = productVariant.title;

        Metafields = productVariant.metafields?.Select(metafield => new MetaField(metafield));
    }
}
