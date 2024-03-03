using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using System.Globalization;
using Moo.FO.Helper;

namespace Moo.FO.Model.DTO
{
    public class StoreTransactioHeaderDTO : Moo.FO.Model.StoreTransactionHeader
    {
        private const string dateMinValue = "1900-01-01T00:00:00Z";
        public StoreTransactioHeaderDTO(OrdersTransaction ordersTransaction)
        {
            Validate(ordersTransaction.Header);
            string storeNumber = ordersTransaction.Header.ChannelReferenceId;
            string warehouseId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.WarehouseId).FirstOrDefault() ?? throw new Exception("warehouseId is null or empty");
            string CYWAREcustomerAccount = !String.IsNullOrEmpty(ordersTransaction.Header.CustomerAccount) ? ordersTransaction.Header.CustomerAccount : ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception("CustomerAccount is null or empty");
            // Check if CYWAREcustomerAccount contains "|" character
            if (CYWAREcustomerAccount.Contains("|"))
            {
                // Split CYWAREcustomerAccount by "|" and take the first part
                CYWAREcustomerAccount = CYWAREcustomerAccount.Split('|')[0];
            }
            string customerAccount = ordersTransaction.Config.Customers.Any(c => c.CustomerAccount == CYWAREcustomerAccount) ? CYWAREcustomerAccount : ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception("CustomerAccount is null or empty");

            this.dataAreaId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception("dataAreaId is null or empty");
            this.TransactionNumber = ordersTransaction.Header.TransactionNumber;
            this.Terminal = ordersTransaction.Header.Terminal.Length < 6 ? ordersTransaction.Header.Terminal.PadLeft(6, '0') : ordersTransaction.Header.Terminal;
            this.RreceiptId = ordersTransaction.Header.RreceiptId;
            this.CustomerAccount = customerAccount;
            this.TransactionOrderType = "SalesOrder";
            this.ChannelReferenceId = storeNumber;
            this.OperatingUnitNumber = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception("OperatingUnitNumber is null or empty");
            this.NetPrice = 0;
            this.TotalManualDiscountPercentage = 0;
            this.DiscountAmount = 0;
            this.CostAmount = 0;
            this.AmountPostedToAccount = 0;
            this.NumberOfItemLines = 0;
            this.PaymentAmount = 0;
            this.TransactionStatus = "None";
            this.NumberOfPaymentLines = 0;
            this.Warehouse = warehouseId;
            this.TransactionType = "AsyncCustomerOrder";
            this.SiteId = ordersTransaction.Config.Warehouses.Where(w => w.WarehouseId == warehouseId).Select(rs => rs.OperationalSiteId).FirstOrDefault() ?? throw new Exception("SiteId is null or empty");
            this.TaxCalculationType = "Regular";
            this.NetAmount = 0;
            this.CustomerDiscountAmount = 0;
            this.SalesPaymentDifference = 0;
            this.ExchangeRate = 0;
            this.SalesInvoiceAmount = 0;
            this.DiscountAmountWithoutTax = 0;
            this.IncomeExpenseAmount = 0;
            this.NumberOfItems = 0;
            this.GrossAmount = 0;
            this.TransactionTime = TimeConversion.ConvertMilitaryTimeToSeconds(ordersTransaction.Header.TransactionTime);
            this.CreatedOffline = ordersTransaction.Header.CreatedOffline;
            this.GiftCardIssueAmount = 0;
            this.Currency = ordersTransaction.Header.Currency;
            this.SaleIsReturnSale = ordersTransaction.Header.SaleIsReturnSale;
            this.SalesOrderAmount = 0;
            this.TotalDiscountAmount = 0;
            this.IsTaxExemptedForPriceInclusive = ordersTransaction.Header.IsTaxExemptedForPriceInclusive;
            this.LogisticsPostalZipCode = ordersTransaction.Header.LogisticsPostalZipCode;
            this.TotalManualDiscountAmount = 0;
            this.IsTaxIncludedInPrice = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.PriceIncludesSalesTax).FirstOrDefault() ?? throw new Exception("IsTaxIncludedInPrice is null or empty");
            this.PostAsShipment = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.IsTransactionPostedAsShipment).FirstOrDefault() ?? throw new Exception("PostAsShipment is null or empty");
            this.SaleOnAccount = "No";
            this.ToAccount = "No";
            this.ItemsPosted = "No";
            this.Shift = "";
            this.BatchID = 0;
            this.Comment = ordersTransaction.Header.Comment;
            this.CustomerName = ordersTransaction.Header.CustomerName;
            this.GiftCardBalance = 0;
            this.LogisticsPostalState = ordersTransaction.Header.LogisticsPostalState;
            this.SalesOrderId = "";
            this.GiftCardIdMasked = "";
            this.RetailTransactionAggregationFieldList = 0;
            this.LoyaltyCardId = "";
            this.LogisticsPostalStreet = ordersTransaction.Header.LogisticsPostalStreet;
            this.LogisticsPostalCity = ordersTransaction.Header.LogisticsPostalCity;
            this.InfocodeDiscountGroup = "";
            this.DeliveryMode = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.DeliveryMode).FirstOrDefault() ?? throw new Exception("DeliveryMode is null or empty");
            this.Staff = "";
            this.SuspendedTransactionId = "";
            this.RefundReceiptId = "";
            this.BatchTerminalId = "";
            this.InvoiceId = "";
            this.LogisticsLocationId = "";
            this.CreatedOnPosTerminal = "";
            this.LanguageId = "";
            this.LogisticsPostalCounty = ordersTransaction.Header.LogisticsPostalCounty;
            this.GiftCardHistoryDetails = "";
            if (DateTimeOffset.TryParseExact(ordersTransaction.Header.ShippingDateRequested, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset shipDate))
            {
                this.ShippingDateRequested = shipDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else
            {
                this.ShippingDateRequested = DateTimeOffset.Parse(ordersTransaction.Header.ShippingDateRequested).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            if (DateTimeOffset.TryParseExact(ordersTransaction.Header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset transDate))
            {
                this.TransactionDate = transDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            } 
            else
            {
                this.TransactionDate = DateTimeOffset.Parse(ordersTransaction.Header.TransactionDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            this.BeginDateTime = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            this.businessDate = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            this.GiftCardActiveFrom = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            this.GiftCardExpireDate = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            this.LogisticsPostalAddressValidFrom = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            this.LogisticPostalAddressValidTo = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        private static void Validate(FOSalesTransactionHeader header)
        {
            if (string.IsNullOrEmpty(header.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(header.Terminal))
            {
                throw new ArgumentException("Terminal", "Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(header.IsTaxExemptedForPriceInclusive))
            {
                throw new ArgumentException("IsTaxExemptedForPriceInclusive", "IsTaxExemptedForPriceInclusive is null or empty.");
            }

            if (string.IsNullOrEmpty(header.RreceiptId))
            {
                throw new ArgumentException("RreceiptId", "RreceiptId is null or empty.");
            }

            if (string.IsNullOrEmpty(header.DiscountAmount))
            {
                throw new ArgumentException("DiscountAmount", "DiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.NetPrice))
            {
                throw new ArgumentException("NetPrice", "NetPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TotalManualDiscountPercentage))
            {
                throw new ArgumentException("TotalManualDiscountPercentage", "TotalManualDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TransactionDate))
            {
                throw new ArgumentException("TransactionDate", "TransactionDate is null or empty.");
            }

            if (string.IsNullOrEmpty(header.PaymentAmount))
            {
                throw new ArgumentException("PaymentAmount", "PaymentAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.ShippingDateRequested))
            {
                throw new ArgumentException("ShippingDateRequested", "ShippingDateRequested is null or empty.");
            }

            if (string.IsNullOrEmpty(header.NetAmount))
            {
                throw new ArgumentException("NetAmount", "NetAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.CustomerDiscountAmount))
            {
                throw new ArgumentException("CustomerDiscountAmount", "CustomerDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.SalesPaymentDifference))
            {
                throw new ArgumentException("SalesPaymentDifference", "SalesPaymentDifference is null or empty.");
            }

            if (string.IsNullOrEmpty(header.ExchangeRate))
            {
                throw new ArgumentException("ExchangeRate", "ExchangeRate is null or empty.");
            }

            if (string.IsNullOrEmpty(header.SalesInvoiceAmount))
            {
                throw new ArgumentException("SalesInvoiceAmount", "SalesInvoiceAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TotalDiscountAmount))
            {
                throw new ArgumentException("TotalDiscountAmount", "TotalDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.GrossAmount))
            {
                throw new ArgumentException("GrossAmount", "GrossAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TransactionTime))
            {
                throw new ArgumentException("TransactionTime", "TransactionTime is null or empty.");
            }

            if (string.IsNullOrEmpty(header.CreatedOffline))
            {
                throw new ArgumentException("CreatedOffline", "CreatedOffline is null or empty.");
            }

            if (string.IsNullOrEmpty(header.Currency))
            {
                throw new ArgumentException("Currency", "Currency is null or empty.");
            }

            if (string.IsNullOrEmpty(header.SaleIsReturnSale))
            {
                throw new ArgumentException("SaleIsReturnSale", "SaleIsReturnSale is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TotalManualDiscountAmount))
            {
                throw new ArgumentException("TotalManualDiscountAmount", "TotalManualDiscountAmount is null or empty.");
            }
        }

        public StoreTransactioHeaderDTO() { }

        //public StoreTransactioHeaderDTO(OrdersTransaction ordersTransaction)
        //{
        //    Validate(ordersTransaction);
        //    string warehouseId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.WarehouseId)).Select(rs => rs.WarehouseId).FirstOrDefault() ?? "";
        //    bool hasNegativeQuantity = ordersTransaction.Details.Any(detail =>
        //    {
        //        if (int.TryParse(detail.QuantitySold, out int quantity))
        //        {
        //            return (quantity / 100) < 0;
        //        }
        //        return false;
        //    });

        //    decimal totalDiscountAmount = ordersTransaction.Discounts.Where(d => d.TransNumber == ordersTransaction.Header.TransNumber).Sum(d => Decimal.Parse(d.DiscountAmount)); // divide 1000

        //    this.dataAreaId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.DefaultCustomerLegalEntity)).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? ""; // direct
        //    this.TransactionNumber = ordersTransaction.Header.TransNumber;
        //    this.Terminal = ordersTransaction.Header.RegisterID; // direct
        //    this.RreceiptId = ordersTransaction.Header.InvoiceNumber; // invoicenumber
        //    this.CustomerAccount = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.DefaultCustomerAccount)).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? ""; // direct/config // (store)DefaultCustomerAccount
        //    this.TransactionOrderType = "SalesOrder"; // config
        //    this.ChannelReferenceId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.RetailChannelId)).Select(rs => rs.RetailChannelId).FirstOrDefault() ?? ""; // direct // (store)RetailChannelId
        //    this.OperatingUnitNumber = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.OperatingUnitNumber)).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? ""; // direct // (store)OperatingUnitNumber
        //    this.NetPrice = Decimal.Parse(ordersTransaction.Header.SalesAmount) / 1000;
        //    this.TotalManualDiscountPercentage = (totalDiscountAmount / Decimal.Parse(ordersTransaction.Header.SalesAmount)) * 100; // check sales trans line highest discount percentage
        //    this.DiscountAmount = totalDiscountAmount; //need to change
        //    this.CostAmount = Decimal.Parse(ordersTransaction.Header.SalesAmount) / 1000;
        //    this.AmountPostedToAccount = Decimal.Parse(ordersTransaction.Header.TenderAmount) / 1000;
        //    this.NumberOfItemLines = ordersTransaction.Details.Count;
        //    this.PaymentAmount = Decimal.Parse(ordersTransaction.Header.TenderAmount) / 1000;
        //    this.TransactionStatus = "None"; // config
        //    this.NumberOfPaymentLines = ordersTransaction.Tenders.Count;
        //    this.Warehouse = warehouseId; // (store)WarehouseId
        //    this.TransactionType = "AsyncCustomerOrder"; // config
        //    this.SiteId = ordersTransaction.Config.Warehouses.Where(w => w.WarehouseId == warehouseId && !string.IsNullOrEmpty(w.OperationalSiteId)).Select(rs => rs.OperationalSiteId).FirstOrDefault() ?? ""; // direct // (warehouse)OperationalSiteId
        //    this.TaxCalculationType = "Regular"; //config
        //    this.NetAmount = ordersTransaction.Details.Where(l => l.TransNumber == ordersTransaction.Header.TransNumber).Sum(l => Decimal.Parse(l.UnitRetail)); //without tax, check with trans lines for net amount per line and sum all lines // divide by 1000
        //    this.CustomerDiscountAmount = ordersTransaction.Discounts.Where(d => d.TransNumber == ordersTransaction.Header.TransNumber).Sum(d => Decimal.Parse(d.DiscountAmount)); ; // same with discount amount
        //    this.SalesPaymentDifference = Decimal.Parse(ordersTransaction.Header.SalesAmount) - Decimal.Parse(ordersTransaction.Header.TenderAmount); // netprice - total tender amount
        //    this.ExchangeRate = ordersTransaction.Details.Where(l => l.TransNumber == ordersTransaction.Header.TransNumber && !string.IsNullOrEmpty(l.ExchangeRate)).Select(l => Decimal.Parse(l.ExchangeRate)).FirstOrDefault(); // direct
        //    this.SalesInvoiceAmount = Decimal.Parse(ordersTransaction.Header.SalesAmount) / 1000; // need to changes
        //    this.DiscountAmountWithoutTax = ordersTransaction.Discounts.Where(d => d.TransNumber == ordersTransaction.Header.TransNumber).Sum(d => Decimal.Parse(d.TransDiscAmount)); // check discount transaction without tax field then sum
        //    this.IncomeExpenseAmount = 0; //need to change
        //    this.NumberOfItems = ordersTransaction.Details.Count;
        //    this.GrossAmount = ordersTransaction.Details.Where(l => l.TransNumber == ordersTransaction.Header.TransNumber).Sum(l => Decimal.Parse(l.ExtendedPrice)); // should be with tax, check with trans lines for gross amount per line and sum all lines
        //    this.TransactionTime = Convert.ToInt32(ordersTransaction.Header.TransactionTime);
        //    this.CreatedOffline = "No"; // direct 
        //    this.GiftCardIssueAmount = 0; // direct
        //    this.Currency = ordersTransaction.Header.CurrencyCode;
        //    this.SaleIsReturnSale = hasNegativeQuantity ? "Yes" : "No";
        //    this.SalesOrderAmount = ordersTransaction.Details.Where(l => l.TransNumber == ordersTransaction.Header.TransNumber).Sum(l => Decimal.Parse(l.ExtendedPrice)); ; // same with gross amount
        //    this.TotalDiscountAmount = totalDiscountAmount; // same with discount amount
        //    this.IsTaxExemptedForPriceInclusive = "No";
        //    this.LogisticsPostalZipCode = ordersTransaction.Header.ZipCode;
        //    this.TotalManualDiscountAmount = totalDiscountAmount; ;
        //    this.IsTaxIncludedInPrice = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber && !string.IsNullOrEmpty(rs.PriceIncludesSalesTax)).Select(rs => rs.PriceIncludesSalesTax).FirstOrDefault() ?? "";
        //    this.PostAsShipment = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == ordersTransaction.Header.CustomerNumber && !string.IsNullOrEmpty(c.IsTransactionPostedAsShipment)).Select(c => c.IsTransactionPostedAsShipment).FirstOrDefault() ?? "";
        //    this.SaleOnAccount = "No";
        //    this.ToAccount = "No"; // default No
        //    this.ItemsPosted = "No";
        //    this.Shift = "";
        //    this.BatchID = 0;
        //    this.Comment = "";
        //    this.CustomerName = $"{ordersTransaction.Header.CustomerFirstName} {ordersTransaction.Header.CustomerMiddleName} {ordersTransaction.Header.CustomerLastName}";
        //    this.GiftCardBalance = 0;
        //    this.LogisticsPostalState = "";
        //    this.SalesOrderId = "";
        //    this.GiftCardIdMasked = "";
        //    this.RetailTransactionAggregationFieldList = 0;
        //    this.LoyaltyCardId = "";
        //    this.LogisticsPostalStreet = "";
        //    this.LogisticsPostalCity = "";
        //    this.InfocodeDiscountGroup = "";
        //    this.DeliveryMode = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == ordersTransaction.Header.CustomerNumber && !string.IsNullOrEmpty(c.DeliveryMode)).Select(c => c.DeliveryMode).FirstOrDefault() ?? "";
        //    this.Staff = "";
        //    this.SuspendedTransactionId = "";
        //    this.RefundReceiptId = "";
        //    this.BatchTerminalId = "";
        //    this.InvoiceId = ""; // default blank
        //    this.LogisticsLocationId = "";
        //    this.CreatedOnPosTerminal = "";
        //    this.LanguageId = ordersTransaction.Config.Customers.Where(c => c.CustomerAccount == ordersTransaction.Header.CustomerNumber && !string.IsNullOrEmpty(c.LanguageId)).Select(c => c.LanguageId).FirstOrDefault() ?? "";
        //    this.LogisticsPostalCounty = "";
        //    this.GiftCardHistoryDetails = "";
        //    if (DateTimeOffset.TryParseExact(ordersTransaction.Header.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset shipDate))
        //    {
        //        this.ShippingDateRequested = shipDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(ordersTransaction.Header.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset transDate))
        //    {
        //        this.TransactionDate = transDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(ordersTransaction.Header.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset beginDateTime))
        //    {
        //        this.BeginDateTime = beginDateTime;
        //    }
        //    if (DateTimeOffset.TryParseExact(ordersTransaction.Header.TransDate, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset businessDate))
        //    {
        //        this.businessDate = businessDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(dateMinValue, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset giftCardExpireDate))
        //    {
        //        this.GiftCardExpireDate = giftCardExpireDate;
        //    }
        //    if (DateTimeOffset.TryParseExact(dateMinValue, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset giftCardActiveFrom))
        //    {
        //        this.GiftCardActiveFrom = giftCardActiveFrom;
        //    }
        //    if (DateTimeOffset.TryParseExact(dateMinValue, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset logisticsPostalAddressValidFrom))
        //    {
        //        this.LogisticsPostalAddressValidFrom = logisticsPostalAddressValidFrom;
        //    }
        //    if (DateTimeOffset.TryParseExact(dateMinValue, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset logisticPostalAddressValidTo))
        //    {
        //        this.LogisticPostalAddressValidTo = logisticPostalAddressValidTo;
        //    }

        //}

        //private static void Validate(OrdersTransaction ordersTransaction)
        //{
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.TransNumber))
        //    {
        //        throw new ArgumentException("TransNumber", "TransNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.RegisterID))
        //    {
        //        throw new ArgumentException("RegisterID", "RegisterID is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.CustomerNumber))
        //    {
        //        throw new ArgumentException("CustomerNumber", "CustomerNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(storeNumber))
        //    {
        //        throw new ArgumentException("StoreNumber", "StoreNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.SalesAmount))
        //    {
        //        throw new ArgumentException("SalesAmount", "SalesAmount is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.TenderAmount))
        //    {
        //        throw new ArgumentException("TenderAmount", "TenderAmount is null or empty.");
        //    }
        //    if (ordersTransaction.Details.Count == 0)
        //    {
        //        throw new ArgumentException("Count", "Count is zero.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.TransactionTime))
        //    {
        //        throw new ArgumentException("TransactionTime", "TransactionTime is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.CurrencyCode))
        //    {
        //        throw new ArgumentException("CurrencyCode", "CurrencyCode is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.ZipCode))
        //    {
        //        throw new ArgumentException("ZipCode", "ZipCode is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.CustomerFirstName))
        //    {
        //        throw new ArgumentException("CustomerFirstName", "CustomerFirstName is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.CustomerLastName))
        //    {
        //        throw new ArgumentException("CustomerLastName", "CustomerLastName is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(ordersTransaction.Header.TransDate))
        //    {
        //        throw new ArgumentException("TransDate", "TransDate is null or empty.");
        //    }
        //}
    }
}
