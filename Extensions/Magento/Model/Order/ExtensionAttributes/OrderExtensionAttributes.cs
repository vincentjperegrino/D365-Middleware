using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class OrderExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public List<ShippingAssignment> shipping_assignments { get; set; }

        [JsonProperty("payment_additional_info")]
        public List<PaymentAdditionalInfo> payment_additional_info { get; set; }

        [JsonProperty("applied_taxes")]
        public List<AppliedTaxes> applied_taxes { get; set; }

        [JsonProperty("item_applied_taxes")]
        public List<ItemAppliedTaxes> item_applied_taxes { get; set; }

        public OrderExtensionAttributes()
        {
            shipping_assignments = new();

        }
    }


}
