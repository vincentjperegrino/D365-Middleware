using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class BOMVersionPOLL
    {
        private ISFTPService _sftpService { get; init; }

        public BOMVersionPOLL(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }


        [SortOrder(1)]
        [MaxLength(20)]
        public string MANUFACTUREDITEMNUMBER { get; set; }

        [SortOrder(2)]
        [MaxLength(20)]
        public string BOMID { get; set; }

        [SortOrder(3)]
        [MaxLength(10)]
        public string PRODUCTIONSITEID { get; set; }

        [SortOrder(4)]
        [MaxLength(50)]
        public string PRODUCTCONFIGURATIONID { get; set; }

        [SortOrder(5)]
        [MaxLength(10)]
        public string PRODUCTCOLORID { get; set; }

        [SortOrder(6)]
        [MaxLength(10)]
        public string PRODUCTSIZEID { get; set; }

        [SortOrder(7)]
        [MaxLength(10)]
        public string PRODUCTSTYLEID { get; set; }

        [SortOrder(8)]
        [MaxLength(10)]
        public string PRODUCTVERSIONID { get; set; }

        [SortOrder(9)]
        [MaxLength(11)]
        public string ISACTIVE { get; set; }

        [SortOrder(10)]
        [MaxLength(8)]
        public string VALIDFROMDATE { get; set; }

        [SortOrder(11)]
        [MaxLength(33)]
        public string FROMQUANTITY { get; set; }

        [SortOrder(12)]
        [MaxLength(11)]
        public string SEQUENCEID { get; set; }

        [SortOrder(13)]
        [MaxLength(25)]
        public string APPROVERPERSONNELNUMBER { get; set; }

        [SortOrder(14)]
        [MaxLength(33)]
        public string CATCHWEIGHTSIZE { get; set; }

        [SortOrder(15)]
        [MaxLength(33)]
        public string FROMCATCHWEIGHTQUANTITY { get; set; }

        [SortOrder(16)]
        [MaxLength(11)]
        public string ISAPPROVED { get; set; }

        [SortOrder(17)]
        [MaxLength(11)]
        public string ISSELECTEDFORDESIGNER { get; set; }

        [SortOrder(18)]
        [MaxLength(8)]
        public string VALIDTODATE { get; set; }

        [SortOrder(19)]
        [MaxLength(60)]
        public string VERSIONNAME { get; set; }


        PollMapping helper = new PollMapping();

        public BOMVersionPOLL(BOMVersion bomVersion)
        {
            this.MANUFACTUREDITEMNUMBER = helper.FormatStringAddSpacePadding(bomVersion.MANUFACTUREDITEMNUMBER, (typeof(BOMVersionPOLL).GetProperty("MANUFACTUREDITEMNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.BOMID = helper.FormatStringAddSpacePadding(bomVersion.BOMID, (typeof(BOMVersionPOLL).GetProperty("BOMID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTIONSITEID = helper.FormatStringAddSpacePadding(bomVersion.PRODUCTIONSITEID, (typeof(BOMVersionPOLL).GetProperty("PRODUCTIONSITEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTCONFIGURATIONID = helper.FormatStringAddSpacePadding(bomVersion.PRODUCTCONFIGURATIONID, (typeof(BOMVersionPOLL).GetProperty("PRODUCTCONFIGURATIONID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTCOLORID = helper.FormatStringAddSpacePadding(bomVersion.PRODUCTCOLORID, (typeof(BOMVersionPOLL).GetProperty("PRODUCTCOLORID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTSIZEID = helper.FormatStringAddSpacePadding(bomVersion.PRODUCTSIZEID, (typeof(BOMVersionPOLL).GetProperty("PRODUCTSIZEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTSTYLEID = helper.FormatStringAddSpacePadding(bomVersion.PRODUCTSTYLEID, (typeof(BOMVersionPOLL).GetProperty("PRODUCTSTYLEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTVERSIONID = helper.FormatStringAddSpacePadding(bomVersion.PRODUCTVERSIONID, (typeof(BOMVersionPOLL).GetProperty("PRODUCTVERSIONID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ISACTIVE = helper.FormatStringAddSpacePadding(bomVersion.ISACTIVE, (typeof(BOMVersionPOLL).GetProperty("ISACTIVE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.VALIDFROMDATE = helper.FormatDateToyyyyMMdd(bomVersion.VALIDFROMDATE);
            this.FROMQUANTITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomVersion.FROMQUANTITY.ToString(), ((typeof(BOMVersionPOLL).GetProperty("FROMQUANTITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 6);
            this.SEQUENCEID = helper.FormatIntAddZeroPrefix(bomVersion.SEQUENCEID.ToString(), (typeof(BOMVersionPOLL).GetProperty("SEQUENCEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.APPROVERPERSONNELNUMBER = helper.FormatStringAddSpacePadding(bomVersion.APPROVERPERSONNELNUMBER, (typeof(BOMVersionPOLL).GetProperty("APPROVERPERSONNELNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CATCHWEIGHTSIZE = helper.FormatDecimalAddZeroPrefixAndSuffix(bomVersion.CATCHWEIGHTSIZE.ToString(), ((typeof(BOMVersionPOLL).GetProperty("CATCHWEIGHTSIZE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 6);
            this.FROMCATCHWEIGHTQUANTITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomVersion.FROMCATCHWEIGHTQUANTITY.ToString(), ((typeof(BOMVersionPOLL).GetProperty("FROMCATCHWEIGHTQUANTITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 6);
            this.ISAPPROVED = helper.FormatStringAddSpacePadding(bomVersion.ISAPPROVED, (typeof(BOMVersionPOLL).GetProperty("ISAPPROVED").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ISSELECTEDFORDESIGNER = helper.FormatStringAddSpacePadding(bomVersion.ISSELECTEDFORDESIGNER, (typeof(BOMVersionPOLL).GetProperty("ISSELECTEDFORDESIGNER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.VALIDTODATE = helper.FormatDateToyyyyMMdd(bomVersion.VALIDTODATE);
            this.VERSIONNAME = helper.FormatStringAddSpacePadding(bomVersion.VERSIONNAME, (typeof(BOMVersionPOLL).GetProperty("VERSIONNAME").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);

        }

        public string Concat(BOMVersionPOLL bomVersionPOLL)
        {
            return helper.ConcatenateValues(bomVersionPOLL);
        }
    }
}
