
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Receivers;

public class InvoiceItem : CRM.Model.InvoiceItemBase
{
    public InvoiceItem()
    {

    }


    public InvoiceItem(KTI.Moo.Extensions.Magento.Model.InvoiceItem _invoiceDetails, int _company)
    {
        #region properties

        // this.baseamount = _invoiceDetails.baseamount;
        this.description = _invoiceDetails.description;
        this.invoicedetailid = _invoiceDetails.invoicedetailid;
        this.priceperunit = _invoiceDetails.priceperunit;

        this.ispriceoverridden = true; //Price should be always overridden
        this.lineitemnumber = _invoiceDetails.lineitemnumber;
        this.manualdiscountamount = _invoiceDetails.manualdiscountamount;
        this.priceperunit = _invoiceDetails.priceperunit;
        this.productdescription = _invoiceDetails.productdescription;
        this.productid = _invoiceDetails.productid;
        this.quantity = _invoiceDetails.quantity;
        this.tax = _invoiceDetails.tax;
        this.moosourcesystem = _invoiceDetails.moosourcesystem;
        this.kti_lineitemnumber = _invoiceDetails.kti_lineitemnumber;
        this.shipto_city = _invoiceDetails.shipto_city;
        this.shipto_country = _invoiceDetails.shipto_country;
        this.shipto_fax = _invoiceDetails.shipto_fax;
        this.shipto_line1 = _invoiceDetails.shipto_line1;
        this.shipto_line2 = _invoiceDetails.shipto_line2;
        this.shipto_line3 = _invoiceDetails.shipto_line3;
        this.shipto_name = _invoiceDetails.shipto_name;
        this.shipto_postalcode = _invoiceDetails.shipto_postalcode;
        this.shipto_stateorprovince = _invoiceDetails.shipto_stateorprovince;
        this.shipto_telephone = _invoiceDetails.shipto_telephone;

        //this.kti_sourceid = _invoiceDetails.invoiceid;
       // this.ncci_remark = _invoiceDetails.description;
        //this.ncci_Salesman = _invoiceDetails.SalesmanCode;
        //this.ncci_discountpercentage = _invoiceDetails.DiscountPercentage;
        //this.ncci_taxpercentage = _invoiceDetails.TaxPercentage;

        var MAGENTOchannelorigin = 959080010;
        this.kti_socialchannelorigin = MAGENTOchannelorigin;


        //Relate invoice header
     

        Domain.Modules.Product modProduct = new(_company);
        var productResponse = modProduct.get_by_productnumber(_invoiceDetails.sku).GetAwaiter().GetResult();
        JObject jsonObject = JObject.Parse(productResponse);

        this.productid = default;
        this.isproductoverridden = true;
        this.productdescription = _invoiceDetails.productdescription;


        if (jsonObject.ContainsKey("value"))
        {
            var products = jsonObject["value"].ToObject<dynamic>();
            if (Enumerable.Count(products) > 0)
            {
                var product = products[0];

                this.productid = $"/products({product.productid})";
                this.uomid = $"/uoms({product._defaultuomid_value})";
                this.productdescription = default;
                this.isproductoverridden = false;
            }
        }


        #endregion



    }


    public InvoiceItem(KTI.Moo.Extensions.OctoPOS.Model.InvoiceItem _invoiceDetails, int _company)
    {
        #region properties
        this.domainType = KTI.Moo.Base.Helpers.DomainType.order;
        this.baseamount = _invoiceDetails.RetailSalesPrice;
        this.description = _invoiceDetails.description;
        this.invoicedetailid = _invoiceDetails.invoicedetailid;
        this.ispriceoverridden = true; //Price should be always overridden
        this.lineitemnumber = _invoiceDetails.lineitemnumber;
        this.manualdiscountamount = _invoiceDetails.manualdiscountamount;


        this.priceperunit = _invoiceDetails.priceperunit;

       // this.priceperunit = _invoiceDetails.RetailSalesPrice;


        this.productdescription = _invoiceDetails.productdescription;
        this.quantity = _invoiceDetails.quantity;
        this.tax = _invoiceDetails.tax;
        this.moosourcesystem = _invoiceDetails.moosourcesystem;
        this.kti_lineitemnumber = _invoiceDetails.lineitemnumber.ToString();
        this.kti_sourceid = _invoiceDetails.invoiceid;


        this.kti_remark = _invoiceDetails.description;
        // this.ncci_Salesman = $"/ncci_salesmans(ncci_code='{_invoiceDetails.SalesmanCode}')";
        //this.ncci_discountpercentage = _invoiceDetails.DiscountPercentage;
        //this.ncci_taxpercentage = _invoiceDetails.TaxPercentage;

        this.kti_salesperson = _invoiceDetails.SalesmanName;

        var OCTOPOSchannelorigin = 959080011;
        this.kti_socialchannelorigin = OCTOPOSchannelorigin;


        //this.invoiceid = $"/invoices(kti_sourceid='{_invoiceDetails.invoiceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
      //  this.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";

        this.productid = _invoiceDetails.productid;


        Domain.Modules.Product modProduct = new(_company);
        var productResponse = modProduct.get_by_productnumber(_invoiceDetails.productid).GetAwaiter().GetResult();
        JObject jsonObject = JObject.Parse(productResponse);

        this.productid = default;
        this.isproductoverridden = true;
        this.productdescription = _invoiceDetails.productdescription;

        if (jsonObject.ContainsKey("value"))
        {
            var products = jsonObject["value"].ToObject<dynamic>();
            if (Enumerable.Count(products) > 0)
            {
                var product = products[0];

                this.productid = $"/products({product.productid})";
                this.uomid = $"/uoms({product._defaultuomid_value})";
                this.productdescription = default;
                this.isproductoverridden = false;
            }

        }


        #endregion

    }

    #region NCCI properties
    public string kti_remark { get; set; }

    [JsonProperty(PropertyName = "ncci_Salesman@odata.bind")]
    public string ncci_Salesman { get; set; }

    [JsonProperty(PropertyName = "kti_Branch@odata.bind")]
    public string kti_Branch { get; set; }

    [JsonProperty(PropertyName = "ncci_CustomerOrdProd@odata.bind")]
    public string ncci_CustomerOrdProd { get; set; }


    public decimal ncci_discountpercentage { get; set; }

    public decimal ncci_taxpercentage { get; set; }
    public decimal ncci_coffee_qty { get; set; }
    public decimal ncci_machine_qty { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public override bool isproductoverridden { get; set; }

    [JsonProperty("salesorderid@odata.bind")]
    public string salesorderid { get; set; }
    public string kti_salesperson { get; set; }
    #endregion



}
