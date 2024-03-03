using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.ChannelApps.Model.NCCI.Receivers.Plugin;
public class OrderItem : CRM.Model.DTO.Orders.Plugin.OrderItem
{
    public OrderItem(KTI.Moo.Extensions.Magento.Model.OrderItem _orderItem)
    {
        #region properties
        // this.baseamount = _orderItem.baseamount;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_magento;
        this.ispriceoverridden = true; //Price should be always overridden   
        this.manualdiscountamount = _orderItem.manualdiscountamount;
        this.priceperunit = _orderItem.priceperunit;
        this.quantity = _orderItem.quantity;
        this.tax = _orderItem.tax;
        //   this.salesorderid = _orderItem.salesorderid;

        if (!string.IsNullOrWhiteSpace(_orderItem.sku))
        {
            this.productid = _orderItem.sku;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.productdescription))
        {
            this.productdescription = _orderItem.productdescription;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.description))
        {
            this.description = _orderItem.description;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_city))
        {
            this.shipto_city = _orderItem.shipto_city;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_contactname))
        {
            this.shipto_contactname = _orderItem.shipto_contactname;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_country))
        {
            this.shipto_country = _orderItem.shipto_country;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_fax))
        {
            this.shipto_fax = _orderItem.shipto_fax;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_line1))
        {
            this.shipto_line1 = _orderItem.shipto_line1;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_line2))
        {
            this.shipto_line2 = _orderItem.shipto_line2;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_line3))
        {
            this.shipto_line3 = _orderItem.shipto_line3;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_name))
        {
            this.shipto_name = _orderItem.shipto_name;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_postalcode))
        {
            this.shipto_postalcode = _orderItem.shipto_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_stateorprovince))
        {
            this.shipto_stateorprovince = _orderItem.shipto_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_telephone))
        {
            this.shipto_telephone = _orderItem.shipto_telephone;
        }

        //  this.kti_lineitemnumber = _orderItem.kti_lineitemnumber != null ? _orderItem.kti_lineitemnumber : (_orderItem.lineitemnumber != 0 ? _orderItem.lineitemnumber.ToString() : "0");
        // this.kti_orderstatus = 959080001;
        //this.kti_sourceid = _orderItem.salesorderid;     
        // this.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";



        #endregion

    }

    public OrderItem(KTI.Moo.Extensions.OctoPOS.Model.InvoiceItem _invoiceDetails, string invoiceType)
    {

        #region properties

        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_octopos;
        this.ispriceoverridden = true;
        this.tax = _invoiceDetails.tax;
        this.quantity = _invoiceDetails.quantity;
        this.manualdiscountamount = _invoiceDetails.manualdiscountamount;
        this.priceperunit = _invoiceDetails.priceperunit;
        // this.kti_lineitemnumber = _invoiceDetails.lineitemnumber.ToString();

        //this.baseamount = _invoiceDetails.RetailSalesPrice;

        // this.invoicedetailid = _invoiceDetails.invoicedetailid;
        //Price should be always overridden

        if (!string.IsNullOrWhiteSpace(_invoiceDetails.description))
        {
            this.description = _invoiceDetails.description;
            this.kti_remark = _invoiceDetails.description;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceDetails.SalesmanName))
        {
            this.kti_salesperson = _invoiceDetails.SalesmanName;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceDetails.productid))
        {
            this.productid = _invoiceDetails.productid;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceDetails.productdescription))
        {
            this.productdescription = _invoiceDetails.productdescription;
        }
   
        IsCreditNote(invoiceType, _invoiceDetails.DiscountPercentage);

        // this.ncci_Salesman = $"/ncci_salesmans(ncci_code='{_invoiceDetails.SalesmanCode}')";
        //this.ncci_discountpercentage = _invoiceDetails.DiscountPercentage;
        //this.ncci_taxpercentage = _invoiceDetails.TaxPercentage;
        //this.invoiceid = $"/invoices(kti_sourceid='{_invoiceDetails.invoiceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
        //  this.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
        #endregion

    }

    public OrderItem(KTI.Moo.Extensions.Lazada.Model.OrderItem _orderItem)
    {
        #region properties
        // this.baseamount = _orderItem.baseamount;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_lazada;
        this.ispriceoverridden = true; //Price should be always overridden   
        this.manualdiscountamount = _orderItem.laz_DailyDiscount;
        this.priceperunit = _orderItem.priceperunit;
        this.quantity = _orderItem.quantity;
        this.tax = _orderItem.tax;
        this.kti_sourceitemid = _orderItem.laz_OrderItemID.ToString();


        if (!string.IsNullOrWhiteSpace(_orderItem.description))
        {
            this.description = _orderItem.description;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_city))
        {
            this.shipto_city = _orderItem.shipto_city;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_contactname))
        {
            this.shipto_contactname = _orderItem.shipto_contactname;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_country))
        {
            this.shipto_country = _orderItem.shipto_country;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_fax))
        {
            this.shipto_fax = _orderItem.shipto_fax;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_line1))
        {
            this.shipto_line1 = _orderItem.shipto_line1;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_line2))
        {
            this.shipto_line2 = _orderItem.shipto_line2;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_line3))
        {
            this.shipto_line3 = _orderItem.shipto_line3;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_name))
        {
            this.shipto_name = _orderItem.shipto_name;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_postalcode))
        {
            this.shipto_postalcode = _orderItem.shipto_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_stateorprovince))
        {
            this.shipto_stateorprovince = _orderItem.shipto_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.shipto_telephone))
        {
            this.shipto_telephone = _orderItem.shipto_telephone;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.productid))
        {
            this.productid = _orderItem.productid;
        }

        if (!string.IsNullOrWhiteSpace(_orderItem.productdescription))
        {
            this.productdescription = _orderItem.productdescription;
        }
 
        //    this.kti_lineitemnumber = _orderItem.kti_lineitemnumber != null ? _orderItem.kti_lineitemnumber : (_orderItem.lineitemnumber != 0 ? _orderItem.lineitemnumber.ToString() : "0");
        // this.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";

        //  this.kti_orderstatus = 959080001;

        //   this.productid = productid;

        //Domain.Modules.Product modProduct = new(companyID);
        //var productResponse = modProduct.get_by_productnumber(_orderItem.productid).GetAwaiter().GetResult();
        //JObject jsonObject = JObject.Parse(productResponse);

        //this.productid = default;
        //this.isproductoverridden = true;
        //this.productdescription = _orderItem.productdescription;

        //if (jsonObject.ContainsKey("value"))
        //{
        //    var products = jsonObject["value"].ToObject<dynamic>();
        //    if (Enumerable.Count(products) > 0)
        //    {
        //        var product = products[0];

        //        this.productid = $"/products({product.productid})";
        //        this.uomid = $"/uoms({product._defaultuomid_value})";
        //        this.productdescription = default;
        //        this.isproductoverridden = false;
        //    }

        //}

        #endregion

    }


    public string kti_remark { get; set; }

    [JsonProperty("productid")]
    public override string productid { get; set; }
    [JsonProperty("uomid")]
    public override string uomid { get; set; }


    private void IsCreditNote(string invoiceType, decimal DiscountPercentage)
    {
        // make price and tax to negative
        if (invoiceType == "credit note")
        {
            decimal discount = 0;
            if (this.manualdiscountamount != 0)
            {
                discount = this.manualdiscountamount / this.quantity;
            }

            this.manualdiscountamount = 0;
            this.priceperunit = DiscountPercentage == (decimal)(100) ? 0 : -(this.priceperunit - discount);


            this.tax = -this.tax;

        }
    }


}
