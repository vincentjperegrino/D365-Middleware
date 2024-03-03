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
    public class BOMLinesPOLL
    {
        [SortOrder(1)]
        [MaxLength(20)]
        public string BOMID { get; set; }
        [SortOrder(2)]
        [MaxLength(11)]
        public string LINECREATIONSEQUENCENUMBER { get; set; }
        [SortOrder(3)]
        [MaxLength(33)]
        public string CATCHWEIGHTQUANTITY { get; set; }
        [SortOrder(4)]
        [MaxLength(10)]
        public string CONFIGURATIONGROUPID { get; set; }
        [SortOrder(5)]
        [MaxLength(33)]
        public string CONSTANTSCRAPQUANTITY { get; set; }
        [SortOrder(6)]
        [MaxLength(33)]
        public string CONSUMPTIONCALCULATIONCONSTANT { get; set; }
        [SortOrder(7)]
        [MaxLength(43)]
        public string CONSUMPTIONCALCULATIONMETHOD { get; set; }
        [SortOrder(8)]
        [MaxLength(10)]
        public string CONSUMPTIONSITEID { get; set; }
        [SortOrder(9)]
        [MaxLength(11)]
        public string CONSUMPTIONTYPE { get; set; }
        [SortOrder(10)]
        [MaxLength(10)]
        public string CONSUMPTIONWAREHOUSEID { get; set; }
        [SortOrder(11)]
        [MaxLength(21)]
        public string FLUSHINGPRINCIPLE { get; set; }
        [SortOrder(12)]
        [MaxLength(11)]
        public string ISCONSUMEDATOPERATIONCOMPLETE { get; set; }
        [SortOrder(13)]
        [MaxLength(11)]
        public string ISRESOURCECONSUMPTIONUSED { get; set; }
        [SortOrder(14)]
        [MaxLength(20)]
        public string ITEMNUMBER { get; set; }
        [SortOrder(15)]
        [MaxLength(33)]
        public string LINENUMBER { get; set; }
        [SortOrder(16)]
        [MaxLength(13)]
        public string LINETYPE { get; set; }
        [SortOrder(17)]
        [MaxLength(33)]
        public string PHYSICALPRODUCTDENSITY { get; set; }
        [SortOrder(18)]
        [MaxLength(33)]
        public string PHYSICALPRODUCTDEPTH { get; set; }
        [SortOrder(19)]
        [MaxLength(33)]
        public string PHYSICALPRODUCTHEIGHT { get; set; }
        [SortOrder(20)]
        [MaxLength(33)]
        public string PHYSICALPRODUCTWIDTH { get; set; }
        [SortOrder(21)]
        [MaxLength(30)]
        public string POSITIONNUMBER { get; set; }
        [SortOrder(22)]
        [MaxLength(10)]
        public string PRODUCTCOLORID { get; set; }
        [SortOrder(23)]
        [MaxLength(50)]
        public string PRODUCTCONFIGURATIONID { get; set; }
        [SortOrder(24)]
        [MaxLength(10)]
        public string PRODUCTSIZEID { get; set; }
        [SortOrder(25)]
        [MaxLength(10)]
        public string PRODUCTSTYLEID { get; set; }
        [SortOrder(26)]
        [MaxLength(10)]
        public string PRODUCTUNITSYMBOL { get; set; }
        [SortOrder(27)]
        [MaxLength(10)]
        public string PRODUCTVERSIONID { get; set; }
        [SortOrder(28)]
        [MaxLength(33)]
        public string QUANTITY { get; set; }
        [SortOrder(29)]
        [MaxLength(33)]
        public string QUANTITYDENOMINATOR { get; set; }
        [SortOrder(30)]
        [MaxLength(33)]
        public string QUANTITYROUNDINGUPMULTIPLES { get; set; }
        [SortOrder(31)]
        [MaxLength(13)]
        public string ROUNDINGUPMETHOD { get; set; }
        [SortOrder(32)]
        [MaxLength(11)]
        public string ROUTEOPERATIONNUMBER { get; set; }
        [SortOrder(33)]
        [MaxLength(20)]
        public string SUBBOMID { get; set; }
        [SortOrder(34)]
        [MaxLength(20)]
        public string SUBROUTEID { get; set; }
        [SortOrder(35)]
        [MaxLength(8)]
        public string VALIDFROMDATE { get; set; }
        [SortOrder(36)]
        [MaxLength(8)]
        public string VALIDTODATE { get; set; }
        [SortOrder(37)]
        [MaxLength(33)]
        public string VARIABLESCRAPPERCENTAGE { get; set; }
        [SortOrder(38)]
        [MaxLength(20)]
        public string VENDORACCOUNTNUMBER { get; set; }
        [SortOrder(39)]
        [MaxLength(25)]
        public string WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE { get; set; }
        [SortOrder(40)]
        [MaxLength(11)]
        public string WILLCOSTCALCULATIONINCLUDELINE { get; set; }
        [SortOrder(41)]
        [MaxLength(11)]
        public string WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES { get; set; }
        [SortOrder(42)]
        [MaxLength(11)]
        public string WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES { get; set; }

        PollMapping helper = new PollMapping();

        public BOMLinesPOLL(KTI.Moo.Extensions.Cyware.Model.BOMLines bomLines)
        {

            this.BOMID = helper.FormatStringAddSpacePadding(bomLines.BOMID ?? "", (typeof(BOMLinesPOLL).GetProperty("BOMID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.LINECREATIONSEQUENCENUMBER = helper.FormatIntAddZeroPrefix(bomLines.LINECREATIONSEQUENCENUMBER.ToString(), (typeof(BOMLinesPOLL).GetProperty("LINECREATIONSEQUENCENUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CATCHWEIGHTQUANTITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.CATCHWEIGHTQUANTITY.ToString(), ((typeof(BOMLinesPOLL).GetProperty("CATCHWEIGHTQUANTITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.CONFIGURATIONGROUPID = helper.FormatStringAddSpacePadding(bomLines.CONFIGURATIONGROUPID, (typeof(BOMLinesPOLL).GetProperty("CONFIGURATIONGROUPID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CONSTANTSCRAPQUANTITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.CONSTANTSCRAPQUANTITY.ToString(), ((typeof(BOMLinesPOLL).GetProperty("CONSTANTSCRAPQUANTITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.CONSUMPTIONCALCULATIONCONSTANT = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.CONSUMPTIONCALCULATIONCONSTANT.ToString(), ((typeof(BOMLinesPOLL).GetProperty("CONSUMPTIONCALCULATIONCONSTANT").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.CONSUMPTIONCALCULATIONMETHOD = helper.FormatStringAddSpacePadding(bomLines.CONSUMPTIONCALCULATIONMETHOD, (typeof(BOMLinesPOLL).GetProperty("CONSUMPTIONCALCULATIONMETHOD").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CONSUMPTIONSITEID = helper.FormatStringAddSpacePadding(bomLines.CONSUMPTIONSITEID, (typeof(BOMLinesPOLL).GetProperty("CONSUMPTIONSITEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CONSUMPTIONTYPE = helper.FormatStringAddSpacePadding(bomLines.CONSUMPTIONTYPE, (typeof(BOMLinesPOLL).GetProperty("CONSUMPTIONTYPE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CONSUMPTIONWAREHOUSEID = helper.FormatStringAddSpacePadding(bomLines.CONSUMPTIONWAREHOUSEID, (typeof(BOMLinesPOLL).GetProperty("CONSUMPTIONWAREHOUSEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FLUSHINGPRINCIPLE = helper.FormatStringAddSpacePadding(bomLines.FLUSHINGPRINCIPLE, (typeof(BOMLinesPOLL).GetProperty("FLUSHINGPRINCIPLE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ISCONSUMEDATOPERATIONCOMPLETE = helper.FormatStringAddSpacePadding(bomLines.ISCONSUMEDATOPERATIONCOMPLETE, (typeof(BOMLinesPOLL).GetProperty("ISCONSUMEDATOPERATIONCOMPLETE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ISRESOURCECONSUMPTIONUSED = helper.FormatStringAddSpacePadding(bomLines.ISRESOURCECONSUMPTIONUSED, (typeof(BOMLinesPOLL).GetProperty("ISRESOURCECONSUMPTIONUSED").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ITEMNUMBER = helper.FormatStringAddSpacePadding(bomLines.ITEMNUMBER, (typeof(BOMLinesPOLL).GetProperty("ITEMNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.LINENUMBER = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.LINENUMBER.ToString(), ((typeof(BOMLinesPOLL).GetProperty("LINENUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.LINETYPE = helper.FormatStringAddSpacePadding(bomLines.LINETYPE, (typeof(BOMLinesPOLL).GetProperty("LINETYPE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PHYSICALPRODUCTDENSITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.PHYSICALPRODUCTDENSITY.ToString(), ((typeof(BOMLinesPOLL).GetProperty("PHYSICALPRODUCTDENSITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.PHYSICALPRODUCTDEPTH = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.PHYSICALPRODUCTDEPTH.ToString(), ((typeof(BOMLinesPOLL).GetProperty("PHYSICALPRODUCTDEPTH").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.PHYSICALPRODUCTHEIGHT = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.PHYSICALPRODUCTHEIGHT.ToString(), ((typeof(BOMLinesPOLL).GetProperty("PHYSICALPRODUCTHEIGHT").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.PHYSICALPRODUCTWIDTH = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.PHYSICALPRODUCTWIDTH.ToString(), ((typeof(BOMLinesPOLL).GetProperty("PHYSICALPRODUCTWIDTH").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.POSITIONNUMBER = helper.FormatStringAddSpacePadding(bomLines.POSITIONNUMBER, (typeof(BOMLinesPOLL).GetProperty("POSITIONNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTCOLORID = helper.FormatStringAddSpacePadding(bomLines.PRODUCTCOLORID, (typeof(BOMLinesPOLL).GetProperty("PRODUCTCOLORID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTCONFIGURATIONID = helper.FormatStringAddSpacePadding(bomLines.PRODUCTCONFIGURATIONID, (typeof(BOMLinesPOLL).GetProperty("PRODUCTCONFIGURATIONID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTSIZEID = helper.FormatStringAddSpacePadding(bomLines.PRODUCTSIZEID, (typeof(BOMLinesPOLL).GetProperty("PRODUCTSIZEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTSTYLEID = helper.FormatStringAddSpacePadding(bomLines.PRODUCTSTYLEID, (typeof(BOMLinesPOLL).GetProperty("PRODUCTSTYLEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTUNITSYMBOL = helper.FormatStringAddSpacePadding(bomLines.PRODUCTUNITSYMBOL, (typeof(BOMLinesPOLL).GetProperty("PRODUCTUNITSYMBOL").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTVERSIONID = helper.FormatStringAddSpacePadding(bomLines.PRODUCTVERSIONID, (typeof(BOMLinesPOLL).GetProperty("PRODUCTVERSIONID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.QUANTITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.QUANTITY.ToString(), ((typeof(BOMLinesPOLL).GetProperty("QUANTITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.QUANTITYDENOMINATOR = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.QUANTITYDENOMINATOR.ToString(), ((typeof(BOMLinesPOLL).GetProperty("QUANTITYDENOMINATOR").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.QUANTITYROUNDINGUPMULTIPLES = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.QUANTITYROUNDINGUPMULTIPLES.ToString(), ((typeof(BOMLinesPOLL).GetProperty("QUANTITYROUNDINGUPMULTIPLES").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0)  , 6);
            this.ROUNDINGUPMETHOD = helper.FormatStringAddSpacePadding(bomLines.ROUNDINGUPMETHOD, (typeof(BOMLinesPOLL).GetProperty("ROUNDINGUPMETHOD").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ROUTEOPERATIONNUMBER = helper.FormatIntAddZeroPrefix(bomLines.ROUTEOPERATIONNUMBER.ToString(), (typeof(BOMLinesPOLL).GetProperty("ROUTEOPERATIONNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.SUBBOMID = helper.FormatStringAddSpacePadding(bomLines.SUBBOMID, (typeof(BOMLinesPOLL).GetProperty("SUBBOMID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.SUBROUTEID = helper.FormatStringAddSpacePadding(bomLines.SUBROUTEID, (typeof(BOMLinesPOLL).GetProperty("SUBROUTEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.VALIDFROMDATE = helper.FormatDateToyyyyMMdd(bomLines.VALIDFROMDATE);
            this.VALIDTODATE = helper.FormatDateToyyyyMMdd(bomLines.VALIDTODATE);
            this.VARIABLESCRAPPERCENTAGE = helper.FormatDecimalAddZeroPrefixAndSuffix(bomLines.VARIABLESCRAPPERCENTAGE.ToString(), ((typeof(BOMLinesPOLL).GetProperty("VARIABLESCRAPPERCENTAGE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 6);
            this.VENDORACCOUNTNUMBER = helper.FormatStringAddSpacePadding(bomLines.VENDORACCOUNTNUMBER, (typeof(BOMLinesPOLL).GetProperty("VENDORACCOUNTNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE = helper.FormatStringAddSpacePadding(bomLines.WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE, (typeof(BOMLinesPOLL).GetProperty("WAREHOUSEBOMRELEASERESERVATIONREQUIREMENTRULE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.WILLCOSTCALCULATIONINCLUDELINE = helper.FormatStringAddSpacePadding(bomLines.WILLCOSTCALCULATIONINCLUDELINE, (typeof(BOMLinesPOLL).GetProperty("WILLCOSTCALCULATIONINCLUDELINE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES = helper.FormatStringAddSpacePadding(bomLines.WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES, (typeof(BOMLinesPOLL).GetProperty("WILLMANUFACTUREDITEMINHERITBATCHATTRIBUTES").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES = helper.FormatStringAddSpacePadding(bomLines.WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES, (typeof(BOMLinesPOLL).GetProperty("WILLMANUFACTUREDITEMINHERITSHELFLIFEDATES").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(BOMLinesPOLL bomLinesPOLL)
        {
            return helper.ConcatenateValues(bomLinesPOLL);
        }
    }
}
