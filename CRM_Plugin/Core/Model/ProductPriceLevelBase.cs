
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRM_Plugin.Core.Model
{
    public class ProductPriceLevelBase
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region Properties
        public decimal amount { get; set; }
        public string discounttypeid { get; set; }
        public DateTime createdon { get; set; }
        public string percentage { get; set; }
        public string pricelevelid { get; set; }
        public string pricingmethodcode { get; set; }
        public string productid { get; set; }
        public string productpricelevelid { get; set; }
        public int quantitysellingcode { get; set; }
        public decimal roundingoptionamount { get; set; }
        public int roundingoptioncode { get; set; }
        public int roundingpolicycode { get; set; }
        public string transactioncurrencyid { get; set; }
        public string uomid { get; set; }
        public string uomscheduleid { get; set; }
        public string moosourcesystem { get; set; }
        public string mooexternalid { get; set; }
        public int companyid { get; set; }
        #endregion
    }
}
