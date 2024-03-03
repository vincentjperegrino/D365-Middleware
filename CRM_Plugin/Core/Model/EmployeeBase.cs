using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.Core.Model
{
    public class EmployeeBase : Entity
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region properties
        public string kti_branchcode { get; set; }
        public string kti_email { get; set; }
        public string kti_employeeid { get; set; }
        public string kti_fullname { get; set; }
        public string kti_firstname { get; set; }
        public string kti_lastname { get; set; }
        public string kti_middlename { get; set; }
        public string kti_mobilenumber { get; set; }
        public string kti_name { get; set; }
        public string kti_password { get; set; }
        public string kti_passwordflag { get; set; }
        public string kti_pincode { get; set; }
        public string kti_profilepicture { get; set; }
        public OptionSetValue kti_role { get; set; }
        public string kti_salt { get; set; }
        #endregion
    }
}
