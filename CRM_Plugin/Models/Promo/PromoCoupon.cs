using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Models.Promo
{

    public class PromoCoupon
    {

        public PromoCoupon()
        {


        }

        public PromoCoupon(Entity _entity, IOrganizationService service)
        {
            #region properties
            this.kti_name = _entity.Contains("kti_name") ? (string)_entity["kti_name"] : default;
            this.kti_promocouponid = _entity.Id.ToString();
            this.kti_uniquepromocode = _entity.Contains("kti_uniquepromocode") ? (string)_entity["kti_uniquepromocode"] : default;
            this.kti_customerid = _entity.Contains("kti_customerid") ? ((EntityReference)_entity["kti_customerid"]).Id.ToString() : default;
            this.kti_promocode = _entity.Contains("kti_promocode") ? ((EntityReference)_entity["kti_promocode"]).Id.ToString() : default;
            #endregion
        }



        #region properties
        public virtual string kti_name { get; set; }
        public virtual string kti_promocouponid { get; set; }
        public virtual string kti_uniquepromocode { get; set; }

        [JsonProperty(PropertyName = "kti_customerid@odata.bind")]
        public virtual string kti_customerid { get; set; }

        [JsonProperty(PropertyName = "kti_promocode@odata.bind")]
        public virtual string kti_promocode { get; set; }



        public string createdby { get; set; }
        public DateTime createdon { get; set; }
        public string createdonbehalfby { get; set; }
        public int importsequencenumber { get; set; }
        public string modifiedby { get; set; }
        public DateTime modifiedon { get; set; }
        public string modifiedonbehalfby { get; set; }
        public DateTime overriddencreatedon { get; set; }
        public string ownerid { get; set; }
        public string owningbusinessunit { get; set; }
        public string owningteam { get; set; }
        public string owninguser { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }

        #endregion

    }

}

