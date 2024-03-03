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
    public class AccountBase : Entity
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region properties
        public OptionSetValue accountcategorycode { get; set; }
        public OptionSetValue accountclassificationcode { get; set; }
        [Required]
        public Guid accountid { get; set; }
        [StringLength(20)]
        public string accountnumber { get; set; }
        public OptionSetValue accountratingcode { get; set; }
        public Guid address1_addressid { get; set; }
        public OptionSetValue address1_addresstypecode { get; set; }
        [StringLength(80)]
        public string address1_city { get; set; }
        [StringLength(80)]
        public string address1_country { get; set; }
        [StringLength(50)]
        public string address1_county { get; set; }
        [StringLength(50)]
        public string address1_fax { get; set; }
        public OptionSetValue address1_freighttermscode { get; set; }
        [Range(-90, 90)]
        public decimal address1_latitude { get; set; }
        [StringLength(250)]
        public string address1_line1 { get; set; }
        [StringLength(250)]
        public string address1_line2 { get; set; }
        [StringLength(250)]
        public string address1_line3 { get; set; }
        [Range(-180, 180)]
        public decimal address1_longitude { get; set; }
        [StringLength(200)]
        public string address1_name { get; set; }
        [StringLength(20)]
        public string address1_postalcode { get; set; }
        [StringLength(20)]
        public string address1_postofficebox { get; set; }
        [StringLength(100)]
        public string address1_primarycontactname { get; set; }
        public OptionSetValue address1_shippingmethodcode { get; set; }
        [StringLength(50)]
        public string address1_stateorprovince { get; set; }
        [StringLength(50)]
        public string address1_telephone1 { get; set; }
        [StringLength(50)]
        public string address1_telephone2 { get; set; }
        [StringLength(50)]
        public string address1_telephone3 { get; set; }
        [StringLength(4)]
        public string address1_upszone { get; set; }
        [Range(-1500, 1500)]
        public int address1_utcoffset { get; set; }
        public Guid address2_addressid { get; set; }
        public OptionSetValue address2_addresstypecode { get; set; }
        [StringLength(80)]
        public string address2_city { get; set; }
        [StringLength(80)]
        public string address2_country { get; set; }
        [StringLength(50)]
        public string address2_county { get; set; }
        [StringLength(50)]
        public string address2_fax { get; set; }
        public OptionSetValue address2_freighttermscode { get; set; }
        [Range(-90, 90)]
        public decimal address2_latitude { get; set; }
        [StringLength(250)]
        public string address2_line1 { get; set; }
        [StringLength(250)]
        public string address2_line2 { get; set; }
        [StringLength(250)]
        public string address2_line3 { get; set; }
        [Range(-180, 180)]
        public decimal address2_longitude { get; set; }
        [StringLength(200)]
        public string address2_name { get; set; }
        [StringLength(20)]
        public string address2_postalcode { get; set; }
        [StringLength(20)]
        public string address2_postofficebox { get; set; }
        [StringLength(100)]
        public string address2_primarycontactname { get; set; }
        public OptionSetValue address2_shippingmethodcode { get; set; }
        [StringLength(50)]
        public string address2_stateorprovince { get; set; }
        [StringLength(50)]
        public string address2_telephone1 { get; set; }
        [StringLength(50)]
        public string address2_telephone2 { get; set; }
        [StringLength(50)]
        public string address2_telephone3 { get; set; }
        [StringLength(4)]
        public string address2_upszone { get; set; }
        [Range(-1500, 1500)]
        public int address2_utcoffset { get; set; }
        public OptionSetValue businesstypecode { get; set; }
        public Money creditlimit { get; set; }
        public bool creditonhold { get; set; }
        public OptionSetValue customersizecode { get; set; }
        public OptionSetValue customertypecode { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        public bool donotbulkemail { get; set; }
        public bool donotbulkpostalmail { get; set; }
        public bool donotemail { get; set; }
        public bool donotfax { get; set; }
        public bool donotphone { get; set; }
        public bool donotpostalmail { get; set; }
        public bool donotsendmm { get; set; }
        [StringLength(100)]
        public string emailaddress1 { get; set; }
        [StringLength(100)]
        public string emailaddress2 { get; set; }
        [StringLength(100)]
        public string emailaddress3 { get; set; }
        [StringLength(50)]
        public string fax { get; set; }
        public bool followemail { get; set; }
        [StringLength(20)]
        public string ftpsiteurl { get; set; }
        public OptionSetValue industrycode { get; set; }
        public DateTime lastonholdtime { get; set; }
        public DateTime lastusedincampaign { get; set; }
        public Money marketcap { get; set; }
        public bool marketingonly { get; set; }
        [Required]
        [StringLength(160)]
        public string name { get; set; }
        [Range(0, 1000000000)]
        public int numberofemployees { get; set; }
        public EntityReference originatingleadid { get; set; }
        public DateTime overriddencreatedon { get; set; }
        public OptionSetValue ownershipcode { get; set; }
        public EntityReference parentaccountid { get; set; }
        public OptionSetValue paymenttermscode { get; set; }
        public OptionSetValue preferredappointmentdaycode { get; set; }
        public OptionSetValue preferredappointmenttimecode { get; set; }
        public OptionSetValue preferredcontactmethodcode { get; set; }
        public EntityReference primarycontactid { get; set; }
        [StringLength(200)]
        public string primarysatoriid { get; set; }
        [StringLength(128)]
        public string primarytwitterid { get; set; }
        public Money revenue { get; set; }
        [Range(0, 1000000000)]
        public int sharesoutstanding { get; set; }
        public OptionSetValue shippingmethodcode { get; set; }
        [StringLength(20)]
        public string sic { get; set; }
        public EntityReference slaid { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        [StringLength(20)]
        public string stockexchange { get; set; }
        [StringLength(50)]
        public string telephone1 { get; set; }
        [StringLength(50)]
        public string telephone2 { get; set; }
        [StringLength(50)]
        public string telephone3 { get; set; }
        public OptionSetValue territorycode { get; set; }
        public EntityReference territoryid { get; set; }
        [StringLength(10)]
        public string tickersymbol { get; set; }
        public EntityReference transactioncurrencyid { get; set; }
        [StringLength(200)]
        public string websiteurl { get; set; }
        [StringLength(160)]
        public string yominame { get; set; }

        public int kti_socialchannelorigin { get; set; }
        public string kti_sourceid { get; set; }

        #endregion
    }
}
