using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class OrderItem : OrderItemBase
    {
        [JsonProperty("RetailSalesPrice", DefaultValueHandling = DefaultValueHandling.Include)]
        [Range(-922337203685477, 922337203685477)]
        public override decimal baseamount { get; set; }

        [JsonProperty("LocalItemId", DefaultValueHandling = DefaultValueHandling.Include)]
        [Range(0, 1000000000)]
        public override int lineitemnumber { get; set; }

        [JsonProperty("ProductCode")]
        public override string productid { get; set; }

        [JsonProperty("Quantity")]
        [Required]
        [Range(-100000000000, 100000000000)]
        public override decimal quantity { get; set; }

        [JsonProperty("UnitCost")]
        public override decimal costpriceperunit { get; set; }

        [JsonProperty("TotalDiscount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal manualdiscountamount { get; set; }

        [JsonProperty("DiscountPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal DiscountPercentage { get; set; }

        [JsonProperty("ItemNetAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal ItemNetAmount { get; set; }

        [JsonProperty("TaxAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TaxAmount { get; set; }

        [JsonProperty("TaxPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TaxPercentage { get; set; }
    }
}
