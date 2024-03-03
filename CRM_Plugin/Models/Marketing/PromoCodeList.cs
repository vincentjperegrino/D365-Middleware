#region Namespaces
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Marketing
{
    public class PromoCodeList
    {
        [JsonIgnoreAttribute]
        public static string entity_name = "ncci_promocodelist";
        [JsonIgnoreAttribute]
        public static string entity_id = "ncci_promocodelistid";

        public PromoCodeList()
        {
        }

        public PromoCodeList(PromoCode promoCode)
        {
            #region properties
            #endregion
        }

        #region Properties
        [DisplayName("ncci_promocode")]
        [DataType(DataType.Text)]
        public string ncci_promocode { get; set; }
        [DisplayName("ncci_firstname")]
        [DataType(DataType.Text)]
        public string ncci_firstname { get; set; }
        [DisplayName("ncci_lastname")]
        [DataType(DataType.Text)]
        public string ncci_lastname { get; set; }
        [DisplayName("ncci_mobilenumber")]
        [DataType(DataType.Text)]
        public string ncci_mobilenumber { get; set; }
        [DisplayName("ncci_uniquepromocode")]
        [DataType(DataType.Text)]
        public string ncci_uniquepromocode { get; set; }
        [DisplayName("ncci_customer")]
        [DataType(DataType.Text)]
        public string ncci_customer { get; set; }
        [DisplayName("statecode")]
        [Range(0, 1)]
        public int statecode { get; set; }
        [DisplayName("statuscode")]
        [Range(0, 714430002)]
        public int statuscode { get; set; }
        #endregion
    }
}
