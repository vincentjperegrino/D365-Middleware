using System;


namespace KTI.Moo.Extensions.Core.Model
{
    public class PaymentInfoBase
    {
        public virtual decimal Amount { get; set; }
        public virtual DateTime PaymentDate { get; set; }
        public virtual string PaymentMode { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string CurrencyCode { get; set; }
        public virtual decimal CustomerPoint { get; set; }
        public virtual string CardNumber { get; set; }
        public virtual string ApprovalCode { get; set; }
        public virtual string DocumentNumber { get; set; }
    }
}
