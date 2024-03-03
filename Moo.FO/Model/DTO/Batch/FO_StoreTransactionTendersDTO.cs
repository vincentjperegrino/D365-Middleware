using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model.DTO.Batch
{
    public class FO_StoreTendersDTO : StoreTransactionTenders
    {
        private static string errorPrefix = "";
        public FO_StoreTendersDTO(FOSalesTenderDetail tender, D365FOConfig config)
        {
            Validate(tender);
            try
            {
                string storeNumber = tender.Store;

                this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, dataAreaId is null or empty");
                this.TransactionNumber = tender.TransactionNumber;
                this.Terminal = config.Terminals.Where(rt => rt.StoreNumber == storeNumber && rt.Name == tender.Terminal).Select(rt => rt.TerminalNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, Terminal is null or empty");
                this.OperatingUnitNumber = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, OperatingUnitNumber is null or empty");
                this.LineNumber = int.Parse(tender.LineNumber);
                this.CurrencyCode = tender.CurrencyCode;
                this.ReceiptId = tender.ReceiptId;
                this.AmountTendered = Decimal.Parse(tender.AmountTendered);
                this.AmountInTenderedCurrency = Decimal.Parse(tender.AmountInTenderedCurrency);
                this.Store = storeNumber;
                this.TransactionStatus = "Posted";
                this.IsLinkedRefund = tender.IsLinkedRefund;
                this.TenderType = tender.TenderType;
                this.IsPaymentCaptured = "No";
                this.ExchangeRateInTenderedCurrency = Decimal.Parse(tender.ExchangeRateInTenderedCurrency);
                this.ExchangeRateInAccountingCurrency = Decimal.Parse(tender.ExchangeRateInTenderedCurrency);
                this.GiftCardId = tender.GiftCardId;
                this.LinkedPaymentTransactionNumber = tender.LinkedPaymentTransactionNumber;
                this.PaymentCaptureToken = tender.PaymentCaptureToken;
            }
            catch (Exception ex)
            {
                throw new Exception($"{errorPrefix}, {ex.Message}");
            }
        }

        private static void Validate(FOSalesTenderDetail tender)
        {
            if (string.IsNullOrEmpty(tender.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }
            errorPrefix = $"Tenders Transaction Number: {tender.TransactionNumber}";

            if (string.IsNullOrEmpty(tender.Terminal))
            {
                throw new ArgumentException("Terminal", $"{errorPrefix}, Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.LineNumber))
            {
                throw new ArgumentException("LineNumber", $"{errorPrefix}, LineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.Store))
            {
                throw new ArgumentException("Store", $"{errorPrefix}, Store is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.ExchangeRateInTenderedCurrency))
            {
                throw new ArgumentException("ExchangeRateInTenderedCurrency", $"{errorPrefix}, ExchangeRateInTenderedCurrency is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.AmountInTenderedCurrency))
            {
                throw new ArgumentException("AmountInTenderedCurrency", $"{errorPrefix}, AmountInTenderedCurrency is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.AmountTendered))
            {
                throw new ArgumentException("AmountTendered", $"{errorPrefix}, AmountTendered is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.ReceiptId))
            {
                throw new ArgumentException("ReceiptId", $"{errorPrefix}, ReceiptId is null or empty.");
            }

            if (string.IsNullOrEmpty(tender.TenderType))
            {
                throw new ArgumentException("TenderType", $"{errorPrefix}, TenderType is null or empty.");
            }
        }

        public override string ToString()
        {
            // Using Newtonsoft.Json to serialize the object to JSON
            return JsonConvert.SerializeObject(this);
        }
    }
}
