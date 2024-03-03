using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using Microsoft.Azure.Amqp.Framing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model.DTO.Batch
{
    public class FO_StoreTransactionLinesDTO : StoreTransactionLines
    {
        private const string dateMinValue = "1900-01-01T00:00:00Z";
        private static string errorPrefix = "";
        public FO_StoreTransactionLinesDTO(FOSalesTransactionDetail line, FOSalesTransactionHeader header, D365FOConfig config)
        {
            Validate(line);
            try
            {
                string storeNumber = "";
                string register = line.Terminal;
                if (line.Terminal.Contains('-'))
                {
                    string[] storeAndRegister = register.Split('-');
                    storeNumber = storeAndRegister[0];
                    register = storeAndRegister[1];
                }
                string warehouseId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.WarehouseId).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, warehouseId is null or empty");
                string operatingUnitNumber = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, OperatingUnitNumber is null or empty");
                string CYWAREcustomerAccount = !String.IsNullOrEmpty(header.CustomerAccount) ? header.CustomerAccount : config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, CustomerAccount is null or empty");
                if (CYWAREcustomerAccount.Contains('|'))
                {
                    // Split CYWAREcustomerAccount by "|" and take the first part
                    CYWAREcustomerAccount = CYWAREcustomerAccount.Split('|')[0];
                }
                string customerAccount = config.Customers.Any(c => c.CustomerAccount == CYWAREcustomerAccount) ? CYWAREcustomerAccount : config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, CustomerAccount is null or empty");

                this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, dataAreaId is null or empty");
                this.TransactionNumber = line.TransactionNumber;
                this.Terminal = config.Terminals.Where(rt => rt.StoreNumber == storeNumber && rt.Name == register).Select(rt => rt.TerminalNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, Terminal is null or empty");
                this.ReceiptNumber = line.ReceiptNumber;
                this.CustomerAccount = customerAccount;
                this.OperatingUnitNumber = operatingUnitNumber;
                this.LineNumber = int.Parse(line.LineNumber);
                this.CustomerDiscount = Decimal.Parse(line.CustomerDiscount);
                this.CashDiscountAmount = Decimal.Parse(line.CashDiscountAmount);
                this.SiteId = config.Warehouses.Where(w => w.WarehouseId == warehouseId).Select(rs => rs.OperationalSiteId).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, SiteId is null or empty");
                this.Warehouse = warehouseId;
                this.NetPrice = Decimal.Parse(line.NetPrice);
                this.TransactionCode = "ItemOnFile";
                this.Unit = line.Unit;
                this.PriceInBarCode = "No";
                this.TransactionStatus = "Posted";
                this.Currency = line.Currency;
                this.InventoryStatus = "None";
                this.GiftCard = "No";
                this.NetAmount = Decimal.Parse(line.NetAmount) * -1;
                this.IsPriceChange = "No";
                this.IsWeightProduct = "No";
                this.IsScaleProduct = "No";
                this.IsOriginalOfLinkedProductList = "No";
                this.KeyboardProductEntry = "No";
                this.IsReturnNoSale = line.IsReturnNoSale;
                this.ItemId = line.ItemId;
                this.Quantity = Decimal.Parse(line.Quantity) * -1;
                this.UnitPrice = Decimal.Parse(line.UnitPrice);
                this.IsWeightManuallyEntered = "No";
                this.ProductScanned = line.ProductScanned;
                this.ReturnTrackingStatus = "None";
                this.NetAmountInclusiveTax = Decimal.Parse(line.NetAmountInclusiveTax) * -1;
                this.Price = Decimal.Parse(line.Price);
                this.LinePercentageDiscount = Decimal.Parse(line.LinePercentageDiscount);
                this.TotalDiscountPercentage = Decimal.Parse(line.TotalDiscountPercentage);
                this.LineDiscount = Decimal.Parse(line.LineDiscount);
                this.LineManualDiscountAmount = Decimal.Parse(line.LineManualDiscountAmount);
                this.LineManualDiscountPercentage = Decimal.Parse(line.LineManualDiscountPercentage);
                this.TotalDiscount = Decimal.Parse(line.TotalDiscount);
                this.ModeOfDelivery = config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.DeliveryMode).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, ModeOfDelivery is null or empty");
                this.IsLinkedProductNotOriginal = "No";
                this.SkipReports = "No";
                this.OriginalItemSalesTaxGroup = "";
                this.RetailEmailAddressContent = "";
                this.ItemSize = "";
                this.PickupStartTime = 0;
                this.TaxExemptPriceInclusiveOriginalPrice = 0;
                this.DiscountAmountWithoutTax = Decimal.Parse(line.DiscountAmountWithoutTax);
                this.PriceGroups = "";
                this.RFIDTagId = "";
                this.VariantNumber = "";
                this.CategoryHierarchyName = "";
                this.FixedPriceCharges = !String.IsNullOrEmpty(line.FixedPriceCharges) ? Decimal.Parse(line.FixedPriceCharges) : 0;
                this.TaxExemptPriceInclusiveReductionAmount = Decimal.Parse(line.TaxExemptPriceInclusiveReductionAmount);
                this.LotID = "";
                this.ReturnQuantity = !String.IsNullOrEmpty(line.ReturnQuantity) ? Decimal.Parse(line.ReturnQuantity) : 0;
                this.OriginalPrice = Decimal.Parse(line.OriginalPrice);
                this.ItemRelation = "";
                this.ItemConfigId = "";
                this.ReturnOperatingUnitNumber = string.Equals(header.SaleIsReturnSale, "Yes", StringComparison.OrdinalIgnoreCase) ? operatingUnitNumber : "";
                this.ChannelListingID = "";
                this.ItemColor = "";
                this.BarCode = line.BarCode;
                this.ItemVersion = "";
                this.ReturnTerminal = !string.IsNullOrEmpty(line.ReturnTerminal)
                    ? line.ReturnTerminal.PadLeft(6, '0')
                    : line.ReturnTerminal;
                this.LogisticLocationId = "";
                this.ItemStyle = "";
                this.ReasonCodeDiscount = 0;
                this.CategoryName = "";
                this.TotalDiscountInfoCodeLineNum = 0;
                this.CancelledTransactionNumber = line.CancelledTransactionNumber;
                this.ElectronicDeliveryEmail = "";
                this.SalesTaxAmount = Decimal.Parse(line.SalesTaxAmount);
                this.ReturnTransactionNumber = line.ReturnTransactionNumber;
                this.SalesTaxGroup = config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.SalesTaxGroup).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, SalesTaxGroup is null or empty");
                this.OriginalSalesTaxGroup = "";
                this.ShelfNumber = "";
                this.ItemSalesTaxGroup = line.ItemSalesTaxGroup;
                this.SerialNumber = "";
                this.OfferNumber = "";
                this.SectionNumber = "";
                this.DiscountAmountForPrinting = Decimal.Parse(line.DiscountAmountForPrinting);
                this.CustomerInvoiceDiscountAmount = Decimal.Parse(line.CustomerInvoiceDiscountAmount);
                this.PeriodicDiscountPercentage = Decimal.Parse(line.PeriodicDiscountPercentage);
                this.ReturnLineNumber = !String.IsNullOrEmpty(line.ReturnLineNumber) ? int.Parse(line.ReturnLineNumber) : 0;
                this.PeriodicDiscountAmount = Decimal.Parse(line.PeriodicDiscountAmount);
                this.PeriodicDiscountGroup = line.PeriodicDiscountGroup;
                this.PickupEndTime = 0;
                if (DateTimeOffset.TryParseExact(line.RequestedReceiptDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset receiptDate))
                {
                    this.RequestedReceiptDate = receiptDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    this.RequestedReceiptDate = DateTimeOffset.Parse(line.RequestedReceiptDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                if (DateTimeOffset.TryParseExact(line.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset transDate))
                {
                    this.TransactionDate = transDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    this.TransactionDate = DateTimeOffset.Parse(line.TransactionDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                if (DateTimeOffset.TryParseExact(line.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset shipDate))
                {
                    this.RequestedShipDate = shipDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    this.RequestedShipDate = DateTimeOffset.Parse(line.TransactionDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                this.LogisticsPostalAddressValidFrom = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                this.BusinessDate = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            catch (Exception ex)
            {
                throw new Exception($"{errorPrefix}, {ex.Message}");
            }
        }

        private static void Validate(FOSalesTransactionDetail detail)
        {
            if (string.IsNullOrEmpty(detail.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }
            errorPrefix = $"Lines Transaction Number: {detail.TransactionNumber}";

            if (string.IsNullOrEmpty(detail.Terminal))
            {
                throw new ArgumentException("Terminal", $"{errorPrefix}, Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineNumber))
            {
                throw new ArgumentException("LineNumber", $"{errorPrefix}, LineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.CustomerDiscount))
            {
                throw new ArgumentException("CustomerDiscount", $"{errorPrefix}, CustomerDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.RequestedReceiptDate))
            {
                throw new ArgumentException("RequestedReceiptDate", $"{errorPrefix}, RequestedReceiptDate is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TransactionDate))
            {
                throw new ArgumentException("TransactionDate", $"{errorPrefix}, TransactionDate is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TotalDiscount))
            {
                throw new ArgumentException("TotalDiscount", $"{errorPrefix}, TotalDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.NetPrice))
            {
                throw new ArgumentException("NetPrice", $"{errorPrefix}, NetPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TotalDiscountPercentage))
            {
                throw new ArgumentException("TotalDiscountPercentage", $"{errorPrefix}, TotalDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Unit))
            {
                throw new ArgumentException("Unit", $"{errorPrefix}, Unit is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.PriceInBarCode))
            {
                throw new ArgumentException("PriceInBarCode", $"{errorPrefix}, PriceInBarCode is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ReturnQuantity))
            {
                throw new ArgumentException("ReturnQuantity", $"{errorPrefix}, ReturnQuantity is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.OriginalPrice))
            {
                throw new ArgumentException("OriginalPrice", $"{errorPrefix}, OriginalPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Currency))
            {
                throw new ArgumentException("Currency", $"{errorPrefix}, Currency is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineDiscount))
            {
                throw new ArgumentException("LineDiscount", $"{errorPrefix}, LineDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.NetAmountInclusiveTax))
            {
                throw new ArgumentException("NetAmountInclusiveTax", $"{errorPrefix}, NetAmountInclusiveTax is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineManualDiscountPercentage))
            {
                throw new ArgumentException("LineManualDiscountPercentage", $"{errorPrefix}, LineManualDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.NetAmount))
            {
                throw new ArgumentException("NetAmount", $"{errorPrefix}, NetAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.UnitQuantity))
            {
                throw new ArgumentException("UnitQuantity", $"{errorPrefix}, UnitQuantity is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineManualDiscountAmount))
            {
                throw new ArgumentException("LineManualDiscountAmount", $"{errorPrefix}, LineManualDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ReceiptNumber))
            {
                throw new ArgumentException("ReceiptNumber", $"{errorPrefix}, ReceiptNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LinePercentageDiscount))
            {
                throw new ArgumentException("LinePercentageDiscount", $"{errorPrefix}, LinePercentageDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.KeyboardProductEntry))
            {
                throw new ArgumentException("KeyboardProductEntry", $"{errorPrefix}, KeyboardProductEntry is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.IsReturnNoSale))
            {
                throw new ArgumentException("IsReturnNoSale", $"{errorPrefix}, IsReturnNoSale is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ItemId))
            {
                throw new ArgumentException("ItemId", $"{errorPrefix}, ItemId is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.SalesTaxAmount))
            {
                throw new ArgumentException("SalesTaxAmount", $"{errorPrefix}, SalesTaxAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Quantity))
            {
                throw new ArgumentException("Quantity", $"{errorPrefix}, Quantity is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Price))
            {
                throw new ArgumentException("Price", $"{errorPrefix}, Price is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Unit))
            {
                throw new ArgumentException("Unit", $"{errorPrefix}, Unit is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.UnitPrice))
            {
                throw new ArgumentException("UnitPrice", $"{errorPrefix}, UnitPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ItemSalesTaxGroup))
            {
                throw new ArgumentException("ItemSalesTaxGroup", $"{errorPrefix}, ItemSalesTaxGroup is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.DiscountAmountForPrinting))
            {
                throw new ArgumentException("DiscountAmountForPrinting", $"{errorPrefix}, DiscountAmountForPrinting is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.CustomerInvoiceDiscountAmount))
            {
                throw new ArgumentException("CustomerInvoiceDiscountAmount", $"{errorPrefix}, CustomerInvoiceDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ProductScanned))
            {
                throw new ArgumentException("ProductScanned", $"{errorPrefix}, ProductScanned is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.PeriodicDiscountPercentage))
            {
                throw new ArgumentException("PeriodicDiscountPercentage", $"{errorPrefix}, PeriodicDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.PeriodicDiscountAmount))
            {
                throw new ArgumentException("PeriodicDiscountAmount", $"{errorPrefix}, PeriodicDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.BusinessDate))
            {
                throw new ArgumentException("BusinessDate", $"{errorPrefix}, BusinessDate is null or empty.");
            }
        }

        public override string ToString()
        {
            // Using Newtonsoft.Json to serialize the object to JSON
            return JsonConvert.SerializeObject(this);
        }
    }
}
