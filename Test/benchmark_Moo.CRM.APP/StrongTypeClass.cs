using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benchmark_Moo.CRM.APP
{
    public class StrongTypeClass
    {
        public StrongTypeClass()
        {


        }


        #region properties

        //     public virtual string kti_sourceid { get; set; }
        public virtual int companyid { get; set; }
        [Range(1, 3)]
        public virtual int accountrolecode { get; set; }
        public virtual DateTime anniversary { get; set; }
        [Range(0, 100000000000000)]
        public virtual decimal annualincome { get; set; }
        [StringLength(100)]
        public virtual string assistantname { get; set; }
        [StringLength(50)]
        public virtual string assistantphone { get; set; }
        public virtual string birthdate { get; set; }
        [StringLength(50)]
        public virtual string business2 { get; set; }
        [StringLength(50)]
        public virtual string callback { get; set; }
        [StringLength(255)]
        public virtual string childrensnames { get; set; }
        [StringLength(50)]
        public virtual string company { get; set; }
        public virtual string contactid { get; set; }
        [Range(0, 100000000000000)]
        public virtual decimal creditlimit { get; set; }
        public virtual bool creditonhold { get; set; }
        [Range(1, 1)]
        public virtual int customersizecode { get; set; }
        [Range(1, 1)]
        public virtual int customertypecode { get; set; }
        public virtual string defaultpricelevelid { get; set; }
        [StringLength(100)]
        public virtual string department { get; set; }
        [StringLength(2000)]
        public virtual string description { get; set; }
        public virtual string donotbulkemail { get; set; }
        public virtual bool donotbulkpostalmail { get; set; }
        public virtual bool donotemail { get; set; }
        public virtual bool donotfax { get; set; }
        public virtual bool donotphone { get; set; }
        public virtual bool donotpostalmail { get; set; }
        public virtual bool donotsendmm { get; set; }
        [Range(1, 1)]
        public virtual int educationcode { get; set; }
        [StringLength(50)]
        public virtual string employeeid { get; set; }
        public virtual string entityimage { get; set; }
        [StringLength(50)]
        public virtual string externaluseridentifier { get; set; }
        [Range(1, 4)]
        public virtual int familystatuscode { get; set; }
        [StringLength(50)]
        public virtual string fax { get; set; }
        [StringLength(50)]
        public virtual string firstname { get; set; }
        public virtual string followemail { get; set; }
        [StringLength(200)]
        public virtual string ftpsiteurl { get; set; }
        [Range(1, 2)]
        public virtual int gendercode { get; set; }
        [StringLength(50)]
        public virtual string governmentid { get; set; }
        [Range(1, 1)]
        public virtual int haschildrencode { get; set; }
        [StringLength(50)]
        public virtual string home2 { get; set; }
        [Range(-2147483648, 2147483647)]
        public virtual int importsequencenumber { get; set; }
        public virtual bool isbackofficecustomer { get; set; }
        [StringLength(100)]
        public virtual string jobtitle { get; set; }
        [StringLength(50)]
        [Required]
        public virtual string lastname { get; set; }
        public virtual DateTime lastonholdtime { get; set; }
        public virtual DateTime lastusedincampaign { get; set; }
        [Range(1, 1)]
        public virtual int leadsourcecode { get; set; }
        [StringLength(100)]
        public virtual string managername { get; set; }
        [StringLength(50)]
        public virtual string managerphone { get; set; }
        public virtual bool marketingonly { get; set; }
        [StringLength(50)]
        public virtual string middlename { get; set; }
        [StringLength(50)]
        public virtual string mobilephone { get; set; }
        [Range(0, 2)]
        public virtual int orgchangestatus { get; set; }
        [StringLength(100)]
        public virtual string nickname { get; set; }
        [Range(0, 1000000000)]
        public virtual int numberofchildren { get; set; }
        public virtual string originatingleadid { get; set; }
        public virtual DateTime overriddencreatedon { get; set; }
        public virtual string ownerid { get; set; }
        public virtual string owneridtype { get; set; }
        [StringLength(50)]
        public virtual string pager { get; set; }
        public virtual string parentcustomerid { get; set; }
        public virtual string parentcustomeridtype { get; set; }
        public virtual bool participatesinworkflow { get; set; }
        [Range(1, 4)]
        public virtual int paymenttermscode { get; set; }
        [Range(0, 6)]
        public virtual int preferredappointmentdaycode { get; set; }
        [Range(1, 3)]
        public virtual int preferredappointmenttimecode { get; set; }
        [Range(1, 5)]
        public virtual int preferredcontactmethodcode { get; set; }
        public virtual string preferredequipmentid { get; set; }
        public virtual string preferredserviceid { get; set; }
        public virtual string preferredsystemuserid { get; set; }
        public virtual string processid { get; set; }
        [StringLength(100)]
        public virtual string salutation { get; set; }
        [Range(1, 1)]
        public virtual int shippingmethodcode { get; set; }
        public virtual string slaid { get; set; }
        [StringLength(100)]
        public virtual string spousesname { get; set; }
        public virtual string stageid { get; set; }
        public virtual int statecode { get; set; }
        public virtual int statuscode { get; set; }
        public virtual string subscriptionid { get; set; }
        [StringLength(10)]
        public virtual string suffix { get; set; }
        [Range(-2147483648, 2147483647)]
        public virtual string teamsfollowed { get; set; }
        [Range(1, 1)]
        public virtual string territorycode { get; set; }
        public virtual string transactioncurrencyid { get; set; }
        [StringLength(200)]
        public virtual string websiteurl { get; set; }
        [StringLength(150)]
        public virtual string yomifirstname { get; set; }
        [StringLength(150)]
        public virtual string yomilastname { get; set; }
        [StringLength(150)]
        public virtual string yomimiddlename { get; set; }
        public virtual string sourcesystem { get; set; }
        public virtual string country { get; set; }
        public virtual string contactperson { get; set; }
        [JsonIgnore]
        public virtual string moosourcesystem { get; set; }
        [JsonIgnore]
        public virtual string MooExternalId { get; set; }
        #endregion

        #region NCCI_Properties
        public string sap_customer_card_code { get; set; }
        public string ncci_customerjoineddate { get; set; }
        public string ncci_clubmembershipid { get; set; }
        [JsonProperty(PropertyName = "ncci_customerjoinedbranch@odata.bind")]
        public string ncci_customerjoinedbranch { get; set; }
        #endregion

        #region properties

        public string address1_addressid { get; set; }
        [Range(1, 4)]
        public int address1_addresstypecode { get; set; }
        [StringLength(80)]
        public string address1_city { get; set; }
        [StringLength(80)]
        public string address1_country { get; set; }
        [StringLength(50)]
        public string address1_county { get; set; }
        [StringLength(50)]
        public string address1_fax { get; set; }
        [Range(1, 2)]
        public int address1_freighttermscode { get; set; }
        [Range(-90, 90)]
        public double address1_latitude { get; set; }
        [StringLength(250)]
        public string address1_line1 { get; set; }
        [StringLength(250)]
        public string address1_line2 { get; set; }
        [StringLength(250)]
        public string address1_line3 { get; set; }
        [Range(-180, 180)]
        public double address1_longitude { get; set; }
        [StringLength(200)]
        public string address1_name { get; set; }
        [StringLength(20)]
        public string address1_postalcode { get; set; }
        [StringLength(20)]
        public string address1_postofficebox { get; set; }
        [StringLength(100)]
        public string address1_primarycontactname { get; set; }
        [Range(1, 7)]
        public int address1_shippingmethodcode { get; set; }
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
        public string address2_addressid { get; set; }
        [Range(1, 1)]
        public string address2_addresstypecode { get; set; }
        [StringLength(80)]
        public string address2_city { get; set; }
        [StringLength(80)]
        public string address2_country { get; set; }
        [StringLength(50)]
        public string address2_county { get; set; }
        [StringLength(50)]
        public string address2_fax { get; set; }
        [Range(1, 1)]
        public int address2_freighttermscode { get; set; }
        [Range(-90, 90)]
        public double address2_latitude { get; set; }
        [StringLength(250)]
        public string address2_line1 { get; set; }
        [StringLength(250)]
        public string address2_line2 { get; set; }
        [StringLength(250)]
        public string address2_line3 { get; set; }
        [Range(-180, 180)]
        public double address2_longitude { get; set; }
        [StringLength(200)]
        public string address2_name { get; set; }
        [StringLength(20)]
        public string address2_postalcode { get; set; }
        [StringLength(20)]
        public string address2_postofficebox { get; set; }
        [StringLength(100)]
        public string address2_primarycontactname { get; set; }
        [Range(1, 1)]
        public int address2_shippingmethodcode { get; set; }
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
        public string address3_addressid { get; set; }
        [Range(1, 1)]
        public string address3_addresstypecode { get; set; }
        [StringLength(80)]
        public string address3_city { get; set; }
        [StringLength(80)]
        public string address3_country { get; set; }
        [StringLength(50)]
        public string address3_county { get; set; }
        [StringLength(50)]
        public string address3_fax { get; set; }
        [Range(1, 1)]
        public int address3_freighttermscode { get; set; }
        [Range(-90, 90)]
        public double address3_latitude { get; set; }
        [StringLength(250)]
        public string address3_line1 { get; set; }
        [StringLength(250)]
        public string address3_line2 { get; set; }
        [StringLength(250)]
        public string address3_line3 { get; set; }
        [Range(-180, 180)]
        public double address3_longitude { get; set; }
        [StringLength(200)]
        public string address3_name { get; set; }
        [StringLength(20)]
        public string address3_postalcode { get; set; }
        [StringLength(20)]
        public string address3_postofficebox { get; set; }
        [StringLength(100)]
        public string address3_primarycontactname { get; set; }
        [Range(1, 1)]
        public int address3_shippingmethodcode { get; set; }
        [StringLength(50)]
        public string address3_stateorprovince { get; set; }
        [StringLength(50)]
        public string address3_telephone1 { get; set; }
        [StringLength(50)]
        public string address3_telephone2 { get; set; }
        [StringLength(50)]
        public string address3_telephone3 { get; set; }
        [StringLength(4)]
        public string address3_upszone { get; set; }
        [Range(-1500, 1500)]
        public int address3_utcoffset { get; set; }

        [StringLength(100)]
        public string emailaddress1 { get; set; }
        [StringLength(100)]
        public string emailaddress2 { get; set; }
        [StringLength(100)]
        public string emailaddress3 { get; set; }
        [StringLength(50)]
        public string telephone1 { get; set; }
        [StringLength(50)]
        public string telephone2 { get; set; }
        [StringLength(50)]
        public string telephone3 { get; set; }
        public int kti_socialchannelorigin { get; set; }
        public string kti_sourceid { get; set; }
        public int kti_customertype { get; set; }

        #endregion
    }
}
