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
    public class BOMHeaderPOLL
    {
        private ISFTPService _sftpService { get; init; }

        public BOMHeaderPOLL(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        [SortOrder(1)]
        [MaxLength(20)]
        public string BOMID { get; set; }
        [SortOrder(2)]
        [MaxLength(20)]
        public string MANUFACTUREDITEMNUMBER { get; set; }
        [SortOrder(3)]
        [MaxLength(10)]
        public string PRODUCTIONSITEID { get; set; }
        [SortOrder(4)]
        [MaxLength(10)]
        public string PRODUCTCOLORID { get; set; }
        [SortOrder(5)]
        [MaxLength(50)]
        public string PRODUCTCONFIGURATIONID { get; set; }
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
        [MaxLength(33)]
        public string FROMQUANTITY { get; set; }
        [SortOrder(11)]
        [MaxLength(10)]
        public string VALIDFROMDATE { get; set; }
        [SortOrder(12)]
        [MaxLength(25)]
        public string APPROVERPERSONNELNUMBER { get; set; }
        [SortOrder(13)]
        [MaxLength(60)]
        public string BOMNAME { get; set; }
        [SortOrder(14)]
        [MaxLength(11)]
        public string ISAPPROVED { get; set; }


        PollMapping helper = new PollMapping();

        public BOMHeaderPOLL(KTI.Moo.Extensions.Cyware.Model.BOMHeader bomheader)
        {
            this.BOMID = helper.FormatStringAddSpacePadding(bomheader.BOMID, (typeof(BOMHeaderPOLL).GetProperty("BOMID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.MANUFACTUREDITEMNUMBER = helper.FormatStringAddSpacePadding(bomheader.MANUFACTUREDITEMNUMBER, (typeof(BOMHeaderPOLL).GetProperty("MANUFACTUREDITEMNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTIONSITEID = helper.FormatStringAddSpacePadding(bomheader.PRODUCTIONSITEID, (typeof(BOMHeaderPOLL).GetProperty("PRODUCTIONSITEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTCOLORID = helper.FormatStringAddSpacePadding(bomheader.PRODUCTCOLORID, (typeof(BOMHeaderPOLL).GetProperty("PRODUCTCOLORID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTCONFIGURATIONID = helper.FormatStringAddSpacePadding(bomheader.PRODUCTCONFIGURATIONID, (typeof(BOMHeaderPOLL).GetProperty("PRODUCTCONFIGURATIONID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTSIZEID = helper.FormatStringAddSpacePadding(bomheader.PRODUCTSIZEID, (typeof(BOMHeaderPOLL).GetProperty("PRODUCTSIZEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTSTYLEID = helper.FormatStringAddSpacePadding(bomheader.PRODUCTSTYLEID, (typeof(BOMHeaderPOLL).GetProperty("PRODUCTSTYLEID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PRODUCTVERSIONID = helper.FormatStringAddSpacePadding(bomheader.PRODUCTVERSIONID, (typeof(BOMHeaderPOLL).GetProperty("PRODUCTVERSIONID").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ISACTIVE = helper.FormatStringAddSpacePadding(bomheader.ISACTIVE, (typeof(BOMHeaderPOLL).GetProperty("ISACTIVE").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FROMQUANTITY = helper.FormatDecimalAddZeroPrefixAndSuffix(bomheader.FROMQUANTITY.ToString(), ((typeof(BOMHeaderPOLL).GetProperty("FROMQUANTITY").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0), 6);
            this.VALIDFROMDATE = helper.FormatDateToyyyyMMdd(bomheader.VALIDFROMDATE);
            this.APPROVERPERSONNELNUMBER = helper.FormatStringAddSpacePadding(bomheader.APPROVERPERSONNELNUMBER, (typeof(BOMHeaderPOLL).GetProperty("APPROVERPERSONNELNUMBER").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.BOMNAME = helper.FormatStringAddSpacePadding(bomheader.BOMNAME, (typeof(BOMHeaderPOLL).GetProperty("BOMNAME").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ISAPPROVED = helper.FormatStringAddSpacePadding(bomheader.ISAPPROVED, (typeof(BOMHeaderPOLL).GetProperty("ISAPPROVED").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
        }

        public string Concat(BOMHeaderPOLL bomHeaderPOLL)
        {
            return helper.ConcatenateValues(bomHeaderPOLL);
        }


    }
}
