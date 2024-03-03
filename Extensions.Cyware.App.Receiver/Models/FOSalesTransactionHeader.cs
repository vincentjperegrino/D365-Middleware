using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class FOSalesTransactionHeader
    {

        public FOSalesTransactionHeader()
        {
        }

        public FOSalesTransactionHeader(string[] records)
        {
            this.TransactionDate = records[1];
            this.ShippingDateRequested = records[2];
            this.dataAreaId = records[3];
            //this.TransactionNumber = $"R-{records[4]}-1";
            //this.RreceiptId = $"R-{records[5]}-1";
            this.TransactionNumber = records[4];
            this.RreceiptId = records[5];
            this.OperatingUnitNumber = records[6];
            this.Terminal = records[7];
            this.ChannelReferenceId = records[8];
            this.Warehouse = records[9];
            this.SiteId = records[10];
            this.CustomerAccount = records[11];
            this.TransactionOrderType = records[12];
            this.TransactionType = records[13];
            this.TransactionStatus = records[14];
            this.IsTaxExemptedForPriceInclusive = records[15];
            this.LogisticsPostalZipCode = records[16];
            this.DiscountAmount = records[17];
            this.NetPrice = records[18];
            this.CostAmount = records[19];
            this.AmountPostedToAccount = records[20];
            this.PaymentAmount = records[21];
            this.SalesInvoiceAmount = records[22];
            this.NetAmount = records[23];
            this.GrossAmount = records[24];
            this.TotalManualDiscountPercentage = records[25];
            this.TotalDiscountAmount = records[26];
            this.TotalManualDiscountAmount = records[27];
            this.DiscountAmountWithoutTax = records[28];
            this.Currency = records[29];
            this.IsTaxIncludedInPrice = records[30];
            this.PostAsShipment = records[31];
            this.TaxCalculationType = records[32];
            this.NumberOfItemLines = records[33];
            this.NumberOfPaymentLines = records[34];
            this.SaleOnAccount = records[35];
            this.ToAccount = records[36];
            this.SaleIsReturnSale = records[37];
            this.CreatedOffline = records[38];
            this.ItemsPosted = records[39];
            this.Shift = records[40];
            this.BatchID = records[41];
            this.LogisticPostalAddressValidTo = records[42];
            this.Comment = records[43];
            this.CustomerName = records[44];
            this.GiftCardActiveFrom = records[45];
            this.GiftCardBalance = records[46];
            this.LogisticsPostalState = records[47];
            this.SalesOrderId = records[48];
            this.GiftCardIdMasked = records[49];
            this.RetailTransactionAggregationFieldList = records[50];
            this.GiftCardExpireDate = records[51];
            this.LoyaltyCardId = records[52];
            this.LogisticsPostalStreet = records[53];
            this.LogisticsPostalCity = records[54];
            this.InfocodeDiscountGroup = records[55];
            this.DeliveryMode = records[56];
            this.Staff = records[57];
            this.CustomerDiscountAmount = records[58];
            this.businessDate = records[59];
            this.SalesPaymentDifference = records[60];
            this.SuspendedTransactionId = records[61];
            this.ExchangeRate = records[62];
            this.RefundReceiptId = records[63];
            this.BatchTerminalId = records[64];
            this.BeginDateTime = records[65];
            this.InvoiceId = records[66];
            this.IncomeExpenseAmount = records[67];
            this.NumberOfItems = records[68];
            this.LogisticsLocationId = records[69];
            this.CreatedOnPosTerminal = records[70];
            this.TransactionTime = records[71];
            this.LanguageId = records[72];
            this.GiftCardIssueAmount = records[73];
            this.LogisticsPostalAddressValidFrom = records[74];
            this.LogisticsPostalCounty = records[75];
            this.GiftCardHistoryDetails = records[76];
            this.SalesOrderAmount = records[77];
        }

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
        public string BatchID { get; set; }

        [JsonProperty("LogisticsPostalZipCode")]
        public string LogisticsPostalZipCode { get; set; }

        [JsonProperty("RreceiptId")]
        public string RreceiptId { get; set; }

        [JsonProperty("DiscountAmount")]
        public string DiscountAmount { get; set; }

        [JsonProperty("NetPrice")]
        public string NetPrice { get; set; }

        [JsonProperty("TotalManualDiscountPercentage")]
        public string TotalManualDiscountPercentage { get; set; }

        [JsonProperty("CustomerAccount")]
        public string CustomerAccount { get; set; }

        [JsonProperty("TransactionOrderType")]
        public string TransactionOrderType { get; set; }

        [JsonProperty("CostAmount")]
        public string CostAmount { get; set; }

        [JsonProperty("AmountPostedToAccount")]
        public string AmountPostedToAccount { get; set; }

        [JsonProperty("ChannelReferenceId")]
        public string ChannelReferenceId { get; set; }

        [JsonProperty("TransactionDate")]
        public string TransactionDate { get; set; }

        [JsonProperty("LogisticPostalAddressValidTo")]
        public string LogisticPostalAddressValidTo { get; set; }

        [JsonProperty("NumberOfItemLines")]
        public string NumberOfItemLines { get; set; }

        [JsonProperty("PaymentAmount")]
        public string PaymentAmount { get; set; }

        [JsonProperty("TransactionStatus")]
        public string TransactionStatus { get; set; }

        [JsonProperty("NumberOfPaymentLines")]
        public string NumberOfPaymentLines { get; set; }

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
        public string GiftCardBalance { get; set; }

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
        public string RetailTransactionAggregationFieldList { get; set; }

        [JsonPropertyName("LoyaltyCardId")]
        public string LoyaltyCardId { get; set; }

        [JsonPropertyName("NetAmount")]
        public string NetAmount { get; set; }

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
        public string CustomerDiscountAmount { get; set; }

        [JsonPropertyName("SalesPaymentDifference")]
        public string SalesPaymentDifference { get; set; }

        [JsonPropertyName("SuspendedTransactionId")]
        public string SuspendedTransactionId { get; set; }

        [JsonPropertyName("ExchangeRate")]
        public string ExchangeRate { get; set; }

        [JsonPropertyName("SalesInvoiceAmount")]
        public string SalesInvoiceAmount { get; set; }

        [JsonPropertyName("RefundReceiptId")]
        public string RefundReceiptId { get; set; }

        [JsonPropertyName("BatchTerminalId")]
        public string BatchTerminalId { get; set; }

        [JsonPropertyName("TotalDiscountAmount")]
        public string TotalDiscountAmount { get; set; }

        [JsonPropertyName("InvoiceId")]
        public string InvoiceId { get; set; }

        [JsonPropertyName("DiscountAmountWithoutTax")]
        public string DiscountAmountWithoutTax { get; set; }

        [JsonPropertyName("SaleOnAccount")]
        public string SaleOnAccount { get; set; }

        [JsonPropertyName("IncomeExpenseAmount")]
        public string IncomeExpenseAmount { get; set; }

        [JsonPropertyName("NumberOfItems")]
        public string NumberOfItems { get; set; }

        [JsonPropertyName("GrossAmount")]
        public string GrossAmount { get; set; }

        [JsonPropertyName("LogisticsLocationId")]
        public string LogisticsLocationId { get; set; }

        [JsonPropertyName("CreatedOnPosTerminal")]
        public string CreatedOnPosTerminal { get; set; }

        [JsonPropertyName("TransactionTime")]
        public string TransactionTime { get; set; }

        [JsonPropertyName("LanguageId")]
        public string LanguageId { get; set; }

        [JsonPropertyName("CreatedOffline")]
        public string CreatedOffline { get; set; }

        [JsonPropertyName("GiftCardIssueAmount")]
        public string GiftCardIssueAmount { get; set; }

        [JsonPropertyName("Currency")]
        public string Currency { get; set; }

        [JsonPropertyName("SaleIsReturnSale")]
        public string SaleIsReturnSale { get; set; }

        [JsonPropertyName("LogisticsPostalCounty")]
        public string LogisticsPostalCounty { get; set; }

        [JsonPropertyName("GiftCardHistoryDetails")]
        public string GiftCardHistoryDetails { get; set; }

        [JsonPropertyName("SalesOrderAmount")]
        public string SalesOrderAmount { get; set; }

        [JsonPropertyName("TotalManualDiscountAmount")]
        public string TotalManualDiscountAmount { get; set; }

        [JsonProperty("LogisticsPostalAddress")]
        public string LogisticsPostalAddress { get; set; }

        [JsonPropertyName("GiftCardActiveFrom")]
        public string GiftCardActiveFrom { get; set; }

        [JsonPropertyName("GiftCardExpireDate")]
        public string GiftCardExpireDate { get; set; }

        [JsonPropertyName("BeginDateTime")]
        public string BeginDateTime { get; set; }

        [JsonPropertyName("businessDate")]
        public string businessDate { get; set; }

        [JsonPropertyName("LogisticsPostalAddressValidFrom")]
        public string LogisticsPostalAddressValidFrom { get; set; }
    }
}
