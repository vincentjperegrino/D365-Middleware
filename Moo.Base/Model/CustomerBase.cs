using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace KTI.Moo.Base.Model;

public class CustomerBase
{
    #region properties

    public virtual int kti_socialchannelorigin { get; set; }
    public virtual string kti_sourceid { get; set; }
    public virtual int companyid { get; set; }
    public virtual string domainType { get; init; } = Helpers.DomainType.customer;
    public virtual string customerid { get; set; }
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
    [StringLength(150)]
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
    [StringLength(150)]
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
}
