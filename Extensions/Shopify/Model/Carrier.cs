using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model
{
    public class Carrier : KTI.Moo.Extensions.Core.Model.CarrierBase

    {
        public Carrier()

        {

        }

        public Carrier(ShopifySharp.Carrier carrier)

        {
            active = carrier.Active ?? default;
            callback_url = carrier.CallbackUrl;
            id = carrier.Id ?? default;
            format = carrier.Format;
            name = carrier.Name;
            service_discovery = carrier.ServiceDiscovery ?? default;
            admin_graphql_api_id = carrier.AdminGraphQLAPIId;


        }

        public bool active { get; set; }
        public string callback_url { get; set; }
        public string callback_method { get; set; }
        public long id { get; set; }
        public string format { get; set; }
        public string name { get; set; }
        public bool service_discovery { get; set; }
        public string admin_graphql_api_id { get; set; }





    }
}
