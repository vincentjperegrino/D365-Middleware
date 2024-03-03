using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Helper.EntityHelper
{
    public class SalesChannel : BaseEntity
    {
        public static new string entity_name = "kti_saleschannel";
        public static new string entity_id = "kti_saleschannelid";

        public static string appKey = "kti_appkey";
        public static string appSecret = "kti_appsecret";
        public static string password = "kti_password";

        public static string appKeyFlag = "kti_appkeyflag";
        public static string appSecretFlag = "kti_appsecretflag";
        public static string passwordFlag = "kti_passwordflag";

        public static string salt = "kti_salt";

        public static string account = "kti_account";
        public static string defaultPriceList = "kti_defaultpricelist";
        public static string salePriceList = "kti_salepricelist";
        public static string warehouseCode = "kti_warehousecode";
        public static string branch = "kti_branch";
    }
}
