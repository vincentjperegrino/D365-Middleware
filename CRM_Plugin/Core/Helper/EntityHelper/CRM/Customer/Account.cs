using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Helper.EntityHelper.CRM
{
    public class Account : BaseEntity
    {
        new public static string entity_name = "account";
        new public static string entity_id = "accountid";

        public static string name = "name";
        public static string kti_sourceid = "kti_sourceid";
        public static string kti_socialchannelorigin = "kti_socialchannelorigin";
        public static string emailaddress1 = "emailaddress1";
        public static string telephone1 = "telephone1";
    }
}
