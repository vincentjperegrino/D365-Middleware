#region Namespaces
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#endregion

namespace CRM_Plugin.Models.Items
{
    public class Currency
    {
        public string EntityName = "transactioncurrency";
        public string PrimaryKey = "transactioncurrencyid";

        public Currency()
        {
        }

        public Currency(Entity _entity)
        {
            #region Mapping
            string erroron = "";
            try
            {
                erroron = "transactioncurrencyid";
                this.transactioncurrencyid = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
                erroron = "currencyname";
                this.currencyname = _entity.Contains("currencyname") ? (string)_entity["currencyname"] : default;
                erroron = "isocurrencycode";
                this.isocurrencycode = _entity.Contains("isocurrencycode") ? (string)_entity["isocurrencycode"] : default;
                erroron = "currencyprecision";
                this.currencyprecision = _entity.Contains("currencyprecision") ? (int)_entity["currencyprecision"] : default;
                erroron = "currencysymbol";
                this.currencysymbol = _entity.Contains("currencysymbol") ? (string)_entity["currencysymbol"] : default;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");
            }
            #endregion
        }

        public Currency(Currency _currency)
        {
            #region properties
            this.transactioncurrencyid = _currency.transactioncurrencyid;
            this.currencyname = _currency.currencyname;
            this.isocurrencycode = _currency.isocurrencycode;
            this.currencyprecision = _currency.currencyprecision;
            this.currencysymbol = _currency.currencysymbol;
            #endregion
        }

        #region Properties
        public string transactioncurrencyid { get; set; }
        [StringLength(100)]
        public string currencyname { get; set; }
        [StringLength(5)]
        public string isocurrencycode { get; set; }
        public int currencyprecision { get; set; }
        public string currencysymbol { get; set; }
        #endregion
    }
}
