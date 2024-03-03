
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM_Plugin.Core.Model
{
    public class PriceLevelBase
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region Properties
        [StringLength(2000)]
        public string description { get; set; }
        public DateTime begindate { get; set; }
        public DateTime enddate { get; set; }
        public OptionSetValue freighttermscode { get; set; }
        [StringLength(100)]
        public string name { get; set; }
        public OptionSetValue paymentmethodcode { get; set; }
        public Guid pricelevelid { get; set; }
        public OptionSetValue shippingmethodcode { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        public EntityReference transactioncurrencyid { get; set; }
        public bool kti_mooenabled { get; set; }
        public bool kti_pickupdelivery { get; set; }
        public OptionSetValue kti_pricelisttype { get; set; }
        public OptionSetValue kti_socialchannel { get; set; }
        #endregion
    }
}
