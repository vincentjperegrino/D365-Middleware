using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class MetaField : ShopifySharp.MetaField
    {
        public MetaField()
        {
        }

        public MetaField(Model.MetaField metaField)
        {
            Id = metaField.id;
            Namespace = metaField.Namespace;
            Key = metaField.key;
            Value = metaField.value;
            Type = metaField.type;

            if (metaField.created_at != default)
            {
                CreatedAt = metaField.created_at;
            }
            if (metaField.updated_at != default)
            {
                UpdatedAt = metaField.updated_at;
            }

        
            OwnerId = metaField.owner_id;
            OwnerResource = metaField.owner_resource;
            Description = metaField.description;
        }

    }
}
