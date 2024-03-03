using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Core.Domain;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class DiscountTypeProductPOLL
    {
        [SortOrder(1)]
        [MaxLength(32)]
        public string ItemCode { get; set; }

        [SortOrder(2)]
        [MaxLength(20)]
        public string DisccountCd { get; set; }

        [SortOrder(3)]
        [MaxLength(8)]
        public string DiscValueInAmount { get; set; }

        [SortOrder(4)]
        [MaxLength(30)]
        public string Disctype { get; set; }

        [SortOrder(5)]
        [MaxLength(64)]
        public string ItemGroup { get; set; }

        [SortOrder(6)]
        [MaxLength(32)]
        public string FreeItem { get; set; }

        [SortOrder(7)]
        [MaxLength(8)]
        public string FreeItemQty { get; set; }

        [SortOrder(8)]
        [MaxLength(8)]
        public string Tolerance { get; set; }

        [SortOrder(9)]
        [MaxLength(12)]
        public string EventNum { get; set; }

        [SortOrder(10)]
        [MaxLength(8)]
        public string ReqAmount { get; set; }

        [SortOrder(11)]
        [MaxLength(16)]
        public string ReqQty { get; set; }

        /// <summary>
        /// Additional Fields
        /// </summary>
        /// 

        [SortOrder(12)]
        [MaxLength(5)]
        public string PartyCodeType { get; set; }

        [SortOrder(13)]
        [MaxLength(20)]
        public string AccountSelection { get; set; }

        [SortOrder(14)]
        [MaxLength(50)]
        public string Configuration { get; set; }

        [SortOrder(15)]
        [MaxLength(10)]
        public string Site { get; set; }

        [SortOrder(16)]
        [MaxLength(10)]
        public string Warehouse { get; set; }

        [SortOrder(17)]
        [MaxLength(22)]
        public string From { get; set; }

        [SortOrder(18)]
        [MaxLength(22)]
        public string To { get; set; }

        [SortOrder(19)]
        [MaxLength(10)]
        public string Unit { get; set; }

        [SortOrder(20)]
        [MaxLength(3)]
        public string Currency { get; set; }

        [SortOrder(21)]
        [MaxLength(10)]
        public string AttributeBasedPricingID { get; set; }

        [SortOrder(22)]
        [MaxLength(20)]
        public string DimensionValidation { get; set; }

        [SortOrder(23)]
        [MaxLength(20)]
        public string TradeAgreementValidation { get; set; }

        [SortOrder(24)]
        [MaxLength(20)]
        public string DimensionNumber { get; set; }

        [SortOrder(25)]
        [MaxLength(22)]
        public string DiscountPercentage2 { get; set; }

        [SortOrder(26)]
        [MaxLength(3)]
        public string DisregardLeadTime { get; set; }

        [SortOrder(27)]
        [MaxLength(3)]
        public string FindNext { get; set; }

        [SortOrder(28)]
        [MaxLength(8)]
        public string FromDate { get; set; }

        [SortOrder(29)]
        [MaxLength(3)]
        public string IncludeInUnitPrice { get; set; }

        [SortOrder(30)]
        [MaxLength(3)]
        public string IncludeGenericCurrency { get; set; }

        [SortOrder(31)]
        [MaxLength(60)]
        public string LeadTime { get; set; }

        [SortOrder(32)]
        [MaxLength(60)]
        public string Log { get; set; }

        [SortOrder(33)]
        [MaxLength(20)]
        public string Module { get; set; }

        [SortOrder(34)]
        [MaxLength(10)]
        public string PriceAgreements { get; set; }

        [SortOrder(35)]
        [MaxLength(22)]
        public string PriceCharges { get; set; }

        [SortOrder(36)]
        [MaxLength(22)]
        public string PriceUnit { get; set; }

        [SortOrder(37)]
        [MaxLength(8)]
        public string ToDate { get; set; }

        [SortOrder(38)]
        [MaxLength(6)]
        public string FromTime { get; set; }

        [SortOrder(39)]
        [MaxLength(6)]
        public string ToTime { get; set; }

        [SortOrder(40)]
        [MaxLength(22)]
        public string DiscValueInPercentage { get; set; }


        PollMapping helper = new PollMapping();
        public DiscountTypeProductPOLL(DiscountTypeProduct discountTypeProduct)
        {
            this.ItemCode = helper.FormatStringAddSpacePadding(discountTypeProduct.ItemCode, (typeof(DiscountTypeProductPOLL).GetProperty("ItemCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DisccountCd = helper.FormatStringAddSpacePadding(discountTypeProduct.DisccountCd, (typeof(DiscountTypeProductPOLL).GetProperty("DisccountCd").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DiscValueInAmount = helper.FormatDecimalAddZeroPrefixAndSuffix(discountTypeProduct.DiscValueInAmount.ToString() ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("DiscValueInAmount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.DiscValueInPercentage = helper.FormatStringAddSpacePadding(discountTypeProduct.DiscValueInPercentage, (typeof(DiscountTypeProductPOLL).GetProperty("DiscValueInPercentage").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Disctype = helper.FormatStringAddSpacePadding(discountTypeProduct.DiscType, (typeof(DiscountTypeProductPOLL).GetProperty("Disctype").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ItemGroup = helper.FormatStringAddSpacePadding(discountTypeProduct.ItemGroup, (typeof(DiscountTypeProductPOLL).GetProperty("ItemGroup").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FreeItem = helper.FormatStringAddSpacePadding(discountTypeProduct.FreeItem, (typeof(DiscountTypeProductPOLL).GetProperty("FreeItem").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FreeItemQty = helper.FormatStringAddSpacePadding(discountTypeProduct.FreeItemQty, (typeof(DiscountTypeProductPOLL).GetProperty("FreeItemQty").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Tolerance = helper.FormatStringAddSpacePadding(discountTypeProduct.Tolerance, (typeof(DiscountTypeProductPOLL).GetProperty("Tolerance").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.EventNum = helper.FormatStringAddSpacePadding(discountTypeProduct.EventNum, (typeof(DiscountTypeProductPOLL).GetProperty("EventNum").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ReqAmount = helper.FormatStringAddSpacePadding(discountTypeProduct.ReqAmount, (typeof(DiscountTypeProductPOLL).GetProperty("ReqAmount").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ReqQty = helper.FormatStringAddSpacePadding(discountTypeProduct.ReqQty, (typeof(DiscountTypeProductPOLL).GetProperty("ReqQty").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);

            ///Additional Fields
            this.PartyCodeType = helper.FormatStringAddSpacePadding(discountTypeProduct.PartyCodeType, (typeof(DiscountTypeProductPOLL).GetProperty("PartyCodeType").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.AccountSelection = helper.FormatStringAddSpacePadding(discountTypeProduct.AccountSelection, (typeof(DiscountTypeProductPOLL).GetProperty("AccountSelection").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Configuration = helper.FormatStringAddSpacePadding(discountTypeProduct.Configuration, (typeof(DiscountTypeProductPOLL).GetProperty("Configuration").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Site = helper.FormatStringAddSpacePadding(discountTypeProduct.Site, (typeof(DiscountTypeProductPOLL).GetProperty("Site").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Warehouse = helper.FormatStringAddSpacePadding(discountTypeProduct.Warehouse, (typeof(DiscountTypeProductPOLL).GetProperty("Warehouse").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.From = helper.FormatIntAddZeroPrefix(discountTypeProduct.From.ToString() ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("From").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.To = helper.FormatIntAddZeroPrefix(discountTypeProduct.To.ToString() ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("To").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Unit = helper.FormatStringAddSpacePadding(discountTypeProduct.Unit, (typeof(DiscountTypeProductPOLL).GetProperty("Unit").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Currency = helper.FormatStringAddSpacePadding(discountTypeProduct.Currency, (typeof(DiscountTypeProductPOLL).GetProperty("Currency").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.AttributeBasedPricingID = helper.FormatStringAddSpacePadding(discountTypeProduct.AttributeBasedPricingID, (typeof(DiscountTypeProductPOLL).GetProperty("AttributeBasedPricingID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DimensionValidation = helper.FormatStringAddSpacePadding(discountTypeProduct.DimensionValidation, (typeof(DiscountTypeProductPOLL).GetProperty("DimensionValidation").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            
            this.TradeAgreementValidation = helper.FormatIntAddZeroPrefix(discountTypeProduct.TradeAgreementValidation ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("TradeAgreementValidation").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);

            this.DimensionNumber = helper.FormatStringAddSpacePadding(discountTypeProduct.DimensionNumber, (typeof(DiscountTypeProductPOLL).GetProperty("DimensionNumber").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.DiscountPercentage2 = helper.FormatDecimalAddZeroPrefixAndSuffix(discountTypeProduct.FindNext ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("DiscountPercentage2").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.DisregardLeadTime = helper.FormatIntAddZeroPrefix(discountTypeProduct.DisregardLeadTime ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("DisregardLeadTime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FindNext = helper.FormatIntAddZeroPrefix(discountTypeProduct.FindNext ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("FindNext").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FromDate = helper.FormatDateToyyyyMMdd(discountTypeProduct.FromDate);
            this.IncludeInUnitPrice = helper.FormatIntAddZeroPrefix(discountTypeProduct.IncludeInUnitPrice ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("IncludeInUnitPrice").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.IncludeGenericCurrency = helper.FormatIntAddZeroPrefix(discountTypeProduct.IncludeGenericCurrency ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("IncludeGenericCurrency").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.LeadTime = helper.FormatDecimalAddZeroPrefixAndSuffix(discountTypeProduct.LeadTime ?? "0.0", (typeof(DiscountTypeProductPOLL).GetProperty("LeadTime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.Log = helper.FormatStringAddSpacePadding(discountTypeProduct.Log, (typeof(DiscountTypeProductPOLL).GetProperty("Log").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PriceAgreements = helper.FormatStringAddSpacePadding(discountTypeProduct.PriceAgreements, (typeof(DiscountTypeProductPOLL).GetProperty("PriceAgreements").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PriceCharges = helper.FormatDecimalAddZeroPrefixAndSuffix(discountTypeProduct.PriceCharges ?? "0.0", (typeof(DiscountTypeProductPOLL).GetProperty("PriceCharges").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.PriceUnit = helper.FormatDecimalAddZeroPrefixAndSuffix(discountTypeProduct.PriceUnit ?? "0.0", (typeof(DiscountTypeProductPOLL).GetProperty("PriceUnit").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0, 2);
            this.ToDate = helper.FormatDateToyyyyMMdd(discountTypeProduct.ToDate);
            this.FromTime = helper.FormatStringAddSpacePadding(discountTypeProduct.FromTime, (typeof(DiscountTypeProductPOLL).GetProperty("FromTime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ToTime = helper.FormatStringAddSpacePadding(discountTypeProduct.ToTime, (typeof(DiscountTypeProductPOLL).GetProperty("ToTime").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);

            this.Module = helper.FormatIntAddZeroPrefix(discountTypeProduct.Module ?? "0", (typeof(DiscountTypeProductPOLL).GetProperty("Module").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(DiscountTypeProductPOLL obj)
        {
            return helper.ConcatenateValues(obj);
        }
    }
}
