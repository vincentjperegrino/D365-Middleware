using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class SalesTransactionDetail : BasePollLog
    {
        public SalesTransactionDetail() 
        {
        }

        public SalesTransactionDetail(string[] records)
        {
            this.StoreNumber = records[0];
            this.TransDate = records[1];
            this.RegisterID = records[2];
            this.RollOverCode = records[3];
            this.TransNumber = records[4];
            this.TransSeq = records[5];
            this.SubSeq = records[6];
            this.StringType = records[7];
            this.DetailTransType = records[8];
            this.TillNumber = records[9];
            this.Status = records[10];
            this.SKUNumber = records[11];
            this.QuantitySold = records[12];
            this.NegativeSign = records[13];
            this.UnitRetail = records[14];
            this.ExtendedDiscount = records[15];
            this.NegativeSign1 = records[16];
            this.ExtendedPrice = records[17];
            this.NegativeSign2 = records[18];
            this.PriceOverride = records[19];
            this.TaxExempt = records[20];
            this.ProductNumber = records[21];
            this.DocNumber = records[22];
            this.ReasonCodeType = records[23];
            this.ReasonCode = records[24];
            this.ScanBarCode = records[25];
            this.AccountType = records[26];
            this.AccountNumber = records[27];
            this.PriceBook = records[28];
            this.OriginalStore = records[29];
            this.OriginalDate = records[30];
            this.TransUnitRetail = records[31];
            this.TransExtDiscount = records[32];
            this.TransExtendedPrice = records[33];
            this.CurrencyCode = records[34];
            this.ExchangeRate = records[35];
            this.MultDivFlag = records[36];
            this.RemainderAmount = records[37];
            this.NegativeSign3 = records[38];
            this.TransRemainderAmount = records[39];
        }

        public string DetailTransType { get; set; }
        public string TillNumber { get; set; }
        public string Status { get; set; }
        public string SKUNumber { get; set; }
        public string QuantitySold { get; set; }
        public string NegativeSign { get; set; }
        public string UnitRetail { get; set; }
        public string ExtendedDiscount { get; set; }
        public string NegativeSign1 { get; set; }
        public string ExtendedPrice { get; set; }
        public string NegativeSign2 { get; set; }
        public string PriceOverride { get; set; }
        public string TaxExempt { get; set; }
        public string ProductNumber { get; set; }
        public string DocNumber { get; set; }
        public string ReasonCodeType { get; set; }
        public string ReasonCode { get; set; }
        public string ScanBarCode { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string PriceBook { get; set; }
        public string OriginalStore { get; set; }
        public string OriginalDate { get; set; }
        public string TransUnitRetail { get; set; }
        public string TransExtDiscount { get; set; }
        public string TransExtendedPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string ExchangeRate { get; set; }
        public string MultDivFlag { get; set; }
        public string RemainderAmount { get; set; }
        public string NegativeSign3 { get; set; }
        public string TransRemainderAmount { get; set; }
    }
}
