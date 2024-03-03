using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class TenderBase
    {
        public virtual string tender_cd { get; set; }
        public virtual string tender_type_cd { get; set; }
        public virtual string description { get; set; }
        public virtual string is_default { get; set; }
        public virtual string currency_cd { get; set; }
        public virtual string validation_spacing { get; set; }
        public virtual string max_change { get; set; }
        public virtual string change_currency_code { get; set; }
        public virtual string mms_code { get; set; }
        public virtual string display_subtotal { get; set; }
        public virtual string min_amount { get; set; }
        public virtual string max_amount { get; set; }
        public virtual string is_layaway_refund { get; set; }
        public virtual string max_refund { get; set; }
        public virtual string refund_type { get; set; }
        public virtual string is_mobile_payment { get; set; }
        public virtual string is_account { get; set; }
        public virtual string acct_type_code { get; set; }
        public virtual string is_manager { get; set; }
        public virtual string garbage_tender_cd { get; set; }
        public virtual string rebate_tender_cd { get; set; }
        public virtual string rebate_percent { get; set; }
        public virtual string is_cashfund { get; set; }
        public virtual string is_takeout { get; set; }
        public virtual string item_code { get; set; }
        public virtual string surcharge_sku { get; set; }
        public virtual string mobile_payment_number { get; set; }
        public virtual string mobile_payment_return { get; set; }
        public virtual string is_padss { get; set; }
        public virtual string is_credit_card { get; set; }
        public virtual string eft_port { get; set; }
        public virtual string is_voucher { get; set; }
        public virtual string discount_cd { get; set; }
    }
}
