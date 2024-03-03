#region Namespaces
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Marketing
{
    /// <summary>
    /// Price level
    /// </summary>
    public class PromoCode
    {
        [JsonIgnoreAttribute]
        public static string entity_name = "ncci_promocode";
        [JsonIgnoreAttribute]
        public static string entity_id = "ncci_promocodeid";

        public PromoCode()
        {
        }

        public PromoCode(PromoCode promoCode)
        {
            #region properties
            this.ncci_promocode = promoCode.ncci_promocode;
            this.ncci_validfrom = promoCode.ncci_validfrom;
            this.ncci_validto = promoCode.ncci_validto;
            this.statecode = promoCode.statecode;
            this.statuscode = promoCode.statuscode;
            #endregion
        }

        #region Properties
        [DisplayName("ncci_campaignactivity")]
        [DataType(DataType.Text)]
        public string ncci_campaignactivity { get; set; }
        [DisplayName("ncci_promocode")]
        [DataType(DataType.Text)]
        public string ncci_promocode { get; set; }
        [DisplayName("ncci_validfrom")]
        [DataType(DataType.Date)]
        public DateTime ncci_validfrom { get; set; }
        [DisplayName("ncci_validto")]
        [DataType(DataType.Date)]
        public DateTime ncci_validto { get; set; }
        [DisplayName("statecode")]
        [Range(0, 1)]
        public int statecode { get; set; }
        [DisplayName("statuscode")]
        [Range(0, 714430002)]
        public int statuscode { get; set; }
        #endregion
    }
}
