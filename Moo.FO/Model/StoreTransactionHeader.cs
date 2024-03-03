using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Moo.FO.Model
{
    public class StoreTransactionHeader
    {
        [JsonProperty("dataAreaId")]
        public string dataAreaId { get; set; }

        [JsonProperty("TransactionNumber")]
        public string TransactionNumber { get; set; }

        [JsonProperty("OperatingUnitNumber")]
        public string OperatingUnitNumber { get; set; }

        [JsonProperty("Terminal")]
        public string Terminal { get; set; }

        [JsonProperty("Shift")]
        public string Shift { get; set; }

        [JsonProperty("IsTaxExemptedForPriceInclusive")]
        public string IsTaxExemptedForPriceInclusive { get; set; }

        [JsonProperty("BatchID")]
        public int BatchID { get; set; }

        [JsonProperty("LogisticsPostalZipCode")]
        public string LogisticsPostalZipCode { get; set; }

        [JsonProperty("RreceiptId")]
        public string RreceiptId { get; set; }

        [JsonProperty("DiscountAmount")]
        public decimal DiscountAmount { get; set; }

        [JsonProperty("NetPrice")]
        public decimal NetPrice { get; set; }

        [JsonProperty("TotalManualDiscountPercentage")]
        public decimal TotalManualDiscountPercentage { get; set; }

        [JsonProperty("CustomerAccount")]
        public string CustomerAccount { get; set; }

        [JsonProperty("TransactionOrderType")]
        public string TransactionOrderType { get; set; }

        [JsonProperty("CostAmount")]
        public decimal CostAmount { get; set; }

        [JsonProperty("AmountPostedToAccount")]
        public decimal AmountPostedToAccount { get; set; }

        [JsonProperty("ChannelReferenceId")]
        public string ChannelReferenceId { get; set; }

        [JsonProperty("TransactionDate")]
        public string TransactionDate { get; set; }

        [JsonProperty("LogisticPostalAddressValidTo")]
        public string LogisticPostalAddressValidTo { get; set; }

        [JsonProperty("NumberOfItemLines")]
        public int NumberOfItemLines { get; set; }

        [JsonProperty("PaymentAmount")]
        public decimal PaymentAmount { get; set; }

        [JsonProperty("TransactionStatus")]
        public string TransactionStatus { get; set; }

        [JsonProperty("NumberOfPaymentLines")]
        public int NumberOfPaymentLines { get; set; }

        [JsonProperty("Comment")]
        public string Comment { get; set; }

        [JsonProperty("Warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        [JsonProperty("ToAccount")]
        public string ToAccount { get; set; }

        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("GiftCardBalance")]
        public decimal GiftCardBalance { get; set; }

        [JsonProperty("ShippingDateRequested")]
        public string ShippingDateRequested { get; set; }

        [JsonProperty("LogisticsPostalState")]
        public string LogisticsPostalState { get; set; }

        [JsonProperty("SiteId")]
        public string SiteId { get; set; }

        [JsonProperty("IsTaxIncludedInPrice")]
        public string IsTaxIncludedInPrice { get; set; }

        [JsonProperty("PostAsShipment")]
        public string PostAsShipment { get; set; }

        [JsonProperty("TaxCalculationType")]
        public string TaxCalculationType { get; set; }
        [JsonPropertyName("SalesOrderId")]
        public string SalesOrderId { get; set; }

        [JsonPropertyName("GiftCardIdMasked")]
        public string GiftCardIdMasked { get; set; }

        [JsonPropertyName("ItemsPosted")]
        public string ItemsPosted { get; set; }

        [JsonPropertyName("RetailTransactionAggregationFieldList")]
        public int RetailTransactionAggregationFieldList { get; set; }

        [JsonPropertyName("LoyaltyCardId")]
        public string LoyaltyCardId { get; set; }

        [JsonPropertyName("NetAmount")]
        public decimal NetAmount { get; set; }

        [JsonPropertyName("LogisticsPostalStreet")]
        public string LogisticsPostalStreet { get; set; }

        [JsonPropertyName("LogisticsPostalCity")]
        public string LogisticsPostalCity { get; set; }

        [JsonPropertyName("InfocodeDiscountGroup")]
        public string InfocodeDiscountGroup { get; set; }

        [JsonPropertyName("DeliveryMode")]
        public string DeliveryMode { get; set; }

        [JsonPropertyName("Staff")]
        public string Staff { get; set; }

        [JsonPropertyName("CustomerDiscountAmount")]
        public decimal CustomerDiscountAmount { get; set; }

        [JsonPropertyName("SalesPaymentDifference")]
        public decimal SalesPaymentDifference { get; set; }

        [JsonPropertyName("SuspendedTransactionId")]
        public string SuspendedTransactionId { get; set; }

        [JsonPropertyName("ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        [JsonPropertyName("SalesInvoiceAmount")]
        public decimal SalesInvoiceAmount { get; set; }

        [JsonPropertyName("RefundReceiptId")]
        public string RefundReceiptId { get; set; }

        [JsonPropertyName("BatchTerminalId")]
        public string BatchTerminalId { get; set; }

        [JsonPropertyName("TotalDiscountAmount")]
        public decimal TotalDiscountAmount { get; set; }

        [JsonPropertyName("InvoiceId")]
        public string InvoiceId { get; set; }

        [JsonPropertyName("DiscountAmountWithoutTax")]
        public decimal DiscountAmountWithoutTax { get; set; }

        [JsonPropertyName("SaleOnAccount")]
        public string SaleOnAccount { get; set; }

        [JsonPropertyName("IncomeExpenseAmount")]
        public decimal IncomeExpenseAmount { get; set; }

        [JsonPropertyName("NumberOfItems")]
        public int NumberOfItems { get; set; }

        [JsonPropertyName("GrossAmount")]
        public decimal GrossAmount { get; set; }

        [JsonPropertyName("LogisticsLocationId")]
        public string LogisticsLocationId { get; set; }

        [JsonPropertyName("CreatedOnPosTerminal")]
        public string CreatedOnPosTerminal { get; set; }

        [JsonPropertyName("TransactionTime")]
        public int TransactionTime { get; set; }

        [JsonPropertyName("LanguageId")]
        public string LanguageId { get; set; }

        [JsonPropertyName("CreatedOffline")]
        public string CreatedOffline { get; set; }

        [JsonPropertyName("GiftCardIssueAmount")]
        public decimal GiftCardIssueAmount { get; set; }

        [JsonPropertyName("Currency")]
        public string Currency { get; set; }

        [JsonPropertyName("SaleIsReturnSale")]
        public string SaleIsReturnSale { get; set; }

        [JsonPropertyName("LogisticsPostalCounty")]
        public string LogisticsPostalCounty { get; set; }

        [JsonPropertyName("GiftCardHistoryDetails")]
        public string GiftCardHistoryDetails { get; set; }

        [JsonPropertyName("SalesOrderAmount")]
        public decimal SalesOrderAmount { get; set; }

        [JsonPropertyName("TotalManualDiscountAmount")]
        public decimal TotalManualDiscountAmount { get; set; }

        [JsonPropertyName("GiftCardActiveFrom")]
        public string GiftCardActiveFrom { get; set; }

        [JsonPropertyName("GiftCardExpireDate")]
        public string GiftCardExpireDate { get; set; }

        [JsonPropertyName("businessDate")]
        public string businessDate { get; set; }

        [JsonPropertyName("BeginDateTime")]
        public string BeginDateTime { get; set; }

        [JsonPropertyName("LogisticsPostalAddressValidFrom")]
        public string LogisticsPostalAddressValidFrom { get; set; }

        //[JsonProperty("Channel")]
        //public int Channel { get; set; }

        //[JsonProperty("LogisticsPostalAddress")]
        //public int LogisticsPostalAddress { get; set; }

        //[JsonPropertyName("ModifiedBy")]
        //public string ModifiedBy { get; set; }

        //[JsonPropertyName("CreatedBy")]
        //public string CreatedBy { get; set; }

        //[JsonPropertyName("ModifiedDateTime")]
        //public DateTimeOffset ModifiedDateTime { get; set; }

        //[JsonPropertyName("CreatedDateTime")]
        //public DateTimeOffset CreatedDateTime { get; set; }

        //[JsonPropertyName("RecVersion")]
        //public int RecVersion { get; set; }

        //[JsonPropertyName("Partition")]
        //public int Partition { get; set; }

        //[JsonPropertyName("RecID")]
        //public int RecID { get; set; }

        //[JsonPropertyName("LogisticsPostalAddressLocation")]
        //public int LogisticsPostalAddressLocation { get; set; }

        //[JsonPropertyName("LogisticsPostalAddressValidTo")]
        //public DateTimeOffset LogisticsPostalAddressValidTo { get; set; }

        //[JsonPropertyName("RecVersion2")]
        //public int RecVersion2 { get; set; }

        //[JsonPropertyName("Partition2")]
        //public int Partition2 { get; set; }

        //[JsonPropertyName("RecID2")]
        //public int RecID2 { get; set; }

        //[JsonPropertyName("RetailChannelTableOMOperatingUnitID")]
        //public int RetailChannelTableOMOperatingUnitID { get; set; }

        //[JsonPropertyName("RecVersion3")]
        //public int RecVersion3 { get; set; }

        //[JsonPropertyName("Partition3")]
        //public int Partition3 { get; set; }

        //[JsonPropertyName("RecID3")]
        //public int RecID3 { get; set; }

        //[JsonPropertyName("RecVersion4")]
        //public int RecVersion4 { get; set; }

        //[JsonPropertyName("Partition4")]
        //public int Partition4 { get; set; }

        //[JsonPropertyName("RecID4")]
        //public int RecID4 { get; set; }

        //[JsonPropertyName("RecVersion5")]
        //public int RecVersion5 { get; set; }

        //[JsonPropertyName("Partition5")]
        //public int Partition5 { get; set; }

        //[JsonPropertyName("RecID5")]
        //public int RecID5 { get; set; }
    }
}
