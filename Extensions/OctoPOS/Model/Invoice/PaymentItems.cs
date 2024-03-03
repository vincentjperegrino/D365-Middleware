using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;


namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class PaymentItems : PaymentInfoBase
    {

        [JsonProperty("Amount")]
        public override decimal Amount { get; set; }

        [JsonProperty("PaymentDate")]
        public override DateTime PaymentDate { get; set; }

        [JsonProperty("PaymentMode")]
        public override string PaymentMode { get; set; }

        [JsonProperty("Remarks")]
        public override string Remarks { get; set; }

        [JsonProperty("CurrencyCode")]
        public override string CurrencyCode { get; set; }

        [JsonProperty("CustomerPoint")]
        public override decimal CustomerPoint { get; set; }

        [JsonProperty("CardNumber")]
        public override string CardNumber { get; set; }

        [JsonProperty("ApprovalCode")]
        public override string ApprovalCode { get; set; }

        [JsonProperty("DocumentNumber")]
        public override string DocumentNumber { get; set; }

        [JsonProperty("Extrapayment")]
        public decimal Extrapayment { get; set; }

    }
}
