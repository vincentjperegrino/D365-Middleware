using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class SalesTransactionDiscount : BasePollLog
    {
        public SalesTransactionDiscount()
        {
        }

        public SalesTransactionDiscount(string[] records)
        {
            this.StoreNumber = records[0];
            this.TransDate = records[1];
            this.RegisterID = records[2];
            this.RollOverCode = records[3];
            this.TransNumber = records[4];
            this.TransSeq = records[5];
            this.SubSeq = records[6];
            this.StringType = records[7];
            this.DiscTransType = records[8];
            this.DiscountAmount = records[9];
            this.NegativeSign = records[10];
            this.DiscReasonType = records[11];
            this.DiscReason = records[12];
            this.TransDiscAmount = records[13];
            this.CurrencyCode = records[14];
            this.ExchangeRate = records[15];
            this.MultiDivFlag = records[16];
        }

        public string dataAreaId { get; set; }
        public string DiscTransType { get; set; }
        public string DiscountAmount { get; set; }
        public string NegativeSign { get; set; }
        public string DiscReasonType { get; set; }
        public string DiscReason { get; set; }
        public string TransDiscAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string ExchangeRate { get; set; }
        public string MultiDivFlag { get; set; }
    }
}
