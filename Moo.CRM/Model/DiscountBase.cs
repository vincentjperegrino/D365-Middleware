using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model
{
    public class DiscountBase
    {
        public string BarCode { get; set; }
        public string CombinationHeaderStructure { get; set; }
        public string ConcurrencyMode { get; set; }
        public string CurrencyCode { get; set; }
        public string DateValidationType { get; set; }
        public string Description { get; set; }
        public string DisabledSince { get; set; }
        public string Disclaimer { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountCodeId { get; set; }
        public int DiscountLedgerDimension { get; set; }
        public string DiscountLedgerDimensionDisplayValue { get; set; }
        public double DiscountPercentValue { get; set; }
        public int DiscountRecordId { get; set; }
        public string DiscountVendorClaimGroupName { get; set; }
        public string FreeItemCalculationMethod { get; set; }
        public string FreeItemCriteriaType { get; set; }
        public string Header1 { get; set; }
        public string Header10 { get; set; }
        public string Header11 { get; set; }
        public string Header12 { get; set; }
        public string Header13 { get; set; }
        public string Header14 { get; set; }
        public string Header15 { get; set; }
        public string Header2 { get; set; }
        public string Header3 { get; set; }
        public string Header4 { get; set; }
        public string Header5 { get; set; }
        public string Header6 { get; set; }
        public string Header7 { get; set; }
        public string Header8 { get; set; }
        public string Header9 { get; set; }
        public int HeaderPricingRule { get; set; }
        public string IsDiscountCodeRequired { get; set; }
        public string MatchAllAssociatedPriceGroups { get; set; }
        public string MaxCriteriaType { get; set; }
        public double MaxQtyOrAmount { get; set; }
        public double MixAndMatchDealPrice { get; set; }
        public double MixAndMatchDiscountAmount { get; set; }
        public string MixAndMatchDiscountType { get; set; }
        public int MixAndMatchLeastExpensiveMode { get; set; }
        public int MixAndMatchNoOfLeastExpensiveLines { get; set; }
        public int MixAndMatchNumberOfTimesApplicable { get; set; }
        public string MultibuyDiscountType { get; set; }
        public string Name { get; set; }
        public string OfferId { get; set; }
        public int OfferQuantityLimit { get; set; }
        public string PeriodicDiscountType { get; set; }
        public string PriceAttributeGroupName { get; set; }
        public string PriceAttributeGroupType { get; set; }
        public string PriceComponentCodeName { get; set; }
        public int PricingPriorityNumber { get; set; }
        public string PrintDescriptionOnFiscalReceipt { get; set; }
        public string ProcessingStatus { get; set; }
        public string Repeatable { get; set; }
        public string ShouldUseInterval { get; set; }
        public string Status { get; set; }
        public string ThresholdCountNonDiscountItems { get; set; }
        public string UnitOfMeasureSymbol { get; set; }
        public string ValidationPeriodId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
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
