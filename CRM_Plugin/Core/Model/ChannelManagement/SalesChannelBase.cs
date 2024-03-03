using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Model.ChannelMangement
{
    public class SalesChannelBase
    {
        public string kti_account { get; set; }
        public string kti_appkey { get; set; }
        public string kti_AppKeyflag { get; set; }
        public string kti_appsecret { get; set; }
        public string kti_AppSecretflag { get; set; }
        public int kti_channelorigin { get; set; }
        public string kti_country { get; set; }
        public string kti_defaulturl { get; set; }
        public bool kti_isproduction { get; set; }
        public string kti_name { get; set; }
        public string kti_databasename { get; set; }
        public string kti_password { get; set; }
        public string kti_Passwordflag { get; set; }
        public string kti_storecode { get; set; }
        public string kti_saleschannelId { get; set; }
        public string kti_salt { get; set; }
        public string kti_username { get; set; }
        public string kti_warehousecode { get; set; }
        public string kti_sellerid { get; set; }
        public string kti_azureconnectionstring { get; set; }
        public string kti_moocompanyid { get; set; }
        public string kti_access_token { get; set; }
        public DateTime kti_access_expiration { get; set; }
        public string kti_refresh_token { get; set; }
        public DateTime kti_refresh_expiration { get; set; }
    }

}

