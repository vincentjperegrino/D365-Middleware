#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM.Models.Sales
{

    /// <summary>
    /// Invoice Transaction
    /// </summary>
    public class Invoices
    {
    }

    /// <summary>
    /// Invoice order
    /// </summary>
    public class InvoiceHeader
    {

        public InvoiceHeader()
        {
        }
        public InvoiceHeader(InvoiceHeader _invoiceHeader)
        {
            #region properties
            this.name = _invoiceHeader.name;
            this.billto_city = _invoiceHeader.billto_city;
            this.billto_country = _invoiceHeader.billto_country;
            this.billto_fax = _invoiceHeader.billto_fax;
            this.billto_line1 = _invoiceHeader.billto_line1;
            this.billto_line2 = _invoiceHeader.billto_line2;
            this.billto_line3 = _invoiceHeader.billto_line3;
            this.billto_name = _invoiceHeader.billto_name;
            this.billto_postalcode = _invoiceHeader.billto_postalcode;
            this.billto_stateorprovince = _invoiceHeader.billto_stateorprovince;
            this.billto_telephone = _invoiceHeader.billto_telephone;
            this.customerid = _invoiceHeader.customerid;
            this.customeridtype = _invoiceHeader.customeridtype;
            this.datedelivered = _invoiceHeader.datedelivered;
            this.description = _invoiceHeader.description;
            this.discountamount = _invoiceHeader.discountamount;
            this.discountpercentage = _invoiceHeader.discountpercentage;
            this.duedate = _invoiceHeader.duedate;
            this.emailaddress = _invoiceHeader.emailaddress;
            this.entityimage = _invoiceHeader.entityimage;
            this.freightamount = _invoiceHeader.freightamount;
            this.importsequencenumber = _invoiceHeader.importsequencenumber;
            this.invoiceid = _invoiceHeader.invoiceid;
            this.invoicenumber = _invoiceHeader.invoicenumber;
            this.ispricelocked = _invoiceHeader.ispricelocked;
            this.lastbackofficesubmit = _invoiceHeader.lastbackofficesubmit;
            this.lastonholdtime = _invoiceHeader.lastonholdtime;
            this.amountdue = _invoiceHeader.amountdue;
            this.billtocontactname = _invoiceHeader.billtocontactname;
            this.hascorrections = _invoiceHeader.hascorrections;
            this.msdyninvoicedate = _invoiceHeader.msdyninvoicedate;
            this.ordertype = _invoiceHeader.ordertype;
            this.projectinvoicestatus = _invoiceHeader.projectinvoicestatus;
            this.opportunityid = _invoiceHeader.opportunityid;
            this.overriddencreatedon = _invoiceHeader.overriddencreatedon;
            this.ownerid = _invoiceHeader.ownerid;
            this.owneridtype = _invoiceHeader.owneridtype;
            this.paymenttermscode = _invoiceHeader.paymenttermscode;
            this.pricelevelid = _invoiceHeader.pricelevelid;
            this.pricingerrorcode = _invoiceHeader.pricingerrorcode;
            this.prioritycode = _invoiceHeader.prioritycode;
            this.processid = _invoiceHeader.processid;
            this.salesorderid = _invoiceHeader.salesorderid;
            this.shippingmethodcode = _invoiceHeader.shippingmethodcode;
            this.shipto_city = _invoiceHeader.shipto_city;
            this.shipto_country = _invoiceHeader.shipto_country;
            this.shipto_fax = _invoiceHeader.shipto_fax;
            this.shipto_freighttermscode = _invoiceHeader.shipto_freighttermscode;
            this.shipto_line1 = _invoiceHeader.shipto_line1;
            this.shipto_line2 = _invoiceHeader.shipto_line2;
            this.shipto_line3 = _invoiceHeader.shipto_line3;
            this.shipto_name = _invoiceHeader.shipto_name;
            this.shipto_postalcode = _invoiceHeader.shipto_postalcode;
            this.shipto_stateorprovince = _invoiceHeader.shipto_stateorprovince;
            this.shipto_telephone = _invoiceHeader.shipto_telephone;
            this.slaid = _invoiceHeader.slaid;
            this.stageid = _invoiceHeader.stageid;
            this.statecode = _invoiceHeader.statecode;
            this.statuscode = _invoiceHeader.statuscode;
            this.totalamount = _invoiceHeader.totalamount;
            this.totalamountlessfreight = _invoiceHeader.totalamountlessfreight;
            this.totaldiscountamount = _invoiceHeader.totaldiscountamount;
            this.totallineitemamount = _invoiceHeader.totallineitemamount;
            this.totallineitemdiscountamount = _invoiceHeader.totallineitemdiscountamount;
            this.totaltax = _invoiceHeader.totaltax;
            this.transactioncurrencyid = _invoiceHeader.transactioncurrencyid;
            this.willcall = _invoiceHeader.willcall;
            this.invoicedate = _invoiceHeader.invoicedate;
            this.sourcesystem = _invoiceHeader.sourcesystem;
            this.sourceofsale = _invoiceHeader.sourceofsale;
            this.customercode = _invoiceHeader.customercode;
            this.remarks = _invoiceHeader.remarks;
            this.coffeeqty = _invoiceHeader.coffeeqty;
            this.machineqty = _invoiceHeader.machineqty;
            this.coffee = _invoiceHeader.coffee;
            this.noncoffee = _invoiceHeader.noncoffee;
            this.accessories = _invoiceHeader.accessories;
            this.orno = _invoiceHeader.orno;
            this.requestor = _invoiceHeader.requestor;
            this.weborderno = _invoiceHeader.weborderno;
            this.soremarks = _invoiceHeader.soremarks;
            this.externalno = _invoiceHeader.externalno;
            this.orderby = _invoiceHeader.orderby;
            this.cncremarks = _invoiceHeader.cncremarks;
            this.truck = _invoiceHeader.truck;
            this.check_name = _invoiceHeader.check_name;
            this.rfpno = _invoiceHeader.rfpno;
            this.sono = _invoiceHeader.sono;
            this.ponum = _invoiceHeader.ponum;
            this.billing_ref_no = _invoiceHeader.billing_ref_no;
            this.kas_name = _invoiceHeader.kas_name;
            this.channel = _invoiceHeader.channel;
            this.acct_ww_concern = _invoiceHeader.acct_ww_concern;
            this.cnc_remarks = _invoiceHeader.cnc_remarks;
            this.received_date = _invoiceHeader.received_date;
            this.moosourcesystem = _invoiceHeader.moosourcesystem;
            this.mooexternalid = _invoiceHeader.mooexternalid;
            #endregion
        }

        #region properties
        [Required]
        [StringLength(300)]
        public string name { get; set; }
        [StringLength(80)]
        public string billto_city { get; set; }
        [StringLength(80)]
        public string billto_country { get; set; }
        [StringLength(50)]
        public string billto_fax { get; set; }
        [StringLength(250)]
        public string billto_line1 { get; set; }
        [StringLength(250)]
        public string billto_line2 { get; set; }
        [StringLength(250)]
        public string billto_line3 { get; set; }
        [StringLength(200)]
        public string billto_name { get; set; }
        [StringLength(20)]
        public string billto_postalcode { get; set; }
        [StringLength(50)]
        public string billto_stateorprovince { get; set; }
        [StringLength(50)]
        public string billto_telephone { get; set; }
        public string customerid { get; set; }
        public string customeridtype { get; set; }
        public CRM.CustomDataType.MooDateTime datedelivered { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        [Range(0, 1000000000000)]
        public decimal discountamount { get; set; }
        [Range(0, 100)]
        public double discountpercentage { get; set; }
        public CRM.CustomDataType.MooDateTime duedate { get; set; }
        [StringLength(100)]
        public string emailaddress { get; set; }
        public string entityimage { get; set; }
        [Range(0, 1000000000000)]
        public decimal freightamount { get; set; }
        [Range(-2147483648, 2147483647)]
        public int importsequencenumber { get; set; }
        public string invoiceid { get; set; }
        [Required]
        [StringLength(100)]
        public string invoicenumber { get; set; }
        public bool ispricelocked { get; set; }
        public CRM.CustomDataType.MooDateTime lastbackofficesubmit { get; set; }
        public CRM.CustomDataType.MooDateTime lastonholdtime { get; set; }
        [Range(-1000000000, 1000000000)]
        public decimal amountdue { get; set; }
        [StringLength(150)]
        public string billtocontactname { get; set; }
        public bool hascorrections { get; set; }
        public CRM.CustomDataType.MooDateTime msdyninvoicedate { get; set; }
        [Range(192350000, 690970002)]
        public int ordertype { get; set; }
        [Range(192350000, 192350003)]
        public string projectinvoicestatus { get; set; }
        public string opportunityid { get; set; }
        public CRM.CustomDataType.MooDateTime overriddencreatedon { get; set; }
        public string ownerid { get; set; }
        public string owneridtype { get; set; }
        [Range(1, 4)]
        public int paymenttermscode { get; set; }
        [Required]
        public string pricelevelid { get; set; }
        [Range(0, 38)]
        public int pricingerrorcode { get; set; }
        [Range(1, 1)]
        public int prioritycode { get; set; }
        public string processid { get; set; }
        public string salesorderid { get; set; }
        [Range(1, 7)]
        public int shippingmethodcode { get; set; }
        [StringLength(80)]
        public string shipto_city { get; set; }
        [StringLength(80)]
        public string shipto_country { get; set; }
        [StringLength(50)]
        public string shipto_fax { get; set; }
        [Range(1, 1)]
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
        public string slaid { get; set; }
        public string stageid { get; set; }
        [Range(0, 3)]
        public int statecode { get; set; }
        [Range(1, 100003)]
        public int statuscode { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal totalamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal totalamountlessfreight { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal totaldiscountamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal totallineitemamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal totallineitemdiscountamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal totaltax { get; set; }
        public string transactioncurrencyid { get; set; }
        public bool willcall { get; set; }
        public CRM.CustomDataType.MooDateTime invoicedate { get; set; }
        public string sourcesystem { get; set; }
        public string sourceofsale { get; set; }
        public string customercode { get; set; }
        public string remarks { get; set; }
        public int coffeeqty { get; set; }
        public int machineqty { get; set; }
        public bool coffee { get; set; }
        public bool noncoffee { get; set; }
        public bool accessories { get; set; }
        public string orno { get; set; }
        public string requestor { get; set; }
        public string weborderno { get; set; }
        public string soremarks { get; set; }
        public string externalno { get; set; }
        public string orderby { get; set; }
        public string cncremarks { get; set; }
        public string truck { get; set; }
        public string check_name { get; set; }
        public string rfpno { get; set; }
        public string sono { get; set; }
        public string ponum { get; set; }
        public string billing_ref_no { get; set; }
        public string kas_name { get; set; }
        public string channel { get; set; }
        public string acct_ww_concern { get; set; }
        public string cnc_remarks { get; set; }
        public CRM.CustomDataType.MooDateTime received_date { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }

        #endregion
    }

    /// <summary>
    /// Invoice order line
    /// </summary>
    public class InvoiceDetails
    {

        public InvoiceDetails(InvoiceDetails _invoiceDetails)
        {
            #region properties
            this.actualdeliveryon = _invoiceDetails.actualdeliveryon;
            this.baseamount = _invoiceDetails.baseamount;
            this.description = _invoiceDetails.description;
            this.extendedamount = _invoiceDetails.extendedamount;
            this.invoicedetailid = _invoiceDetails.invoicedetailid;
            this.invoiceid = _invoiceDetails.invoiceid;
            this.ispriceoverridden = _invoiceDetails.ispriceoverridden;
            this.lineitemnumber = _invoiceDetails.lineitemnumber;
            this.manualdiscountamount = _invoiceDetails.manualdiscountamount;
            this.agreement = _invoiceDetails.agreement;
            this.agreementinvoiceproduct = _invoiceDetails.agreementinvoiceproduct;
            this.billingmethod = _invoiceDetails.billingmethod;
            this.chargeableamount = _invoiceDetails.chargeableamount;
            this.complimentaryamount = _invoiceDetails.complimentaryamount;
            this.contractline = _invoiceDetails.contractline;
            this.contractlineamount = _invoiceDetails.contractlineamount;
            this.currency = _invoiceDetails.currency;
            this.invoicedtilldate = _invoiceDetails.invoicedtilldate;
            this.lineorder = _invoiceDetails.lineorder;
            this.linetype = _invoiceDetails.linetype;
            this.nonchargeableamount = _invoiceDetails.nonchargeableamount;
            this.orderinvoicingproduct = _invoiceDetails.orderinvoicingproduct;
            this.project = _invoiceDetails.project;
            this.workorderid = _invoiceDetails.workorderid;
            this.workorderproductid = _invoiceDetails.workorderproductid;
            this.workorderserviceid = _invoiceDetails.workorderserviceid;
            this.overriddencreatedon = _invoiceDetails.overriddencreatedon;
            this.parentbundleid = _invoiceDetails.parentbundleid;
            this.parentbundleidref = _invoiceDetails.parentbundleidref;
            this.priceperunit = _invoiceDetails.priceperunit;
            this.productdescription = _invoiceDetails.productdescription;
            this.productid = _invoiceDetails.productid;
            this.productname = _invoiceDetails.productname;
            this.producttypecode = _invoiceDetails.producttypecode;
            this.propertyconfigurationstatus = _invoiceDetails.propertyconfigurationstatus;
            this.quantity = _invoiceDetails.quantity;
            this.quantitybackordered = _invoiceDetails.quantitybackordered;
            this.quantitycancelled = _invoiceDetails.quantitycancelled;
            this.quantityshipped = _invoiceDetails.quantityshipped;
            this.salesorderdetailId = _invoiceDetails.salesorderdetailId;
            this.salesrepid = _invoiceDetails.salesrepid;
            this.shippingtrackingnumber = _invoiceDetails.shippingtrackingnumber;
            this.shipto_city = _invoiceDetails.shipto_city;
            this.shipto_country = _invoiceDetails.shipto_country;
            this.shipto_fax = _invoiceDetails.shipto_fax;
            this.shipto_freighttermscode = _invoiceDetails.shipto_freighttermscode;
            this.shipto_line1 = _invoiceDetails.shipto_line1;
            this.shipto_line2 = _invoiceDetails.shipto_line2;
            this.shipto_line3 = _invoiceDetails.shipto_line3;
            this.shipto_name = _invoiceDetails.shipto_name;
            this.shipto_postalcode = _invoiceDetails.shipto_postalcode;
            this.shipto_stateorprovince = _invoiceDetails.shipto_stateorprovince;
            this.shipto_telephone = _invoiceDetails.shipto_telephone;
            this.skippriccalculation = _invoiceDetails.skippriccalculation;
            this.tax = _invoiceDetails.tax;
            this.transactioncurrencyid = _invoiceDetails.transactioncurrencyid;
            this.uomid = _invoiceDetails.uomid;
            this.willcall = _invoiceDetails.willcall;
            this.mooexternalid = _invoiceDetails.mooexternalid;
            this.moosourcesystem = _invoiceDetails.moosourcesystem;
            #endregion
        }

        public InvoiceDetails()
        {
        }

        #region Properties
        public CRM.CustomDataType.MooDateTime actualdeliveryon { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal baseamount { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal extendedamount { get; set; }
        public string invoicedetailid { get; set; }
        [StringLength(100)]
        public string invoicedetailname { get; set; }
        [Required]
        public string invoiceid { get; set; }
        public bool iscopied { get; set; }
        public bool ispriceoverridden { get; set; }
        public bool isproductoverridden { get; set; }
        [Range(0, 1000000000)]
        public int lineitemnumber { get; set; }
        [Range(0, 1000000000)]
        public decimal manualdiscountamount { get; set; }
        public string agreement { get; set; }
        public string agreementinvoiceproduct { get; set; }
        [Range(192350000, 192350001)]
        public string billingmethod { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal chargeableamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal complimentaryamount { get; set; }
        [StringLength(100)]
        public string contractline { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal contractlineamount { get; set; }
        public string currency { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal invoicedtilldate { get; set; }
        [Range(-2147483648, 2147483647)]
        public string lineorder { get; set; }
        [Range(690970000, 690970001)]
        public string linetype { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal nonchargeableamount { get; set; }
        public string orderinvoicingproduct { get; set; }
        public string project { get; set; }
        public string workorderid { get; set; }
        public string workorderproductid { get; set; }
        public string workorderserviceid { get; set; }
        public CRM.CustomDataType.MooDateTime overriddencreatedon { get; set; }
        public string parentbundleid { get; set; }
        public string parentbundleidref { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public double priceperunit { get; set; }
        [Range(0, 38)]
        public int pricingerrorcode { get; set; }
        public string productassociationid { get; set; }
        [StringLength(500)]
        public string productdescription { get; set; }
        public string productid { get; set; }
        [StringLength(500)]
        public string productname { get; set; }
        [Range(1, 5)]
        public int producttypecode { get; set; }
        [Range(0, 2)]
        public string propertyconfigurationstatus { get; set; }
        [Range(-100000000000, 100000000000)]
        public double quantity { get; set; }
        [Range(0, 1000000000)]
        public double quantitybackordered { get; set; }
        [Range(0, 1000000000)]
        public double quantitycancelled { get; set; }
        [Range(-1000000000, 1000000000)]
        public double quantityshipped { get; set; }
        public string salesorderdetailId { get; set; }
        public string salesrepid { get; set; }
        [StringLength(100)]
        public string shippingtrackingnumber { get; set; }
        [StringLength(80)]
        public string shipto_city { get; set; }
        [StringLength(80)]
        public string shipto_country { get; set; }
        [StringLength(50)]
        public string shipto_fax { get; set; }
        [Range(1, 2)]
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
        public double tax { get; set; }
        public string transactioncurrencyid { get; set; }
        public string uomid { get; set; }
        public bool willcall { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }

        #endregion
    }

    /// <summary>
    /// Sales order headers and line data-transfer object
    /// </summary>
    public class InvoiceAndDetails
    {
        public InvoiceAndDetails()
        {
        }

        public InvoiceAndDetails(InvoiceAndDetails _invoiceAndDetails)
        {
            this.invoiceHeader = _invoiceAndDetails.invoiceHeader;
            this.invoiceDetails = _invoiceAndDetails.invoiceDetails;
        }

        #region properties
        [Required] public InvoiceHeader invoiceHeader { get; set; }
        public IEnumerable<InvoiceDetails> invoiceDetails { get; set; }
        #endregion
    }
}