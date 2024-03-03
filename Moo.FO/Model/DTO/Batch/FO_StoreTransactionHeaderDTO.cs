using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using Moo.FO.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model.DTO.Batch
{
    public class FO_StoreTransactionHeaderDTO : StoreTransactionHeader
    {
        private const string dateMinValue = "1900-01-01T00:00:00Z";
        private static string errorPrefix = "";
        public FO_StoreTransactionHeaderDTO(FOSalesTransactionHeader header, D365FOConfig config)
        {
            Validate(header);
            try
            {
                string storeNumber = header.ChannelReferenceId;
                string warehouseId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.WarehouseId).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, warehouseId is null or empty");
                string CYWAREcustomerAccount = !String.IsNullOrEmpty(header.CustomerAccount) ? header.CustomerAccount : config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, CustomerAccount is null or empty");
                // Check if CYWAREcustomerAccount contains "|" character
                if (CYWAREcustomerAccount.Contains("|"))
                {
                    // Split CYWAREcustomerAccount by "|" and take the first part
                    CYWAREcustomerAccount = CYWAREcustomerAccount.Split('|')[0];
                }
                string customerAccount = config.Customers.Any(c => c.CustomerAccount == CYWAREcustomerAccount) ? CYWAREcustomerAccount : config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerAccount).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, CustomerAccount is null or empty");

                this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, dataAreaId is null or empty");
                this.TransactionNumber = header.TransactionNumber;
                this.Terminal = config.Terminals.Where(rt => rt.StoreNumber == storeNumber && rt.Name == header.Terminal).Select(rt => rt.TerminalNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, Terminal is null or empty");
                this.RreceiptId = header.RreceiptId;
                this.CustomerAccount = customerAccount;
                this.TransactionOrderType = "SalesOrder";
                this.ChannelReferenceId = storeNumber;
                this.OperatingUnitNumber = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, OperatingUnitNumber is null or empty");
                this.NetPrice = 0;
                this.TotalManualDiscountPercentage = 0;
                this.DiscountAmount = decimal.Parse(header.DiscountAmount);
                this.CostAmount = 0;
                this.AmountPostedToAccount = 0;
                this.NumberOfItemLines = 0;
                this.PaymentAmount = 0;
                this.TransactionStatus = "None";
                this.NumberOfPaymentLines = 0;
                this.Warehouse = warehouseId;
                this.TransactionType = "AsyncCustomerOrder";
                this.SiteId = config.Warehouses.Where(w => w.WarehouseId == warehouseId).Select(rs => rs.OperationalSiteId).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, SiteId is null or empty");
                this.TaxCalculationType = "Regular";
                this.NetAmount = 0;
                this.CustomerDiscountAmount = decimal.Parse(header.CustomerDiscountAmount);
                this.SalesPaymentDifference = 0;
                this.ExchangeRate = 0;
                this.SalesInvoiceAmount = 0;
                this.DiscountAmountWithoutTax = decimal.Parse(header.DiscountAmountWithoutTax);
                this.IncomeExpenseAmount = 0;
                this.NumberOfItems = 0;
                this.GrossAmount = decimal.Parse(header.GrossAmount);
                this.TransactionTime = TimeConversion.ConvertMilitaryTimeToSeconds(header.TransactionTime);
                this.CreatedOffline = header.CreatedOffline;
                this.GiftCardIssueAmount = 0;
                this.Currency = header.Currency;
                this.SaleIsReturnSale = header.SaleIsReturnSale;
                this.SalesOrderAmount = 0;
                this.TotalDiscountAmount = decimal.Parse(header.TotalDiscountAmount);
                this.IsTaxExemptedForPriceInclusive = header.IsTaxExemptedForPriceInclusive;
                this.LogisticsPostalZipCode = header.LogisticsPostalZipCode;
                this.TotalManualDiscountAmount = 0;
                this.IsTaxIncludedInPrice = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.PriceIncludesSalesTax).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, IsTaxIncludedInPrice is null or empty");
                this.PostAsShipment = config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.IsTransactionPostedAsShipment).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, PostAsShipment is null or empty");
                this.SaleOnAccount = "No";
                this.ToAccount = "No";
                this.ItemsPosted = "No";
                this.Shift = "";
                this.BatchID = 0;
                this.Comment = !String.IsNullOrEmpty(header.Comment) ? header.Comment : header.CustomerName;
                this.CustomerName = header.CustomerName;
                this.GiftCardBalance = 0;
                this.LogisticsPostalState = header.LogisticsPostalState;
                this.SalesOrderId = "";
                this.GiftCardIdMasked = "";
                this.RetailTransactionAggregationFieldList = 0;
                this.LoyaltyCardId = "";
                this.LogisticsPostalStreet = header.LogisticsPostalStreet;
                this.LogisticsPostalCity = header.LogisticsPostalCity;
                this.InfocodeDiscountGroup = "";
                this.DeliveryMode = config.Customers.Where(c => c.CustomerAccount == customerAccount).Select(c => c.DeliveryMode).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, DeliveryMode is null or empty");
                this.Staff = "";
                this.SuspendedTransactionId = "";
                this.RefundReceiptId = "";
                this.BatchTerminalId = "";
                this.InvoiceId = "";
                this.LogisticsLocationId = "";
                this.CreatedOnPosTerminal = "";
                this.LanguageId = "";
                this.LogisticsPostalCounty = header.LogisticsPostalCounty;
                this.GiftCardHistoryDetails = "";
                if (DateTimeOffset.TryParseExact(header.ShippingDateRequested, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset shipDate))
                {
                    this.ShippingDateRequested = shipDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    this.ShippingDateRequested = DateTimeOffset.Parse(header.ShippingDateRequested).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                if (DateTimeOffset.TryParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset transDate))
                {
                    this.TransactionDate = transDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                else
                {
                    this.TransactionDate = DateTimeOffset.Parse(header.TransactionDate).ToString("yyyy-MM-ddTHH:mm:ssZ");
                }
                this.BeginDateTime = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                this.businessDate = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                this.GiftCardActiveFrom = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                this.GiftCardExpireDate = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                this.LogisticsPostalAddressValidFrom = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
                this.LogisticPostalAddressValidTo = DateTimeOffset.Parse(dateMinValue).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            catch (Exception ex)
            {
                throw new Exception($"{errorPrefix}, {ex.Message}");
            }
        }

        private static void Validate(FOSalesTransactionHeader header)
        {
            if (string.IsNullOrEmpty(header.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "Header TransactionNumber is null or empty.");
            }
            errorPrefix = $"Header Transaction Number: {header.TransactionNumber}";

            if (string.IsNullOrEmpty(header.Terminal))
            {
                throw new ArgumentException("Terminal", $"{errorPrefix}, Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(header.IsTaxExemptedForPriceInclusive))
            {
                throw new ArgumentException("IsTaxExemptedForPriceInclusive", $"{errorPrefix}, IsTaxExemptedForPriceInclusive is null or empty.");
            }

            if (string.IsNullOrEmpty(header.RreceiptId))
            {
                throw new ArgumentException("RreceiptId", $"{errorPrefix}, RreceiptId is null or empty.");
            }

            if (string.IsNullOrEmpty(header.DiscountAmount))
            {
                throw new ArgumentException("DiscountAmount", $"{errorPrefix}, DiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.NetPrice))
            {
                throw new ArgumentException("NetPrice", $"{errorPrefix}, NetPrice is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TotalManualDiscountPercentage))
            {
                throw new ArgumentException("TotalManualDiscountPercentage", $"{errorPrefix}, TotalManualDiscountPercentage is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TransactionDate))
            {
                throw new ArgumentException("TransactionDate", $"{errorPrefix}, TransactionDate is null or empty.");
            }

            if (string.IsNullOrEmpty(header.PaymentAmount))
            {
                throw new ArgumentException("PaymentAmount", $"{errorPrefix}, PaymentAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.ShippingDateRequested))
            {
                throw new ArgumentException("ShippingDateRequested", $"{errorPrefix}, ShippingDateRequested is null or empty.");
            }

            if (string.IsNullOrEmpty(header.NetAmount))
            {
                throw new ArgumentException("NetAmount", $"{errorPrefix}, NetAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.CustomerDiscountAmount))
            {
                throw new ArgumentException("CustomerDiscountAmount", $"{errorPrefix}, CustomerDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.SalesPaymentDifference))
            {
                throw new ArgumentException("SalesPaymentDifference", $"{errorPrefix}, SalesPaymentDifference is null or empty.");
            }

            if (string.IsNullOrEmpty(header.ExchangeRate))
            {
                throw new ArgumentException("ExchangeRate", $"{errorPrefix}, ExchangeRate is null or empty.");
            }

            if (string.IsNullOrEmpty(header.SalesInvoiceAmount))
            {
                throw new ArgumentException("SalesInvoiceAmount", $"{errorPrefix}, SalesInvoiceAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TotalDiscountAmount))
            {
                throw new ArgumentException("TotalDiscountAmount", $"{errorPrefix}, TotalDiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.GrossAmount))
            {
                throw new ArgumentException("GrossAmount", $"{errorPrefix}, GrossAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TransactionTime))
            {
                throw new ArgumentException("TransactionTime", $"{errorPrefix}, TransactionTime is null or empty.");
            }

            if (string.IsNullOrEmpty(header.CreatedOffline))
            {
                throw new ArgumentException("CreatedOffline", $"{errorPrefix}, CreatedOffline is null or empty.");
            }

            if (string.IsNullOrEmpty(header.Currency))
            {
                throw new ArgumentException("Currency", $"{errorPrefix}, Currency is null or empty.");
            }

            if (string.IsNullOrEmpty(header.SaleIsReturnSale))
            {
                throw new ArgumentException("SaleIsReturnSale", $"{errorPrefix}, SaleIsReturnSale is null or empty.");
            }

            if (string.IsNullOrEmpty(header.TotalManualDiscountAmount))
            {
                throw new ArgumentException("TotalManualDiscountAmount", $"{errorPrefix}, TotalManualDiscountAmount is null or empty.");
            }
        }

        public override string ToString()
        {
            // Using Newtonsoft.Json to serialize the object to JSON
            return JsonConvert.SerializeObject(this);
        }
    }
}
