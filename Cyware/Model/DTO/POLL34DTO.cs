using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kti.Moo.Cyware.Model.DTO
{
    public class POLL34DTO
    {
        [SortOrder(1)]
        [MaxLength(20)]
        public string strNum  { get; set; }
        [SortOrder(2)]
        [MaxLength(30)]
        public string strNam { get; set; }
        [SortOrder(3)]
        [MaxLength(30)]
        public string strAd1 { get; set; }
        [SortOrder(4)]
        [MaxLength(30)]
        public string strAd2 { get; set; }
        [SortOrder(5)]
        [MaxLength(30)]
        public string strAd3 { get; set; }
        [SortOrder(6)]
        [MaxLength(18)]
        public string strPhn { get; set; }
        [SortOrder(7)]
        [MaxLength(20)]
        public string stMngr { get; set; }
        [SortOrder(8)]
        [MaxLength(1)]
        public string strHdo { get; set; }
        [SortOrder(9)]
        [MaxLength(3)]
        public string strCod { get; set; }
        [SortOrder(10)]
        [MaxLength(3)]
        public string strTxc { get; set; }
        [SortOrder(11)]
        [MaxLength(10)]
        public string strLan { get; set; }


        PollMapping helper = new PollMapping();


        public POLL34DTO(KTI.Moo.Extensions.Cyware.Model.Store _store)
        {
            this.strNum = helper.FormatStringAddSpacePadding(_store.StoreNumber.ToString(), (typeof(POLL34DTO).GetProperty("strNum").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strNam = helper.FormatStringAddSpacePadding(_store.Name, (typeof(POLL34DTO).GetProperty("strNam").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strAd1 = helper.FormatStringAddSpacePadding(_store.Address_Line1, (typeof(POLL34DTO).GetProperty("strAd1").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strAd2 = helper.FormatStringAddSpacePadding(_store.Address_Line2, (typeof(POLL34DTO).GetProperty("strAd2").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strAd3 = helper.FormatStringAddSpacePadding(_store.Address_Line3, (typeof(POLL34DTO).GetProperty("strAd3").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strPhn = helper.FormatStringAddSpacePadding(_store.PhoneNumber, (typeof(POLL34DTO).GetProperty("strPhn").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.stMngr = helper.FormatStringAddSpacePadding(_store.ManagerName, (typeof(POLL34DTO).GetProperty("stMngr").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strHdo = helper.FormatStringAddSpacePadding(_store.StoreOffice, (typeof(POLL34DTO).GetProperty("strHdo").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strCod = helper.FormatStringAddSpacePadding(_store.Currency, (typeof(POLL34DTO).GetProperty("strCod").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strTxc = helper.FormatStringAddSpacePadding(_store.TaxCurrency, (typeof(POLL34DTO).GetProperty("strTxc").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.strLan = helper.FormatStringAddSpacePadding(_store.Language, (typeof(POLL34DTO).GetProperty("strLan").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);

        }

        public string Concat(POLL34DTO storePoll34DTO)
        {
            return helper.ConcatenateValues(storePoll34DTO);
        }

    }
}
