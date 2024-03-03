using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCyware.App.Model.Data
{
    public class Retail_Customer_Simple_Fixed_Percentage
    {
        public static string company = "rwri";
        public static string customer_id = "CUS-000000022";
        public static string current_timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        public static string terminal = "000001";
        public static string transaction_number = $"JOELPOSTEST{current_timestamp}";
        public static string receipt_id = $"{terminal}-{current_timestamp}";
        public static string transaction_date = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        public static string min_date = "1900-01-01T00:00:00Z";
        public static string discount_percentage = "8";
        public static List<Helper.CalculationHelper.Item> items = new()
        {
            new Helper.CalculationHelper.Item
            {
                Name = "AO00000902",
                Price = 120.00m,
                Quantity = 1,
                Discount = 9.6m
            }
        };
        public static decimal total_amount = Helper.CalculationHelper.CalculateTotalAmount(items);

        public static List<FOSalesTransactionHeader> SalesTransactionHeaders = new()
        {
            new FOSalesTransactionHeader
            {
                dataAreaId = company, // company or legal entity
                TransactionNumber = transaction_number, // primary key
                OperatingUnitNumber = "", // store ownership
                Terminal = terminal, // register
                Shift = "", // optional
                IsTaxExemptedForPriceInclusive = "No", // when tax code is 0, No
                BatchID = "0", // optional
                LogisticsPostalZipCode = "", // customer zip code
                RreceiptId = receipt_id, // receipt number
                DiscountAmount = items[0].Discount.ToString(), // total discount amount
                NetPrice = "0.00", // net amount due
                TotalManualDiscountPercentage = discount_percentage, // total manual discount %
                CustomerAccount = customer_id, // possbile scenarios: specific customer(customerID), walk-in customer("")
                TransactionOrderType = "SalesOrder", // default value on sales
                CostAmount = "0.00", // optional, purchase cost
                AmountPostedToAccount = "0.00", // optional
                ChannelReferenceId = "000004", // store number
                TransactionDate = transaction_date, // date of transaction
                LogisticPostalAddressValidTo = transaction_date, // optional, default min value
                NumberOfItemLines = "0", // optional
                PaymentAmount = "0.00", // amount due
                TransactionStatus = "None", // default
                NumberOfPaymentLines = "0", // optional
                Comment = "", // remarks
                Warehouse = "", // warehouse number
                CustomerName = "", // name of the customer
                ToAccount = "No", // default
                TransactionType = "AsyncCustomerOrder", // default
                GiftCardBalance = "0", // phase 2
                ShippingDateRequested = transaction_date, // sames as transaction date
                LogisticsPostalState = "", // optional
                SiteId = "", // warehouse site id
                IsTaxIncludedInPrice = "No", // depends if net or gross
                PostAsShipment = "No", // default
                TaxCalculationType = "Regular", // default
                SalesOrderId = "", // default
                GiftCardIdMasked = "", // phase 2
                ItemsPosted = "No", // default
                RetailTransactionAggregationFieldList = "0", // default
                LoyaltyCardId = "", // phase 2
                NetAmount = "0.00", // total net amount due
                LogisticsPostalStreet = "", // customer address
                LogisticsPostalCity = "", // customer address
                InfocodeDiscountGroup = "", // default
                DeliveryMode = "", // mode of delivery
                Staff = "", // staff on shift, optional
                CustomerDiscountAmount = items[0].Discount.ToString(), // total customer discount amount
                SalesPaymentDifference = "0", // payment balance
                SuspendedTransactionId = "", // optional
                ExchangeRate = "1.00", // PHP
                SalesInvoiceAmount = "0.00", // payment due
                RefundReceiptId = "", // refund receipt
                BatchTerminalId = "", // default
                TotalDiscountAmount = items[0].Discount.ToString(), // total discount amount of non-manual
                InvoiceId = "", // default
                DiscountAmountWithoutTax = items[0].Discount.ToString(), // optional
                SaleOnAccount = "No", // optional
                IncomeExpenseAmount = "0", // optional
                NumberOfItems = "0", // optional
                GrossAmount = "0.00", // total gross amount with tax
                LogisticsLocationId = "", // optional
                CreatedOnPosTerminal = "", // optional
                TransactionTime = "43200", // integer time of transaction
                LanguageId = "", // config
                CreatedOffline = "Yes", // depends from POS
                GiftCardIssueAmount = "0", // phase 2
                Currency = "PHP", // from POS
                SaleIsReturnSale = "No", // conditional, yes if sale is return
                LogisticsPostalCounty = "", // customer address
                GiftCardHistoryDetails = "", // phase 2
                SalesOrderAmount = "0", // default
                TotalManualDiscountAmount = items[0].Discount.ToString(), // total manual discount amount
                GiftCardActiveFrom = min_date,
                GiftCardExpireDate = min_date,
                BeginDateTime = transaction_date,
                businessDate = transaction_date,
                LogisticsPostalAddressValidFrom = min_date
            }
        };

        public static List<FOSalesTransactionDetail> SalesTransactionLines = new()
        {
            new FOSalesTransactionDetail
            {
                dataAreaId = company, // company or legal entity
                Terminal = terminal, // register
                TransactionNumber = transaction_number, // primary key
                LineNumber = "1", // line number of item
                OperatingUnitNumber = "", // store ownership
                CustomerDiscount = items[0].Discount.ToString(), // specific discount per line
                RequestedReceiptDate = transaction_date, // transaction date
                CashDiscountAmount = items[0].Discount.ToString(), // optional
                TransactionDate = transaction_date,
                OriginalItemSalesTaxGroup = "", // item tax code
                SiteId = "", // warehouse site id
                RetailEmailAddressContent = "", // optional
                ItemSize = "", // optional
                ModeOfDelivery = "Pick-up", // default
                SkipReports = "No", // default
                Warehouse = "", // warehouse number
                PickupStartTime = "0", // default
                TotalDiscount = items[0].Discount.ToString(), // total discount of line item
                TaxExemptPriceInclusiveOriginalPrice = "0", // optional
                IsLinkedProductNotOriginal = "No", // optional
                DiscountAmountWithoutTax = items[0].Discount.ToString(), // optional
                NetPrice = items[0].Price.ToString(), // with tax
                PriceGroups = "", // optional
                RFIDTagId = "", // optional
                VariantNumber = "", // optional
                CategoryHierarchyName = "", // optional
                TransactionCode = "ItemOnFile", // default
                TotalDiscountPercentage = discount_percentage, // total discount percentage of line
                FixedPriceCharges = "0", // default
                Unit = "ORD", // unit of measurement of item
                TaxExemptPriceInclusiveReductionAmount = "0", // optional
                PriceInBarCode = "No", // default
                LotID = "", // default
                ReturnQuantity = "0", // possible to change
                CustomerAccount = customer_id,
                OriginalPrice = "0", // default
                ItemRelation = "", // default
                RequestedShipDate = transaction_date, // transaction date
                TransactionStatus = "Posted", // default
                ItemConfigId = "", // default
                Currency = "PHP", // depends on POS
                ReturnOperatingUnitNumber = "",
                LineDiscount = items[0].Discount.ToString(), // total line discount with code
                NetAmountInclusiveTax = items[0].Price.ToString(), // gross amount of line
                InventoryStatus = "None", // 
                LineManualDiscountPercentage = discount_percentage, // total manual discount % per line
                GiftCard = "No", // phase 2
                ChannelListingID = "", // optional
                NetAmount = items[0].Price.ToString(), // net amount of line with tax
                ItemColor = "", // optional
                IsPriceChange = "No", // optional
                BarCode = "", // depends on POS
                UnitQuantity = "0",
                LineManualDiscountAmount = items[0].Discount.ToString(), // 
                ItemVersion = "", // optional
                ReturnTerminal = "", // which terminal returned
                IsWeightProduct = "No", // depends if product is sold by weight
                LogisticLocationId = "", // default
                IsScaleProduct = "No", // same with weight product
                IsOriginalOfLinkedProductList = "No", // default
                ItemStyle = "", // default
                ReceiptNumber = receipt_id,
                LinePercentageDiscount = discount_percentage, // total line % discount
                ReasonCodeDiscount = "0", // optional
                CategoryName = "", // optional
                TotalDiscountInfoCodeLineNum = "0", // optional
                KeyboardProductEntry = "Yes", // default
                CancelledTransactionNumber = "", // default
                IsReturnNoSale = "No", // may vary on test case
                ElectronicDeliveryEmail = "", //  optional
                ItemId = items[0].Name, // sku
                SalesTaxAmount = "0", // total sales tax amount
                ReturnTransactionNumber = "", // transaction number for return
                Quantity = items[0].Quantity.ToString(), // number of items per line
                Price = items[0].Price.ToString(), // gross amount per item
                UnitPrice = items[0].Price.ToString(), // net amount per item
                SalesTaxGroup = "", // tax code
                OriginalSalesTaxGroup = "", // tax code
                IsLineDiscounted = "", // default
                ShelfNumber = "", // optional
                ItemSalesTaxGroup = "OVAT-G", // default
                SerialNumber = "", // default   
                OfferNumber = "", // default
                CostAmount = "", // default
                SectionNumber = "", // default
                IsWeightManuallyEntered = "No", // default
                DiscountAmountForPrinting = items[0].Discount.ToString(), // default
                CustomerInvoiceDiscountAmount = items[0].Discount.ToString(), // default
                ProductScanned = "No", // depends on POS
                PeriodicDiscountPercentage = discount_percentage, // discount % of line
                ReturnTrackingStatus = "None", // default
                ReturnLineNumber = "", // return line number
                PeriodicDiscountAmount = items[0].Discount.ToString(), // total discount amount of line
                PeriodicDiscountGroup = "", // optional
                PickupEndTime = "0", // optional
                BusinessDate = transaction_date,
                LogisticsPostalAddressValidFrom = min_date
            }
        };

        public static List<FOSalesTransactionDiscount> SalesTransactionDiscounts = new()
        {
            new FOSalesTransactionDiscount
            {
                dataAreaId = company,
                Terminal = terminal,
                TransactionNumber = transaction_number,
                SalesLineNumber = "1", // related to Store Transction line
                OperatingUnitNumber = "",
                LineNumber = "1", // line number of discount
                DiscountAmount = items[0].Discount.ToString(), // discount amount of line
                DiscountCost = "0", // optional
                DiscountOfferId = "", // optional
                DiscountOriginType = "None", // default
                DiscountCode = "", // code of discount
                DiscountPercentage = discount_percentage, // % of discount
                CustomerDiscountType = "None", // optional
                BundleId = "0", // default
                ManualDiscountType = "None", // default
                EffectiveAmount = items[0].Discount.ToString(), // total discount amount of line
                DealPrice = "0", // default
            }
        };

        public static List<FOSalesTenderDetail> SalesTransactionTenders = new()
        {
            new FOSalesTenderDetail
            {
                dataAreaId = company,
                Terminal = terminal,
                TransactionNumber = transaction_number,
                LineNumber = "1",
                OperatingUnitNumber = "",
                Store = "000004", // channel reference
                TransactionStatus = "Posted", // default
                GiftCardId = "", // phase 2
                ExchangeRateInTenderedCurrency = "1.00", // PHP default
                IsPaymentCaptured = "No", // default
                LinkedPaymentTransactionNumber = "", // default
                AmountInTenderedCurrency = total_amount.ToString(), // total payment amount due
                IsLinkedRefund = "No", // default
                AmountTendered = total_amount.ToString(), // total payment amount due
                CurrencyCode = "PHP", // currency
                ReceiptId = transaction_number,
                TenderType = "2", // depends on payment method on finops
                PaymentCaptureToken = "", // optional
                ExchangeRateInAccountingCurrency = "1.00",
            }
        };
    }
}
