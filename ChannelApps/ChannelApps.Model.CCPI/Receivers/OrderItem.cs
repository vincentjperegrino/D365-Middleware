using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.CCPI.Receivers;

public class OrderItem : CRM.Model.OrderItemBase
{

    public OrderItem()
    {

    }

    public OrderItem(KTI.Moo.Extensions.Lazada.Model.OrderItem _orderItem, int companyID)
    {
        #region properties
       // this.baseamount = _orderItem.baseamount;
        this.description = _orderItem.description;
        this.ispriceoverridden = true; //Price should be always overridden   
        this.manualdiscountamount = _orderItem.manualdiscountamount;
        this.priceperunit = _orderItem.priceperunit;

        this.quantity = _orderItem.quantity;
        //   this.salesorderid = _orderItem.salesorderid;
        this.shipto_city = _orderItem.shipto_city;
        this.shipto_contactname = _orderItem.shipto_contactname;
        this.shipto_country = _orderItem.shipto_country;
        this.shipto_fax = _orderItem.shipto_fax;
        this.shipto_line1 = _orderItem.shipto_line1;
        this.shipto_line2 = _orderItem.shipto_line2;
        this.shipto_line3 = _orderItem.shipto_line3;
        this.shipto_name = _orderItem.shipto_name;
        this.shipto_postalcode = _orderItem.shipto_postalcode;
        this.shipto_stateorprovince = _orderItem.shipto_stateorprovince;
        this.shipto_telephone = _orderItem.shipto_telephone;
        this.tax = _orderItem.tax;
        var MAGENTOchannelorigin = 959080010;
        this.kti_socialchannelorigin = MAGENTOchannelorigin;

        this.kti_lineitemnumber = _orderItem.kti_lineitemnumber != null ? _orderItem.kti_lineitemnumber : (_orderItem.lineitemnumber != 0 ? _orderItem.lineitemnumber.ToString() : "0");

        this.kti_sourceid = _orderItem.salesorderid;
        this.kti_orderstatus = 959080001;
        this.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";


        Domain.Modules.Product modProduct = new(companyID);
        var productResponse = modProduct.get_by_productnumber(_orderItem.productid).GetAwaiter().GetResult();
        JObject jsonObject = JObject.Parse(productResponse);

        this.productid = default;
        this.isproductoverridden = true;
        this.productdescription = _orderItem.productdescription;

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

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public override bool isproductoverridden { get; set; }

}
