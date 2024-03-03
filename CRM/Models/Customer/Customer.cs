#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM.Models.Customer
{

    /// <summary>
    /// Customer
    /// </summary>
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(Customer _customer)
        {
            #region properties
            this.sap_customer_card_code = _customer.sap_customer_card_code;
            this.accountrolecode = _customer.accountrolecode;
            this.address1_addressid = _customer.address1_addressid;
            this.address1_addresstypecode = _customer.address1_addresstypecode;
            this.address1_city = _customer.address1_city;
            this.address1_country = _customer.address1_country;
            this.address1_county = _customer.address1_county;
            this.address1_fax = _customer.address1_fax;
            this.address1_freighttermscode = _customer.address1_freighttermscode;
            this.address1_latitude = _customer.address1_latitude;
            this.address1_line1 = _customer.address1_line1;
            this.address1_line2 = _customer.address1_line2;
            this.address1_line3 = _customer.address1_line3;
            this.address1_longitude = _customer.address1_longitude;
            this.address1_name = _customer.address1_name;
            this.address1_postalcode = _customer.address1_postalcode;
            this.address1_postofficebox = _customer.address1_postofficebox;
            this.address1_primarycontactname = _customer.address1_primarycontactname;
            this.address1_shippingmethodcode = _customer.address1_shippingmethodcode;
            this.address1_stateorprovince = _customer.address1_stateorprovince;
            this.address1_telephone1 = _customer.address1_telephone1;
            this.address1_telephone2 = _customer.address1_telephone2;
            this.address1_telephone3 = _customer.address1_telephone3;
            this.address1_upszone = _customer.address1_upszone;
            this.address1_utcoffset = _customer.address1_utcoffset;
            this.address2_addressid = _customer.address2_addressid;
            this.address2_addresstypecode = _customer.address2_addresstypecode;
            this.address2_city = _customer.address2_city;
            this.address2_country = _customer.address2_country;
            this.address2_county = _customer.address2_county;
            this.address2_fax = _customer.address2_fax;
            this.address2_freighttermscode = _customer.address2_freighttermscode;
            this.address2_latitude = _customer.address2_latitude;
            this.address2_line1 = _customer.address2_line1;
            this.address2_line2 = _customer.address2_line2;
            this.address2_line3 = _customer.address2_line3;
            this.address2_longitude = _customer.address2_longitude;
            this.address2_name = _customer.address2_name;
            this.address2_postalcode = _customer.address2_postalcode;
            this.address2_postofficebox = _customer.address2_postofficebox;
            this.address2_primarycontactname = _customer.address2_primarycontactname;
            this.address2_shippingmethodcode = _customer.address2_shippingmethodcode;
            this.address2_stateorprovince = _customer.address2_stateorprovince;
            this.address2_telephone1 = _customer.address2_telephone1;
            this.address2_telephone2 = _customer.address2_telephone2;
            this.address2_telephone3 = _customer.address2_telephone3;
            this.address2_upszone = _customer.address2_upszone;
            this.address2_utcoffset = _customer.address2_utcoffset;
            this.address3_addressid = _customer.address3_addressid;
            this.address3_addresstypecode = _customer.address3_addresstypecode;
            this.address3_city = _customer.address3_city;
            this.address3_country = _customer.address3_country;
            this.address3_county = _customer.address3_county;
            this.address3_fax = _customer.address3_fax;
            this.address3_freighttermscode = _customer.address3_freighttermscode;
            this.address3_latitude = _customer.address3_latitude;
            this.address3_line1 = _customer.address3_line1;
            this.address3_line2 = _customer.address3_line2;
            this.address3_line3 = _customer.address3_line3;
            this.address3_longitude = _customer.address3_longitude;
            this.address3_name = _customer.address3_name;
            this.address3_postalcode = _customer.address3_postalcode;
            this.address3_postofficebox = _customer.address3_postofficebox;
            this.address3_primarycontactname = _customer.address3_primarycontactname;
            this.address3_shippingmethodcode = _customer.address3_shippingmethodcode;
            this.address3_stateorprovince = _customer.address3_stateorprovince;
            this.address3_telephone1 = _customer.address3_telephone1;
            this.address3_telephone2 = _customer.address3_telephone2;
            this.address3_telephone3 = _customer.address3_telephone3;
            this.address3_upszone = _customer.address3_upszone;
            this.address3_utcoffset = _customer.address3_utcoffset;
            this.anniversary = _customer.anniversary;
            this.annualincome = _customer.annualincome;
            this.assistantname = _customer.assistantname;
            this.assistantphone = _customer.assistantphone;
            this.birthdate = _customer.birthdate;
            this.business2 = _customer.business2;
            this.callback = _customer.callback;
            this.childrensnames = _customer.childrensnames;
            this.company = _customer.company;
            this.contactid = _customer.contactid;
            this.creditlimit = _customer.creditlimit;
            this.creditonhold = _customer.creditonhold;
            this.customersizecode = _customer.customersizecode;
            this.customertypecode = _customer.customertypecode;
            this.defaultpricelevelid = _customer.defaultpricelevelid;
            this.department = _customer.department;
            this.description = _customer.description;
            this.donotbulkemail = _customer.donotbulkemail;
            this.donotbulkpostalmail = _customer.donotbulkpostalmail;
            this.donotemail = _customer.donotemail;
            this.donotfax = _customer.donotfax;
            this.donotphone = _customer.donotphone;
            this.donotpostalmail = _customer.donotpostalmail;
            this.donotsendmm = _customer.donotsendmm;
            this.educationcode = _customer.educationcode;
            this.emailaddress1 = _customer.emailaddress1;
            this.emailaddress2 = _customer.emailaddress2;
            this.emailaddress3 = _customer.emailaddress3;
            this.employeeid = _customer.employeeid;
            this.entityimage = _customer.entityimage;
            this.externaluseridentifier = _customer.externaluseridentifier;
            this.familystatuscode = _customer.familystatuscode;
            this.fax = _customer.fax;
            this.firstname = _customer.firstname;
            this.followemail = _customer.followemail;
            this.ftpsiteurl = _customer.ftpsiteurl;
            this.gendercode = _customer.gendercode;
            this.governmentid = _customer.governmentid;
            this.haschildrencode = _customer.haschildrencode;
            this.home2 = _customer.home2;
            this.importsequencenumber = _customer.importsequencenumber;
            this.isbackofficecustomer = _customer.isbackofficecustomer;
            this.jobtitle = _customer.jobtitle;
            this.lastname = _customer.lastname;
            this.lastonholdtime = _customer.lastonholdtime;
            this.lastusedincampaign = _customer.lastusedincampaign;
            this.leadsourcecode = _customer.leadsourcecode;
            this.managername = _customer.managername;
            this.managerphone = _customer.managerphone;
            this.marketingonly = _customer.marketingonly;
            this.middlename = _customer.middlename;
            this.mobilephone = _customer.mobilephone;
            this.orgchangestatus = _customer.orgchangestatus;
            this.nickname = _customer.nickname;
            this.numberofchildren = _customer.numberofchildren;
            this.originatingleadid = _customer.originatingleadid;
            this.overriddencreatedon = _customer.overriddencreatedon;
            this.ownerid = _customer.ownerid;
            this.owneridtype = _customer.owneridtype;
            this.pager = _customer.pager;
            this.parentcustomerid = _customer.parentcustomerid;
            this.parentcustomeridtype = _customer.parentcustomeridtype;
            this.participatesinworkflow = _customer.participatesinworkflow;
            this.paymenttermscode = _customer.paymenttermscode;
            this.preferredappointmentdaycode = _customer.preferredappointmentdaycode;
            this.preferredappointmenttimecode = _customer.preferredappointmenttimecode;
            this.preferredcontactmethodcode = _customer.preferredcontactmethodcode;
            this.preferredequipmentid = _customer.preferredequipmentid;
            this.preferredserviceid = _customer.preferredserviceid;
            this.preferredsystemuserid = _customer.preferredsystemuserid;
            this.processid = _customer.processid;
            this.salutation = _customer.salutation;
            this.shippingmethodcode = _customer.shippingmethodcode;
            this.slaid = _customer.slaid;
            this.spousesname = _customer.spousesname;
            this.stageid = _customer.stageid;
            this.statecode = _customer.statecode;
            this.statuscode = _customer.statuscode;
            this.subscriptionid = _customer.subscriptionid;
            this.suffix = _customer.suffix;
            this.teamsfollowed = _customer.teamsfollowed;
            this.telephone1 = _customer.telephone1;
            this.telephone2 = _customer.telephone2;
            this.telephone3 = _customer.telephone3;
            this.territorycode = _customer.territorycode;
            this.transactioncurrencyid = _customer.transactioncurrencyid;
            this.websiteurl = _customer.websiteurl;
            this.yomifirstname = _customer.yomifirstname;
            this.yomilastname = _customer.yomilastname;
            this.yomimiddlename = _customer.yomimiddlename;
            this.sourcesystem = _customer.sourcesystem;
            this.clubmembershipid = _customer.clubmembershipid;
            this.country = _customer.country;
            this.customerjoineddate = _customer.customerjoineddate;
            this.customerjoinedoutlet = _customer.customerjoinedoutlet;
            this.firstpointofpurchase = _customer.firstpointofpurchase;
            this.customertype = _customer.customertype;
            this.customerseniority = _customer.customerseniority;
            this.lastcoffeepurchasedate = _customer.lastcoffeepurchasedate;
            this.customerstatus = _customer.customerstatus;
            this.changeofstatusdate = _customer.changeofstatusdate;
            this.customertier = _customer.customertier;
            this.totalcoffeeqtyytd = _customer.totalcoffeeqtyytd;
            this.totalmachineqtyytd = _customer.totalmachineqtyytd;
            this.grandtotalcoffeeqty = _customer.grandtotalcoffeeqty;
            this.grandtotalmachineqty = _customer.grandtotalmachineqty;
            this.channel = _customer.channel;
            this.soremarks = _customer.soremarks;
            this.membercode = _customer.membercode;
            this.acct_w_concern = _customer.acct_w_concern;
            this.kas_name = _customer.kas_name;
            this.checkname = _customer.checkname;
            this.contactperson = _customer.contactperson;
            this.MooExternalId = _customer.MooExternalId;
            this.moosourcesystem = _customer.sourcesystem;
            #endregion
        }

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
        public CRM.CustomDataType.MooDateTime anniversary { get; set; }
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
        public string donotbulkemail { get; set; }
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
        public string followemail { get; set; }
        [StringLength(200)]
        public string ftpsiteurl { get; set; }
        [Range(1, 2)]
        public string gendercode { get; set; }
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
        public CRM.CustomDataType.MooDateTime lastonholdtime { get; set; }
        public CRM.CustomDataType.MooDateTime lastusedincampaign { get; set; }
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
        public CRM.CustomDataType.MooDateTime overriddencreatedon { get; set; }
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
        public string territorycode { get; set; }
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
        public string customerjoineddate { get; set; }
        public string customerjoinedoutlet { get; set; }
        public string firstpointofpurchase { get; set; }
        public string customertype { get; set; }
        public string customerseniority { get; set; }
        public string lastcoffeepurchasedate { get; set; }
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
        #endregion
    }
}