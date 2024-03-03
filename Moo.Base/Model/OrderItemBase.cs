using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
namespace KTI.Moo.Base.Model;

public class OrderItemBase
{
    //public virtual int kti_socialchannelorigin { get; set; }
    [DataType(DataType.Text)]
    [JsonIgnore]
    public virtual string moosourcesystem { get; set; }
    public virtual bool ispriceoverridden { get; set; }
    public virtual int companyid { get; set; }
    public virtual string domainType { get; init; } = Helpers.DomainType.order;

    [Range(-922337203685477, 922337203685477)]
    public virtual decimal baseamount { get; set; }
    [StringLength(2000)]
    public virtual string description { get; set; }
    [Range(-922337203685477, 922337203685477)]
    public virtual decimal extendedamount { get; set; }
    public virtual bool iscopied { get; set; }
    public virtual bool isproductoverridden { get; set; }
    [Range(0, 1000000000)]
    public virtual int lineitemnumber { get; set; }

    [Range(0, 1000000000000)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public virtual decimal manualdiscountamount { get; set; }
    [JsonIgnore]
    public virtual string agreement { get; set; }
    [Range(192350000, 192350001)]
    [JsonIgnore]
    public virtual int billingmethod { get; set; }
    [JsonIgnore]
    public virtual DateTime billingstartdate { get; set; }
    [Range(192350000, 192350000)]
    [JsonIgnore]
    public virtual int billingstatus { get; set; }
    [JsonIgnore]
    [Range(-922337203685477, 922337203685477)]
    public virtual decimal budgetamount { get; set; }
    [Range(0, 922337203685477)]
    [JsonIgnore]
    public virtual decimal costamount { get; set; }
    [Range(0, 922337203685477)]
    [JsonIgnore]
    public virtual decimal costpriceperunit { get; set; }
    [JsonIgnore]
    public virtual bool includeexpense { get; set; }
    [JsonIgnore]
    public virtual bool includefee { get; set; }
    [JsonIgnore]
    public virtual bool includematerial { get; set; }
    [JsonIgnore]
    public virtual bool includetime { get; set; }
    [JsonIgnore]
    public virtual string invoicefrequency { get; set; }
    [JsonIgnore]
    [Range(690970000, 690970001)]
    public virtual int linetype { get; set; }
    [StringLength(100)]
    [JsonIgnore]
    public virtual string project { get; set; }
    [JsonIgnore]
    public virtual string quoteline { get; set; }
    [JsonIgnore]
    public virtual DateTime overriddencreatedon { get; set; }
    public virtual string parentbundleid { get; set; }
    public virtual string parentbundleidref { get; set; }

    [Range(-922337203685477, 922337203685477)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public virtual decimal priceperunit { get; set; }
    [Range(0, 38)]
    public virtual int pricingerrorcode { get; set; }
    public virtual string productassociationid { get; set; }
    [StringLength(500)]
    public virtual string productdescription { get; set; }
    [JsonProperty("productid@odata.bind")]
    public virtual string productid { get; set; }
    [StringLength(500)]
    public virtual string productname { get; set; }
    [Range(1, 5)]
    public virtual int producttypecode { get; set; }
    [Range(0, 2)]
    public virtual int propertyconfigurationstatus { get; set; }
    [Required]
    [Range(-100000000000, 100000000000)]
    public virtual decimal quantity { get; set; }
    [Range(0, 100000000000)]
    public virtual decimal quantitybackordered { get; set; }
    [Range(0, 100000000000)]
    public virtual decimal quantitycancelled { get; set; }
    [Range(-100000000000, 100000000000)]
    public virtual decimal quantityshipped { get; set; }
    public virtual string quotedetailid { get; set; }
    public virtual DateTime requestdeliveryby { get; set; }
    public virtual string salesorderdetailid { get; set; }
    [StringLength(100)]
    public virtual string salesorderdetailname { get; set; }
    [Required]
    [JsonProperty("salesorderid@odata.bind")]
    public virtual string salesorderid { get; set; }
    public virtual string salesrepid { get; set; }
    public virtual string shipto_addressid { get; set; }
    [StringLength(80)]
    public virtual string shipto_city { get; set; }
    [StringLength(150)]
    public virtual string shipto_contactname { get; set; }
    [StringLength(80)]
    public virtual string shipto_country { get; set; }
    [StringLength(50)]
    public virtual string shipto_fax { get; set; }
    [Range(1, 2)]
    public virtual int shipto_freighttermscode { get; set; }
    [StringLength(250)]
    public virtual string shipto_line1 { get; set; }
    [StringLength(250)]
    public virtual string shipto_line2 { get; set; }
    [StringLength(250)]
    public virtual string shipto_line3 { get; set; }
    [StringLength(200)]
    public virtual string shipto_name { get; set; }
    [StringLength(20)]
    public virtual string shipto_postalcode { get; set; }
    [StringLength(50)]
    public virtual string shipto_stateorprovince { get; set; }
    [StringLength(50)]
    public virtual string shipto_telephone { get; set; }
    [Range(0, 2)]
    public virtual int skippriccalculation { get; set; }

    [Range(-1000000000000, 1000000000000)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public virtual decimal tax { get; set; }
    [JsonIgnore]
    [JsonProperty("transactioncurrencyid@odata.bind")]
    public virtual string transactioncurrencyid { get; set; }
    [JsonProperty("uomid@odata.bind")]
    public virtual string uomid { get; set; }
    public virtual bool willcall { get; set; }
    [DataType(DataType.Text)]
    [JsonIgnore]
    public virtual string mooexternalid { get; set; }
    public virtual string kti_lineitemnumber { get; set; } 
    public virtual string kti_sourceid { get; set; }
    public virtual string kti_sourceitemid { get; set; }
    public virtual int kti_orderstatus { get; set; }
    public virtual int kti_cancel_reason { get; set; }
    public virtual string kti_salesperson { get; set; }

}
