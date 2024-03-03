using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Models.Promo
{
    public class Promo
    {
        public Promo()
        {


        }




        public Promo(Entity _entity, IOrganizationService service)
        {
            #region properties
            this.kti_name = _entity.Contains("kti_name") ? (string)_entity["kti_name"] : default;
            this.kti_promoid = _entity.Id.ToString();
            this.kti_channelid = _entity.Contains("kti_channelid") ? (string)_entity["kti_channelid"] : default;
            this.kti_promocode = _entity.Contains("kti_promocode") ? (string)_entity["kti_promocode"] : default;
            this.kti_discountamount = _entity.Contains("kti_discountamount") ? ((Money)_entity["kti_discountamount"]).Value : default;
            this.kti_discountpercent = _entity.Contains("kti_discountpercent") ? (decimal)_entity["kti_discountpercent"] : default;
            this.kti_freedelivery = _entity.Contains("kti_freedelivery") ? (bool)_entity["kti_freedelivery"] : default;
            this.kti_ismaxdiscountamount = _entity.Contains("kti_ismaxdiscountamount") ? (bool)_entity["kti_ismaxdiscountamount"] : default;
            this.kti_maxdiscountamount = _entity.Contains("kti_maxdiscountamount") ? ((Money)_entity["kti_maxdiscountamount"]).Value : default;
            this.kti_minspent = _entity.Contains("kti_minspent") ? ((Money)_entity["kti_minspent"]).Value : default;
            this.kti_minquantity = _entity.Contains("kti_minquantity") ? (decimal)_entity["kti_minquantity"] : default;
            this.kti_validfrom = _entity.Contains("kti_validfrom") ? (DateTime)_entity["kti_validfrom"] : default;
            this.kti_validto = _entity.Contains("kti_validto") ? (DateTime)_entity["kti_validto"] : default;
            #endregion
        }

        #region properties


        // public string kti_saleschannel { get; set; }
        public virtual string kti_name { get; set; }
        public virtual string kti_promoid { get; set; }

        public virtual string kti_channelid { get; set; }
        public virtual string kti_promocode { get; set; }
        public virtual decimal kti_discountamount { get; set; }
        public virtual decimal kti_discountpercent { get; set; }
        public virtual bool kti_freedelivery { get; set; }
        public virtual bool kti_ismaxdiscountamount { get; set; }
        public virtual decimal kti_maxdiscountamount { get; set; }
        public virtual decimal kti_minspent { get; set; }
        public virtual decimal kti_minquantity { get; set; }

        public virtual DateTime kti_validfrom { get; set; }
        public virtual DateTime kti_validto { get; set; }

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


