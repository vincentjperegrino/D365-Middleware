using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class FOSalesTenderDetail
    {
        public FOSalesTenderDetail()
        {
        }

        public FOSalesTenderDetail(string[] records)
        {
            this.dataAreaId = records[1];
            //this.TransactionNumber = $"R-{records[2]}-1";
            //this.ReceiptId = $"R-{records[3]}-1";
            this.TransactionNumber = records[2];
            this.ReceiptId = records[3];
            this.Terminal = records[4];
            this.OperatingUnitNumber = records[5];
            this.Store = records[6];
            this.LineNumber = records[7];
            this.AmountInTenderedCurrency = records[8];
            this.AmountTendered = records[9];
            this.TransactionStatus = records[10];
            this.GiftCardId = records[11];
            this.ExchangeRateInTenderedCurrency = records[12];
            this.IsPaymentCaptured = records[13];
            this.LinkedPaymentTransactionNumber = records[14];
            this.IsLinkedRefund = records[15];
            this.CurrencyCode = records[16];
            this.TenderType = records[17];
            this.PaymentCaptureToken = records[18];
            //this.ExchangeRateInAccountingCurrency = records[19];
        }

        public string dataAreaId { get; set; }
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public string LineNumber { get; set; }
        public string OperatingUnitNumber { get; set; }
        public string Store { get; set; }
        public string TransactionStatus { get; set; }
        public string GiftCardId { get; set; }
        public string ExchangeRateInTenderedCurrency { get; set; }
        public string IsPaymentCaptured { get; set; }
        public string LinkedPaymentTransactionNumber { get; set; }
        public string AmountInTenderedCurrency { get; set; }
        public string IsLinkedRefund { get; set; }
        public string AmountTendered { get; set; }
        public string CurrencyCode { get; set; }
        public string ReceiptId { get; set; }
        public string TenderType { get; set; }
        public string PaymentCaptureToken { get; set; }
        public string ExchangeRateInAccountingCurrency { get; set; }
    }
}
