using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM_Plugin.Core.Model
{
    public class CustomerBase
    {
        #region columnSet
        public static ColumnSet columnSet = new ColumnSet(true);
        #endregion

        #region properties
        public string sap_customer_card_code { get; set; }
        [Range(1, 3)]
        public int accountrolecode { get; set; }
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
        public int address2_addresstypecode { get; set; }
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
        public int address3_addresstypecode { get; set; }
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
        public DateTime anniversary { get; set; }
        [Range(0, 100000000000000)]
        public decimal annualincome { get; set; }
        [StringLength(100)]
        public string assistantname { get; set; }
        [StringLength(50)]
        public string assistantphone { get; set; }
        public string birthdate { get; set; }
        [StringLength(50)]
        public string business2 { get; set; }
        [StringLength(50)]
        public string callback { get; set; }
        [StringLength(255)]
        public string childrensnames { get; set; }
        [StringLength(50)]
        public string company { get; set; }
        public string contactid { get; set; }
        [Range(0, 100000000000000)]
        public decimal creditlimit { get; set; }
        public bool creditonhold { get; set; }
        [Range(1, 1)]
        public int customersizecode { get; set; }
        [Range(1, 1)]
        public int customertypecode { get; set; }
        public string defaultpricelevelid { get; set; }
        [StringLength(100)]
        public string department { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        public bool donotbulkemail { get; set; }
        public bool donotbulkpostalmail { get; set; }
        public bool donotemail { get; set; }
        public bool donotfax { get; set; }
        public bool donotphone { get; set; }
        public bool donotpostalmail { get; set; }
        public bool donotsendmm { get; set; }
        [Range(1, 1)]
        public int educationcode { get; set; }
        [StringLength(100)]
        public string emailaddress1 { get; set; }
        [StringLength(100)]
        public string emailaddress2 { get; set; }
        [StringLength(100)]
        public string emailaddress3 { get; set; }
        [StringLength(50)]
        public string employeeid { get; set; }
        public string entityimage { get; set; }
        [StringLength(50)]
        public string externaluseridentifier { get; set; }
        [Range(1, 4)]
        public int familystatuscode { get; set; }
        [StringLength(50)]
        public string fax { get; set; }
        [StringLength(50)]
        public string firstname { get; set; }
        public bool followemail { get; set; }
        [StringLength(200)]
        public string ftpsiteurl { get; set; }
        [Range(1, 2)]
        public int gendercode { get; set; }
        [StringLength(50)]
        public string governmentid { get; set; }
        [Range(1, 1)]
        public int haschildrencode { get; set; }
        [StringLength(50)]
        public string home2 { get; set; }
        [Range(-2147483648, 2147483647)]
        public int importsequencenumber { get; set; }
        public bool isbackofficecustomer { get; set; }
        [StringLength(100)]
        public string jobtitle { get; set; }
        [StringLength(50)]
        [Required]
        public string lastname { get; set; }
        public DateTime lastonholdtime { get; set; }
        public DateTime lastusedincampaign { get; set; }
        [Range(1, 1)]
        public int leadsourcecode { get; set; }
        [StringLength(100)]
        public string managername { get; set; }
        [StringLength(50)]
        public string managerphone { get; set; }
        public bool marketingonly { get; set; }
        [StringLength(50)]
        public string middlename { get; set; }
        [StringLength(50)]
        public string mobilephone { get; set; }
        [Range(0, 2)]
        public int orgchangestatus { get; set; }
        [StringLength(100)]
        public string nickname { get; set; }
        [Range(0, 1000000000)]
        public int numberofchildren { get; set; }
        public string originatingleadid { get; set; }
        public DateTime overriddencreatedon { get; set; }
        public string ownerid { get; set; }
        public string owneridtype { get; set; }
        [StringLength(50)]
        public string pager { get; set; }
        public string parentcustomerid { get; set; }
        public string parentcustomeridtype { get; set; }
        public bool participatesinworkflow { get; set; }
        [Range(1, 4)]
        public int paymenttermscode { get; set; }
        [Range(0, 6)]
        public int preferredappointmentdaycode { get; set; }
        [Range(1, 3)]
        public int preferredappointmenttimecode { get; set; }
        [Range(1, 5)]
        public int preferredcontactmethodcode { get; set; }
        public string preferredequipmentid { get; set; }
        public string preferredserviceid { get; set; }
        public string preferredsystemuserid { get; set; }
        public string processid { get; set; }
        [StringLength(100)]
        public string salutation { get; set; }
        [Range(1, 1)]
        public int shippingmethodcode { get; set; }
        public string slaid { get; set; }
        [StringLength(100)]
        public string spousesname { get; set; }
        public string stageid { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        public string subscriptionid { get; set; }
        [StringLength(10)]
        public string suffix { get; set; }
        [Range(-2147483648, 2147483647)]
        public string teamsfollowed { get; set; }
        [StringLength(50)]
        public string telephone1 { get; set; }
        [StringLength(50)]
        public string telephone2 { get; set; }
        [StringLength(50)]
        public string telephone3 { get; set; }
        [Range(1, 1)]
        public int territorycode { get; set; }
        public string transactioncurrencyid { get; set; }
        [StringLength(200)]
        public string websiteurl { get; set; }
        [StringLength(150)]
        public string yomifirstname { get; set; }
        [StringLength(150)]
        public string yomilastname { get; set; }
        [StringLength(150)]
        public string yomimiddlename { get; set; }
        public string sourcesystem { get; set; }
        public string clubmembershipid { get; set; }
        public string country { get; set; }
        public DateTime customerjoineddate { get; set; }
        public string customerjoinedoutlet { get; set; }
        public string firstpointofpurchase { get; set; }
        public int customertype { get; set; } 
        public int customerseniority { get; set; }
        public DateTime lastcoffeepurchasedate { get; set; }
        public string customerstatus { get; set; }
        public string changeofstatusdate { get; set; }
        public string customertier { get; set; }
        public string totalcoffeeqtyytd { get; set; }
        public string totalmachineqtyytd { get; set; }
        public string grandtotalcoffeeqty { get; set; }
        public string grandtotalmachineqty { get; set; }
        public string channel { get; set; }
        public string soremarks { get; set; }
        public string membercode { get; set; }
        public string acct_w_concern { get; set; }
        public string kas_name { get; set; }
        public string checkname { get; set; }
        public string contactperson { get; set; }
        public string moosourcesystem { get; set; }
        public string MooExternalId { get; set; }

        public int kti_socialchannelorigin { get; set; }
        public string kti_sourceid { get; set; }
   
        #endregion
    }
}
