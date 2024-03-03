using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Model
{
    public class BranchBase : Entity
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region properties
        public string kti_addresscity { get; set; }
        public string kti_addresspostalcode { get; set; }
        public string kti_addressregion { get; set; }
        public string kti_addressstreet { get; set; }
        public bool kti_autoassignrider { get; set; }
        public string kti_branchid { get; set; }
        public string kti_branchcode { get; set; }
        public int ktideliveryminimumorderop { get; set; }
        public string kti_description { get; set; }
        public string kti_emailaddress { get; set; }
        public bool kti_enabledelivery { get; set; }
        public bool kti_enablegeofencing { get; set; }
        public bool kti_enablelalamove { get; set; }
        public bool kti_enablemrspeedy { get; set; }
        public bool kti_enablepickup { get; set; }
        public bool kti_enablexendit { get; set; }
        public bool kti_fridaystate { get; set; }
        public string kti_fridayend { get; set; }
        public string kti_fridaystart { get; set; }
        public bool kti_thursdaystate { get; set; }
        public string kti_thursdayend { get; set; }
        public string kti_thursdaystart { get; set; }
        public bool kti_wednesdaystate { get; set; }
        public string kti_wednesdayend { get; set; }
        public string kti_wednesdaystart { get; set; }
        public bool kti_tuesdaystate { get; set; }
        public string kti_tuesdayend { get; set; }
        public string kti_tuesdaystart { get; set; }
        public bool kti_mondaystate { get; set; }
        public string kti_mondayend { get; set; }
        public string kti_mondaystart { get; set; }
        public bool kti_sundaystate { get; set; }
        public string kti_sundayend { get; set; }
        public string kti_sundaystart { get; set; }
        public bool kti_saturdaystate { get; set; }
        public string kti_saturdayend { get; set; }
        public string kti_saturdaystart { get; set; }
        public decimal kti_geofenceradius { get; set; }
        public string kti_latitude { get; set; }
        public string kti_longitude { get; set; }
        public string kti_mobilephone { get; set; }
        public bool kti_mooenabled { get; set; }
        public string kti_name { get; set; }
        public bool kti_overridewarehousedelivery { get; set; }
        public string kti_phonenumber { get; set; }
        public int kti_pickupminimumsizeop { get; set; }
        public EntityReference kti_saleschannel { get; set; }
        public bool kti_branchstate { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        public string kti_website { get; set; }
        public string kti_warehouseaddress { get; set; }
        public string kti_warehouselatitude { get; set; }
        public string kti_warehouselongitude { get; set; }
        public string kti_warehousephone { get; set; }
        public string kti_warehousewebsite { get; set; }
        public string kti_xenditapikey { get; set; }
        public string kti_xenditinvoiceurl { get; set; }

        #endregion
    }
}