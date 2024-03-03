#region Namespaces
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#endregion

namespace CRM_Plugin.Models.Customer
{

    /// <summary>
    /// Customer
    /// </summary>
    public class Customer
    {
        public string EntityName = "contact";
        public string PrimaryKey = "contactid";
        public Customer()
        {
        }

        public Customer(Entity _entity)
        {
            #region properties
            string erroron = "";

            try
            {
                erroron = "accountid";
                this.contactid = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");

            }
            #endregion
        }

        public Customer(Entity _entity, IOrganizationService service)
        {
            #region properties
            string erroron = "";
            try
            {

                erroron = "sap_customer_card_code";
                this.sap_customer_card_code = _entity.Contains("sap_customer_card_code") ? (string)_entity["sap_customer_card_code"] : default;
                erroron = "accountrolecode";
                this.accountrolecode = _entity.Contains("accountrolecode") ? (int)_entity["accountrolecode"] : default;
                erroron = "address1";
                this.address1_addressid = _entity.Contains("address1_addressid") ? ((Guid)_entity["address1_addressid"]).ToString() : default;
                this.address1_addresstypecode = _entity.Contains("address1_addresstypecode") ? ((OptionSetValue)_entity["address1_addresstypecode"]).Value : default;
                this.address1_city = _entity.Contains("address1_city") ? (string)_entity["address1_city"] : default;
                this.address1_country = _entity.Contains("address1_country") ? (string)_entity["address1_country"] : default;
                this.address1_county = _entity.Contains("address1_county") ? (string)_entity["address1_county"] : default;
                this.address1_fax = _entity.Contains("address1_fax") ? (string)_entity["address1_fax"] : default;
                this.address1_freighttermscode = _entity.Contains("address1_freighttermscode") ? ((OptionSetValue)_entity["address1_freighttermscode"]).Value : default;
                this.address1_latitude = _entity.Contains("address1_latitude") ? (double)_entity["address1_latitude"] : default;
                this.address1_line1 = _entity.Contains("address1_line1") ? (string)_entity["address1_line1"] : default;
                this.address1_line2 = _entity.Contains("address1_line2") ? (string)_entity["address1_line2"] : default;
                this.address1_line3 = _entity.Contains("address1_line3") ? (string)_entity["address1_line3"] : default;
                this.address1_longitude = _entity.Contains("address1_longitude") ? (double)_entity["address1_longitude"] : default;
                this.address1_name = _entity.Contains("address1_name") ? (string)_entity["address1_name"] : default;
                this.address1_postalcode = _entity.Contains("address1_postalcode") ? (string)_entity["address1_postalcode"] : default;
                this.address1_postofficebox = _entity.Contains("address1_postofficebox") ? (string)_entity["address1_postofficebox"] : default;
                this.address1_primarycontactname = _entity.Contains("address1_primarycontactname") ? (string)_entity["address1_primarycontactname"] : default;
                this.address1_shippingmethodcode = _entity.Contains("address1_shippingmethodcode") ? ((OptionSetValue)_entity["address1_shippingmethodcode"]).Value : default;
                this.address1_stateorprovince = _entity.Contains("address1_stateorprovince") ? (string)_entity["address1_stateorprovince"] : default;
                this.address1_telephone1 = _entity.Contains("address1_telephone1") ? (string)_entity["address1_telephone1"] : default;
                this.address1_telephone2 = _entity.Contains("address1_telephone2") ? (string)_entity["address1_telephone2"] : default;
                this.address1_telephone3 = _entity.Contains("address1_telephone3") ? (string)_entity["address1_telephone3"] : default;
                this.address1_upszone = _entity.Contains("address1_upszone") ? (string)_entity["address1_upszone"] : default;
                this.address1_utcoffset = _entity.Contains("address1_utcoffset") ? (int)_entity["address1_utcoffset"] : default;
                erroron = "address2";
                this.address2_addressid = _entity.Contains("address2_addressid") ? ((Guid)_entity["address2_addressid"]).ToString() : default;
                this.address2_addresstypecode = _entity.Contains("address2_addresstypecode") ? ((OptionSetValue)_entity["address2_addresstypecode"]).Value : default;
                this.address2_city = _entity.Contains("address2_city") ? (string)_entity["address2_city"] : default;
                this.address2_country = _entity.Contains("address2_country") ? (string)_entity["address2_country"] : default;
                this.address2_county = _entity.Contains("address2_county") ? (string)_entity["address2_county"] : default;
                this.address2_fax = _entity.Contains("address2_fax") ? (string)_entity["address2_fax"] : default;
                this.address2_freighttermscode = _entity.Contains("address2_freighttermscode") ? ((OptionSetValue)_entity["address2_freighttermscode"]).Value : default;
                this.address2_latitude = _entity.Contains("address2_latitude") ? (double)_entity["address2_latitude"] : default;
                this.address2_line1 = _entity.Contains("address2_line1") ? (string)_entity["address2_line1"] : default;
                this.address2_line2 = _entity.Contains("address2_line2") ? (string)_entity["address2_line2"] : default;
                this.address2_line3 = _entity.Contains("address2_line3") ? (string)_entity["address2_line3"] : default;
                this.address2_longitude = _entity.Contains("address2_longitude") ? (double)_entity["address2_longitude"] : default;
                this.address2_name = _entity.Contains("address2_name") ? (string)_entity["address2_name"] : default;
                this.address2_postalcode = _entity.Contains("address2_postalcode") ? (string)_entity["address2_postalcode"] : default;
                this.address2_postofficebox = _entity.Contains("address2_postofficebox") ? (string)_entity["address2_postofficebox"] : default;
                this.address2_primarycontactname = _entity.Contains("address2_primarycontactname") ? (string)_entity["address2_primarycontactname"] : default;
                this.address2_shippingmethodcode = _entity.Contains("address2_shippingmethodcode") ? ((OptionSetValue)_entity["address2_shippingmethodcode"]).Value : default;
                this.address2_stateorprovince = _entity.Contains("address2_stateorprovince") ? (string)_entity["address2_stateorprovince"] : default;
                this.address2_telephone1 = _entity.Contains("address2_telephone1") ? (string)_entity["address2_telephone1"] : default;
                this.address2_telephone2 = _entity.Contains("address2_telephone2") ? (string)_entity["address2_telephone2"] : default;
                this.address2_telephone3 = _entity.Contains("address2_telephone3") ? (string)_entity["address2_telephone3"] : default;
                this.address2_upszone = _entity.Contains("address2_upszone") ? (string)_entity["address2_upszone"] : default;
                this.address2_utcoffset = _entity.Contains("address2_utcoffset") ? (int)_entity["address2_utcoffset"] : default;
                erroron = "address3";
                this.address3_addressid = _entity.Contains("address3_addressid") ? ((Guid)_entity["address3_addressid"]).ToString() : default;
                this.address3_addresstypecode = _entity.Contains("address3_addresstypecode") ? ((OptionSetValue)_entity["address3_addresstypecode"]).Value : default;
                this.address3_city = _entity.Contains("address3_city") ? (string)_entity["address3_city"] : default;
                this.address3_country = _entity.Contains("address3_country") ? (string)_entity["address3_country"] : default;
                this.address3_county = _entity.Contains("address3_county") ? (string)_entity["address3_county"] : default;
                this.address3_fax = _entity.Contains("address3_fax") ? (string)_entity["address3_fax"] : default;
                this.address3_freighttermscode = _entity.Contains("address3_freighttermscode") ? ((OptionSetValue)_entity["address3_freighttermscode"]).Value : default;
                this.address3_latitude = _entity.Contains("address3_fax") ? (double)_entity["address3_fax"] : default;
                this.address3_line1 = _entity.Contains("address3_line1") ? (string)_entity["address3_line1"] : default;
                this.address3_line2 = _entity.Contains("address3_line2") ? (string)_entity["address3_line2"] : default;
                this.address3_line3 = _entity.Contains("address3_line3") ? (string)_entity["address3_line3"] : default;
                this.address3_longitude = _entity.Contains("address3_longitude") ? (double)_entity["address3_longitude"] : default;
                this.address3_name = _entity.Contains("address3_name") ? (string)_entity["address3_name"] : default;
                this.address3_postalcode = _entity.Contains("address3_postalcode") ? (string)_entity["address3_postalcode"] : default;
                this.address3_postofficebox = _entity.Contains("address3_postofficebox") ? (string)_entity["address3_postofficebox"] : default;
                this.address3_primarycontactname = _entity.Contains("address3_primarycontactname") ? (string)_entity["address3_primarycontactname"] : default;
                this.address3_shippingmethodcode = _entity.Contains("address3_shippingmethodcode") ? ((OptionSetValue)_entity["address3_shippingmethodcode"]).Value : default;
                this.address3_stateorprovince = _entity.Contains("address3_stateorprovince") ? (string)_entity["address3_stateorprovince"] : default;
                this.address3_telephone1 = _entity.Contains("address3_telephone1") ? (string)_entity["address3_telephone1"] : default;
                this.address3_telephone2 = _entity.Contains("address3_telephone2") ? (string)_entity["address3_telephone2"] : default;
                this.address3_telephone3 = _entity.Contains("address3_telephone3") ? (string)_entity["address3_telephone3"] : default;
                this.address3_upszone = _entity.Contains("address3_upszone") ? (string)_entity["address3_upszone"] : default;
                this.address3_utcoffset = _entity.Contains("address3_utcoffset") ? (int)_entity["address3_utcoffset"] : default;
                erroron = "anniversary";
                this.anniversary = _entity.Contains("anniversary") ? (DateTime)_entity["anniversary"] : default;
                erroron = "annualincome";
                this.annualincome = _entity.Contains("annualincome") ? ((Money)_entity["annualincome"]).Value : default;
                erroron = "assistantname";
                this.assistantname = _entity.Contains("assistantname") ? (string)_entity["assistantname"] : default;
                erroron = "assistantphone";
                this.assistantphone = _entity.Contains("assistantphone") ? (string)_entity["assistantphone"] : default;
                erroron = "birthdate";
                this.birthdate = _entity.Contains("birthdate") ? (DateTime)_entity["birthdate"] : default;
                erroron = "business2";
                this.business2 = _entity.Contains("business2") ? (string)_entity["business2"] : default;
                erroron = "callback";
                this.callback = _entity.Contains("callback") ? (string)_entity["callback"] : default;
                erroron = "childrensnames";
                this.childrensnames = _entity.Contains("childrensnames") ? (string)_entity["childrensnames"] : default;
                erroron = "company";
                this.company = _entity.Contains("company") ? (string)_entity["company"] : default;
                erroron = "contactid";
                this.contactid = _entity.Contains("contactid") ? ((Guid)_entity["contactid"]).ToString() : default;
                erroron = "creditlimit";
                this.creditlimit = _entity.Contains("creditlimit") ? (decimal)_entity["creditlimit"] : default;
                erroron = "creditonhold";
                this.creditonhold = _entity.Contains("creditonhold") ? (bool)_entity["creditonhold"] : default;
                erroron = "customersizecode";
                this.customersizecode = _entity.Contains("customersizecode") ? ((OptionSetValue)_entity["customersizecode"]).Value : default;
                erroron = "customertypecode";
                this.customertypecode = _entity.Contains("customertypecode") ? ((OptionSetValue)_entity["customertypecode"]).Value : default;
                erroron = "defaultpricelevelid";
                this.defaultpricelevelid = _entity.Contains("defaultpricelevelid") ? ((Guid)_entity["defaultpricelevelid"]).ToString() : default;
                erroron = "department";
                this.department = _entity.Contains("department") ? (string)_entity["department"] : default;
                erroron = "description";
                this.description = _entity.Contains("description") ? (string)_entity["description"] : default;
                erroron = "donotbulkemail";
                this.donotbulkemail = _entity.Contains("donotbulkemail") ? (bool)_entity["donotbulkemail"] : default;
                erroron = "donotbulkpostalmail";
                this.donotbulkpostalmail = _entity.Contains("donotbulkpostalmail") ? (bool)_entity["donotbulkpostalmail"] : default;
                erroron = "donotemail";
                this.donotemail = _entity.Contains("donotemail") ? (bool)_entity["donotemail"] : default;
                erroron = "donotfax";
                this.donotfax = _entity.Contains("donotfax") ? (bool)_entity["donotfax"] : default;
                erroron = "donotphone";
                this.donotphone = _entity.Contains("donotphone") ? (bool)_entity["donotphone"] : default;
                erroron = "donotpostalmail";
                this.donotpostalmail = _entity.Contains("donotpostalmail") ? (bool)_entity["donotpostalmail"] : default;
                erroron = "donotsendmm";
                this.donotsendmm = _entity.Contains("donotsendmm") ? (bool)_entity["donotsendmm"] : default;
                erroron = "educationcode";
                this.educationcode = _entity.Contains("educationcode") ? ((OptionSetValue)_entity["educationcode"]).Value : default;
                erroron = "emailaddress1";
                this.emailaddress1 = _entity.Contains("emailaddress1") ? (string)_entity["emailaddress1"] : default;
                erroron = "emailaddress2";
                this.emailaddress2 = _entity.Contains("emailaddress2") ? (string)_entity["emailaddress2"] : default;
                erroron = "emailaddress3";
                this.emailaddress3 = _entity.Contains("emailaddress3") ? (string)_entity["emailaddress3"] : default;
                erroron = "employeeid";
                this.employeeid = _entity.Contains("employeeid") ? (string)_entity["employeeid"] : default;
                erroron = "entityimage";
                this.entityimage = _entity.Contains("entityimage") ? (string)_entity["entityimage"] : default;
                erroron = "externaluseridentifier";
                this.externaluseridentifier = _entity.Contains("externaluseridentifier") ? (string)_entity["externaluseridentifier"] : default;
                erroron = "familystatuscode";
                this.familystatuscode = _entity.Contains("familystatuscode") ? ((OptionSetValue)_entity["familystatuscode"]).Value : default;
                erroron = "fax";
                this.fax = _entity.Contains("fax") ? (string)_entity["fax"] : default;
                erroron = "firstname";
                this.firstname = _entity.Contains("firstname") ? (string)_entity["firstname"] : default;
                erroron = "followemail";
                this.followemail = _entity.Contains("followemail") ? (bool)_entity["followemail"] : default;
                erroron = "ftpsiteurl";
                this.ftpsiteurl = _entity.Contains("ftpsiteurl") ? (string)_entity["ftpsiteurl"] : default;
                erroron = "gendercode";
                this.gendercode = _entity.Contains("gendercode") ? ((OptionSetValue)_entity["gendercode"]).Value : default;
                erroron = "governmentid";
                this.governmentid = _entity.Contains("governmentid") ? (string)_entity["governmentid"] : default;
                erroron = "haschildrencode";
                this.haschildrencode = _entity.Contains("haschildrencode") ? ((OptionSetValue)_entity["haschildrencode"]).Value : default;
                erroron = "home2";
                this.home2 = _entity.Contains("home2") ? (string)_entity["home2"] : default;
                erroron = "importsequencenumber";
                this.importsequencenumber = _entity.Contains("importsequencenumber") ? (int)_entity["importsequencenumber"] : default;
                erroron = "isbackofficecustomer";
                this.isbackofficecustomer = _entity.Contains("isbackofficecustomer") ? (bool)_entity["isbackofficecustomer"] : default;
                erroron = "jobtitle";
                this.jobtitle = _entity.Contains("jobtitle") ? (string)_entity["jobtitle"] : default;
                erroron = "lastname";
                this.lastname = _entity.Contains("lastname") ? (string)_entity["lastname"] : default;
                erroron = "lastonholdtime";
                this.lastonholdtime = _entity.Contains("lastonholdtime") ? (DateTime)_entity["lastonholdtime"] : default;
                erroron = "lastusedincampaign";
                this.lastusedincampaign = _entity.Contains("lastusedincampaign") ? (DateTime)_entity["lastusedincampaign"] : default;
                erroron = "leadsourcecode";
                this.leadsourcecode = _entity.Contains("lastusedincampaign") ? ((OptionSetValue)_entity["lastusedincampaign"]).Value : default;
                erroron = "managername";
                this.managername = _entity.Contains("managername") ? (string)_entity["managername"] : default;
                erroron = "managerphone";
                this.managerphone = _entity.Contains("managerphone") ? (string)_entity["managerphone"] : default;
                erroron = "marketingonly";
                this.marketingonly = _entity.Contains("marketingonly") ? (bool)_entity["marketingonly"] : default;
                erroron = "middlename";
                this.middlename = _entity.Contains("middlename") ? (string)_entity["middlename"] : default;
                erroron = "mobilephone";
                this.mobilephone = _entity.Contains("mobilephone") ? (string)_entity["mobilephone"] : default;
                erroron = "orgchangestatus";
                this.orgchangestatus = _entity.Contains("orgchangestatus") ? ((OptionSetValue)_entity["orgchangestatus"]).Value : default;
                erroron = "nickname";
                this.nickname = _entity.Contains("nickname") ? (string)_entity["nickname"] : default;
                erroron = "numberofchildren";
                this.numberofchildren = _entity.Contains("numberofchildren") ? (int)_entity["numberofchildren"] : default;
                erroron = "originatingleadid";
                this.originatingleadid = _entity.Contains("originatingleadid") ? ((Guid)_entity["originatingleadid"]).ToString() : default;
                erroron = "overriddencreatedon";
                this.overriddencreatedon = _entity.Contains("overriddencreatedon") ? (DateTime)_entity["overriddencreatedon"] : default;
                erroron = "ownerid";
                this.ownerid = _entity.Contains("ownerid") ? ((EntityReference)_entity["ownerid"]).Id.ToString() : default;
                erroron = "owneridtype";
                this.owneridtype = _entity.Contains("owneridtype") ? (string)_entity["owneridtype"] : default;
                erroron = "pager";
                this.pager = _entity.Contains("pager") ? (string)_entity["pager"] : default;
                erroron = "parentcustomerid";
                this.parentcustomerid = _entity.Contains("parentcustomerid") ? ((Guid)_entity["parentcustomerid"]).ToString() : default;
                erroron = "parentcustomeridtype";
                this.parentcustomeridtype = _entity.Contains("parentcustomeridtype") ? (string)_entity["parentcustomeridtype"] : default;
                erroron = "participatesinworkflow";
                this.participatesinworkflow = _entity.Contains("participatesinworkflow") ? (bool)_entity["participatesinworkflow"] : default;
                erroron = "paymenttermscode";
                this.paymenttermscode = _entity.Contains("paymenttermscode") ? ((OptionSetValue)_entity["paymenttermscode"]).Value : default;
                erroron = "preferredappointmentdaycode";
                this.preferredappointmentdaycode = _entity.Contains("preferredappointmentdaycode") ? ((OptionSetValue)_entity["preferredappointmentdaycode"]).Value : default;
                erroron = "preferredappointmenttimecode";
                this.preferredappointmenttimecode = _entity.Contains("preferredappointmenttimecode") ? ((OptionSetValue)_entity["preferredappointmenttimecode"]).Value : default;
                erroron = "preferredcontactmethodcode";
                this.preferredcontactmethodcode = _entity.Contains("preferredcontactmethodcode") ? ((OptionSetValue)_entity["preferredcontactmethodcode"]).Value : default;
                erroron = "preferredequipmentid";
                this.preferredequipmentid = _entity.Contains("preferredequipmentid") ? (string)_entity["preferredequipmentid"] : default;
                erroron = "preferredserviceid";
                this.preferredserviceid = _entity.Contains("preferredserviceid") ? (string)_entity["preferredserviceid"] : default;
                erroron = "preferredsystemuserid";
                this.preferredsystemuserid = _entity.Contains("preferredsystemuserid") ? (string)_entity["preferredsystemuserid"] : default;
                erroron = "processid";
                this.processid = _entity.Contains("processid") ? ((Guid)_entity["processid"]).ToString() : default;
                erroron = "salutation";
                this.salutation = _entity.Contains("salutationsalutation") ? (string)_entity["salutation"] : default;
                erroron = "shippingmethodcode";
                this.shippingmethodcode = _entity.Contains("shippingmethodcode") ? ((OptionSetValue)_entity["shippingmethodcode"]).Value : default;
                erroron = "slaid";
                this.slaid = _entity.Contains("slaid") ? ((Guid)_entity["slaid"]).ToString() : default;
                erroron = "spousesname";
                this.spousesname = _entity.Contains("spousesname") ? (string)_entity["spousesname"] : default;
                erroron = "stageid";
                this.stageid = _entity.Contains("stageid") ? ((Guid)_entity["stageid"]).ToString() : default;
                erroron = "statecode";
                this.statecode = _entity.Contains("statecode") ? ((OptionSetValue)_entity["statecode"]).Value : default;
                erroron = "statuscode";
                this.statuscode = _entity.Contains("statuscode") ? ((OptionSetValue)_entity["statuscode"]).Value : default;
                erroron = "subscriptionid";
                this.subscriptionid = _entity.Contains("subscriptionid") ? ((Guid)_entity["subscriptionid"]).ToString() : default;
                erroron = "suffix";
                this.suffix = _entity.Contains("suffix") ? (string)_entity["suffix"] : default;
                erroron = "teamsfollowed";
                this.teamsfollowed = _entity.Contains("teamsfollowed") ? (string)_entity["teamsfollowed"] : default;
                erroron = "telephone1";
                this.telephone1 = _entity.Contains("telephone1") ? (string)_entity["telephone1"] : default;
                erroron = "telephone2";
                this.telephone2 = _entity.Contains("telephone2") ? (string)_entity["telephone2"] : default;
                erroron = "telephone3";
                this.telephone3 = _entity.Contains("telephone3") ? (string)_entity["telephone3"] : default;
                erroron = "territorycode";
                this.territorycode = _entity.Contains("territorycode") ? ((OptionSetValue)_entity["territorycode"]).Value : default;
                //erroron = "transactioncurrencyid"; 
                //this.transactioncurrencyid = _entity.Contains("transactioncurrencyid") ? ((Guid)_entity["transactioncurrencyid"]).ToString() : default;
                erroron = "websiteurl";
                this.websiteurl = _entity.Contains("websiteurl") ? (string)_entity["websiteurl"] : default;
                erroron = "yomifirstname";
                this.yomifirstname = _entity.Contains("yomifirstname") ? (string)_entity["yomifirstname"] : default;
                erroron = "yomilastname";
                this.yomilastname = _entity.Contains("yomilastname") ? (string)_entity["yomilastname"] : default;
                erroron = "yomimiddlename";
                this.yomimiddlename = _entity.Contains("yomimiddlename") ? (string)_entity["yomimiddlename"] : default;
                erroron = "sourcesystem";
                this.sourcesystem = _entity.Contains("sourcesystem") ? (string)_entity["sourcesystem"] : default;

                erroron = "kti_sourceid";
                this.kti_sourceid = _entity.Contains("kti_sourceid") ? (string)_entity["kti_sourceid"] : default;
                erroron = "kti_socialchannelorigin";
                this.kti_socialchannelorigin = _entity.Contains("kti_socialchannelorigin") ? ((OptionSetValue)_entity["kti_socialchannelorigin"]).Value : default;

                erroron = "kti_sapbpcode";
                this.kti_sapbpcode = _entity.Contains("kti_sapbpcode") ? (string)_entity["kti_sapbpcode"] : default;

                erroron = "ncci_newclubmembershipid";
                this.ncci_clubmembershipid = _entity.Contains("ncci_newclubmembershipid") ? (string)_entity["ncci_newclubmembershipid"] : default;

                erroron = "kti_magentoid";
                this.kti_magentoid = _entity.Contains("kti_magentoid") ? (string)_entity["kti_magentoid"] : default;


                //this.MooExternalId = _customer.MooExternalId;
                //this.moosourcesystem = _customer.sourcesystem;

                //All Custom fields

                //this.clubmembershipid = _entity.Contains("clubmembershipid") ? (string)_entity["clubmembershipid"] : default;
                //this.customerjoineddate = _entity.Contains("customerjoineddate") ? (DateTime)_entity["customerjoineddate"] : default;
                //this.customerjoinedoutlet = _entity.Contains("customerjoinedoutlet") ? (string)_entity["customerjoinedoutlet"] : default;
                //this.firstpointofpurchase = _entity.Contains("firstpointofpurchase") ? (string)_entity["firstpointofpurchase"] : default;
                erroron = "customertype";
                this.customertype = _entity.Contains("customertype") ? ((OptionSetValue)_entity["customertype"]).Value : default;



                //this.customerseniority = _entity.Contains("customertype") ? ((OptionSetValue)_entity["customertype"]).Value : default;
                //this.lastcoffeepurchasedate = _entity.Contains("customerjoineddate") ? (DateTime)_entity["customerjoineddate"] : default;
                //this.customerstatus = _customer.customerstatus;
                //this.changeofstatusdate = _customer.changeofstatusdate;
                //this.customertier = _customer.customertier;
                //this.totalcoffeeqtyytd = _customer.totalcoffeeqtyytd;
                //this.totalmachineqtyytd = _customer.totalmachineqtyytd;
                //this.grandtotalcoffeeqty = _customer.grandtotalcoffeeqty;
                //this.grandtotalmachineqty = _customer.grandtotalmachineqty;
                // this.country = _entity.Contains("country") ? (string)_entity["country"] : default;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" error on {erroron}");

            }



            #endregion
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
            this.kti_sourceid = _customer.kti_sourceid;
            this.kti_socialchannelorigin = _customer.kti_socialchannelorigin;
            this.kti_sapbpcode = _customer.kti_sapbpcode;
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
        [StringLength(100)]
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
        [StringLength(100)]
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
        [StringLength(100)]
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
        public DateTime birthdate { get; set; }
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
        public string kti_sapbpcode { get; set; }
        public int kti_socialchannelorigin { get; set; }
        public string kti_sourceid { get; set; }

        public string ncci_clubmembershipid { get; set; }
        public string kti_magentoid { get; set; }

        #endregion
    }
}