using Microsoft.Xrm.Sdk;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM_Plugin.CustomAPI.Model.DTO
{
    public class PaymentTransaction : CRM_Plugin.Models.Sales.PaymentTransaction
    {
        CustomAPI.Domain.Product productDomain;
        CustomAPI.Domain.Order orderDomain;
        CustomAPI.Domain.Currency currencyDomain;

        public PaymentTransaction(CRM_Plugin.CustomAPI.Model.DTO.Orders order, Entity en, IOrganizationService service, ITracingService tracingService)
        {
            productDomain = new CustomAPI.Domain.Product(service, tracingService);
            orderDomain = new CustomAPI.Domain.Order(service, tracingService);
            currencyDomain = new CustomAPI.Domain.Currency(service, tracingService);

            if (en.Id != Guid.Empty)
            {
                this.kti_paymenttransactionid = en.Id;
            }
            //Reference Order number
            if (en.Contains("kti_socialchannelorigin"))
            {
                this.kti_socialchannelorigin = new OptionSetValue((int)en["kti_socialchannelorigin"]);
            }

            if (this.kti_socialchannelorigin == null)
                throw new Exception("No channel origin found.");

            //Reference Order number
            if (en.Contains("kti_sourceid"))
            {
                var orderRecord = orderDomain.GetSalesOrderBySourceIDChannel((string)en["kti_sourceid"], this.kti_socialchannelorigin.Value);

                if (orderRecord != null)
                    this.kti_order = orderRecord.ToEntityReference();
            }

            if (this.kti_order == null && !String.IsNullOrEmpty(order.kti_sourceid))
            {
                var orderRecord = orderDomain.GetSalesOrderBySourceIDChannel(order.kti_sourceid, this.kti_socialchannelorigin.Value);

                if (orderRecord != null)
                    this.kti_order = orderRecord.ToEntityReference();
            }

            if (Guid.Empty == this.kti_order.Id)
                throw new Exception("No source header found.");

            if (en.Contains("kti_transactionid"))
            {
                this.kti_transactionid = (string)en["kti_transactionid"];
            }
            if (en.Contains("kti_merchantname"))
            {
                this.kti_merchantname = (string)en["kti_merchantname"];
            }
            if (en.Contains("kti_amount"))
            {
                this.kti_amount = new Money(decimal.Parse(en["kti_amount"].ToString()));
            }
            if (en.Contains("kti_paymentdate"))
            {
                this.kti_paymentdate = (DateTime)en["kti_paymentdate"];
            }
            if (en.Contains("kti_paymentmethod"))
            {
                this.kti_paymentmethod = new OptionSetValue((int)en["kti_paymentmethod"]);
            }

            if (en.Contains("kti_paymentstatus"))
            {
                this.kti_paymentstatus = new OptionSetValue((int)en["kti_paymentstatus"]);
            }
        }

        public Guid kti_paymenttransactionid { get; set; }
        public string kti_transactionid { get; set; }
        public string kti_merchantname { get; set; }
        public Money kti_amount { get; set; }
        public string kti_authenticationid { get; set; }
        public string kti_authorizationid { get; set; }
        public string kti_chargingid { get; set; }
        new public EntityReference transactioncurrencyid { get; set; }

        public DateTime kti_paymentdate { get; set; }
        public OptionSetValue kti_paymentmethod { get; set; }
        public OptionSetValue kti_paymentstatus { get; set; }
        public string kti_reason { get; set; }
        public EntityReference kti_order { get; set; }
        public EntityReference kti_invoice { get; set; }
        public OptionSetValue kti_socialchannelorigin { get; set; }
    }
}
