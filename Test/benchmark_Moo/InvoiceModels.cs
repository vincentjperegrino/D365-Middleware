using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benchmark_Moo
{
    public class InvoiceModels
    {

        public InvoiceModels(KTI.Moo.Extensions.OctoPOS.Model.InvoiceItem _invoiceDetails)
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
            this.invoiceid = _invoiceDetails.invoiceid;
            this.description = _invoiceDetails.description;

            custom_attributes = new();

            custom_attributes.Add(new()
            {
                Name ="salesman",
                Description = "salesman",
                TypeOf="string",
                Value = _invoiceDetails.SalesmanName

            });

            custom_attributes.Add(new()
            {
                Name = "taxpercentage",
                Description = "taxpercentage",
                TypeOf = "decimal",
                Value = _invoiceDetails.TaxPercentage.ToString()

            });

            custom_attributes.Add(new()
            {
                Name = "discountpercentage",
                Description = "discountpercentage",
                TypeOf = "decimal",
                Value = _invoiceDetails.TaxPercentage.ToString()

            });


            var OCTOPOSchannelorigin = 959080011;
            this.kti_socialchannelorigin = OCTOPOSchannelorigin;

            #endregion
        }
        #region Properties
        public DateTime actualdeliveryon { get; set; }
        public decimal baseamount { get; set; }
        public string description { get; set; }
        public decimal extendedamount { get; set; }
        public string importsequencenumber { get; set; }
        public string invoicedetailid { get; set; }
        public string invoiceid { get; set; }
        public bool ispriceoverridden { get; set; }
        public int lineitemnumber { get; set; }
        public decimal manualdiscountamount { get; set; }
        public string agreement { get; set; }
        public string agreementinvoiceproduct { get; set; }
        public string billingmethod { get; set; }
        public decimal chargeableamount { get; set; }
        public decimal complimentaryamount { get; set; }
        public string contractline { get; set; }
        public decimal contractlineamount { get; set; }
        public string currency { get; set; }
        public DateTime invoicedtilldate { get; set; }
        public string lineorder { get; set; }
        public string linetype { get; set; }
        public decimal nonchargeableamount { get; set; }
        public string orderinvoicingproduct { get; set; }
        public string project { get; set; }
        public string workorderid { get; set; }
        public string workorderproductid { get; set; }
        public string workorderserviceid { get; set; }
        public DateTime overriddencreatedon { get; set; }
        public string parentbundleid { get; set; }
        public string parentbundleidref { get; set; }
        public decimal priceperunit { get; set; }
        public string productdescription { get; set; }
        public string productid { get; set; }
        public string productname { get; set; }
        public int producttypecode { get; set; }
        public string propertyconfigurationstatus { get; set; }
        public decimal quantity { get; set; }
        public double quantitybackordered { get; set; }
        public double quantitycancelled { get; set; }
        public double quantityshipped { get; set; }
        public string salesorderdetailId { get; set; }
        public string salesrepid { get; set; }
        public string sequencenumber { get; set; }
        public string shippingtrackingnumber { get; set; }
        public string shipto_city { get; set; }
        public string shipto_country { get; set; }
        public string shipto_fax { get; set; }
        public int shipto_freighttermscode { get; set; }
        public string shipto_line1 { get; set; }
        public string shipto_line2 { get; set; }
        public string shipto_line3 { get; set; }
        public string shipto_name { get; set; }
        public string shipto_postalcode { get; set; }
        public string shipto_stateorprovince { get; set; }
        public string shipto_telephone { get; set; }
        public bool skippriccalculation { get; set; }
        public decimal tax { get; set; }
        public string timezoneruleversionnumber { get; set; }
        public string transactioncurrencyid { get; set; }
        public string uomid { get; set; }
        public string utcconversiontimezonecode { get; set; }
        public bool willcall { get; set; }

        public string moosourcesystem { get; set; }

        public string mooexternalid { get; set; }
        //Customized

        public int companyid { get; set; }

        public int kti_socialchannelorigin { get; set; }
        public List<CustomAttribute> custom_attributes { get; set; }

        #endregion
    }

    public class CustomAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string TypeOf { get; set; }
        public List<CustomAttribute> custom_attribute { get; set; }
    }
}
