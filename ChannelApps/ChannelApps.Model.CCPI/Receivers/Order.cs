using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.CCPI.Receivers;

public class Order : CRM.Model.OrderBase
{

    public Order()
    {

    }


    public Order(KTI.Moo.Extensions.Lazada.Model.OrderHeader _orderHeader)
    {
        #region properties

        this.companyid = _orderHeader.companyid;
        this.name = _orderHeader.kti_sourceid;
        this.billto_city = _orderHeader.billto_city;
        this.billto_contactname = _orderHeader.billto_contactname;
        this.billto_country = _orderHeader.billto_country;
        this.billto_fax = _orderHeader.billto_fax;
        this.billto_line1 = _orderHeader.billto_line1;
        this.billto_line2 = _orderHeader.billto_line2;
        this.billto_line3 = _orderHeader.billto_line3;
        this.billto_name = _orderHeader.billto_name;
        this.billto_postalcode = _orderHeader.billto_postalcode;
        this.billto_stateorprovince = _orderHeader.billto_stateorprovince;
        this.billto_telephone = _orderHeader.billto_telephone;
        this.description = _orderHeader.description;
        //this.discountamount = _orderHeader.discountamount;
        //this.discountpercentage = _orderHeader.discountpercentage;
        this.emailaddress = _orderHeader.emailaddress;
        this.freightamount = _orderHeader.freightamount;
        this.salesorderid = _orderHeader.kti_sourceid;
        this.shipto_city = _orderHeader.shipto_city;
        this.shipto_contactname = _orderHeader.shipto_contactname;
        this.shipto_country = _orderHeader.shipto_country;
        this.shipto_fax = _orderHeader.shipto_fax;
        this.shipto_line1 = _orderHeader.shipto_line1;
        this.shipto_line2 = _orderHeader.shipto_line2;
        this.shipto_line3 = _orderHeader.shipto_line3;
        this.shipto_name = _orderHeader.shipto_name;
        this.shipto_postalcode = _orderHeader.shipto_postalcode;
        this.shipto_stateorprovince = _orderHeader.shipto_stateorprovince;
        this.shipto_telephone = _orderHeader.shipto_telephone;
        //this.totaltax = _orderHeader.tax_amount;
        this.kti_channelurl = _orderHeader.kti_channelurl;
        this.kti_sourceid = _orderHeader.kti_sourceid;
        this.kti_couponcode = _orderHeader.kti_couponcode;
        var MAGENTOchannelorigin = 959080010;
        this.kti_socialchannelorigin = MAGENTOchannelorigin;

   //     this.pricelevelid = "/pricelevels(name = 'Standard')";

        //if (_orderHeader.order_status == "pending")
        //{
        //    this.statuscode = 2;
        //}

        //if (_orderHeader.order_status == "processing")
        //{
        //    this.statuscode = 959_080_000;
        //}

        //if (_orderHeader.order_status == "holded")
        //{
        //    this.statuscode = 959_080_001;
        //}

        //if (_orderHeader.order_status == "canceled")
        //{
        //    this.statuscode = 959_080_002;
        //}

        //if (_orderHeader.order_status == "complete")
        //{
        //    this.statuscode = 959_080_003;
        //}

        //if (_orderHeader.order_payment.method == "pnx" && _orderHeader.order_payment.additional_information.Contains("CC"))
        //{
        //    this.kti_paymenttermscode = 959_080_015;
        //}


        //if (_orderHeader.order_payment.method == "pnx" && _orderHeader.order_payment.additional_information.Contains("GC"))
        //{
        //    this.kti_paymenttermscode = 959_080_016;
        //}

        ////if (_orderHeader.order_payment.method == "Cash on delivery")
        ////{
        ////    this.kti_paymenttermscode = 959_080_002;
        ////}    

        //if (_orderHeader.order_payment.method == "phoenix_cashondelivery")
        //{
        //    this.kti_paymenttermscode = 959_080_002;
        //}

        //if (_orderHeader.order_payment.method == "cashondelivery")
        //{
        //    this.kti_paymenttermscode = 959_080_017;
        //}


        CRM.Domain.Customer customer = new(companyid);


        var Contactid = customer.GetContactBy_Email(_orderHeader.emailaddress).GetAwaiter().GetResult();

        if (!string.IsNullOrWhiteSpace(Contactid))
        {
            this.customerid = $"/contacts({Contactid})";
        }
        else
        {
            this.customerid = _orderHeader.emailaddress;
        }

        var linenumber = 1;
        this.orderItem = _orderHeader.order_items.Select(orderitem =>
        {
            orderitem.kti_lineitemnumber = linenumber.ToString();
            linenumber++;
            var OrderItemModel = new OrderItem(orderitem, _orderHeader.companyid);
            return OrderItemModel;
        }).ToList();

        #endregion
    }



    public List<OrderItem> orderItem { get; set; }


}
