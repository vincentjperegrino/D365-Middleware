#region Namespaces
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Sales
{

    /// <summary>
    /// Sales Transaction
    /// </summary>
    public class Sales
    {
    }

    /// <summary>
    /// Sales order header
    /// </summary>
    public class Order
    {

        public Order()
        {
        }
        public Order(Order _order)
        {
            #region properties
            this.name = _order.name;
            this.billto_addressid = _order.billto_addressid;
            this.billto_city = _order.billto_city;
            this.billto_contactName = _order.billto_contactName;
            this.billto_country = _order.billto_country;
            this.billto_fax = _order.billto_fax;
            this.billto_line1 = _order.billto_line1;
            this.billto_line2 = _order.billto_line2;
            this.billto_line3 = _order.billto_line3;
            this.billto_name = _order.billto_name;
            this.billto_postalcode = _order.billto_postalcode;
            this.billto_stateorprovince = _order.billto_stateorprovince;
            this.billto_telephone = _order.billto_telephone;
            this.campaignid = _order.campaignid;
            this.customerid = _order.customerid;
            this.customeridtype = _order.customeridtype;
            this.datefulfilled = _order.datefulfilled;
            this.description = _order.description;
            this.discountamount = _order.discountamount;
            this.discountpercentage = _order.discountpercentage;
            this.emailaddress = _order.emailaddress;
            this.entityimage = _order.entityimage;
            this.freightamount = _order.freightamount;
            this.freighttermscode = _order.freighttermscode;
            this.ispricelocked = _order.ispricelocked;
            this.lastbackofficesubmit = _order.lastbackofficesubmit;
            this.lastonholdtime = _order.lastonholdtime;
            this.account = _order.account;
            this.accountmanagerid = _order.accountmanagerid;
            this.contractorganizationalunitid = _order.contractorganizationalunitid;
            this.ordertype = _order.ordertype;
            this.psastate = _order.psastate;
            this.psastatusreason = _order.psastatusreason;
            this.opportunityid = _order.opportunityid;
            this.ordernumber = _order.ordernumber;
            this.overriddencreatedon = _order.overriddencreatedon;
            this.ownerid = _order.ownerid;
            this.owneridtype = _order.owneridtype;
            this.paymenttermscode = _order.paymenttermscode;
            this.pricelevelid = _order.pricelevelid;
            this.pricingerrorcode = _order.pricingerrorcode;
            this.prioritycode = _order.prioritycode;
            this.processid = _order.processid;
            this.quoteid = _order.quoteid;
            this.requestdeliveryby = _order.requestdeliveryby;
            this.salesorderid = _order.salesorderid;
            this.shippingmethodcode = _order.shippingmethodcode;
            this.shipto_addressid = _order.shipto_addressid;
            this.shipto_city = _order.shipto_city;
            this.shipto_contactName = _order.shipto_contactName;
            this.shipto_country = _order.shipto_country;
            this.shipto_fax = _order.shipto_fax;
            this.shipto_freighttermscode = _order.shipto_freighttermscode;
            this.shipto_line1 = _order.shipto_line1;
            this.shipto_line2 = _order.shipto_line2;
            this.shipto_line3 = _order.shipto_line3;
            this.shipto_name = _order.shipto_name;
            this.shipto_postalcode = _order.shipto_postalcode;
            this.shipto_stateorprovince = _order.shipto_stateorprovince;
            this.shipto_telephone = _order.shipto_telephone;
            this.slaid = _order.slaid;
            this.stageid = _order.stageid;
            this.statecode = _order.statecode;
            this.statuscode = _order.statuscode;
            this.submitdate = _order.submitdate;
            this.submitstatus = _order.submitstatus;
            this.submitstatusdescription = _order.submitstatusdescription;
            this.totalamount = _order.totalamount;
            this.totalamountlessfreight = _order.totalamountlessfreight;
            this.totaldiscountamount = _order.totaldiscountamount;
            this.totallineitemamount = _order.totallineitemamount;
            this.totallineitemdiscountamount = _order.totallineitemdiscountamount;
            this.totaltax = _order.totaltax;
            this.transactioncurrencyid = _order.transactioncurrencyid;
            this.willcall = _order.willcall;
            this.sourcesystem = _order.sourcesystem;
            this.sourceofsale = _order.sourceofsale;
            this.customercode = _order.customercode;
            this.remarks = _order.remarks;
            this.coffeeqty = _order.coffeeqty;
            this.machineqty = _order.machineqty;
            this.orno = _order.orno;
            this.requestor = _order.requestor;
            this.weborderno = _order.weborderno;
            this.soremarks = _order.soremarks;
            this.externalno = _order.externalno;
            this.orderby = _order.orderby;
            this.cncremarks = _order.cncremarks;
            this.truck = _order.truck;
            this.check_name = _order.check_name;
            this.rfpno = _order.rfpno;
            this.sono = _order.sono;
            this.ponum = _order.ponum;
            this.billing_ref_no = _order.billing_ref_no;
            this.kas_name = _order.kas_name;
            this.channel = _order.channel;
            this.acct_ww_concern = _order.acct_ww_concern;
            this.cnc_remarks = _order.cnc_remarks;
            this.received_date = _order.received_date;
            this.branch_assigned = _order.branch_assigned;
            this.order_status = _order.order_status;
            this.mooexternalid = _order.mooexternalid;
            this.moosourcesystem = _order.moosourcesystem;
            this.kti_sapdocentry = _order.kti_sapdocentry;
            this.kti_sapdocnum = _order.kti_sapdocnum;
            this.kti_socialchannelorigin = _order.kti_socialchannelorigin;
            this.kti_sourceid = _order.kti_sourceid;
            this.companyid = _order.companyid;

            #endregion
        }

        #region properties

        [Required]
        [StringLength(300)]
        public string name { get; set; }
        public string billto_addressid { get; set; }
        [StringLength(80)]
        public string billto_city { get; set; }
        [StringLength(150)]
        public string billto_contactName { get; set; }
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
        public string campaignid { get; set; }
        [Required]
        [JsonProperty(PropertyName = "customerid_contact@odata.bind")]
        public virtual string customerid { get; set; }
        public string customeridtype { get; set; }
        public DateTime datefulfilled { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        [Range(0, 1000000000000)]
        public decimal discountamount { get; set; }
        [Range(0, 100)]
        public decimal discountpercentage { get; set; }
        [StringLength(100)]
        public string emailaddress { get; set; }
        public string entityimage { get; set; }
        [Range(0, 1000000000000)]
        public decimal freightamount { get; set; }
        [Range(1, 2)]
        public int freighttermscode { get; set; }
        public bool ispricelocked { get; set; }
        public DateTime lastbackofficesubmit { get; set; }
        public DateTime lastonholdtime { get; set; }
        public string account { get; set; }
        public string accountmanagerid { get; set; }
        public string contractorganizationalunitid { get; set; }
        [Range(192350000, 690970002)]
        public int ordertype { get; set; }
        [Range(192350000, 192350003)]
        public int psastate { get; set; }
        [Range(192350000, 192350006)]
        public int psastatusreason { get; set; }
        public string opportunityid { get; set; }
        [Required]
        [StringLength(100)]
        public string ordernumber { get; set; }
        public DateTime overriddencreatedon { get; set; }
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
        public string quoteid { get; set; }
        public string requestdeliveryby { get; set; }
        public string salesorderid { get; set; }
        [Range(1, 7)]
        public int shippingmethodcode { get; set; }
        public string shipto_addressid { get; set; }
        [StringLength(80)]
        public string shipto_city { get; set; }
        [StringLength(150)]
        public string shipto_contactName { get; set; }
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
        [Range(0, 4)]
        public int statecode { get; set; }
        [Range(1, 690970000)]
        public int statuscode { get; set; }
        public DateTime submitdate { get; set; }
        [Range(0, 1000000000)]
        public int submitstatus { get; set; }
        [StringLength(2000)]
        public string submitstatusdescription { get; set; }
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
        public string sourcesystem { get; set; }
        public string sourceofsale { get; set; }
        public string customercode { get; set; }
        public string remarks { get; set; }
        public int coffeeqty { get; set; }
        public int machineqty { get; set; }
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
        public DateTime received_date { get; set; }
        public string branch_assigned { get; set; }
        public string order_status { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        public int kti_sapdocnum { get; set; }
        public int kti_sapdocentry { get; set; }
        public int kti_socialchannelorigin { get; set; }
        public int kti_paymenttermscode { get; set; }
        public int companyid { get; set; }
        public string kti_sourceid { get; set; }
        public int importsequencenumber { get; set; }
        public int timezoneruleversionnumber { get; set; }
        public string traversedpath { get; set; }
        public int utcconversiontimezonecode { get; set; }
        public string kti_channelurl { get; set; }
        public Guid orderId { get; set; }
        public string kti_channelcode { get; set; }
        public string kti_gifttagmessage { get; set; }
        public int kti_orderstatus { get; set; }


        #endregion
    }

    /// <summary>
    /// Sales order line
    /// </summary>
    public class OrderDetails
    {

        public OrderDetails(OrderDetails _orderDetails)
        {
            #region properties
            this.baseamount = _orderDetails.baseamount;
            this.description = _orderDetails.description;
            this.extendedamount = _orderDetails.extendedamount;
            this.iscopied = _orderDetails.iscopied;
            this.ispriceoverridden = _orderDetails.ispriceoverridden;
            this.isproductoverridden = _orderDetails.isproductoverridden;
            this.lineitemnumber = _orderDetails.lineitemnumber;
            this.manualdiscountamount = _orderDetails.manualdiscountamount;
            this.agreement = _orderDetails.agreement;
            this.billingmethod = _orderDetails.billingmethod;
            this.billingstartdate = _orderDetails.billingstartdate;
            this.billingstatus = _orderDetails.billingstatus;
            this.budgetamount = _orderDetails.budgetamount;
            this.costamount = _orderDetails.costamount;
            this.costpriceperunit = _orderDetails.costpriceperunit;
            this.includeexpense = _orderDetails.includeexpense;
            this.includefee = _orderDetails.includefee;
            this.includematerial = _orderDetails.includematerial;
            this.includetime = _orderDetails.includetime;
            this.invoicefrequency = _orderDetails.invoicefrequency;
            this.linetype = _orderDetails.linetype;
            this.project = _orderDetails.project;
            this.quoteline = _orderDetails.quoteline;
            this.overriddencreatedon = _orderDetails.overriddencreatedon;
            this.parentbundleid = _orderDetails.parentbundleid;
            this.parentbundleidref = _orderDetails.parentbundleidref;
            this.priceperunit = _orderDetails.priceperunit;
            this.pricingerrorcode = _orderDetails.pricingerrorcode;
            this.productassociationid = _orderDetails.productassociationid;
            this.productdescription = _orderDetails.productdescription;
            this.productid = _orderDetails.productid;
            this.productname = _orderDetails.productname;
            this.producttypecode = _orderDetails.producttypecode;
            this.propertyconfigurationstatus = _orderDetails.propertyconfigurationstatus;
            this.quantity = _orderDetails.quantity;
            this.quantitybackordered = _orderDetails.quantitybackordered;
            this.quantitycancelled = _orderDetails.quantitycancelled;
            this.quantityshipped = _orderDetails.quantityshipped;
            this.quotedetailid = _orderDetails.quotedetailid;
            this.requestdeliveryby = _orderDetails.requestdeliveryby;
            this.salesorderdetailid = _orderDetails.salesorderdetailid;
            this.salesorderdetailname = _orderDetails.salesorderdetailname;
            this.salesorderid = _orderDetails.salesorderid;
            this.salesrepid = _orderDetails.salesrepid;
            this.shipto_addressid = _orderDetails.shipto_addressid;
            this.shipto_city = _orderDetails.shipto_city;
            this.shipto_contactname = _orderDetails.shipto_contactname;
            this.shipto_country = _orderDetails.shipto_country;
            this.shipto_fax = _orderDetails.shipto_fax;
            this.shipto_freighttermscode = _orderDetails.shipto_freighttermscode;
            this.shipto_line1 = _orderDetails.shipto_line1;
            this.shipTo_line2 = _orderDetails.shipTo_line2;
            this.shipTo_line3 = _orderDetails.shipTo_line3;
            this.shipTo_name = _orderDetails.shipTo_name;
            this.shipto_postalcode = _orderDetails.shipto_postalcode;
            this.shipto_stateorprovince = _orderDetails.shipto_stateorprovince;
            this.shipto_telephone = _orderDetails.shipto_telephone;
            this.skippriccalculation = _orderDetails.skippriccalculation;
            this.tax = _orderDetails.tax;
            this.transactioncurrencyid = _orderDetails.transactioncurrencyid;
            this.uomid = _orderDetails.uomid;
            this.willcall = _orderDetails.willcall;
            this.mooexternalid = _orderDetails.mooexternalid;
            this.moosourcesystem = _orderDetails.moosourcesystem;
            this.kti_lineitemnumber = _orderDetails.kti_lineitemnumber;
            #endregion
        }

        public OrderDetails()
        {
        }

        #region Properties
        [Range(-922337203685477, 922337203685477)]
        public decimal baseamount { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal extendedamount { get; set; }
        public bool iscopied { get; set; }
        public bool ispriceoverridden { get; set; }
        public bool isproductoverridden { get; set; }
        [Range(0, 1000000000)]
        public int lineitemnumber { get; set; }
        [Range(0, 1000000000000)]
        public decimal manualdiscountamount { get; set; }
        public string agreement { get; set; }
        [Range(192350000, 192350001)]
        public int billingmethod { get; set; }
        public DateTime billingstartdate { get; set; }
        [Range(192350000, 192350000)]
        public int billingstatus { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal budgetamount { get; set; }
        [Range(0, 922337203685477)]
        public decimal costamount { get; set; }
        [Range(0, 922337203685477)]
        public decimal costpriceperunit { get; set; }
        public bool includeexpense { get; set; }
        public bool includefee { get; set; }
        public bool includematerial { get; set; }
        public bool includetime { get; set; }
        public string invoicefrequency { get; set; }
        [Range(690970000, 690970001)]
        public int linetype { get; set; }
        [StringLength(100)]
        public string project { get; set; }
        public string quoteline { get; set; }
        public DateTime overriddencreatedon { get; set; }
        public string parentbundleid { get; set; }
        public string parentbundleidref { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public decimal priceperunit { get; set; }
        [Range(0, 38)]
        public int pricingerrorcode { get; set; }
        public string productassociationid { get; set; }
        [StringLength(500)]
        public string productdescription { get; set; }
        [JsonProperty(PropertyName = "productid@odata.bind")]
        public string productid { get; set; }
        [StringLength(500)]
        public string productname { get; set; }
        [Range(1, 5)]
        public int producttypecode { get; set; }
        [Range(0, 2)]
        public int propertyconfigurationstatus { get; set; }
        [Required]
        [Range(-100000000000, 100000000000)]
        public decimal quantity { get; set; }
        [Range(0, 100000000000)]
        public decimal quantitybackordered { get; set; }
        [Range(0, 100000000000)]
        public decimal quantitycancelled { get; set; }
        [Range(-100000000000, 100000000000)]
        public decimal quantityshipped { get; set; }
        public string quotedetailid { get; set; }
        public DateTime requestdeliveryby { get; set; }
        public string salesorderdetailid { get; set; }
        [StringLength(100)]
        public string salesorderdetailname { get; set; }
        [Required]
        public string salesorderid { get; set; }
        public string salesrepid { get; set; }
        public string shipto_addressid { get; set; }
        [StringLength(80)]
        public string shipto_city { get; set; }
        [StringLength(150)]
        public string shipto_contactname { get; set; }
        [StringLength(80)]
        public string shipto_country { get; set; }
        [StringLength(50)]
        public string shipto_fax { get; set; }
        [Range(1, 2)]
        public int shipto_freighttermscode { get; set; }
        [StringLength(250)]
        public string shipto_line1 { get; set; }
        [StringLength(250)]
        public string shipTo_line2 { get; set; }
        [StringLength(250)]
        public string shipTo_line3 { get; set; }
        [StringLength(200)]
        public string shipTo_name { get; set; }
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
        public string transactioncurrencyid { get; set; }
        public string uomid { get; set; }
        public bool willcall { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }

        public string kti_lineitemnumber { get; set; }
        public int importsequencenumber { get; set; }
        public string salesorderdetailId { get; set; }
        public int sequencenumber { get; set; }
        public string timezoneruleversionnumber { get; set; }
        public string utcconversiontimezonecode { get; set; }
        public int companyid { get; set; }
        public Guid orderdetailsid { get; set; }
        public int kti_socialchannelorigin { get; set; }

        #endregion
    }

    /// <summary>
    /// Sales order headers and line
    /// </summary>
    public class OrderAndDetails
    {
        public OrderAndDetails()
        {
        }

        public OrderAndDetails(OrderAndDetails _orderAndDetails)
        {
            this.order = _orderAndDetails.order;
            this.orderdetails = _orderAndDetails.orderdetails;
        }

        #region properties
        [Required] public Order order { get; set; }
        public IEnumerable<OrderDetails> orderdetails { get; set; }
        #endregion
    }
}