#region Namespaces
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Marketing
{
    public class CampaignActivity
    {
        [JsonIgnoreAttribute]
        public static string entity_name = "campaignactivity";
        [JsonIgnoreAttribute]
        public static string entity_id = "campaignactivityid";

        public CampaignActivity()
        {
        }

        public CampaignActivity(PromoCode campaignActivity)
        {
            #region properties

            #endregion
        }

        #region Properties
        [DisplayName("ncci_smsmessage")]
        [DataType(DataType.MultilineText)]
        public string ncci_smsmessage { get; set; }
        [DisplayName("ncci_promocodeprefix")]
        [DataType(DataType.Text)]
        public string ncci_promocodeprefix { get; set; }
        [DisplayName("ncci_validfrom")]
        [DataType(DataType.Date)]
        public DateTime ncci_validfrom { get; set; }
        [DisplayName("ncci_validto")]
        [DataType(DataType.Date)]
        public DateTime ncci_validto { get; set; }
        [DisplayName("ncci_numberofrandomcharacters")]
        [Range(0, 2147483647)]
        public int ncci_numberofrandomcharacters { get; set; }
        [DisplayName("ncci_promocode")]
        [DataType(DataType.Text)]
        public string ncci_promocode { get; set; }
        [DisplayName("ncci_includesuniquepromocode")]
        public bool ncci_includesuniquepromocode { get; set; }
        [DisplayName("channeltypecode")]
        public int channeltypecode { get; set; }
        [DisplayName("statecode")]
        [Range(0, 2)]
        public int statecode { get; set; }
        [DisplayName("statuscode")]
        [Range(0, 6)]
        public int statuscode { get; set; }
        [DisplayName("ncci_approvaltime")]
        public DateTime ncci_approvaltime { get; set; }
        [DisplayName("subject")]
        [DataType(DataType.Text)]
        public string subject { get; set; }
        #endregion
    }
}