using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class TenderTypePOLL
    {
        [MaxLength(8)]
        [SortOrder(1)]
        public  string TenderTypeCd { get; set; }
        [MaxLength(35)]
        [SortOrder(2)]
        public  string Description { get; set; }
        [MaxLength(8)]
        [SortOrder(3)]
        public  string AllowChange { get; set; }
        [MaxLength(8)]
        [SortOrder(4)]
        public  string RequireValidation { get; set; }
        [MaxLength(8)]
        [SortOrder(5)]
        public  string IsCreditCard { get; set; }
        [MaxLength(8)]
        [SortOrder(6)]
        public  string IsGc { get; set; }
        [MaxLength(29)]
        [SortOrder(7)]
        public  string Skey { get; set; }
        [MaxLength(8)]
        [SortOrder(8)]
        public  string IsDrawer { get; set; }
        [MaxLength(8)]
        [SortOrder(9)]
        public  string IsDebitCard { get; set; }
        [MaxLength(8)]
        [SortOrder(10)]
        public  string IsCheck { get; set; }
        [MaxLength(8)]
        [SortOrder(11)]
        public  string IsCharge { get; set; }
        [MaxLength(8)]
        [SortOrder(12)]
        public  string IsCash { get; set; }
        [MaxLength(8)]
        [SortOrder(13)]
        public  string IsGarbage { get; set; }
        [MaxLength(8)]
        [SortOrder(14)]
        public  string IsCashdec { get; set; }
        [MaxLength(8)]
        [SortOrder(15)]
        public  string IsRebate { get; set; }
        [MaxLength(8)]
        [SortOrder(16)]
        public  string IsEgc { get; set; }
        [MaxLength(8)]
        [SortOrder(17)]
        public  string IsTelcoPull { get; set; }
        [MaxLength(8)]
        [SortOrder(18)]
        public  string IsTelcoPush { get; set; }
        [MaxLength(8)]
        [SortOrder(19)]
        public  string IsGuarantor { get; set; }
        [MaxLength(8)]
        [SortOrder(20)]
        public  string IsCardConnect { get; set; }
        [MaxLength(8)]
        [SortOrder(21)]
        public  string IsGovRebate { get; set; }
        [MaxLength(8)]
        [SortOrder(22)]
        public  string ItemCode { get; set; }
        [MaxLength(32)]
        [SortOrder(23)]
        public  string IsIntegrated { get; set; }
        [MaxLength(8)]
        [SortOrder(24)]
        public  string IsLoyalty { get; set; }
        [MaxLength(8)]
        [SortOrder(25)]
        public  string IsHoreca { get; set; }

        [MaxLength(8)]
        [SortOrder(26)]
        public string IsMobile { get; set; }

        public TenderTypePOLL(TenderType _tenderType)
        {
            var helper = new PollMapping();
            this.TenderTypeCd = helper.FormatStringAddSpacePadding(_tenderType.TenderTypeCd ?? "", (typeof(TenderTypePOLL).GetProperty("TenderTypeCd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Description = helper.FormatStringAddSpacePadding(_tenderType.Description ?? "", (typeof(TenderTypePOLL).GetProperty("Description").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.AllowChange = helper.FormatStringAddSpacePadding(_tenderType.AllowChange ?? "", (typeof(TenderTypePOLL).GetProperty("AllowChange").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.RequireValidation = helper.FormatStringAddSpacePadding(_tenderType.RequireValidation ?? "", (typeof(TenderTypePOLL).GetProperty("RequireValidation").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsCreditCard = helper.FormatStringAddSpacePadding(_tenderType.IsCreditCard ?? "", (typeof(TenderTypePOLL).GetProperty("IsCreditCard").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsGc = helper.FormatStringAddSpacePadding(_tenderType.IsGc ?? "", (typeof(TenderTypePOLL).GetProperty("IsGc").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Skey = helper.FormatStringAddSpacePadding(_tenderType.Skey ?? "", (typeof(TenderTypePOLL).GetProperty("Skey").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsDrawer = helper.FormatStringAddSpacePadding(_tenderType.IsDrawer ?? "", (typeof(TenderTypePOLL).GetProperty("IsDrawer").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsDebitCard = helper.FormatStringAddSpacePadding(_tenderType.IsDebitCard ?? "", (typeof(TenderTypePOLL).GetProperty("IsDebitCard").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsCheck = helper.FormatStringAddSpacePadding(_tenderType.IsCheck ?? "", (typeof(TenderTypePOLL).GetProperty("IsCheck").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsCharge = helper.FormatStringAddSpacePadding(_tenderType.IsCharge ?? "", (typeof(TenderTypePOLL).GetProperty("IsCharge").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsCash = helper.FormatStringAddSpacePadding(_tenderType.IsCash ?? "", (typeof(TenderTypePOLL).GetProperty("IsCash").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsGarbage = helper.FormatStringAddSpacePadding(_tenderType.IsGarbage ?? "", (typeof(TenderTypePOLL).GetProperty("IsGarbage").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsCashdec = helper.FormatStringAddSpacePadding(_tenderType.IsCashdec ?? "", (typeof(TenderTypePOLL).GetProperty("IsCashdec").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsRebate = helper.FormatStringAddSpacePadding(_tenderType.IsRebate ?? "", (typeof(TenderTypePOLL).GetProperty("IsRebate").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsEgc = helper.FormatStringAddSpacePadding(_tenderType.IsEgc ?? "", (typeof(TenderTypePOLL).GetProperty("IsEgc").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsTelcoPull = helper.FormatStringAddSpacePadding(_tenderType.IsTelcoPull ?? "", (typeof(TenderTypePOLL).GetProperty("IsTelcoPull").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsTelcoPush = helper.FormatStringAddSpacePadding(_tenderType.IsTelcoPush ?? "", (typeof(TenderTypePOLL).GetProperty("IsTelcoPush").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsGuarantor = helper.FormatStringAddSpacePadding(_tenderType.IsGuarantor ?? "", (typeof(TenderTypePOLL).GetProperty("IsGuarantor").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsCardConnect = helper.FormatStringAddSpacePadding(_tenderType.IsCardConnect ?? "", (typeof(TenderTypePOLL).GetProperty("IsCardConnect").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsGovRebate = helper.FormatStringAddSpacePadding(_tenderType.IsGovRebate ?? "", (typeof(TenderTypePOLL).GetProperty("IsGovRebate").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ItemCode = helper.FormatStringAddSpacePadding(_tenderType.ItemCode ?? "", (typeof(TenderTypePOLL).GetProperty("ItemCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsIntegrated = helper.FormatStringAddSpacePadding(_tenderType.IsIntegrated ?? "", (typeof(TenderTypePOLL).GetProperty("IsIntegrated").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsLoyalty = helper.FormatStringAddSpacePadding(_tenderType.IsLoyalty ?? "", (typeof(TenderTypePOLL).GetProperty("IsLoyalty").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsHoreca = helper.FormatStringAddSpacePadding(_tenderType.IsHoreca ?? "", (typeof(TenderTypePOLL).GetProperty("IsHoreca").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IsMobile = helper.FormatStringAddSpacePadding(_tenderType.IsMobile ?? "", (typeof(TenderTypePOLL).GetProperty("IsMobile").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(TenderTypePOLL obj)
        {
            var helper = new PollMapping();
            return helper.ConcatenateValues(obj);
        }
    }
}
