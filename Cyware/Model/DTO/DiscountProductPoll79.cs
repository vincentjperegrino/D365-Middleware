using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class DiscountProductPoll79
    {

        [SortOrder(1)]
        [MaxLength(9)]
        public virtual string sku_number { get; set; }
        [SortOrder(2)]
        [MaxLength(18)]
        public virtual string upc_code { get; set; }
        [SortOrder(3)]
        [MaxLength(1)]
        public virtual string upc_type { get; set; }
        [SortOrder(4)]
        [MaxLength(6)]
        public virtual string price_event_number { get; set; }
        [SortOrder(5)]
        [MaxLength(3)]
        public virtual string currency_code { get; set; }
        [SortOrder(6)]
        [MaxLength(3)]
        public virtual string price_book { get; set; }
        [SortOrder(7)]
        [MaxLength(8)]
        public virtual string start_date { get; set; }
        [SortOrder(8)]
        [MaxLength(8)]
        public virtual string end_date { get; set; }
        [SortOrder(9)]
        [MaxLength(1)]
        public virtual string promo_flag_yn { get; set; }
        [SortOrder(10)]
        [MaxLength(3)]
        public virtual string event_price_multiple { get; set; }
        [SortOrder(11)]
        [MaxLength(13)]
        public virtual string event_price { get; set; }
        [SortOrder(12)]
        [MaxLength(1)]
        public virtual string price_method_code { get; set; }
        [SortOrder(13)]
        [MaxLength(3)]
        public virtual string mix_match_code { get; set; }
        [SortOrder(14)]
        [MaxLength(3)]
        public virtual string deal_quantity { get; set; }
        [SortOrder(15)]
        [MaxLength(13)]
        public virtual string deal_price { get; set; }
        [SortOrder(16)]
        [MaxLength(3)]
        public virtual string buy_quantity { get; set; }
        [SortOrder(17)]
        [MaxLength(13)]
        public virtual string buy_value { get; set; }
        [SortOrder(18)]
        [MaxLength(1)]
        public virtual string buy_value_type { get; set; }
        [SortOrder(19)]
        [MaxLength(7)]
        public virtual string qty_end_value { get; set; }
        [SortOrder(20)]
        [MaxLength(3)]
        public virtual string quantity_break { get; set; }
        [SortOrder(21)]
        [MaxLength(13)]
        public virtual string quantity_group_price { get; set; }
        [SortOrder(22)]
        [MaxLength(13)]
        public virtual string quantity_unit_price { get; set; }
        [SortOrder(23)]
        [MaxLength(1)]
        public virtual string cust_promo_code { get; set; }
        [SortOrder(24)]
        [MaxLength(10)]
        public virtual string cust_number { get; set; }
        [SortOrder(25)]
        [MaxLength(2)]
        public virtual string precedence_level { get; set; }
        [SortOrder(26)]
        [MaxLength(1)]
        public virtual string default_currency { get; set; }
        [SortOrder(27)]
        [MaxLength(1)]
        public virtual string default_price_book { get; set; }

        public DiscountProductPoll79(DiscountProduct _discountProduct)
        {
            var helper = new PollMapping();
            this.sku_number = helper.FormatIntAddZeroPrefix(_discountProduct.sku_number.ToString(), (typeof(DiscountProductPoll79).GetProperty("sku_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.upc_code = helper.FormatIntAddZeroPrefix(_discountProduct.upc_code.ToString(), (typeof(DiscountProductPoll79).GetProperty("upc_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.upc_type = helper.FormatStringAddSpacePadding(_discountProduct.upc_type, (typeof(DiscountProductPoll79).GetProperty("upc_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.price_event_number = helper.FormatIntAddZeroPrefix(_discountProduct.price_event_number.ToString(), (typeof(DiscountProductPoll79).GetProperty("price_event_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.currency_code = helper.FormatStringAddSpacePadding(_discountProduct.currency_code, (typeof(DiscountProductPoll79).GetProperty("currency_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.price_book = helper.FormatStringAddSpacePadding(_discountProduct.price_book, (typeof(DiscountProductPoll79).GetProperty("price_book").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.start_date = !string.IsNullOrEmpty(_discountProduct.start_date.ToString()) ? helper.FormatDateToyyyyMMdd(DateTime.Parse(_discountProduct.start_date.ToString())) : helper.FormatIntAddZeroPrefix(_discountProduct.start_date.ToString(), (typeof(DiscountProductPoll79).GetProperty("start_date").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.end_date = !string.IsNullOrEmpty(_discountProduct.end_date.ToString()) ? helper.FormatDateToyyyyMMdd(DateTime.Parse(_discountProduct.end_date.ToString())) : helper.FormatIntAddZeroPrefix(_discountProduct.end_date.ToString(), (typeof(DiscountProductPoll79).GetProperty("end_date").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.promo_flag_yn = helper.FormatStringAddSpacePadding(_discountProduct.promo_flag_yn, (typeof(DiscountProductPoll79).GetProperty("promo_flag_yn").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.event_price_multiple = helper.FormatIntAddZeroPrefix(_discountProduct.event_price_multiple, (typeof(DiscountProductPoll79).GetProperty("event_price_multiple").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.event_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_discountProduct.event_price, ((typeof(DiscountProductPoll79).GetProperty("event_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 3, 3);
            this.price_method_code = helper.FormatIntAddZeroPrefix(_discountProduct.price_method_code.ToString(), (typeof(DiscountProductPoll79).GetProperty("price_method_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.mix_match_code = helper.FormatIntAddZeroPrefix(_discountProduct.mix_match_code.ToString(), (typeof(DiscountProductPoll79).GetProperty("mix_match_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.deal_quantity = helper.FormatIntAddZeroPrefix(_discountProduct.deal_quantity.ToString(), (typeof(DiscountProductPoll79).GetProperty("deal_quantity").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.deal_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_discountProduct.deal_price, ((typeof(DiscountProductPoll79).GetProperty("deal_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 3, 3);
            this.buy_quantity = helper.FormatIntAddZeroPrefix(_discountProduct.buy_quantity.ToString(), (typeof(DiscountProductPoll79).GetProperty("buy_quantity").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.buy_value = helper.FormatDecimalAddZeroPrefixAndSuffix(_discountProduct.buy_value.ToString(), ((typeof(DiscountProductPoll79).GetProperty("buy_value").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 3, 3);
            this.buy_value_type = helper.FormatStringAddSpacePadding(_discountProduct.buy_value_type, (typeof(DiscountProductPoll79).GetProperty("buy_value_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.qty_end_value = helper.FormatIntAddZeroPrefix(_discountProduct.qty_end_value.ToString(), (typeof(DiscountProductPoll79).GetProperty("qty_end_value").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.quantity_break = helper.FormatIntAddZeroPrefix(_discountProduct.quantity_break.ToString(), (typeof(DiscountProductPoll79).GetProperty("quantity_break").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.quantity_group_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_discountProduct.quantity_group_price, ((typeof(DiscountProductPoll79).GetProperty("quantity_group_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 3, 3);
            this.quantity_unit_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_discountProduct.quantity_unit_price, ((typeof(DiscountProductPoll79).GetProperty("quantity_unit_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 3, 3);
            this.cust_promo_code = helper.FormatStringAddSpacePadding(_discountProduct.cust_promo_code, (typeof(DiscountProductPoll79).GetProperty("cust_promo_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.cust_number = helper.FormatIntAddZeroPrefix(_discountProduct.cust_number.ToString(), (typeof(DiscountProductPoll79).GetProperty("cust_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.precedence_level = helper.FormatDecimalAddZeroPrefixAndSuffix(_discountProduct.precedence_level.ToString(), ((typeof(DiscountProductPoll79).GetProperty("precedence_level").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 2, 2);
            this.default_currency = helper.FormatStringAddSpacePadding(_discountProduct.default_currency, (typeof(DiscountProductPoll79).GetProperty("default_currency").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.default_price_book = helper.FormatStringAddSpacePadding(_discountProduct.default_price_book, (typeof(DiscountProductPoll79).GetProperty("default_price_book").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(DiscountProductPoll79 obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}
