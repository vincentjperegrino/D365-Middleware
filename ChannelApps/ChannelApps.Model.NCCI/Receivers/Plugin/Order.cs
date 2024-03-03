using KTI.Moo.Base.Helpers;
using KTI.Moo.Extensions.Lazada.Model;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.ChannelApps.Model.NCCI.Receivers.Plugin;
public class Order : CRM.Model.DTO.Orders.Plugin.Order
{
    public Order(KTI.Moo.Extensions.Magento.Model.Order _orderHeader)
    {
        //this.companyid = _orderHeader.companyid;

        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_magento;
        this.overriddencreatedon = _orderHeader.created_at;
        this.freightamount = _orderHeader.freightamount;
        //this.discountamount = _orderHeader.discountamount;
        //this.discountpercentage = _orderHeader.discountpercentage;
        //this.salesorderid = _orderHeader.kti_sourceid;

        if (!string.IsNullOrWhiteSpace(_orderHeader.kti_sourceid))
        {
            this.kti_sourceid = _orderHeader.kti_sourceid;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.kti_couponcode))
        {
            this.kti_couponcode = _orderHeader.kti_couponcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.increment_id))
        {
            this.name = _orderHeader.increment_id;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_city))
        {
            this.billto_city = _orderHeader.billto_city;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_contactname))
        {
            this.billto_contactname = _orderHeader.billto_contactname;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_country))
        {
            this.billto_country = _orderHeader.billto_country;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_fax))
        {
            this.billto_fax = _orderHeader.billto_fax;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_line1))
        {
            this.billto_line1 = _orderHeader.billto_line1;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_line2))
        {
            this.billto_line2 = _orderHeader.billto_line2;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_line3))
        {
            this.billto_line3 = _orderHeader.billto_line3;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_name))
        {
            this.billto_name = _orderHeader.billto_name;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_postalcode))
        {
            this.billto_postalcode = _orderHeader.billto_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_stateorprovince))
        {
            this.billto_stateorprovince = _orderHeader.billto_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_telephone))
        {
            this.billto_telephone = _orderHeader.billto_telephone.FormatPhoneNumber();
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.description))
        {
            this.description = _orderHeader.description;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.emailaddress))
        {
            this.emailaddress = _orderHeader.emailaddress.ToLower();
        }
        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_city))
        {
            this.shipto_city = _orderHeader.shipto_city;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_contactname))
        {
            this.shipto_contactname = _orderHeader.shipto_contactname;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_country))
        {
            this.shipto_country = _orderHeader.shipto_country;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_fax))
        {
            this.shipto_fax = _orderHeader.shipto_fax;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_line1))
        {
            this.shipto_line1 = _orderHeader.shipto_line1;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_line2))
        {
            this.shipto_line2 = _orderHeader.shipto_line2;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_line2))
        {
            this.shipto_line3 = _orderHeader.shipto_line3;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_name))
        {
            this.shipto_name = _orderHeader.shipto_name;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_postalcode))
        {
            this.shipto_postalcode = _orderHeader.shipto_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_stateorprovince))
        {
            this.shipto_stateorprovince = _orderHeader.shipto_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_telephone))
        {
            this.shipto_telephone = _orderHeader.shipto_telephone;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.kti_channelurl))
        {
            this.kti_channelurl = _orderHeader.kti_channelurl;
        }



        //this.totaltax = _orderHeader.tax_amount;


        //this.pricelevelid = "/pricelevels(name = 'Standard')";

        MapMagentoStatusToCRM(_orderHeader);

        MapMagentoPaymentTermToCRM(_orderHeader);


    }

    public Order(KTI.Moo.Extensions.OctoPOS.Model.Invoice _invoiceHeader)
    {

        #region properties

        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_octopos;
        this.overriddencreatedon = _invoiceHeader.invoicedate;
        this.freightamount = _invoiceHeader.freightamount;

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.kti_sourceid))
        {
            this.kti_sourceid = _invoiceHeader.kti_sourceid;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.CustomerEmail))
        {
            this.emailaddress = _invoiceHeader.CustomerEmail.ToLower();
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.CustomerPhone))
        {
            this.billto_telephone = _invoiceHeader.CustomerPhone.FormatPhoneNumber();
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.Location))
        {
            this.branch_assigned = _invoiceHeader.Location;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.name))
        {
            this.name = _invoiceHeader.name;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.CustomerDetails.Address1))
        {
            this.billto_line1 = _invoiceHeader.CustomerDetails.Address1;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.description))
        {
            this.description = _invoiceHeader.description;
        }


        if (!string.IsNullOrWhiteSpace(_invoiceHeader.invoicenumber))
        {
            this.invoicenumber = _invoiceHeader.invoicenumber;
        }

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.CustomerDetails.ShippingAddress))
        {
            this.shipto_line1 = _invoiceHeader.CustomerDetails.ShippingAddress;
        }


        if (_invoiceHeader.PaymentItems is not null && _invoiceHeader.PaymentItems.Count > 0)
        {
            var paymentmethod = _invoiceHeader.PaymentItems.FirstOrDefault().PaymentMode;
            this.kti_paymenttermscode = GetPaymentMethodOctopos(paymentmethod);
        }

        MapOctopusStatusToCRM(_invoiceHeader);


        //this.coffeeqty = _invoiceHeader.coffeeqty;
        //this.machineqty = _invoiceHeader.machineqty;
        //this.coffee = _invoiceHeader.coffee;
        //this.noncoffee = _invoiceHeader.noncoffee;

        //var linenum = 1;
        //this.invoiceItem = _invoiceHeader.InvoiceItems.Select(invoiceitem =>
        //{
        //    invoiceitem.lineitemnumber = linenum;
        //    invoiceitem.kti_lineitemnumber = linenum.ToString();
        //    linenum++;

        //    var InvoiceItemModel = new InvoiceItem(invoiceitem, companyid);

        //    InvoiceItemModel.kti_sourceid = this.kti_sourceid;
        //    InvoiceItemModel.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
        //    //External Mapping of Invoice Items
        //    // InvoiceItemModel.ncci_CustomerOrdProd = this.customerid;
        //    InvoiceItemModel.kti_Branch = $"/kti_branchs(kti_branchcode='{_invoiceHeader.Location}')";

        //    return InvoiceItemModel;
        //}).ToList();



        #endregion

    }

    public Order(KTI.Moo.Extensions.Lazada.Model.OrderHeader _orderHeader)
    {
        #region properties

        // this.companyid = _orderHeader.companyid;
        this.kti_socialchannelorigin = CRM.Helper.ChannelOrigin.OptionSet_lazada;
        this.kti_socialchannel = _orderHeader.kti_socialchannel;
        this.overriddencreatedon = _orderHeader.laz_CreatedOn;
        this.freightamount = _orderHeader.freightamount;
        this.kti_giftwrap = _orderHeader.kti_giftwrap;
        //this.discountamount = _orderHeader.discountamount;
        // this.discountpercentage = _orderHeader.discountpercentage;
        //   this.salesorderid = _orderHeader.kti_sourceid;
        //this.totaltax = _orderHeader.tax_amount;

        if (!string.IsNullOrWhiteSpace(_orderHeader.kti_sourceid))
        {
            this.kti_sourceid = _orderHeader.kti_sourceid;
            this.name = _orderHeader.kti_sourceid;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_city))
        {
            this.billto_city = _orderHeader.billto_city;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_contactname))
        {
            this.billto_contactname = _orderHeader.billto_contactname;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_country))
        {
            this.billto_country = _orderHeader.billto_country;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_fax))
        {
            this.billto_fax = _orderHeader.billto_fax;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_line1))
        {
            this.billto_line1 = _orderHeader.billto_line1;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_line2))
        {
            this.billto_line2 = _orderHeader.billto_line2;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_line3))
        {
            this.billto_line3 = _orderHeader.billto_line3;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_name))
        {
            this.billto_name = _orderHeader.billto_name;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_postalcode))
        {
            this.billto_postalcode = _orderHeader.billto_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_stateorprovince))
        {
            this.billto_stateorprovince = _orderHeader.billto_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.billto_telephone))
        {
            this.billto_telephone = _orderHeader.billto_telephone.FormatPhoneNumber();
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.laz_Remarks))
        {
            this.description = _orderHeader.laz_Remarks;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.emailaddress))
        {
            this.emailaddress = _orderHeader.emailaddress.ToLower();
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_city))
        {
            this.shipto_city = _orderHeader.shipto_city;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_contactname))
        {
            this.shipto_contactname = _orderHeader.shipto_contactname;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_country))
        {
            this.shipto_country = _orderHeader.shipto_country;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_fax))
        {
            this.shipto_fax = _orderHeader.shipto_fax;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_line1))
        {
            this.shipto_line1 = _orderHeader.shipto_line1;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_line2))
        {
            this.shipto_line2 = _orderHeader.shipto_line2;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_line3))
        {
            this.shipto_line3 = _orderHeader.shipto_line3;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_name))
        {
            this.shipto_name = _orderHeader.shipto_name;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_postalcode))
        {
            this.shipto_postalcode = _orderHeader.shipto_postalcode;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_stateorprovince))
        {
            this.shipto_stateorprovince = _orderHeader.shipto_stateorprovince;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.shipto_telephone))
        {
            this.shipto_telephone = _orderHeader.shipto_telephone;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.kti_channelurl))
        {
            this.kti_channelurl = _orderHeader.kti_channelurl;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.laz_VoucherCode))
        {
            this.kti_couponcode = _orderHeader.laz_VoucherCode;
        }

        if (!string.IsNullOrWhiteSpace(_orderHeader.kti_gifttagmessage))
        {
            this.kti_gifttagmessage = _orderHeader.kti_gifttagmessage;
        }

        MapLazadaPaymentTermToCRM(_orderHeader);

        MapLazadaStatusToCRM(_orderHeader);

        //     this.customerid = _orderHeader.emailaddress;

        //CRM.Domain.Customer customer = new(companyid);


        //var Contactid = customer.GetContactBy_Mobile(_orderHeader.billto_telephone).GetAwaiter().GetResult();

        //if (!string.IsNullOrWhiteSpace(Contactid))
        //{
        //    this.customerid = $"/contacts({Contactid})";
        //}
        //else
        //{
        //    this.customerid = _orderHeader.billto_telephone;
        //}

        //var linenumber = 1;
        //this.orderItem = _orderHeader.order_items.Select(orderitem =>
        //{
        //    orderitem.kti_lineitemnumber = linenumber.ToString();
        //    linenumber++;
        //    var OrderItemModel = new OrderItem(orderitem, companyid);

        //    OrderItemModel.kti_sourceid = this.kti_sourceid;
        //    OrderItemModel.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
        //    return OrderItemModel;

        //}).ToList();

        #endregion
    }

    [JsonProperty("kti_invoiceid")]
    public string invoicenumber { get; set; }

    private int GetPaymentMethodOctopos(string paymentmethod)
    {
        paymentmethod = paymentmethod.ToUpper();

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.CASH)
        {
            return 959_080_002;//COD
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.DISCOVER)
        {
            return 959_080_024;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.ALIPAY)
        {
            return 959_080_025;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.WECHATPAY)
        {
            return 959_080_026;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.NETS)
        {
            return 959_080_027;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.NETS_FLASHPAY)
        {
            return 959_080_028;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.NETS_CASH_CARD)
        {
            return 959_080_029;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.PAYPAL)
        {
            return 959_080_030;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.UNION_PAY)
        {
            return 959_080_031;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.DEPOSIT)
        {
            return 959_080_032;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.PREPAID_VOUCHER)
        {
            return 959_080_033;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.CREDIT_NOTES)
        {
            return 959_080_034;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.GIFT_VOUCHER)
        {
            return 959_080_035;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.JCB)
        {
            return 959_080_036;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.DINERS)
        {
            return 959_080_037;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.AMEX)
        {
            return 959_080_038;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.MASTER)
        {
            return 959_080_039;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.CHEQUE)
        {
            return 959_080_040;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.DEBIT_CARD)
        {
            return 959_080_041;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.REWARD_POINTS)
        {
            return 959_080_042;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.VISA)
        {
            return 959_080_043;
        }

        if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.PAYMAYA)
        {
            return 959_080_003;
        }

        return default;
    }

    private void MapOctopusStatusToCRM(KTI.Moo.Extensions.OctoPOS.Model.Invoice _invoiceHeader)
    {

        if (_invoiceHeader.Status == 0)
        {
            return;
        }

        if (_invoiceHeader.Status == 2)
        {
            //this.statecode = 3;
            this.statuscode = 959_080_002;
        }

        if (_invoiceHeader.Status == 1 && _invoiceHeader.OrderStatusDescription.ToUpper() == "PAYMENT RECEIVED")
        {
            this.statuscode = 959_080_003; //Complete
        }

        if (_invoiceHeader.Status == 1 && _invoiceHeader.OrderStatusDescription.ToUpper() == "DELIVERED")
        {

            this.statuscode = 959_080_003; //Complete
        }

        if (_invoiceHeader.Status == 1 && _invoiceHeader.OrderStatusDescription.ToUpper() == "PROCESSING")
        {
            this.statuscode = 959_080_000; //Processing
        }

        if (_invoiceHeader.Status == 1 && _invoiceHeader.OrderStatusDescription.ToUpper() == "READY FOR DELIVERY")
        {
            this.statuscode = 959_080_000; //Processing
        }

        if (_invoiceHeader.Status == 1 && _invoiceHeader.OrderStatusDescription.ToUpper() == "DELIVERING")
        {
            this.statuscode = 959_080_000; //Processing
        }

    }

    private void MapMagentoStatusToCRM(KTI.Moo.Extensions.Magento.Model.Order _orderHeader)
    {
        if (string.IsNullOrWhiteSpace(_orderHeader.order_status))
        {
            return;
        }

        if (_orderHeader.order_status == "pending")
        {
            this.statuscode = 2;
        }

        if (_orderHeader.order_status == "processing")
        {
            this.statuscode = 959_080_000;
        }

        if (_orderHeader.order_status == "holded")
        {
            this.statuscode = 959_080_001;
        }

        if (_orderHeader.order_status == "canceled")
        {
            this.statuscode = 959_080_002;
        }

        if (_orderHeader.order_status == "complete")
        {
            this.statuscode = 959_080_003;
        }

    }

    private void MapMagentoPaymentTermToCRM(KTI.Moo.Extensions.Magento.Model.Order _orderHeader)
    {

        if (_orderHeader.order_payment is null || string.IsNullOrWhiteSpace(_orderHeader.order_payment.method))
        {
            return;
        }

        if (_orderHeader.order_payment.method == "pnx" && _orderHeader.order_payment.additional_information is not null && _orderHeader.order_payment.additional_information.Contains("CC"))
        {
            this.kti_paymenttermscode = 959_080_015;
        }

        if (_orderHeader.order_payment.method == "pnx" && _orderHeader.order_payment.additional_information is not null && _orderHeader.order_payment.additional_information.Contains("GC"))
        {
            this.kti_paymenttermscode = 959_080_016;
        }

        //if (_orderHeader.order_payment.method == "Cash on delivery")
        //{
        //    this.kti_paymenttermscode = 959_080_002;
        //}    

        if (_orderHeader.order_payment.method == "phoenix_cashondelivery")
        {
            this.kti_paymenttermscode = 959_080_002;
        }

        if (_orderHeader.order_payment.method == "cashondelivery")
        {
            this.kti_paymenttermscode = 959_080_017;
        }

        if (_orderHeader.order_payment.method == "pnxcustom")
        {
            this.kti_paymenttermscode = 959_080_044;
        }

    }

    private void MapLazadaStatusToCRM(KTI.Moo.Extensions.Lazada.Model.OrderHeader _orderHeader)
    {
        if (string.IsNullOrWhiteSpace(_orderHeader.order_status))
        {
            return;
        }

        if (_orderHeader.order_status == "unpaid")
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

    }

    private void MapLazadaPaymentTermToCRM(KTI.Moo.Extensions.Lazada.Model.OrderHeader _orderHeader)
    {
        if (string.IsNullOrWhiteSpace(_orderHeader.laz_PaymentMethod))
        {
            return;
        }

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

        if (_orderHeader.laz_PaymentMethod.ToUpper() == "PAYMAYA")
        {
            this.kti_paymenttermscode = 959_080_003;
        }

    }
}
