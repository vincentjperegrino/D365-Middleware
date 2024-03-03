using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Cyware.Model;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class ForExPollEx
    {
        [SortOrder(1)]
        [MaxLength(3)]
        public virtual string from_currency_code { get; set; }
        [SortOrder(2)]
        [MaxLength(3)]
        public virtual string to_currency_code { get; set; }
        [SortOrder(3)]
        [MaxLength(15)]
        public virtual string currency_exch_rate { get; set; }
        [SortOrder(4)]
        [MaxLength(1)]
        public virtual string code { get; set; }
        [SortOrder(5)]
        [MaxLength(3)]
        public virtual string conversion_rate_type { get; set; }
        [SortOrder(6)]
        [MaxLength(8)]
        public virtual string effective_date { get; set; }
        [SortOrder(7)]
        [MaxLength(10)]
        public virtual string rounding_multiple { get; set; }
        [SortOrder(8)]
        [MaxLength(10)]
        public virtual string rounding_multiple_to { get; set; }
        [SortOrder(9)]
        [MaxLength(1)]
        public virtual string currency_exch_rate_mt { get; set; }

        public ForExPollEx(ForEx _forEx)
        {
            var helper = new PollMapping();
            this.from_currency_code = helper.FormatStringAddSpacePadding(_forEx.from_currency_code, (typeof(ForExPollEx).GetProperty("from_currency_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.to_currency_code = helper.FormatStringAddSpacePadding(_forEx.to_currency_code, (typeof(ForExPollEx).GetProperty("to_currency_code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.currency_exch_rate = helper.FormatDecimalAddZeroPrefixAndSuffix(_forEx.currency_exch_rate.ToString(), ((typeof(ForExPollEx).GetProperty("currency_exch_rate").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 8, 8);
            this.code = helper.FormatStringAddSpacePadding(_forEx.code, (typeof(ForExPollEx).GetProperty("code").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.conversion_rate_type = helper.FormatStringAddSpacePadding(_forEx.conversion_rate_type, (typeof(ForExPollEx).GetProperty("conversion_rate_type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.effective_date = !string.IsNullOrEmpty(_forEx.effective_date) ? helper.FormatDateToyyyyMMdd(DateTime.Parse(_forEx.effective_date)) : helper.FormatIntAddZeroPrefix(_forEx.effective_date, (typeof(ForExPollEx).GetProperty("effective_date").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.rounding_multiple = helper.FormatDecimalAddZeroPrefixAndSuffix(_forEx.rounding_multiple, ((typeof(ForExPollEx).GetProperty("rounding_multiple").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 4, 4);
            this.rounding_multiple_to = helper.FormatDecimalAddZeroPrefixAndSuffix(_forEx.rounding_multiple_to, ((typeof(ForExPollEx).GetProperty("rounding_multiple_to").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0) - 4, 4);
            this.currency_exch_rate_mt = helper.FormatStringAddSpacePadding(_forEx.currency_exch_rate_mt, (typeof(ForExPollEx).GetProperty("currency_exch_rate_mt").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(ForExPollEx obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}
