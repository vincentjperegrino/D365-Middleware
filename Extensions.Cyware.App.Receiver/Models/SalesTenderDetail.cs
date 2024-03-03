using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class SalesTenderDetail : BasePollLog
    {
        public SalesTenderDetail()
        {
        }

        public SalesTenderDetail(string[] records)
        {
            this.StoreNumber = records[0];
            this.TransDate = records[1];
            this.RegisterID = records[2];
            this.RollOverCode = records[3];
            this.TransNumber = records[4];
            this.TransSeq = records[5];
            this.SubSeq = records[6];
            this.StringType = records[7];
            this.TransactionType = records[8];
            this.TillNumber = records[9];
            this.Status = records[10];
            this.TenderedAmount = records[11];
            this.NegativeSign = records[12];
            this.TenderDocument = records[13];
            this.CreditCardNumber = records[14];
            this.CardExpirationDate = records[15];
            this.AuthorizationCode = records[16];
            this.SwipeCard = records[17];
            this.ForeignCCYAmount = records[18];
            this.CurrencyCode = records[19];
            this.ExchangeRate = records[20];
            this.MultDivFlag = records[21];
        }

        public string TransactionType { get; set; }
        public string TillNumber { get; set; }
        public string Status { get; set; }
        public string TenderedAmount { get; set; }
        public string NegativeSign { get; set; }
        public string TenderDocument { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string AuthorizationCode { get; set; }
        public string SwipeCard { get; set; }
        public string ForeignCCYAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string ExchangeRate { get; set; }
        public string MultDivFlag { get; set; }
    }
}
