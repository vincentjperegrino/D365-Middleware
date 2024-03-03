using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;
using KTI.Moo.Extensions.OctoPOS.Helper;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class Invoice : InvoiceBase
    {
        [JsonIgnore]
        public override int companyid { get; set; }

        [JsonIgnore]
        public override string moosourcesystem { get; set; } = "OCTOPOS";


        [JsonProperty("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [JsonIgnore]
        public override DateTime invoicedate
        {
            get => Helper.DateTimeHelper.PHT_to_UTC(InvoiceDate);
        }

        [JsonProperty("InvoiceNumber")]
        public override string kti_sourceid { get; set; }

        [JsonIgnore]
        public override string invoicenumber { get => kti_sourceid; }

        [JsonIgnore]
        public override string name { get => kti_sourceid; }

        [JsonProperty("TotalDiscountAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal discountamount { get; set; }

        [JsonProperty("NetSalesAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal totalamount { get; set; }

        [JsonProperty("TaxAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public override decimal totaltax { get; set; }

        [JsonProperty("Remark")]
        public override string description { get; set; }

        [JsonProperty("ShippingCost")]
        public override decimal freightamount { get; set; }

        [JsonProperty("InvoiceType")]
        public string InvoiceType { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("CurrencyCode")]
        public string CurrencyCode { get; set; } = CurrencyCodeHelper.PhilippinesPesos;

        [JsonProperty("RoundOffAmount", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal RoundOffAmount { get; set; }

        [JsonProperty("CashierCode")]
        public string CashierCode { get; set; } // = CashierCodeHelper.Admin;

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("TaxPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal TaxPercentage { get; set; }

        [JsonProperty("Terminal")]
        public string Terminal { get; set; }

        [JsonProperty("SalesmanCode")]
        public string SalesmanCode { get; set; } // = CashierCodeHelper.Admin;

        [JsonProperty("SalesmanName")]
        public string SalesmanName { get; set; }

        [JsonProperty("IsTaxExclusive", DefaultValueHandling = DefaultValueHandling.Include)]
        public int IsTaxExclusive { get; set; }

        [JsonProperty("OrderStatusDescription")]
        public string OrderStatusDescription { get; set; }

        [JsonProperty("PaidAmount")]
        public decimal PaidAmount { get; set; }

        [JsonProperty("BalancePayment")]
        public decimal BalancePayment { get; set; }

        [JsonProperty("CreatedDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("UpdatedDateTime")]
        public DateTime UpdatedDateTime { get; set; }

        [JsonProperty("BalancePoint")]
        public decimal BalancePoint { get; set; }

        [JsonProperty("BdDiscount")]
        public int BdDiscount { get; set; }

        [JsonProperty("RewardPoints")]
        public int RewardPoints { get; set; }

        [JsonProperty("InvoiceTypeInt")]
        public int InvoiceTypeInt { get; set; }

        [JsonProperty("OrganisationCode")]
        public string OrganisationCode { get; set; }

        [JsonProperty("ReturnInvoiceNumber")]
        public string ReturnInvoiceNumber { get; set; }

        [JsonProperty("OrginalInvoiceNumber")]
        public string OrginalInvoiceNumber { get; set; }

        [JsonProperty("SalesOrderNumber")]
        public override string salesorderid { get; set; }

        [JsonProperty("CustomerCode")]
        public string CustomerCode
        {
            get => CustomerDetails.CustomerCode;
            set => CustomerDetails.CustomerCode = value;
        }

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }

        //[JsonProperty("CustomerName")]
        //public string CustomerName
        //{
        //    get => CustomerDetails.firstname;
        //    set => CustomerDetails.firstname = value;
        //}

        [JsonProperty("CustomerEmail")]
        public string CustomerEmail
        {
            get => CustomerDetails.Email;
            set => CustomerDetails.Email = value;
        }

        [JsonProperty("CustomerPhone")]
        public string CustomerPhone
        {
            get => CustomerDetails.HandPhone;
            set => CustomerDetails.HandPhone = value;

        }

        [JsonProperty("CustomerAddress")]
        public string CustomerAddress
        {

            get => CustomerDetails.Address1;
            set
            {
                CustomerDetails.Address1 = value;
                CustomerDetails.ShippingAddress = value;
            }
        }

        public virtual Customer CustomerDetails { get; set; }

        //[JsonProperty("VoidBy")]
        //public string VoidBy { get; set; }

        //[JsonProperty("VoidReason")]
        //public string VoidReason { get; set; }

        //[JsonProperty("VoidDate")]
        //public DateTime VoidDate { get; set; }

        [JsonProperty("InvoiceItems")]
        public List<InvoiceItem> InvoiceItems { get; set; }

        [JsonProperty("PaymentItems")]
        public List<PaymentItems> PaymentItems { get; set; }

        //[JsonProperty("DiscounthistoryItems")]
        //public List<object> DiscounthistoryItems { get; set; }

        public Invoice()
        {
            CustomerDetails = new();
        }


    }
}