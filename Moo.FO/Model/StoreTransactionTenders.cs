using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model
{
    public class StoreTransactionTenders
    {
        public string dataAreaId { get; set; }
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public int LineNumber { get; set; }
        public string OperatingUnitNumber { get; set; }
        public string Store { get; set; }
        public string TransactionStatus { get; set; }
        public string GiftCardId { get; set; }
        public decimal ExchangeRateInTenderedCurrency { get; set; }
        public string IsPaymentCaptured { get; set; }
        public string LinkedPaymentTransactionNumber { get; set; }
        public decimal AmountInTenderedCurrency { get; set; }
        public string IsLinkedRefund { get; set; }
        public decimal AmountTendered { get; set; }
        public string CurrencyCode { get; set; }
        public string ReceiptId { get; set; }
        public string TenderType { get; set; }
        public string PaymentCaptureToken { get; set; }
        public decimal ExchangeRateInAccountingCurrency { get; set; }
        //public decimal AmountInAccountingCurrency { get; set; }
        //public string CardOrAccount { get; set; }
        //public string CardTypeID { get; set; }
        //public int Channel { get; set; }
        //public string CreditVoucherID { get; set; }
        //public int IsChangeLine { get; set; }
        //public int IsPrePayment { get; set; }
        //public int LinkedPaymentLineNumber { get; set; }
        //public string LinkedPaymentStore { get; set; }
        //public string LinkedPaymentTerminal { get; set; }
        //public string LinkedPaymentCurrency { get; set; }
        //public string LoyaltyCardID { get; set; }
        //public decimal RefundableAmount { get; set; }
        //public int Quantity { get; set; }
        //public string Staff { get; set; }
        //public int VoidStatus { get; set; }
        //public DateTimeOffset ModifiedDateTime { get; set; }
        //public string ModifiedBy { get; }
        //public DateTimeOffset CreatedDateTime { get; set; }
        //public string CreatedBy { get; }
        //public int RecVersion { get; set; }
        //public int Partition { get; set; }
        //public int RecID { get; set; }
        //public int RetailChannelTableOMOperatingUnitID { get; set; }
        //public int RecVersion2 { get; set; }
        //public int Partition2 { get; set; }
        //public int RecID2 { get; set; }
        //public decimal AmountTenderedAdjustment { get; set; }
        //public int RecVersion4 { get; set; }
        //public int Partition4 { get; set; }
        //public int RecID4 { get; set; }
        //public int Function_ { get; set; }
        //public int RecVersion5 { get; set; }
        //public int Partition5 { get; set; }
        //public int RecID5 { get; set; }
        //public int RecVersion3 { get; set; }
        //public int Partition3 { get; set; }
        //public int RecID3 { get; set; }
        //public string AccountNumber { get; set; }
        //public string CardNumber { get; set; }
    }
}
