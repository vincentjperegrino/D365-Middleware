using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class ProductPricePoll79
    {
        [SortOrder(1)]

        [MaxLength(20)] ///// CHANGED
        public  string sku_number { get; set; }
        [SortOrder(2)]
        [MaxLength(18)]
        public  string upc_code { get; set; }
        [SortOrder(3)]
        [MaxLength(1)]
        public  string upc_type { get; set; }
        [SortOrder(4)]
        [MaxLength(20)]
        public  string price_event_number { get; set; }
        [SortOrder(5)]
        [MaxLength(3)]
        public  string currency_code { get; set; }
        [SortOrder(6)]
        [MaxLength(20)]
        public  string price_book { get; set; }
        [SortOrder(7)]
        [MaxLength(8)]
        public  string start_date { get; set; }
        [SortOrder(8)]
        [MaxLength(8)]
        public  string end_date { get; set; }
        [SortOrder(9)]
        [MaxLength(1)]
        public  string promo_flag_yn { get; set; }
        [SortOrder(10)]
        [MaxLength(3)]
        public  string event_price_multiple { get; set; }
        [SortOrder(11)]
        [MaxLength(13)]
        public  string event_price { get; set; }
        [SortOrder(12)]
        [MaxLength(1)]
        public  string price_method_code { get; set; }
        [SortOrder(13)]
        [MaxLength(3)]
        public  string mix_match_code { get; set; }
        [SortOrder(14)]
        [MaxLength(3)]
        public  string deal_quantity { get; set; }
        [SortOrder(15)]
        [MaxLength(13)]
        public  string deal_price { get; set; }
        [SortOrder(16)]
        [MaxLength(3)]
        public  string buy_quantity { get; set; }
        [SortOrder(17)]
        [MaxLength(13)]
        public  string buy_value { get; set; }
        [SortOrder(18)]
        [MaxLength(1)]
        public  string buy_value_type { get; set; }
        [SortOrder(19)]
        [MaxLength(7)]
        public  string qty_end_value { get; set; }
        [SortOrder(20)]
        [MaxLength(3)]
        public  string quantity_break { get; set; }
        [SortOrder(21)]
        [MaxLength(13)]
        public  string quantity_group_price { get; set; }
        [SortOrder(22)]
        [MaxLength(13)]
        public  string quantity_unit_price { get; set; }
        [SortOrder(23)]
        [MaxLength(1)]
        public  string cust_promo_code { get; set; }
        [SortOrder(24)]
        [MaxLength(20)]
        public  string cust_number { get; set; }
        [SortOrder(25)]
        [MaxLength(2)]
        public  string precedence_level { get; set; }
        [SortOrder(26)]
        [MaxLength(3)]
        public  string default_currency { get; set; }
        [SortOrder(27)]
        [MaxLength(20)]
        public  string default_price_book { get; set; }

        public ProductPricePoll79(ProductPrice _productPrice)
        {
            var helper = new PollMapping();
            this.sku_number = helper.FormatStringAddSpacePadding(_productPrice.sku_number, (typeof(ProductPricePoll79).GetProperty("sku_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.upc_code = helper.FormatIntAddZeroPrefix(_productPrice.upc_code.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("upc_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.upc_type = helper.FormatStringAddSpacePadding(_productPrice.upc_type, (typeof(ProductPricePoll79).GetProperty("upc_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.price_event_number = helper.FormatStringAddSpacePadding(_productPrice.price_event_number ?? "0", (typeof(ProductPricePoll79).GetProperty("price_event_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.currency_code = helper.FormatStringAddSpacePadding(_productPrice.currency_code, (typeof(ProductPricePoll79).GetProperty("currency_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.price_book = helper.FormatStringAddSpacePadding(_productPrice.price_book, (typeof(ProductPricePoll79).GetProperty("price_book").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.start_date = helper.FormatDateToyyyyMMdd(_productPrice.start_date);
            this.end_date = helper.FormatDateToyyyyMMdd(_productPrice.end_date);
            this.promo_flag_yn = helper.FormatStringAddSpacePadding(_productPrice.promo_flag_yn, (typeof(ProductPricePoll79).GetProperty("promo_flag_yn").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.event_price_multiple = helper.FormatIntAddZeroPrefix(_productPrice.event_price_multiple.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("event_price_multiple").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.event_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_productPrice.event_price.ToString() ?? "0", ((typeof(ProductPricePoll79).GetProperty("event_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 3);
            this.price_method_code = helper.FormatIntAddZeroPrefix(_productPrice.price_method_code.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("price_method_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.mix_match_code = helper.FormatIntAddZeroPrefix(_productPrice.mix_match_code.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("mix_match_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.deal_quantity = helper.FormatIntAddZeroPrefix(_productPrice.deal_quantity.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("deal_quantity").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.deal_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_productPrice.deal_price.ToString() ?? "0", ((typeof(ProductPricePoll79).GetProperty("deal_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 3);
            this.buy_quantity = helper.FormatIntAddZeroPrefix(_productPrice.buy_quantity.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("buy_quantity").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.buy_value = helper.FormatDecimalAddZeroPrefixAndSuffix(_productPrice.buy_value.ToString() ?? "0", ((typeof(ProductPricePoll79).GetProperty("buy_value").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 3);
            this.buy_value_type = helper.FormatStringAddSpacePadding(_productPrice.buy_value_type, (typeof(ProductPricePoll79).GetProperty("buy_value_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.qty_end_value = helper.FormatIntAddZeroPrefix(_productPrice.qty_end_value.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("qty_end_value").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.quantity_break = helper.FormatIntAddZeroPrefix(_productPrice.quantity_break.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("quantity_break").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.quantity_group_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_productPrice.quantity_group_price.ToString() ?? "0", ((typeof(ProductPricePoll79).GetProperty("quantity_group_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 3);
            this.quantity_unit_price = helper.FormatDecimalAddZeroPrefixAndSuffix(_productPrice.quantity_unit_price.ToString() ?? "0", ((typeof(ProductPricePoll79).GetProperty("quantity_unit_price").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 3);
            this.cust_promo_code = helper.FormatStringAddSpacePadding(_productPrice.cust_promo_code, (typeof(ProductPricePoll79).GetProperty("cust_promo_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.cust_number = helper.FormatStringAddSpacePadding(_productPrice.cust_number ?? "", (typeof(ProductPricePoll79).GetProperty("cust_number").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.precedence_level = helper.FormatIntAddZeroPrefix(_productPrice.precedence_level.ToString() ?? "0", (typeof(ProductPricePoll79).GetProperty("precedence_level").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.default_currency = helper.FormatStringAddSpacePadding(_productPrice.default_currency, (typeof(ProductPricePoll79).GetProperty("default_currency").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.default_price_book = helper.FormatStringAddSpacePadding(_productPrice.default_price_book, (typeof(ProductPricePoll79).GetProperty("default_price_book").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(ProductPricePoll79 obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }


}
