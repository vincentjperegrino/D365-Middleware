using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model
{
    public class PricesBase
    {
        public string dataAreaId { get; set; }
        public long RecordId { get; set; }
        public DateTime PriceApplicableFromDate { get; set; }
        public string WillSearchContinue { get; set; }
        public decimal SalesPriceQuantity { get; set; }
        public string QuantityUnitySymbol { get; set; }
        public string ProductNumber { get; set; }
        public string AttributeBasedPricingId { get; set; }
        public string ProductSizeId { get; set; }
        public string ItemNumber { get; set; }
        public string ProductVersionId { get; set; }
        public string PriceCurrencyCode { get; set; }
        public decimal ToQuantity { get; set; }
        public decimal FixedPriceCharges { get; set; }
        public string WillDeliveryDateControlDisregardLeadTime { get; set; }
        public DateTime PriceApplicableToDate { get; set; }
        public string PriceWarehouseId { get; set; }
        public int SalesLeadTimeDays { get; set; }
        public decimal FromQuantity { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string PriceCustomerGroupCode { get; set; }
        public double Price { get; set; }
        public string PriceSiteId { get; set; }
        public string IsGenericCurrencySearchEnabled { get; set; }
        public string ProductColorId { get; set; }
        public string ProductconfigurationId { get; set; }
        public string ProductStyleId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        //Customized
        //[CompanyIdAttribute]
        public int companyid { get; set; }
    }
}
