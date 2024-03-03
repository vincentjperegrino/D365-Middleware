using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class MetaField : Core.Model.MetaFieldBase
{

    public MetaField() { }  

    public MetaField(ShopifySharp.MetaField shopifyMetafield)
    {
        id = shopifyMetafield.Id ?? default;
        Namespace = shopifyMetafield.Namespace;
        key = shopifyMetafield.Key;
        value = shopifyMetafield.Value;
        type = shopifyMetafield.Type;
        created_at = shopifyMetafield.CreatedAt?.DateTime ?? default;
        updated_at = shopifyMetafield.UpdatedAt?.DateTime ?? default;
        owner_id = shopifyMetafield.OwnerId ?? default;
        owner_resource = shopifyMetafield.OwnerResource;
        description = shopifyMetafield.Description;
    }




}
