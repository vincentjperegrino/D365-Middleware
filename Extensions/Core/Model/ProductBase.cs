using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;


namespace KTI.Moo.Extensions.Core.Model
{
    public class ProductBase
    {
        [Required]
        [StringLength(100)]
        public virtual string name { get; set; }
        [StringLength(2000)]
        public virtual string description { get; set; }
        public virtual string category { get; set; }
        public virtual string sku { get; set; }
        public virtual int companyid { get; set; }

        [Range(0, 1000000000000)]
        public virtual decimal currentcost { get; set; }
        [Required]
        [JsonProperty("defaultuomid@odata.bind")]
        public virtual string defaultuomid { get; set; }
        [Required]
        [JsonProperty("defaultuomscheduleid@odata.bind")]
        public virtual string defaultuomscheduleid { get; set; }
        [Range(-2147483647, 2147483647)]
        public virtual int dmtimportstate { get; set; }
        public virtual string entityimage { get; set; }
        [Range(-2147483647, 2147483647)]
        public virtual int importsequencenumber { get; set; }
        public virtual bool iskit { get; set; }
        public virtual bool isreparented { get; set; }
        public virtual bool isstockitem { get; set; }
        [JsonIgnore]
        [JsonProperty("msdyn_converttocustomerasset")]
        public virtual bool converttocustomerasset { get; set; }
        [JsonIgnore]
        [JsonProperty("msdyn_defaultvendor")]
        public virtual string defaultvendor { get; set; }
        [JsonIgnore]
        [JsonProperty("msdyn_fieldserviceproducttype")]
        public virtual int fieldserviceproducttype { get; set; }
        [StringLength(100)]
        [JsonIgnore]
        [JsonProperty("msdyn_purchasename")]
        public virtual string purchasename { get; set; }
        [JsonProperty("msdyn_taxable")]
        [JsonIgnore]
        public virtual bool taxable { get; set; }
        [JsonIgnore]
        [JsonProperty("msdyn_transactioncategory")]
        public virtual string transactioncategory { get; set; }
        [JsonIgnore]
        [StringLength(50)]
        [JsonProperty("msdyn_upccode")]
        public virtual string upccode { get; set; }
        public virtual DateTime overriddencreatedon { get; set; }
        public virtual string parentproductid { get; set; }
        [Range(0, 1000000000000)]
        public virtual decimal price { get; set; }
        [JsonProperty("pricelevelid@odata.bind")]
        public virtual string pricelevelid { get; set; }
        public virtual string processid { get; set; }
        public virtual string productid { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string productnumber { get; set; }
        public virtual int productstructure { get; set; }
        public virtual int productyypecode { get; set; }
        [StringLength(255)]
        public virtual string producturl { get; set; }
        [Range(0, 5)]
        public virtual int quantitydecimal { get; set; }
        [Range(0, 1000000000)]
        public virtual decimal quantityonhand { get; set; }
        [StringLength(200)]
        public virtual string size { get; set; }
        public virtual string stageid { get; set; }
        [Range(0, 1000000000000)]
        public virtual decimal standardcost { get; set; }
        [Range(0, 3)]
        public virtual int statecode { get; set; }
        [Range(0, 3)]
        public virtual int statuscode { get; set; }
        [Range(0, 1000000000)]
        public virtual decimal stockvolume { get; set; }
        [Range(0, 1000000000)]
        public virtual decimal stockweight { get; set; }
        public virtual string subjectid { get; set; }
        [StringLength(100)]
        public virtual string suppliername { get; set; }
        public virtual string transactioncurrencyid { get; set; }
        public virtual string validfromdate { get; set; }
        public virtual string validtodate { get; set; }
        [StringLength(100)]
        public virtual string vendorid { get; set; }
        [StringLength(100)]
        public virtual string vendorname { get; set; }
        [StringLength(100)]
        public virtual string vendorpartnumber { get; set; }
        [JsonIgnore]
        public virtual string moosourcesystem { get; set; }
        [JsonIgnore]
        public virtual string mooexternalid { get; set; }

        public virtual int ncci_productcategory { get; set; }
    }
}