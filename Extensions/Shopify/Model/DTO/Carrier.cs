using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Carrier : ShopifySharp.Carrier
    {
        public Carrier()
        {
        }

        public Carrier(Model.Carrier carrier)
        {


            Active = carrier.active;
            CallbackUrl = carrier.callback_url;
            Id = carrier.id;
            Format = carrier.format;
            Name = carrier.name;
            ServiceDiscovery = carrier.service_discovery;
            AdminGraphQLAPIId = carrier.admin_graphql_api_id;

        }


    }
}
