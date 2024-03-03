using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ProductLinks
    {
        public ProductExtensionAttributes extension_attributes { get; set; }

        public string link_type { get; set; }

        public string linked_product_sku { get; set; }

        public string linked_product_type { get; set; }

        public int position { get; set; }

        public string sku { get; set; }

    }
}
