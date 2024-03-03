using KTI.Moo.Extensions.Cyware.App.Receiver.Models;

namespace Moo.FO.Model.DTO
{
    public class StoreTransactionTenderDTO : StoreTransactionTenders
    {
        public StoreTransactionTenderDTO(FOSalesTenderDetail transactionTender, D365FOConfig config)
        {
            Validate(transactionTender);

            string storeNumber = transactionTender.Store;

            this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception("dataAreaId is null or empty");
            this.TransactionNumber = transactionTender.TransactionNumber;
            this.Terminal = transactionTender.Terminal.Length < 6 ? transactionTender.Terminal.PadLeft(6, '0') : transactionTender.Terminal;
            this.OperatingUnitNumber = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception("OperatingUnitNumber is null or empty");
            this.LineNumber = int.Parse(transactionTender.LineNumber);
            this.CurrencyCode = transactionTender.CurrencyCode;
            this.ReceiptId = transactionTender.ReceiptId;
            this.AmountTendered = Decimal.Parse(transactionTender.AmountTendered);
            this.AmountInTenderedCurrency = Decimal.Parse(transactionTender.AmountInTenderedCurrency);
            this.Store = storeNumber;
            this.TransactionStatus = "Posted";
            this.IsLinkedRefund = transactionTender.IsLinkedRefund;
            this.TenderType = transactionTender.TenderType;
            this.IsPaymentCaptured = "No";
            this.ExchangeRateInTenderedCurrency = Decimal.Parse(transactionTender.ExchangeRateInTenderedCurrency);
            this.ExchangeRateInAccountingCurrency = Decimal.Parse(transactionTender.ExchangeRateInTenderedCurrency);
            this.GiftCardId = transactionTender.GiftCardId;
            this.LinkedPaymentTransactionNumber = transactionTender.LinkedPaymentTransactionNumber;
            this.PaymentCaptureToken = transactionTender.PaymentCaptureToken;
        }

        private static void Validate(FOSalesTenderDetail tender)
        {
            if (string.IsNullOrEmpty(tender.Terminal))
            {
                throw new ArgumentException("Terminal", "Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.LineNumber))
            {
                throw new ArgumentException("LineNumber", "LineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.Store))
            {
                throw new ArgumentException("Store", "Store is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.ExchangeRateInTenderedCurrency))
            {
                throw new ArgumentException("ExchangeRateInTenderedCurrency", "ExchangeRateInTenderedCurrency is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.AmountInTenderedCurrency))
            {
                throw new ArgumentException("AmountInTenderedCurrency", "AmountInTenderedCurrency is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.AmountTendered))
            {
                throw new ArgumentException("AmountTendered", "AmountTendered is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.ReceiptId))
            {
                throw new ArgumentException("ReceiptId", "ReceiptId is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.TenderType))
            {
                throw new ArgumentException("TenderType", "TenderType is null or empty.");
            }
        }

        public StoreTransactionTenderDTO() { }

        //public StoreTransactionTenderDTO(Extensions.Cyware.App.Receiver.Models.SalesTenderDetail transactionTender, int lineNumber, D365FOConfig config)
        //{
        //    Validate(transactionTender);
        //    bool returnTrans = false;

        //    this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == transactionTender.StoreNumber && !string.IsNullOrEmpty(rs.DefaultCustomerLegalEntity)).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? ""; // direct
        //    this.TransactionNumber = transactionTender.TransNumber;
        //    this.Terminal = transactionTender.RegisterID; // direct
        //    this.OperatingUnitNumber = transactionTender.StoreNumber; // direct
        //    this.LineNumber = lineNumber;
        //    this.CurrencyCode = transactionTender.CurrencyCode;
        //    this.ReceiptId = transactionTender.TransNumber;
        //    this.AmountTendered = Decimal.Parse(String.IsNullOrEmpty(transactionTender.TenderedAmount) ? "0" : transactionTender.TenderedAmount) / 1000;//need to change
        //    this.AmountInTenderedCurrency = Decimal.Parse(String.IsNullOrEmpty(transactionTender.ForeignCCYAmount) ? "0" : transactionTender.ForeignCCYAmount) / 1000;//need to change
        //    this.Store = transactionTender.StoreNumber; // direct
        //    this.CurrencyCode = transactionTender.CurrencyCode;
        //    this.LineNumber = lineNumber;
        //    this.TransactionStatus = "Posted"; // config
        //    this.IsLinkedRefund = "No"; // direct/config
        //    this.TenderType = transactionTender.TransactionType; // direct, remove switch case
        //    this.IsPaymentCaptured = "No"; // direct/config
        //    this.ExchangeRateInTenderedCurrency = Decimal.Parse(transactionTender.ExchangeRate);
        //    this.ExchangeRateInAccountingCurrency = Decimal.Parse(transactionTender.ExchangeRate);
        //    this.GiftCardId = "";
        //    this.LinkedPaymentTransactionNumber = transactionTender.TransNumber;
        //    this.PaymentCaptureToken = "No";
        //}

        //private static void Validate(SalesTenderDetail transactionTender)
        //{
        //    if (string.IsNullOrEmpty(transactionTender.TransNumber))
        //    {
        //        throw new ArgumentException("TransNumber", "TransNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.RegisterID))
        //    {
        //        throw new ArgumentException("RegisterID", "RegisterID is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.TenderedAmount))
        //    {
        //        throw new ArgumentException("TenderedAmount", "TenderedAmount is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.StoreNumber))
        //    {
        //        throw new ArgumentException("StoreNumber", "StoreNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.ForeignCCYAmount))
        //    {
        //        throw new ArgumentException("ForeignCCYAmount", "ForeignCCYAmount is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.TransactionType))
        //    {
        //        throw new ArgumentException("TransactionType", "TransactionType is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.ExchangeRate))
        //    {
        //        throw new ArgumentException("ExchangeRate", "ExchangeRate is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionTender.CurrencyCode))
        //    {
        //        throw new ArgumentException("CurrencyCode", "CurrencyCode is null or empty.");
        //    }
        //}
    }
}
