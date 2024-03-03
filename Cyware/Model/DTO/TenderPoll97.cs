using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class TenderPoll97
    {
        PollMapping helper = new();

        [SortOrder(1)]
        [MaxLength(29)]
        public virtual string tender_cd { get; set; }
        [SortOrder(2)]
        [MaxLength(8)]
        public virtual string tender_type_cd { get; set; }
        [SortOrder(3)]
        [MaxLength(40)]
        public virtual string description { get; set; }
        [SortOrder(4)]
        [MaxLength(16)]
        public virtual string is_default { get; set; }
        [SortOrder(5)]
        [MaxLength(29)]
        public virtual string currency_cd { get; set; }
        [SortOrder(6)]
        [MaxLength(16)]
        public virtual string validation_spacing { get; set; }
        [SortOrder(7)]
        [MaxLength(16)]
        public virtual string max_change { get; set; }
        [SortOrder(8)]
        [MaxLength(16)]
        public virtual string change_currency_code { get; set; }
        [SortOrder(9)]
        [MaxLength(10)]
        public virtual string mms_code { get; set; }
        [SortOrder(10)]
        [MaxLength(16)]
        public virtual string display_subtotal { get; set; }
        [SortOrder(11)]
        [MaxLength(16)]
        public virtual string min_amount { get; set; }
        [SortOrder(12)]
        [MaxLength(16)]
        public virtual string max_amount { get; set; }
        [SortOrder(13)]
        [MaxLength(16)]
        public virtual string is_layaway_refund { get; set; }
        [SortOrder(14)]
        [MaxLength(16)]
        public virtual string max_refund { get; set; }
        [SortOrder(15)]
        [MaxLength(16)]
        public virtual string refund_type { get; set; }
        [SortOrder(16)]
        [MaxLength(16)]
        public virtual string is_mobile_payment { get; set; }
        [SortOrder(17)]
        [MaxLength(16)]
        public virtual string is_account { get; set; }
        [SortOrder(18)]
        [MaxLength(10)]
        public virtual string acct_type_code { get; set; }
        [SortOrder(19)]
        [MaxLength(16)]
        public virtual string is_manager { get; set; }
        [SortOrder(20)]
        [MaxLength(10)]
        public virtual string garbage_tender_cd { get; set; }
        [SortOrder(21)]
        [MaxLength(29)]
        public virtual string rebate_tender_cd { get; set; }
        [SortOrder(22)]
        [MaxLength(16)]
        public virtual string rebate_percent { get; set; }
        [SortOrder(23)]
        [MaxLength(16)]
        public virtual string is_cashfund { get; set; }
        [SortOrder(24)]
        [MaxLength(16)]
        public virtual string is_takeout { get; set; }
        [SortOrder(25)]
        [MaxLength(32)]
        public virtual string item_code { get; set; }
        [SortOrder(26)]
        [MaxLength(35)]
        public virtual string surcharge_sku { get; set; }
        [SortOrder(27)]
        [MaxLength(30)]
        public virtual string mobile_payment_number { get; set; }
        [SortOrder(28)]
        [MaxLength(30)]
        public virtual string mobile_payment_return { get; set; }
        [SortOrder(29)]
        [MaxLength(16)]
        public virtual string is_padss { get; set; }
        [SortOrder(30)]
        [MaxLength(16)]
        public virtual string is_credit_card { get; set; }
        [SortOrder(31)]
        [MaxLength(10)]
        public virtual string eft_port { get; set; }
        [SortOrder(32)]
        [MaxLength(16)]
        public virtual string is_voucher { get; set; }
        [SortOrder(33)]
        [MaxLength(16)]
        public virtual string discount_cd { get; set; }

        public TenderPoll97(Tender _tender)
        {
            this.tender_cd = helper.FormatStringAddSpacePadding(_tender.tender_cd, (typeof(TenderPoll97).GetProperty("tender_cd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.tender_type_cd = helper.FormatStringAddSpacePadding(_tender.tender_type_cd, (typeof(TenderPoll97).GetProperty("tender_type_cd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.description = helper.FormatStringAddSpacePadding(_tender.description, (typeof(TenderPoll97).GetProperty("description").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_default = helper.FormatStringAddSpacePadding(_tender.is_default, (typeof(TenderPoll97).GetProperty("is_default").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.currency_cd = helper.FormatStringAddSpacePadding(_tender.currency_cd, (typeof(TenderPoll97).GetProperty("currency_cd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.validation_spacing = helper.FormatStringAddSpacePadding(_tender.validation_spacing, (typeof(TenderPoll97).GetProperty("validation_spacing").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.max_change = helper.FormatStringAddSpacePadding(_tender.max_change, (typeof(TenderPoll97).GetProperty("max_change").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.change_currency_code = helper.FormatStringAddSpacePadding(_tender.change_currency_code, (typeof(TenderPoll97).GetProperty("change_currency_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.mms_code = helper.FormatStringAddSpacePadding(_tender.mms_code, (typeof(TenderPoll97).GetProperty("mms_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.display_subtotal = helper.FormatStringAddSpacePadding(_tender.display_subtotal, (typeof(TenderPoll97).GetProperty("display_subtotal").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.min_amount = helper.FormatStringAddSpacePadding(_tender.min_amount, (typeof(TenderPoll97).GetProperty("min_amount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.max_amount = helper.FormatStringAddSpacePadding(_tender.max_amount, (typeof(TenderPoll97).GetProperty("max_amount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_layaway_refund = helper.FormatStringAddSpacePadding(_tender.is_layaway_refund, (typeof(TenderPoll97).GetProperty("is_layaway_refund").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.max_refund = helper.FormatStringAddSpacePadding(_tender.max_refund, (typeof(TenderPoll97).GetProperty("max_refund").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.refund_type = helper.FormatStringAddSpacePadding(_tender.refund_type, (typeof(TenderPoll97).GetProperty("refund_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_mobile_payment = helper.FormatStringAddSpacePadding(_tender.is_mobile_payment, (typeof(TenderPoll97).GetProperty("is_mobile_payment").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_account = helper.FormatStringAddSpacePadding(_tender.is_account, (typeof(TenderPoll97).GetProperty("is_account").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.acct_type_code = helper.FormatStringAddSpacePadding(_tender.acct_type_code, (typeof(TenderPoll97).GetProperty("acct_type_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_manager = helper.FormatStringAddSpacePadding(_tender.is_manager, (typeof(TenderPoll97).GetProperty("is_manager").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.garbage_tender_cd = helper.FormatStringAddSpacePadding(_tender.garbage_tender_cd, (typeof(TenderPoll97).GetProperty("garbage_tender_cd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.rebate_tender_cd = helper.FormatStringAddSpacePadding(_tender.rebate_tender_cd, (typeof(TenderPoll97).GetProperty("rebate_tender_cd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.rebate_percent = helper.FormatStringAddSpacePadding(_tender.rebate_percent, (typeof(TenderPoll97).GetProperty("rebate_percent").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_cashfund = helper.FormatStringAddSpacePadding(_tender.is_cashfund, (typeof(TenderPoll97).GetProperty("is_cashfund").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_takeout = helper.FormatStringAddSpacePadding(_tender.is_takeout, (typeof(TenderPoll97).GetProperty("is_takeout").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.item_code = helper.FormatStringAddSpacePadding(_tender.item_code, (typeof(TenderPoll97).GetProperty("item_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.surcharge_sku = helper.FormatStringAddSpacePadding(_tender.surcharge_sku, (typeof(TenderPoll97).GetProperty("surcharge_sku").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.mobile_payment_number = helper.FormatStringAddSpacePadding(_tender.mobile_payment_number, (typeof(TenderPoll97).GetProperty("mobile_payment_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.mobile_payment_return = helper.FormatStringAddSpacePadding(_tender.mobile_payment_return, (typeof(TenderPoll97).GetProperty("mobile_payment_return").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_padss = helper.FormatStringAddSpacePadding(_tender.is_padss, (typeof(TenderPoll97).GetProperty("is_padss").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_credit_card = helper.FormatStringAddSpacePadding(_tender.is_credit_card, (typeof(TenderPoll97).GetProperty("is_credit_card").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.eft_port = helper.FormatStringAddSpacePadding(_tender.eft_port, (typeof(TenderPoll97).GetProperty("eft_port").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.is_voucher = helper.FormatStringAddSpacePadding(_tender.is_voucher, (typeof(TenderPoll97).GetProperty("is_voucher").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.discount_cd = helper.FormatStringAddSpacePadding(_tender.discount_cd, (typeof(TenderPoll97).GetProperty("discount_cd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(TenderPoll97 obj)
        {
            return helper.ConcatenateValues(obj);
        }
    }
}
