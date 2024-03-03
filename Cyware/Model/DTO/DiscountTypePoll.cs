using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class DiscountTypePoll
    {
        PollMapping helper = new PollMapping();

        [SortOrder(1)]
        [MaxLength(20)]
        public  string DiscountCode { get; set; }

        [SortOrder(2)]
        [MaxLength(30)]
        public  string DiscountTypeCd { get; set; }

        [SortOrder(3)]
        [MaxLength(40)]
        public  string Description { get; set; }

        [SortOrder(4)]
        [MaxLength(16)]
        public  string DiscType { get; set; }

        [SortOrder(5)]
        [MaxLength(16)]
        public  string Readonly { get; set; }

        [SortOrder(6)]
        [MaxLength(16)]
        public  string DiscValue { get; set; }

        [SortOrder(7)]
        [MaxLength(16)]
        public  string StartDate { get; set; }

        [SortOrder(8)]
        [MaxLength(16)]
        public string StartTime { get; set; }

        [SortOrder(9)]
        [MaxLength(16)]
        public string EndDate { get; set; }

        [SortOrder(10)]
        [MaxLength(16)]
        public string EndTime { get; set; }

        [SortOrder(11)]
        [MaxLength(16)]
        public  string MinAmount { get; set; }

        [SortOrder(12)]
        [MaxLength(16)]
        public  string MaxAmount { get; set; }

        [SortOrder(13)]
        [MaxLength(16)]
        public  string DiscountRule { get; set; }

        [SortOrder(14)]
        [MaxLength(16)]
        public  string AccountTypeCode { get; set; }

        [SortOrder(15)]
        [MaxLength(16)]
        public  string RequireAccount { get; set; }

        [SortOrder(16)]
        [MaxLength(16)]
        public  string FreeItem { get; set; }

        [SortOrder(17)]
        [MaxLength(16)]
        public  string FreeItemLimit { get; set; }

        [SortOrder(18)]
        [MaxLength(16)]
        public  string FreeItemQty { get; set; }

        [SortOrder(19)]
        [MaxLength(16)]
        public  string EventNumber { get; set; }

        [SortOrder(20)]
        [MaxLength(20)]
        public string ExportCurrentPrice { get; set; }

        [SortOrder(21)]
        [MaxLength(1)]
        public string Posted { get; set; }

        [SortOrder(221)]
        [MaxLength(8)]
        public string PostedOn { get; set; } 


        public DiscountTypePoll(DiscountType discountType)
        {
            this.DiscountCode = helper.FormatStringAddSpacePadding(discountType.DiscountCode, (typeof(DiscountTypePoll).GetProperty("DiscountCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DiscountTypeCd = helper.FormatStringAddSpacePadding(discountType.DiscountTypeCd, (typeof(DiscountTypePoll).GetProperty("DiscountTypeCd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Description = helper.FormatStringAddSpacePadding(discountType.Description, (typeof(DiscountTypePoll).GetProperty("Description").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DiscType = helper.FormatStringAddSpacePadding(discountType.DiscType, (typeof(DiscountTypePoll).GetProperty("DiscType").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Readonly = helper.FormatStringAddSpacePadding(discountType.Readonly, (typeof(DiscountTypePoll).GetProperty("Readonly").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DiscValue = helper.FormatStringAddSpacePadding(discountType.DiscValue, (typeof(DiscountTypePoll).GetProperty("DiscValue").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.StartDate = helper.FormatDateToyyyyMMdd(discountType.StartDate);
            this.StartTime = helper.FormatStringAddSpacePadding(discountType.StartTime, (typeof(DiscountTypePoll).GetProperty("StartTime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.EndDate  = helper.FormatDateToyyyyMMdd(discountType.EndDate);
            this.EndTime = helper.FormatStringAddSpacePadding(discountType.EndTime, (typeof(DiscountTypePoll).GetProperty("EndTime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.MinAmount = helper.FormatDecimalAddZeroPrefixAndSuffix(discountType.MinAmount.ToString(), (typeof(DiscountTypePoll).GetProperty("MinAmount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.MaxAmount = helper.FormatDecimalAddZeroPrefixAndSuffix(discountType.MaxAmount.ToString(), (typeof(DiscountTypePoll).GetProperty("MaxAmount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.DiscountRule = helper.FormatStringAddSpacePadding(discountType.DiscountRule, (typeof(DiscountTypePoll).GetProperty("DiscountRule").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.AccountTypeCode = helper.FormatStringAddSpacePadding(discountType.AccountTypeCode, (typeof(DiscountTypePoll).GetProperty("AccountTypeCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.RequireAccount = helper.FormatStringAddSpacePadding(discountType.RequireAccount, (typeof(DiscountTypePoll).GetProperty("RequireAccount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FreeItem = helper.FormatStringAddSpacePadding(discountType.FreeItem, (typeof(DiscountTypePoll).GetProperty("FreeItem").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FreeItemLimit = helper.FormatIntAddZeroPrefix(discountType.FreeItemLimit.ToString(), (typeof(DiscountTypePoll).GetProperty("FreeItemLimit").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FreeItemQty = helper.FormatIntAddZeroPrefix(discountType.FreeItemQty.ToString(), (typeof(DiscountTypePoll).GetProperty("FreeItemQty").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.EventNumber = helper.FormatIntAddZeroPrefix(discountType.EventNumber.ToString(), (typeof(DiscountTypePoll).GetProperty("EventNumber").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ExportCurrentPrice = helper.FormatStringAddSpacePadding(discountType.ExportCurrentPrice, (typeof(DiscountTypePoll).GetProperty("ExportCurrentPrice").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Posted = helper.FormatIntAddZeroPrefix(discountType.Posted.ToString(), (typeof(DiscountTypePoll).GetProperty("Posted").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PostedOn = helper.FormatDateToyyyyMMdd(discountType.PostedOn);
        }

        public string Concat(DiscountTypePoll obj)
        {
            return helper.ConcatenateValues(obj);
        }
    }
}
