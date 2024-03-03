
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace KTI.Moo.Base.Model;

public class InvoiceBase
{

    public virtual int companyid { get; set; }
    public virtual string domainType { get; init; } = Helpers.DomainType.invoice;

    [Required]
    [StringLength(300)]
    public virtual string name { get; set; }
    [StringLength(80)]
    public virtual string billto_city { get; set; }
    [StringLength(80)]
    public virtual string billto_country { get; set; }
    [StringLength(50)]
    public virtual string billto_fax { get; set; }
    [StringLength(250)]
    public virtual string billto_line1 { get; set; }
    [StringLength(250)]
    public virtual string billto_line2 { get; set; }
    [StringLength(250)]
    public virtual string billto_line3 { get; set; }
    [StringLength(200)]
    public virtual string billto_name { get; set; }
    [StringLength(20)]
    public virtual string billto_postalcode { get; set; }
    [StringLength(50)]
    public virtual string billto_stateorprovince { get; set; }
    [StringLength(50)]
    public virtual string billto_telephone { get; set; }
    [Required]
    [JsonProperty(PropertyName = "customerid_contact@odata.bind")]
    public string customerid { get; set; }
    [JsonIgnore]
    public string customeridtype { get; set; }
    public DateTime datedelivered { get; set; }
    [StringLength(2000)]
    public virtual string description { get; set; }
    [Range(0, 1000000000000)]
    public virtual decimal discountamount { get; set; }
    [Range(0, 100)]
    public double discountpercentage { get; set; }
    public DateTime duedate { get; set; }
    [StringLength(100)]
    public virtual string emailaddress { get; set; }
    [JsonIgnore]
    public string entityimage { get; set; }
    [Range(0, 1000000000000)]
    public virtual decimal freightamount { get; set; }
    [Range(-2147483648, 2147483647)]
    [JsonIgnore]
    public int importsequencenumber { get; set; }
    [JsonIgnore]
    public string invoiceid { get; set; }
    [Required]
    [StringLength(100)]
    public virtual string invoicenumber { get; set; }
    public bool ispricelocked { get; set; }
    public DateTime lastbackofficesubmit { get; set; }
    public DateTime lastonholdtime { get; set; }
    [JsonIgnore]
    public string opportunityid { get; set; }
    public DateTime overriddencreatedon { get; set; }
    [JsonIgnore]
    public string ownerid { get; set; }
    [JsonIgnore]
    public string owneridtype { get; set; }
    [Range(0, 4)]
    public int paymenttermscode { get; set; }
    [Required]
    [JsonProperty(PropertyName = "pricelevelid@odata.bind")]
    public string pricelevelid { get; set; }
    [Range(0, 38)]
    public int pricingerrorcode { get; set; }
    [Range(0, 1)]
    public int prioritycode { get; set; }
    [JsonIgnore]
    public string processid { get; set; }
    [JsonProperty(PropertyName = "salesorderid@odata.bind")]
    public virtual string salesorderid { get; set; }
    [Range(0, 7)]
    public int shippingmethodcode { get; set; }
    [StringLength(80)]
    public string shipto_city { get; set; }
    [StringLength(80)]
    public string shipto_country { get; set; }
    [StringLength(50)]
    public string shipto_fax { get; set; }
    [Range(0, 1)]
    public int shipto_freighttermscode { get; set; }
    [StringLength(250)]
    public string shipto_line1 { get; set; }
    [StringLength(250)]
    public string shipto_line2 { get; set; }
    [StringLength(250)]
    public string shipto_line3 { get; set; }
    [StringLength(200)]
    public string shipto_name { get; set; }
    [StringLength(20)]
    public string shipto_postalcode { get; set; }
    [StringLength(50)]
    public string shipto_stateorprovince { get; set; }
    [StringLength(50)]
    public string shipto_telephone { get; set; }
    [JsonIgnore]
    public string slaid { get; set; }
    [JsonIgnore]
    public string stageid { get; set; }
    [Range(0, 4)]
    public virtual int statecode { get; set; }
    [Range(0, 690970000)]
    public virtual int statuscode { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public virtual decimal totalamount { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public decimal totalamountlessfreight { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public decimal totaldiscountamount { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public decimal totallineitemamount { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public decimal totallineitemdiscountamount { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public virtual decimal totaltax { get; set; }
    [JsonIgnore]
    public string transactioncurrencyid { get; set; }
    public bool willcall { get; set; }

    public virtual DateTime invoicedate { get; set; }
    [JsonIgnore]
    public string sourcesystem { get; set; }
    [JsonIgnore]
    public string sourceofsale { get; set; }
    [JsonIgnore]
    public virtual string customercode { get; set; }
    [JsonIgnore]
    public string remarks { get; set; }
    //[JsonIgnore]
    //public int coffeeqty { get; set; }
    //[JsonIgnore]
    //public int machineqty { get; set; }
    //[JsonIgnore]
    //public bool coffee { get; set; }
    //[JsonIgnore]
    //public bool noncoffee { get; set; }
    //[JsonIgnore]
    //public bool accessories { get; set; }
    [JsonIgnore]
    public string orno { get; set; }
    [JsonIgnore]
    public string requestor { get; set; }
    [JsonIgnore]
    public string weborderno { get; set; }
    [JsonIgnore]
    public string soremarks { get; set; }
    [JsonIgnore]
    public string externalno { get; set; }
    [JsonIgnore]
    public string orderby { get; set; }
    [JsonIgnore]
    public string cncremarks { get; set; }
    [JsonIgnore]
    public string truck { get; set; }
    [JsonIgnore]
    public string check_name { get; set; }
    [JsonIgnore]
    public string rfpno { get; set; }
    [JsonIgnore]
    public string sono { get; set; }
    [JsonIgnore]
    public string ponum { get; set; }
    [JsonIgnore]
    public string billing_ref_no { get; set; }
    [JsonIgnore]
    public string kas_name { get; set; }
    [JsonIgnore]
    public string channel { get; set; }
    [JsonIgnore]
    public string acct_ww_concern { get; set; }
    [JsonIgnore]
    public string cnc_remarks { get; set; }
    [JsonIgnore]
    public DateTime received_date { get; set; }
    [DataType(DataType.Text)]
    [JsonIgnore]
    public virtual string moosourcesystem { get; set; }
    [DataType(DataType.Text)]
    [JsonIgnore]
    public string mooexternalid { get; set; }
    public virtual string kti_sourceid { get; set; }

    public virtual int kti_paymentmethod { get; set; }
    //public int kti_socialchannelorigin { get; set; }



}
