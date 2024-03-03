#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM.Models.Sales
{

    /// <summary>
    /// Payment transaction
    /// </summary>
    public class PaymentTransaction
    {

        public PaymentTransaction()
        {
        }
        public PaymentTransaction(PaymentTransaction _paymentTransactionDto)
        {
            #region properties
            this.paymenttransactionid = _paymentTransactionDto.paymenttransactionid;
            this.transactionid = _paymentTransactionDto.transactionid;
            this.statecode = _paymentTransactionDto.statecode;
            this.amount = _paymentTransactionDto.amount;
            this.amount_base = _paymentTransactionDto.amount_base;
            this.authenticationid = _paymentTransactionDto.authenticationid;
            this.authorizationid = _paymentTransactionDto.authorizationid;
            this.chargingid = _paymentTransactionDto.chargingid;
            this.transactioncurrencyid = _paymentTransactionDto.transactioncurrencyid;
            this.emailaddress = _paymentTransactionDto.emailaddress;
            this.paymentdate = _paymentTransactionDto.paymentdate;
            this.paymentmethod = _paymentTransactionDto.paymentmethod;
            this.paymentstatus = _paymentTransactionDto.paymentstatus;
            this.reason = _paymentTransactionDto.reason;
            this.statuscode = _paymentTransactionDto.statuscode;
            this.mooexternalid = _paymentTransactionDto.mooexternalid;
            this.moosourcesystem = _paymentTransactionDto.moosourcesystem;
            #endregion
        }

        #region properties
        public string paymenttransactionid { get; set; }
        [Required]
        public string transactionid { get; set; }
        [Required]
        public int statecode { get; set; }
        public string amount { get; set; }
        public string amount_base { get; set; }
        public string authenticationid { get; set; }
        public string authorizationid { get; set; }
        public string chargingid { get; set; }
        public string transactioncurrencyid { get; set; }
        public string emailaddress { get; set; }

        public CRM.CustomDataType.MooDateTime paymentdate { get; set; }
        public int paymentmethod { get; set; }
        public int paymentstatus { get; set; }
        public string reason { get; set; }
        public int statuscode { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }

        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }

        #endregion
    }
}