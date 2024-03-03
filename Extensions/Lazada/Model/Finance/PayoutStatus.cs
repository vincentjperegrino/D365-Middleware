
using Newtonsoft.Json;
using System;
using System.Linq;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class PayoutStatus
    {
        private decimal _payout;
        private string _payout_suffix;

        private DateTime? _created_at;
        private DateTime? _updated_at;

        [JsonIgnore]
        public decimal subtotal2 { get; set; }
        //[JsonProperty("subtotal1")]
        //public string String_subtotal1
        //{
        //    get => _subtotal1.ToString();
        //    set => decimal.TryParse(value, out _subtotal1);
        //}

        [JsonProperty("subtotal1")]
        public decimal subtotal1 { get; set; }


        [JsonProperty("shipment_fee_credit")]
        public decimal shipment_fee_credit { get; set; }


        [JsonProperty("payout")]
        public string String_payout
        {
            get => !string.IsNullOrEmpty(_payout_suffix) ? _payout.ToString() + " " + _payout_suffix : _payout.ToString();

            set
            {
                string val = new(value.Where(stringValue => char.IsDigit(stringValue)).ToArray());
                decimal.TryParse(val, out _payout);

                _payout_suffix = new(value.Where(stringValue => char.IsLetter(stringValue)).ToArray());
            }
        }

        [JsonIgnore]
        public decimal payout
        {
            get => _payout;
            set => _payout = value;
        }


        [JsonProperty("item_revenue")]
        public decimal item_revenue { get; set; }

        public DateTime? created_at
        {
            get => _created_at;
            set => _created_at = value is null ? null : ToUTC(value.Value);
        }

        [JsonProperty("other_revenue_total")]
        public decimal other_revenue_total { get; set; }

        [JsonProperty("fees_total")]
        public decimal fees_total { get; set; }

        [JsonProperty("refunds")]
        public decimal refunds { get; set; }

        [JsonProperty("guarantee_deposit")]
        public decimal guarantee_deposit { get; set; }

        public DateTime? updated_at
        {
            get => _updated_at;
            set => _updated_at = value is null ? null : ToUTC(value.Value);
        }


        [JsonProperty("fees_on_refunds_total")]
        public decimal fees_on_refunds_total { get; set; }

        [JsonProperty("closing_balance")]
        public decimal closing_balance { get; set; }

        [JsonProperty("paid")]
        public decimal paid { get; set; }

        [JsonProperty("opening_balance")]
        public decimal opening_balance { get; set; }

        [JsonProperty("statement_number")]
        public string statement_number { get; set; }

        [JsonProperty("shipment_fee")]
        public decimal shipment_fee { get; set; }


        private static DateTime ToUTC(DateTime dates)
        {
            return dates.ToUniversalTime();

        }
    }


}
