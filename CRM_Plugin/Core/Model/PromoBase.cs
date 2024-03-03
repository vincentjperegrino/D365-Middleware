using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRM_Plugin.Core.Model
{
    public class PromoBase
    {

        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region Properties
        public EntityReference kti_channel { get; set; }
        public string kti_description { get; set; }
        public Money kti_discountamount { get; set; }
        public decimal kti_discountpercentage { get; set; }
        public string kti_externalid { get; set; }
        public int kti_maximumused { get; set; }
        public Money kti_minimumtotalorderamount { get; set; }
        public int kti_minimumused { get; set; }
        public EntityReference kti_promocategory { get; set; }
        public Guid kti_promoid { get; set; }
        public string kti_promoname { get; set; }
        public DateTime kti_validfrom { get; set; }
        public DateTime kti_validto { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        #endregion
    }
}
