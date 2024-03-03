using KTI.Moo.CRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.KTIdev.Receivers;

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
        this.description = _orderHeader.laz_Remarks;
        this.discountamount = _orderHeader.discountamount;
        // this.discountpercentage = _orderHeader.discountpercentage;
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
        this.kti_couponcode = _orderHeader.laz_VoucherCode;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_lazada;

        this.kti_giftwrap = _orderHeader.kti_giftwrap;
        this.kti_gifttagmessage = _orderHeader.kti_gifttagmessage;

        if (_orderHeader.laz_PaymentMethod == "COD")
        {
            this.kti_paymenttermscode = 959_080_002;
        }

        if (_orderHeader.laz_PaymentMethod == "MIXEDCARD")
        {
            this.kti_paymenttermscode = 959_080_019;
        }

        if (_orderHeader.laz_PaymentMethod == "BDO_IPP")
        {
            this.kti_paymenttermscode = 959_080_018;
        }

        if (_orderHeader.laz_PaymentMethod == "PAYMENT_ACCOUNT")
        {
            this.kti_paymenttermscode = 959_080_023;
        }

        if (_orderHeader.laz_PaymentMethod == "PAYPAL")
        {
            this.kti_paymenttermscode = 959_080_020;
        }

        if (_orderHeader.laz_PaymentMethod == "GCASH_PP")
        {
            this.kti_paymenttermscode = 959_080_021;
        }

        if (_orderHeader.laz_PaymentMethod == "PAY_LATER")
        {
            this.kti_paymenttermscode = 959_080_022;
        }


        this.pricelevelid = "/pricelevels(name = 'Regular Price List')";

        if (_orderHeader.order_status == "unpaid" )
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.ForPayment;
        }

        if (_orderHeader.order_status == "pending")
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.Checkout;
        }

        if (_orderHeader.order_status == "canceled" || _orderHeader.order_status == "cancelled")
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.CancelOrder;
        }

        if (_orderHeader.order_status == "packed")
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.OrderPacked;
        }

        if (_orderHeader.order_status == "ready_to_ship_pending" || _orderHeader.order_status == "ready_to_ship")
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.OrderReleased;
        }

        if (_orderHeader.order_status == "shipped")
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.ReceivedByDelivery;
        }

        if (_orderHeader.order_status == "delivered")
        {
            this.kti_orderstatus = Base.Helpers.OrderStatus.DeliveredToCustomer;
        }

        //     this.customerid = _orderHeader.emailaddress;

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
            var OrderItemModel = new OrderItem(orderitem, companyid);

            OrderItemModel.kti_sourceid = this.kti_sourceid;
            OrderItemModel.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
            return OrderItemModel;

        }).ToList();

        #endregion
    }

    public List<OrderItem> orderItem { get; set; }


}
