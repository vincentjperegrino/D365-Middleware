using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model
{
    public class StoreBase
    {
        public string RetailChannelId { get; set; }
        public string WarehouseId { get; set; }
        public string LiveDatabaseConnectionProfileName { get; set; }
        public string StartAmountCalculation { get; set; }
        public string ChannelProfileName { get; set; }
        public string Phone { get; set; }
        public string UseCustomerBasedTaxExemption { get; set; }
        public string GeneratesItemLabels { get; set; }
        public string TaxOverrideGroupCodeLegalEntity { get; set; }
        public int NumberOfTopOrBottomLines { get; set; }
        public string InventoryLookup { get; set; }
        public string TaxGroupCode { get; set; }
        public string PriceIncludesSalesTax { get; set; }
        public string TermsOfPayment { get; set; }
        public string WarehouseLegalEntity { get; set; }
        public string GeneratesShelfLabels { get; set; }
        public string ClosingMethod { get; set; }
        public string ElectronicFundsTransferStoreNumber { get; set; }
        public string OneStatementPerDay { get; set; }
        public string PurchaseOrderItemFilter { get; set; }
        public string FunctionalityProfile { get; set; }
        public int OpenFrom { get; set; }
        public string ServiceChargePrompt { get; set; }
        public string PaymentMethodToRemoveOrAdd { get; set; }
        public string ProductCategoryHierarchyName { get; set; }
        public string EventNotificationProfileId { get; set; }
        public string TransactionServiceProfile { get; set; }
        public string TenderDeclarationCalculation { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string UseDefaultCustomerAccount { get; set; }
        public string BankDropCalculation { get; set; }
        public string ChannelTimeZone { get; set; }
        public string UseCustomerBasedTax { get; set; }
        public decimal MaxTransactionDifferenceAmount { get; set; }
        public string Currency { get; set; }
        public decimal MaximumPostingDifference { get; set; }
        public int MaxRoundingTaxAmount { get; set; }
        public string UseDestinationBasedTax { get; set; }
        public string StoreNumber { get; set; }
        public string SQLServerName { get; set; }
        public string OperatingUnitNumber { get; set; }
        public string DefaultCustomerAccount { get; set; }
        public string LayoutId { get; set; }
        public string ChannelTimeZoneInfoId { get; set; }
        public decimal ServiceChargePercentage { get; set; }
        public string TaxOverrideGroupCode { get; set; }
        public string PaymentMethodName { get; set; }
        public string RoundingTaxAccount { get; set; }
        public string HideTrainingMode { get; set; }
        public string TaxGroupLegalEntity { get; set; }
        public decimal StoreArea { get; set; }
        public string OfflineProfileName { get; set; }
        public string CreateLabelsForZeroPrice { get; set; }
        public string SeparateStatementPerStaffTerminal { get; set; }
        public string StatementMethod { get; set; }
        public string StatementPostAsBusinessDay { get; set; }
        public int MaxRoundingAmount { get; set; }
        public string DefaultDimensionDisplayValue { get; set; }
        public string CultureName { get; set; }
        public string RoundingAccountLedgerDimensionDisplayValue { get; set; }
        public decimal MaxShiftDifferenceAmount { get; set; }
        public string DatabaseName { get; set; }
        public int EndOfBusinessDay { get; set; }
        public string ProductNumberOnReceipt { get; set; }
        public int OpenTo { get; set; }
        public string WarehouseIdForCustomerOrder { get; set; }
        public string OperatingUnitPartyNumber { get; set; }
        public string DefaultCustomerLegalEntity { get; set; }
        public string DisplayTaxPerTaxComponent { get; set; }
        public int MaximumTextLengthOnReceipt { get; set; }

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
