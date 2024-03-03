using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Properties : ShopifySharp.LineItemProperty {


        public Properties() { }

        public Properties(Model.Properties properties) {

            Name = properties.name;
            Value = properties.value;


        }

    }
}
