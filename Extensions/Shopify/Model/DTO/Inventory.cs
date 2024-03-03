using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Inventory : ShopifySharp.InventoryItem
    {
        public Inventory()
        {

        }


        public Inventory(Model.Inventory inventory)
        {
            Cost = inventory.cost;
            CountryCodeOfOrigin = inventory.country_code_of_origin;
         
            if (inventory.created_at != default)
            {
                CreatedAt = inventory.created_at;
            }
            HarmonizedSystemCode = inventory.harmonized_system_code;
            Id = inventory.id;
            ProvinceCodeOfOrigin = inventory.province_code_of_origin;
            SKU = inventory.sku;
            Tracked = inventory.tracked;
            if (inventory.updated_at != default)
            {
                UpdatedAt = inventory.updated_at;
            }
            RequiresShipping = inventory.requires_shipping;

        }



    }

}
