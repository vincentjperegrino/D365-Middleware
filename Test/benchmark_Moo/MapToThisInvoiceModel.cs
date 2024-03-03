#region Namespaces
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#endregion

namespace benchmark_Moo
{
    public class MapToThisInvoiceModel
    {
        public MapToThisInvoiceModel(InvoiceModels _invoiceDetails)
        {
            #region properties

            this.baseamount = _invoiceDetails.baseamount;
            this.description = _invoiceDetails.description;
            this.invoicedetailid = _invoiceDetails.invoicedetailid;
            this.invoiceid = _invoiceDetails.invoiceid;
            this.ispriceoverridden = true; //Price should be always overridden
            this.lineitemnumber = _invoiceDetails.lineitemnumber;
            this.manualdiscountamount = _invoiceDetails.manualdiscountamount;
            this.priceperunit = _invoiceDetails.priceperunit;
            this.productdescription = _invoiceDetails.productdescription;
            this.productid = _invoiceDetails.productid;
            this.quantity = _invoiceDetails.quantity;
            this.tax = _invoiceDetails.tax;
            this.moosourcesystem = _invoiceDetails.moosourcesystem;
            this.kti_lineitemnumber = _invoiceDetails.lineitemnumber.ToString();
            this.kti_sourceid = _invoiceDetails.invoiceid;
            this.ncci_remark = _invoiceDetails.description;

            var salesmanlist = _invoiceDetails.custom_attributes.Where(details => details.Name == "salesman");

            if (salesmanlist.Any())
            {
                this.ncci_Salesman = salesmanlist.First().Value;
            }

            var discountlist = _invoiceDetails.custom_attributes.Where(details => details.Name == "discountpercentage");

            if (discountlist.Any())
            {
                this.ncci_discountpercentage = decimal.Parse(discountlist.First().Value);
            }

            var taxpercentlist = _invoiceDetails.custom_attributes.Where(details => details.Name == "taxpercentage");

            if (taxpercentlist.Any())
            {
                this.ncci_taxpercentage = decimal.Parse(taxpercentlist.First().Value);
            }



            var OCTOPOSchannelorigin = 959080011;
            this.kti_socialchannelorigin = OCTOPOSchannelorigin;

            #endregion
        }

        public MapToThisInvoiceModel(InvoicewithDynamicModel _invoiceDetails)
        {
            #region properties

            this.baseamount = _invoiceDetails.baseamount;
            this.description = _invoiceDetails.description;
            this.invoicedetailid = _invoiceDetails.invoicedetailid;
            this.invoiceid = _invoiceDetails.invoiceid;
            this.ispriceoverridden = true; //Price should be always overridden
            this.lineitemnumber = _invoiceDetails.lineitemnumber;
            this.manualdiscountamount = _invoiceDetails.manualdiscountamount;
            this.priceperunit = _invoiceDetails.priceperunit;
            this.productdescription = _invoiceDetails.productdescription;
            this.productid = _invoiceDetails.productid;
            this.quantity = _invoiceDetails.quantity;
            this.tax = _invoiceDetails.tax;
            this.moosourcesystem = _invoiceDetails.moosourcesystem;
            this.kti_lineitemnumber = _invoiceDetails.lineitemnumber.ToString();
            this.kti_sourceid = _invoiceDetails.invoiceid;
            this.ncci_remark = _invoiceDetails.description;

            if (_invoiceDetails.custom_attributes.salesman is not null)
            {
                this.ncci_Salesman = _invoiceDetails.custom_attributes.salesman;
            }

     
            if (_invoiceDetails.custom_attributes.discountpercentage is not null)
            {
                this.ncci_discountpercentage = _invoiceDetails.custom_attributes.discountpercentage;
            }



            if (_invoiceDetails.custom_attributes.taxpercentage is not null)
            {
                this.ncci_taxpercentage = _invoiceDetails.custom_attributes.taxpercentage;
            }



            var OCTOPOSchannelorigin = 959080011;
            this.kti_socialchannelorigin = OCTOPOSchannelorigin;

            #endregion
        }


        #region Properties
        public DateTime actualdeliveryon { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal baseamount { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal extendedamount { get; set; }
        [JsonIgnore]
        public string invoicedetailid { get; set; }
        [StringLength(100)]
        public string invoicedetailname { get; set; }
        [Required]
        [JsonProperty(PropertyName = "invoiceid@odata.bind")]
        public string invoiceid { get; set; }
        public bool iscopied { get; set; }
        public bool ispriceoverridden { get; set; }
        [JsonIgnore]
        public bool isproductoverridden { get; set; }
        [Range(0, 1000000000)]
        public int lineitemnumber { get; set; }
        [Range(0, 1000000000)]
        public decimal manualdiscountamount { get; set; }
        public DateTime overriddencreatedon { get; set; }
        [JsonIgnore]
        public string parentbundleid { get; set; }
        [JsonIgnore]
        public string parentbundleidref { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal priceperunit { get; set; }
        [Range(0, 38)]
        public int pricingerrorcode { get; set; }
        [JsonIgnore]
        public string productassociationid { get; set; }
        [StringLength(500)]
        public string productdescription { get; set; }
        [JsonProperty(PropertyName = "productid@odata.bind")]
        public string productid { get; set; }
        [StringLength(500)]
        public string productname { get; set; }
        [Range(0, 5)]
        public int producttypecode { get; set; }
        [Range(0, 2)]
        public int propertyconfigurationstatus { get; set; }
        [Range(-100000000000, 100000000000)]
        public decimal quantity { get; set; }
        [Range(0, 1000000000)]
        public double quantitybackordered { get; set; }
        [Range(0, 1000000000)]
        public double quantitycancelled { get; set; }
        [Range(-1000000000, 1000000000)]
        public double quantityshipped { get; set; }
        [JsonProperty(PropertyName = "salesorderdetailid@odata.bind")]
        [JsonIgnore]
        public string salesorderdetailid { get; set; }
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
        public decimal tax { get; set; }
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
        public string kti_sourceid { get; set; }
        public int kti_socialchannelorigin { get; set; }

        public string ncci_remark { get; set; }

        [JsonProperty(PropertyName = "ncci_Salesman@odata.bind")]
        public string ncci_Salesman { get; set; }

        [JsonProperty(PropertyName = "ncci_Branch@odata.bind")]
        public string ncci_Branch { get; set; }

        [JsonProperty(PropertyName = "ncci_Customer_contact@odata.bind")]
        public string ncci_Customer { get; set; }

        public decimal ncci_discountpercentage { get; set; }

        public decimal ncci_taxpercentage { get; set; }
        #endregion
    }
}
