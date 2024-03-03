using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using System.Globalization;

namespace Moo.FO.Model.DTO
{
    public class StoreTransactionLinesDTO : StoreTransactionLines
    {
        private const string dateMinValue = "1900-01-01T00:00:00Z";
        public StoreTransactionLinesDTO(FOSalesTransactionDetail transactionDetail, OrdersTransaction ordersTransaction)
        {
            Validate(transactionDetail);
            string storeNumber = ordersTransaction.Header.ChannelReferenceId;
            string warehouseId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.WarehouseId).FirstOrDefault() ?? throw new Exception("warehouseId is null or empty");
            string operatingUnitNumber = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception("OperatingUnitNumber is null or empty");
            string CYWAREcustomerAccount = !String.IsNullOrEmpty(ordersTransaction.Header.CustomerAccount) ? ordersTransaction.Header.CustomerAccount : ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception("CustomerAccount is null or empty");
            if (CYWAREcustomerAccount.Contains("|"))
            {
                // Split CYWAREcustomerAccount by "|" and take the first part
                CYWAREcustomerAccount = CYWAREcustomerAccount.Split('|')[0];
            }
            string customerAccount = ordersTransaction.Config.Customers.Any(c => c.CustomerAccount == CYWAREcustomerAccount) ? CYWAREcustomerAccount : ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception("CustomerAccount is null or empty");

            this.dataAreaId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception("dataAreaId is null or empty");
            this.TransactionNumber = transactionDetail.TransactionNumber;
            this.Terminal = ordersTransaction.Header.Terminal.Length < 6 ? ordersTransaction.Header.Terminal.PadLeft(6, '0') : ordersTransaction.Header.Terminal;
            this.ReceiptNumber = transactionDetail.ReceiptNumber;
            this.CustomerAccount = customerAccount;
            this.OperatingUnitNumber = operatingUnitNumber;
            this.LineNumber = int.Parse(transactionDetail.LineNumber); 
            this.CustomerDiscount = Decimal.Parse(transactionDetail.CustomerDiscount);
            this.CashDiscountAmount = Decimal.Parse(transactionDetail.CashDiscountAmount);
            this.SiteId = ordersTransaction.Config.Warehouses.Where(w => w.WarehouseId == warehouseId).Select(rs => rs.OperationalSiteId).FirstOrDefault() ?? throw new Exception("SiteId is null or empty");
            this.Warehouse = warehouseId;
            this.NetPrice = Decimal.Parse(transactionDetail.NetPrice);
            this.TransactionCode = "ItemOnFile";
            this.Unit = transactionDetail.Unit;
            this.PriceInBarCode = "No";
            this.TransactionStatus = "Posted";
            this.Currency = transactionDetail.Currency;
            this.InventoryStatus = "None";
            this.GiftCard = "No";
            this.NetAmount = Decimal.Parse(transactionDetail.NetAmount) * -1;
            this.IsPriceChange = "No";
            this.IsWeightProduct = "No";
            this.IsScaleProduct = "No";
            this.IsOriginalOfLinkedProductList = "No";
            this.KeyboardProductEntry = "No";
            this.IsReturnNoSale = transactionDetail.IsReturnNoSale;
            this.ItemId = transactionDetail.ItemId;
            this.Quantity = Decimal.Parse(transactionDetail.Quantity) * -1;
            this.UnitPrice = Decimal.Parse(transactionDetail.UnitPrice);
            this.IsWeightManuallyEntered = "No";
            this.ProductScanned = transactionDetail.ProductScanned;
            this.ReturnTrackingStatus = "None";
            this.NetAmountInclusiveTax = Decimal.Parse(transactionDetail.NetAmountInclusiveTax) * -1;
            this.Price = Decimal.Parse(transactionDetail.Price);
            this.LinePercentageDiscount = Decimal.Parse(transactionDetail.LinePercentageDiscount);
            this.TotalDiscountPercentage = Decimal.Parse(transactionDetail.TotalDiscountPercentage);
            this.LineDiscount = Decimal.Parse(transactionDetail.LineDiscount);
            this.LineManualDiscountAmount = Decimal.Parse(transactionDetail.LineManualDiscountAmount);
            this.LineManualDiscountPercentage = Decimal.Parse(transactionDetail.LineManualDiscountPercentage);
            this.TotalDiscount = Decimal.Parse(transactionDetail.TotalDiscount);
            this.ModeOfDelivery = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.DeliveryMode).FirstOrDefault() ?? throw new Exception("ModeOfDelivery is null or empty");
            this.IsLinkedProductNotOriginal = "No";
            this.SkipReports = "No";
            this.OriginalItemSalesTaxGroup = "";
            this.RetailEmailAddressContent = "";
            this.ItemSize = "";
            this.PickupStartTime = 0;
            this.TaxExemptPriceInclusiveOriginalPrice = 0;
            this.DiscountAmountWithoutTax = Decimal.Parse(transactionDetail.DiscountAmountWithoutTax);
            this.PriceGroups = "";
            this.RFIDTagId = "";
            this.VariantNumber = "";
            this.CategoryHierarchyName = "";
            this.FixedPriceCharges = !String.IsNullOrEmpty(transactionDetail.FixedPriceCharges) ? Decimal.Parse(transactionDetail.FixedPriceCharges) : 0;
            this.TaxExemptPriceInclusiveReductionAmount = Decimal.Parse(transactionDetail.TaxExemptPriceInclusiveReductionAmount);
            this.LotID = "";
            this.ReturnQuantity = !String.IsNullOrEmpty(transactionDetail.ReturnQuantity) ? Decimal.Parse(transactionDetail.ReturnQuantity) : 0;
            this.OriginalPrice = Decimal.Parse(transactionDetail.OriginalPrice);
            this.ItemRelation = "";
            this.ItemConfigId = "";
            this.ReturnOperatingUnitNumber = string.Equals(ordersTransaction.Header.SaleIsReturnSale, "Yes", StringComparison.OrdinalIgnoreCase) ? operatingUnitNumber : "";
            this.ChannelListingID = "";
            this.ItemColor = "";
            this.BarCode = transactionDetail.BarCode;
            this.ItemVersion = "";
            this.ReturnTerminal = !string.IsNullOrEmpty(transactionDetail.ReturnTerminal)
                ? transactionDetail.ReturnTerminal.PadLeft(6, '0')
                : transactionDetail.ReturnTerminal;
            this.LogisticLocationId = "";
            this.ItemStyle = "";
            this.ReasonCodeDiscount = 0;
            this.CategoryName = "";
            this.TotalDiscountInfoCodeLineNum = 0;
            this.CancelledTransactionNumber = transactionDetail.CancelledTransactionNumber;
            this.ElectronicDeliveryEmail = "";
            this.SalesTaxAmount = Decimal.Parse(transactionDetail.SalesTaxAmount);
            this.ReturnTransactionNumber = transactionDetail.ReturnTransactionNumber;
            this.SalesTaxGroup = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.SalesTaxGroup).FirstOrDefault() ?? throw new Exception("SalesTaxGroup is null or empty");
            this.OriginalSalesTaxGroup = "";
            this.ShelfNumber = "";
            this.ItemSalesTaxGroup = transactionDetail.ItemSalesTaxGroup;
            this.SerialNumber = "";
            this.OfferNumber = "";
            this.SectionNumber = "";
            this.DiscountAmountForPrinting = Decimal.Parse(transactionDetail.DiscountAmountForPrinting);
            this.CustomerInvoiceDiscountAmount = Decimal.Parse(transactionDetail.CustomerInvoiceDiscountAmount);
            this.PeriodicDiscountPercentage = Decimal.Parse(transactionDetail.PeriodicDiscountPercentage);
            this.ReturnLineNumber = !String.IsNullOrEmpty(transactionDetail.ReturnLineNumber) ? int.Parse(transactionDetail.ReturnLineNumber) : 0;
            this.PeriodicDiscountAmount = Decimal.Parse(transactionDetail.PeriodicDiscountAmount);
            this.PeriodicDiscountGroup = transactionDetail.PeriodicDiscountGroup;
            this.PickupEndTime = 0;
            if (DateTimeOffset.TryParseExact(transactionDetail.RequestedReceiptDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset receiptDate))
            {
                this.RequestedReceiptDate = receiptDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else
            {
                this.RequestedReceiptDate = DateTimeOffset.Parse(transactionDetail.RequestedReceiptDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            if (DateTimeOffset.TryParseExact(transactionDetail.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset transDate))
            {
                this.TransactionDate = transDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else
            {
                this.TransactionDate = DateTimeOffset.Parse(transactionDetail.TransactionDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            if (DateTimeOffset.TryParseExact(transactionDetail.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset shipDate))
            {
                this.RequestedShipDate = shipDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else
            {
                this.RequestedShipDate = DateTimeOffset.Parse(transactionDetail.TransactionDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            this.LogisticsPostalAddressValidFrom = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            this.BusinessDate = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private static void Validate(FOSalesTransactionDetail detail)
        {
            if (string.IsNullOrEmpty(detail.Terminal))
            {
                throw new ArgumentException("Terminal", "Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineNumber))
            {
                throw new ArgumentException("LineNumber", "LineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.CustomerDiscount))
            {
                throw new ArgumentException("CustomerDiscount", "CustomerDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.RequestedReceiptDate))
            {
                throw new ArgumentException("RequestedReceiptDate", "RequestedReceiptDate is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TransactionDate))
            {
                throw new ArgumentException("TransactionDate", "TransactionDate is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TotalDiscount))
            {
                throw new ArgumentException("TotalDiscount", "TotalDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.NetPrice))
            {
                throw new ArgumentException("NetPrice", "NetPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.TotalDiscountPercentage))
            {
                throw new ArgumentException("TotalDiscountPercentage", "TotalDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Unit))
            {
                throw new ArgumentException("Unit", "Unit is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.PriceInBarCode))
            {
                throw new ArgumentException("PriceInBarCode", "PriceInBarCode is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ReturnQuantity))
            {
                throw new ArgumentException("ReturnQuantity", "ReturnQuantity is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.OriginalPrice))
            {
                throw new ArgumentException("OriginalPrice", "OriginalPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Currency))
            {
                throw new ArgumentException("Currency", "Currency is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineDiscount))
            {
                throw new ArgumentException("LineDiscount", "LineDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.NetAmountInclusiveTax))
            {
                throw new ArgumentException("NetAmountInclusiveTax", "NetAmountInclusiveTax is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineManualDiscountPercentage))
            {
                throw new ArgumentException("LineManualDiscountPercentage", "LineManualDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.NetAmount))
            {
                throw new ArgumentException("NetAmount", "NetAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.UnitQuantity))
            {
                throw new ArgumentException("UnitQuantity", "UnitQuantity is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LineManualDiscountAmount))
            {
                throw new ArgumentException("LineManualDiscountAmount", "LineManualDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ReceiptNumber))
            {
                throw new ArgumentException("ReceiptNumber", "ReceiptNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.LinePercentageDiscount))
            {
                throw new ArgumentException("LinePercentageDiscount", "LinePercentageDiscount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.KeyboardProductEntry))
            {
                throw new ArgumentException("KeyboardProductEntry", "KeyboardProductEntry is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.IsReturnNoSale))
            {
                throw new ArgumentException("IsReturnNoSale", "IsReturnNoSale is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ItemId))
            {
                throw new ArgumentException("ItemId", "ItemId is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.SalesTaxAmount))
            {
                throw new ArgumentException("SalesTaxAmount", "SalesTaxAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Quantity))
            {
                throw new ArgumentException("Quantity", "Quantity is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Price))
            {
                throw new ArgumentException("Price", "Price is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.Unit))
            {
                throw new ArgumentException("Unit", "Unit is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.UnitPrice))
            {
                throw new ArgumentException("UnitPrice", "UnitPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ItemSalesTaxGroup))
            {
                throw new ArgumentException("ItemSalesTaxGroup", "ItemSalesTaxGroup is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.DiscountAmountForPrinting))
            {
                throw new ArgumentException("DiscountAmountForPrinting", "DiscountAmountForPrinting is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.CustomerInvoiceDiscountAmount))
            {
                throw new ArgumentException("CustomerInvoiceDiscountAmount", "CustomerInvoiceDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.ProductScanned))
            {
                throw new ArgumentException("ProductScanned", "ProductScanned is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.PeriodicDiscountPercentage))
            {
                throw new ArgumentException("PeriodicDiscountPercentage", "PeriodicDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.PeriodicDiscountAmount))
            {
                throw new ArgumentException("PeriodicDiscountAmount", "PeriodicDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(detail.BusinessDate))
            {
                throw new ArgumentException("BusinessDate", "BusinessDate is null or empty.");
            }
        }

        public StoreTransactionLinesDTO() { }


        //public StoreTransactionLinesDTO(Extensions.Cyware.App.Receiver.Models.SalesTransactionDetail transactionDetail, int lineNumber, SalesTransactionHeader transactionHeader, D365FOConfig config)
        //{
        //    Validate(transactionDetail);
        //    string warehouseId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.WarehouseId)).Select(rs => rs.WarehouseId).FirstOrDefault() ?? "";
        //    bool returnTrans = false;
        //    int quantitySold = ((int)Decimal.Parse(String.IsNullOrEmpty(transactionDetail.QuantitySold) ? "1" : transactionDetail.QuantitySold) / 100);

        //    if (transactionDetail.QuantitySold.Contains("-"))
        //    {
        //        returnTrans = true;
        //    }

        //    this.dataAreaId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.DefaultCustomerLegalEntity)).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? ""; // direct
        //    this.TransactionNumber = transactionDetail.TransNumber;
        //    this.Terminal = transactionDetail.RegisterID; // direct
        //    this.ReceiptNumber = transactionHeader.InvoiceNumber;
        //    this.CustomerAccount = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.DefaultCustomerAccount)).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? ""; // direct/config
        //    this.OperatingUnitNumber = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.OperatingUnitNumber)).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? ""; // direct
        //    this.LineNumber = lineNumber;
        //    this.CustomerDiscount = int.Parse(transactionDetail.ExtendedDiscount); // check transaction line else discount trans line
        //    this.CashDiscountAmount = int.Parse(transactionDetail.ExtendedDiscount);// Discount amount
        //    this.SiteId = ordersTransaction.Config.Warehouses.Where(w => w.WarehouseId == warehouseId && !string.IsNullOrEmpty(w.OperationalSiteId)).Select(rs => rs.OperationalSiteId).FirstOrDefault() ?? ""; // direct
        //    this.Warehouse = warehouseId; // direct
        //    this.NetPrice = Decimal.Parse(transactionDetail.UnitRetail) / 1000;//without tax
        //    this.TransactionCode = "ItemOnFile"; // config
        //    this.Unit = transactionDetail.UnitRetail; //"pcs"; // direct
        //    this.PriceInBarCode = "No"; // direct
        //    this.TransactionStatus = "Posted"; // config
        //    this.Currency = transactionDetail.CurrencyCode; // direct
        //    this.InventoryStatus = "None"; //config 
        //    this.GiftCard = "No"; //direct
        //    this.NetAmount = (Decimal.Parse(transactionDetail.UnitRetail) * quantitySold) / 1000;//without tax, unit retail * qty
        //    this.IsPriceChange = transactionDetail.PriceOverride == "1" ? "Yes" : "No";
        //    this.IsWeightProduct = "No"; // direct
        //    this.IsScaleProduct = "No"; // direct
        //    this.IsOriginalOfLinkedProductList = "No"; // direct
        //    this.KeyboardProductEntry = "Yes"; // direct
        //    this.IsReturnNoSale = returnTrans ? "Yes" : "No"; // use returnTrans
        //    this.ItemId = transactionDetail.SKUNumber;
        //    this.Quantity = returnTrans ? quantitySold * -1 : quantitySold;
        //    this.UnitPrice = Decimal.Parse(transactionDetail.UnitRetail) / 1000;
        //    this.IsLineDiscounted = int.Parse(transactionDetail.ExtendedDiscount) > 0 ? "Yes" : "No"; // check discount amount
        //    this.IsWeightManuallyEntered = "No"; // direct
        //    this.ProductScanned = "No"; // direct
        //    this.ReturnTrackingStatus = "None"; // config
        //    this.NetAmountInclusiveTax = (int)(int.Parse(transactionDetail.UnitRetail) + (int.Parse(transactionDetail.UnitRetail) * .12));
        //    this.Price = Decimal.Parse(String.IsNullOrEmpty(transactionDetail.UnitRetail) ? "0" : transactionDetail.UnitRetail) / 1000;
        //    this.LinePercentageDiscount = (int.Parse(transactionDetail.UnitRetail) - int.Parse(transactionDetail.ExtendedPrice)) / int.Parse(transactionDetail.UnitRetail) * 100;
        //    this.TotalDiscountPercentage = (int.Parse(transactionDetail.UnitRetail) - int.Parse(transactionDetail.ExtendedPrice)) / int.Parse(transactionDetail.UnitRetail) * 100;
        //    this.LineDiscount = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.LineManualDiscountAmount = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.LineManualDiscountPercentage = (int.Parse(transactionDetail.UnitRetail) - int.Parse(transactionDetail.ExtendedPrice)) / int.Parse(transactionDetail.UnitRetail) * 100;
        //    this.TotalDiscount = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.ModeOfDelivery = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == transactionHeader.CustomerNumber && !string.IsNullOrEmpty(c.DeliveryMode)).Select(c => c.DeliveryMode).FirstOrDefault() ?? "";
        //    this.IsLinkedProductNotOriginal = "No";
        //    this.SkipReports = "No";
        //    this.OriginalItemSalesTaxGroup = "";
        //    this.RetailEmailAddressContent = "";
        //    this.ItemSize = "";
        //    this.PickupStartTime = 0;
        //    this.TaxExemptPriceInclusiveOriginalPrice = (int)(int.Parse(transactionDetail.UnitRetail) + (int.Parse(transactionDetail.UnitRetail) * .12));
        //    this.DiscountAmountWithoutTax = int.Parse(transactionDetail.UnitRetail) - int.Parse(transactionDetail.ExtendedDiscount);
        //    this.PriceGroups = "";
        //    this.RFIDTagId = "";
        //    this.VariantNumber = "";
        //    this.CategoryHierarchyName = "";
        //    this.FixedPriceCharges = 0;
        //    this.TaxExemptPriceInclusiveReductionAmount = 0;
        //    this.LotID = "";
        //    this.ReturnQuantity = returnTrans ? quantitySold : 0;
        //    this.OriginalPrice = int.Parse(transactionDetail.UnitRetail) / 1000;
        //    this.ItemRelation = "";
        //    this.ItemConfigId = "";
        //    this.ReturnOperatingUnitNumber = returnTrans ? storeNumber : "";
        //    this.InventoryStatus = transactionDetail.Status;
        //    this.ChannelListingID = "";
        //    this.ItemColor = "";
        //    this.BarCode = transactionDetail.ScanBarCode;
        //    this.UnitQuantity = returnTrans ? quantitySold * -1 : quantitySold;
        //    this.ItemVersion = "";
        //    this.ReturnTerminal = returnTrans ? transactionDetail.RegisterID : "";
        //    this.LogisticLocationId = "";
        //    this.ItemStyle = "";
        //    this.ReasonCodeDiscount = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.CategoryName = "";
        //    this.TotalDiscountInfoCodeLineNum = 0;
        //    this.CancelledTransactionNumber = "";
        //    this.ElectronicDeliveryEmail = "";
        //    this.SalesTaxAmount = 0;
        //    this.ReturnTransactionNumber = "";
        //    this.SalesTaxGroup = "";
        //    this.OriginalSalesTaxGroup = "";
        //    this.ShelfNumber = "";
        //    this.ItemSalesTaxGroup = "";
        //    this.SerialNumber = "";
        //    this.OfferNumber = "";
        //    this.SectionNumber = "";
        //    this.DiscountAmountForPrinting = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.CustomerInvoiceDiscountAmount = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.PeriodicDiscountPercentage = (int.Parse(transactionDetail.UnitRetail) - int.Parse(transactionDetail.ExtendedPrice)) / int.Parse(transactionDetail.UnitRetail) * 100; ;
        //    this.ReturnLineNumber = returnTrans ? lineNumber : 0;
        //    this.PeriodicDiscountAmount = int.Parse(transactionDetail.ExtendedDiscount);
        //    this.PeriodicDiscountGroup = "";
        //    this.PickupEndTime = 0;
        //    if (DateTimeOffset.TryParseExact(transactionDetail.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset receiptDate))
        //    {
        //        this.RequestedReceiptDate = receiptDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(transactionDetail.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset transDate))
        //    {
        //        this.TransactionDate = transDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(transactionDetail.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset shipDate))
        //    {
        //        this.RequestedShipDate = shipDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(dateMinValue, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset LogisticsPostalAddressValidFrom))
        //    {
        //        this.LogisticsPostalAddressValidFrom = LogisticsPostalAddressValidFrom;
        //    }
        //    if (DateTimeOffset.TryParseExact(dateMinValue, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset BusinessDate))
        //    {
        //        this.BusinessDate = BusinessDate;
        //    }
        //}

        //private static void Validate(SalesTransactionDetail transactionDetail)
        //{
        //    if (string.IsNullOrEmpty(transactionDetail.TransNumber))
        //    {
        //        throw new ArgumentException("TransNumber", "TransNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.RegisterID))
        //    {
        //        throw new ArgumentException("RegisterID", "RegisterID is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.AccountNumber))
        //    {
        //        throw new ArgumentException("AccountNumber", "AccountNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(storeNumber))
        //    {
        //        throw new ArgumentException("StoreNumber", "StoreNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.ExtendedDiscount))
        //    {
        //        throw new ArgumentException("ExtendedDiscount", "ExtendedDiscount is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.UnitRetail))
        //    {
        //        throw new ArgumentException("UnitRetail", "UnitRetail is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.Status))
        //    {
        //        throw new ArgumentException("Status", "Status is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.CurrencyCode))
        //    {
        //        throw new ArgumentException("CurrencyCode", "CurrencyCode is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.SKUNumber))
        //    {
        //        throw new ArgumentException("SKUNumber", "SKUNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.PriceOverride))
        //    {
        //        throw new ArgumentException("ZipCode", "PriceOverride is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.ExtendedDiscount))
        //    {
        //        throw new ArgumentException("ExtendedDiscount", "ExtendedDiscount is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.QuantitySold))
        //    {
        //        throw new ArgumentException("QuantitySold", "QuantitySold is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.TransDate))
        //    {
        //        throw new ArgumentException("TransDate", "TransDate is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDetail.ScanBarCode))
        //    {
        //        throw new ArgumentException("ScanBarCode", "ScanBarCode is null or empty.");
        //    }
        //}
    }
}