using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM_Plugin.Core.Model
{
    public class CurrencyBase
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        public string transactioncurrencyid { get; set; }
        [StringLength(100)]
        public string currencyname { get; set; }
        [StringLength(5)]
        public string isocurrencycode { get; set; }
        public int currencyprecision { get; set; }
        public string currencysymbol { get; set; }
    }
}
