using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace KTI.Moo.Base.Model;
public class InvoiceItemBase
{
    public virtual int companyid { get; set; }

    public virtual string domainType { get; init; } = Helpers.DomainType.invoice;

    public DateTime actualdeliveryon { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public virtual decimal baseamount { get; set; }
    [StringLength(2000)]
    public virtual string description { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public decimal extendedamount { get; set; }
    [JsonIgnore]
    public virtual string invoicedetailid { get; set; }
    [StringLength(100)]
    public string invoicedetailname { get; set; }
    [Required]
    [JsonProperty(PropertyName = "invoiceid@odata.bind")]
    public virtual string invoiceid { get; set; }
    public bool iscopied { get; set; }
    public bool ispriceoverridden { get; set; }
    [JsonIgnore]
    public virtual bool isproductoverridden { get; set; }
    [Range(0, 1000000000)]
    public int lineitemnumber { get; set; }
    [Range(0, 1000000000)]
    public virtual decimal manualdiscountamount { get; set; }
    public DateTime overriddencreatedon { get; set; }
    [JsonIgnore]
    public string parentbundleid { get; set; }
    [JsonIgnore]
    public string parentbundleidref { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public virtual decimal priceperunit { get; set; }
    [Range(0, 38)]
    public int pricingerrorcode { get; set; }
    [JsonIgnore]
    public string productassociationid { get; set; }
    [StringLength(500)]
    public virtual string productdescription { get; set; }

    [JsonProperty(PropertyName = "productid@odata.bind")]
    public virtual string productid { get; set; }

    [StringLength(500)]
    public virtual string productname { get; set; }
    [Range(0, 5)]
    public int producttypecode { get; set; }
    [Range(0, 2)]
    public int propertyconfigurationstatus { get; set; }
    [Range(-100000000000, 100000000000)]
    public virtual decimal quantity { get; set; }
    [Range(0, 1000000000)]
    public double quantitybackordered { get; set; }
    [Range(0, 1000000000)]
    public double quantitycancelled { get; set; }
    [Range(-1000000000, 1000000000)]
    public double quantityshipped { get; set; }
    [JsonProperty(PropertyName = "salesorderdetailid@odata.bind")]
    [JsonIgnore]
    public virtual string salesorderdetailid { get; set; }
    [JsonIgnore]
    public string salesrepid { get; set; }
    [StringLength(100)]
    public string shippingtrackingnumber { get; set; }
    [StringLength(80)]
    public string shipto_city { get; set; }
    [StringLength(80)]
    public string shipto_country { get; set; }
    [StringLength(50)]
    public string shipto_fax { get; set; }
    [Range(0, 2)]
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
    [Range(0, 2)]
    public int skippriccalculation { get; set; }
    [Range(-1000000000000, 1000000000000)]
    public virtual decimal tax { get; set; }
    [JsonIgnore]
    [JsonProperty(PropertyName = "transactioncurrencyid@odata.bind")]
    public string transactioncurrencyid { get; set; }
    [JsonProperty(PropertyName = "uomid@odata.bind")]
    public string uomid { get; set; }
    public bool willcall { get; set; }
    [DataType(DataType.Text)]
    [JsonIgnore]
    public string moosourcesystem { get; set; }
    [DataType(DataType.Text)]
    [JsonIgnore]
    public string mooexternalid { get; set; }
    public string kti_lineitemnumber { get; set; }
    public virtual string kti_sourceid { get; set; }
    //public int kti_socialchannelorigin { get; set; }

}
