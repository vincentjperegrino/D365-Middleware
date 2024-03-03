using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Helper.EntityHelper
{
    public class Employee : BaseEntity
    {
        public static new string entity_name = "kti_employees";
        public static new string entity_id = "kti_employeesid";

        public static string password = "kti_password";
        public static string passwordFlag = "kti_passwordflag";
        public static string salt = "kti_salt";
    }
}
