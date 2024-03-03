using Extensions.Cyware.App.Receiver.Receivers.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class SalesTransactionHeader : BasePollLog
    {
        public SalesTransactionHeader()
        {
        }

        public SalesTransactionHeader(string[] records)
        {
            this.StoreNumber = records[0];
            this.TransDate = records[1];
            this.RegisterID = records[2];
            this.RollOverCode = records[3];
            this.TransNumber = records[4];
            this.TransSeq = records[5];
            this.SubSeq = records[6];
            this.StringType = records[7];
            this.TransType = records[8];
            this.TillNumber = records[9];
            this.Status = records[10];
            this.TransactionTime = records[11];
            this.CashierNumber = records[12];
            this.CurrencyCode = records[13];
            this.SalesAmount = records[14];
            this.NegativeSign = records[15];
            this.TenderAmount = records[16];
            this.NegativeSign1 = records[17];
            this.CustomerNumber = records[18];
            this.MembershipNumber = records[19];
            this.AcctType = records[20];
            this.AccountNumber = records[21];
            this.InvoiceNumber = records[22];
            this.PostVoid = records[23];
            this.ReasonCodeType = records[24];
            this.TransReasonCode = records[25];
            this.EmployeeNumber = records[26];
            this.ZipCode = records[27];
            this.TaxID = records[28];
            this.CustomerDate = records[29];
            this.CustomerFirstName = records[30];
            this.CustomerMiddleName = records[31];
            this.CustomerLastName = records[32];
            this.CustomerDate = records[33];
            this.CustomerEmail = records[34];
            this.CustomerContactNumber = records[35];
        }

        public string TransType { get; set; }
        public string TillNumber { get; set; }
        public string Status { get; set; }
        public string TransactionTime { get; set; }
        public string CashierNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string SalesAmount { get; set; }
        public string NegativeSign { get; set; }
        public string TenderAmount { get; set; }
        public string NegativeSign1 { get; set; }
        public string CustomerNumber { get; set; }
        public string MembershipNumber { get; set; }
        public string AcctType { get; set; }
        public string AccountNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string PostVoid { get; set; }
        public string ReasonCodeType { get; set; }
        public string TransReasonCode { get; set; }
        public string EmployeeNumber { get; set; }
        public string ZipCode { get; set; }
        public string TaxID { get; set; }
        public string CustomerDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerJoinDate { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContactNumber { get; set; }
    }
}
