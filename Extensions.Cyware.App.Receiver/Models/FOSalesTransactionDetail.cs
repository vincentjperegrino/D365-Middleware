using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class FOSalesTransactionDetail
    {
        public FOSalesTransactionDetail()
        {
        }

        public FOSalesTransactionDetail(string[] records)
        {
            this.TransactionDate = records[1];
            this.RequestedReceiptDate = records[2];
            this.RequestedShipDate = records[3];
            //this.TransactionNumber = $"R-{records[4]}-1";
            //this.ReceiptNumber = $"R-{records[5]}-1";
            this.TransactionNumber = records[4];
            this.ReceiptNumber = records[5];
            this.Terminal = records[6];
            this.OperatingUnitNumber = records[7];
            this.Warehouse = records[8];
            this.CustomerAccount = records[9];
            this.LineNumber = records[10];
            this.ItemId = records[11];
            this.NetPrice = records[12];
            this.NetAmount = records[13];
            this.NetAmountInclusiveTax = records[14];
            this.StandardNetPrice = records[15];
            this.Quantity = records[16];
            this.Price = records[17];
            this.UnitPrice = records[18];
            this.LinePercentageDiscount = records[19];
            this.TotalDiscountPercentage = records[20];
            this.LineDiscount = records[21];
            this.LineManualDiscountAmount = records[22];
            this.LineManualDiscountPercentage = records[23];
            this.TotalDiscount = records[24];
            this.ModeOfDelivery = records[25];
            this.Unit = records[26];
            this.dataAreaId = records[27];
            this.SiteId = records[28];
            this.Currency = records[29];
            this.TransactionStatus = records[30];
            this.IsLinkedProductNotOriginal = records[31];
            this.PriceInBarCode = records[32];
            this.SkipReports = records[33];
            this.TransactionCode = records[34];
            this.CustomerDiscount = records[35];
            this.CashDiscountAmount = records[36];
            this.LogisticsPostalAddressValidFrom = records[37];
            this.OriginalItemSalesTaxGroup = records[38];
            this.RetailEmailAddressContent = records[39];
            this.ItemSize = records[40];
            this.PickupStartTime = records[41];
            this.TaxExemptPriceInclusiveOriginalPrice = records[42];
            this.DiscountAmountWithoutTax = records[43];
            this.PriceGroups = records[44];
            this.RFIDTagId = records[45];
            this.VariantNumber = records[46];
            this.CategoryHierarchyName = records[47];
            this.FixedPriceCharges = records[48];
            this.TaxExemptPriceInclusiveReductionAmount = records[49];
            this.LotID = records[50];
            this.ReturnQuantity = records[51];
            this.OriginalPrice = records[52];
            this.ItemRelation = records[53];
            this.ItemConfigId = records[54];
            this.ReturnOperatingUnitNumber = records[55];
            this.InventoryStatus = records[56];
            this.GiftCard = records[57];
            this.ChannelListingID = records[58];
            this.ItemColor = records[59];
            this.IsPriceChange = records[60];
            this.BarCode = records[61];
            this.UnitQuantity = records[62];
            this.BusinessDate = records[63];
            this.ItemVersion = records[64];
            this.ReturnTerminal = records[65];
            this.IsWeightProduct = records[66];
            this.LogisticLocationId = records[67];
            this.IsScaleProduct = records[68];
            this.IsOriginalOfLinkedProductList = records[69];
            this.ItemStyle = records[70];
            this.ReasonCodeDiscount = records[71];
            this.CategoryName = records[72];
            this.TotalDiscountInfoCodeLineNum = records[73];
            this.KeyboardProductEntry = records[74];
            this.CancelledTransactionNumber = records[75];
            this.IsReturnNoSale = records[76];
            this.ElectronicDeliveryEmail = records[77];
            this.SalesTaxAmount = records[78];
            this.ReturnTransactionNumber = records[79];
            this.SalesTaxGroup = records[80];
            this.OriginalSalesTaxGroup = records[81];
            this.ShelfNumber = records[82];
            this.ItemSalesTaxGroup = records[83];
            this.SerialNumber = records[84];
            this.OfferNumber = records[85];
            this.SectionNumber = records[86];
            this.IsWeightManuallyEntered = records[87];
            this.DiscountAmountForPrinting = records[88];
            this.CustomerInvoiceDiscountAmount = records[89];
            this.ProductScanned = records[90];
            this.PeriodicDiscountPercentage = records[91];
            this.ReturnTrackingStatus = records[92];
            this.ReturnLineNumber = records[93];
            this.PeriodicDiscountAmount = records[94];
            this.PeriodicDiscountGroup = records[95];
            this.PickupEndTime = records[96];
        }

        public string dataAreaId { get; set; }
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public string LineNumber { get; set; }
        public string OperatingUnitNumber { get; set; }
        public string CustomerDiscount { get; set; }
        public string RequestedReceiptDate { get; set; }
        public string CashDiscountAmount { get; set; }
        public string TransactionDate { get; set; }
        public string OriginalItemSalesTaxGroup { get; set; }
        public string SiteId { get; set; }
        public string RetailEmailAddressContent { get; set; }
        public string ItemSize { get; set; }
        public string ModeOfDelivery { get; set; }
        public string SkipReports { get; set; }
        public string Warehouse { get; set; }
        public string PickupStartTime { get; set; }
        public string TotalDiscount { get; set; }
        public string TaxExemptPriceInclusiveOriginalPrice { get; set; }
        public string IsLinkedProductNotOriginal { get; set; }
        public string DiscountAmountWithoutTax { get; set; }
        public string NetPrice { get; set; }
        public string PriceGroups { get; set; }
        public string RFIDTagId { get; set; }
        public string VariantNumber { get; set; }
        public string CategoryHierarchyName { get; set; }
        public string TransactionCode { get; set; }
        public string TotalDiscountPercentage { get; set; }
        public string FixedPriceCharges { get; set; }
        public string Unit { get; set; }
        public string TaxExemptPriceInclusiveReductionAmount { get; set; }
        public string PriceInBarCode { get; set; }
        public string LotID { get; set; }
        public string ReturnQuantity { get; set; }
        public string CustomerAccount { get; set; }
        public string OriginalPrice { get; set; }
        public string ItemRelation { get; set; }
        public string RequestedShipDate { get; set; }
        public string TransactionStatus { get; set; }
        public string ItemConfigId { get; set; }
        public string Currency { get; set; }
        public string ReturnOperatingUnitNumber { get; set; }
        public string LineDiscount { get; set; }
        public string NetAmountInclusiveTax { get; set; }
        public string InventoryStatus { get; set; }
        public string LineManualDiscountPercentage { get; set; }
        public string GiftCard { get; set; }
        public string ChannelListingID { get; set; }
        public string NetAmount { get; set; }
        public string ItemColor { get; set; }
        public string IsPriceChange { get; set; }
        public string BarCode { get; set; }
        public string UnitQuantity { get; set; }
        public string LineManualDiscountAmount { get; set; }
        public string StandardNetPrice { get; set; }
        public string ItemVersion { get; set; }
        public string ReturnTerminal { get; set; }
        public string IsWeightProduct { get; set; }
        public string LogisticLocationId { get; set; }
        public string IsScaleProduct { get; set; }
        public string IsOriginalOfLinkedProductList { get; set; }
        public string ItemStyle { get; set; }
        public string ReceiptNumber { get; set; }
        public string LinePercentageDiscount { get; set; }
        public string ReasonCodeDiscount { get; set; }
        public string CategoryName { get; set; }
        public string TotalDiscountInfoCodeLineNum { get; set; }
        public string KeyboardProductEntry { get; set; }
        public string CancelledTransactionNumber { get; set; }
        public string IsReturnNoSale { get; set; }
        public string ElectronicDeliveryEmail { get; set; }
        public string ItemId { get; set; }
        public string SalesTaxAmount { get; set; }
        public string ReturnTransactionNumber { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string UnitPrice { get; set; }
        public string SalesTaxGroup { get; set; }
        public string OriginalSalesTaxGroup { get; set; }
        public string IsLineDiscounted { get; set; }
        public string ShelfNumber { get; set; }
        public string ItemSalesTaxGroup { get; set; }
        public string SerialNumber { get; set; }
        public string OfferNumber { get; set; }
        public string CostAmount { get; set; }
        public string SectionNumber { get; set; }
        public string IsWeightManuallyEntered { get; set; }
        public string DiscountAmountForPrinting { get; set; }
        public string CustomerInvoiceDiscountAmount { get; set; }
        public string ProductScanned { get; set; }
        public string PeriodicDiscountPercentage { get; set; }
        public string ReturnTrackingStatus { get; set; }
        public string ReturnLineNumber { get; set; }
        public string PeriodicDiscountAmount { get; set; }
        public string PeriodicDiscountGroup { get; set; }
        public string PickupEndTime { get; set; }
        public string BusinessDate { get; set; }
        public string LogisticsPostalAddressValidFrom { get; set; }
    }
}
