using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class InvoiceItem : InvoiceItemBase
    {
        private decimal _RetailSalesPrice = 0;

        [JsonProperty("UnitCost", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal UnitCost { get; set; }

        [JsonProperty("TotalDiscount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal manualdiscountamount { get; set; }

        [JsonProperty("DiscountPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal DiscountPercentage { get; set; }

        [JsonProperty("ItemNetAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal baseamount { get; set; }

        [JsonProperty("ProductCode")]
        public override string productid { get; set; }

        [JsonProperty("ProductSKU")]
        public string ProductSKU { get; set; }

        [JsonProperty("ProductDescription")]
        public override string productdescription { get; set; }

        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("ItemRemark")]
        public override string description { get; set; }

        [JsonProperty("Quantity", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal quantity { get; set; }

        [JsonProperty("Returnqty")]
        public double Returnqty { get; set; }

        [JsonProperty("RetailSalesPrice", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal RetailSalesPrice
        {

            get
            {

                if (_RetailSalesPrice == 0)
                {
                    return baseamount;

                }

                return _RetailSalesPrice;
            }

            set => _RetailSalesPrice = value;
        }

        [JsonProperty("TaxAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal tax { get; set; }

        [JsonProperty("TaxPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TaxPercentage { get; set; }

        [JsonProperty("SalesmanCode")]
        public string SalesmanCode { get; set; }

        [JsonProperty("SalesmanName")]
        public string SalesmanName { get; set; }


    }
}
