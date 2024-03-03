using Newtonsoft.Json;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class InventoryCustomFilter : ListFilter<ShopifySharp.InventoryItem>
    {

        [JsonProperty("sku")]
        public string? sku { get; set; }


    }
}
