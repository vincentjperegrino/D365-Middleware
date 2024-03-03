using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.ChannelApps.Model.NCCI.Receivers;

public class Invoice : CRM.Model.InvoiceBase
{

    public Invoice()
    {

    }


    public Invoice(KTI.Moo.Extensions.Magento.Model.Invoice _invoiceHeader)
    {
        #region properties
        this.companyid = _invoiceHeader.companyid;
        this.name = _invoiceHeader.increment_id;
        this.billto_line1 = _invoiceHeader.billto_line1;
        this.description = _invoiceHeader.description;
        this.emailaddress = _invoiceHeader.emailaddress;
        this.freightamount = _invoiceHeader.freightamount;
        this.invoicenumber = _invoiceHeader.invoicenumber;
        this.shipto_line1 = _invoiceHeader.billto_line1;
        this.totaltax = _invoiceHeader.totaltax;
        //this.invoicedate = default;


        this.kti_invoicedate = _invoiceHeader.created_at;
        this.discountamount = _invoiceHeader.discountamount;
        //this.coffeeqty = _invoiceHeader.coffeeqty;
        //this.machineqty = _invoiceHeader.machineqty;
        //this.coffee = _invoiceHeader.coffee;
        //this.noncoffee = _invoiceHeader.noncoffee;

        this.kti_sourceid = _invoiceHeader.kti_sourceid;
        var MAGENTOchannelorigin = 959080010;
        this.kti_socialchannelorigin = MAGENTOchannelorigin;

        this.pricelevelid = "/pricelevels(name = 'Standard')";

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.salesorderid))
        {
            this.salesorderid = $"/salesorders(kti_sourceid='{_invoiceHeader.salesorderid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
        }
        else
        {
            this.salesorderid = _invoiceHeader.salesorderid;
        }



        CRM.Domain.Customer customer = new(companyid);

        var Contactid = customer.GetContactBy_Email(_invoiceHeader.CustomerDetails.email).GetAwaiter().GetResult();

        if (!string.IsNullOrWhiteSpace(Contactid))
        {
            this.customerid = $"/contacts({Contactid})";
        }
        else
        {
            this.customerid = _invoiceHeader.CustomerDetails.email;
        }


        var linenum = 1;
        this.invoiceItem = _invoiceHeader.invoiceItems.Select(invoiceitem =>
        {
            invoiceitem.kti_lineitemnumber = linenum.ToString();
            linenum++;

            var InvoiceItemModel = new InvoiceItem(invoiceitem, companyid);

            //if (this.customerid.Contains("/contacts"))
            //{
            //    InvoiceItemModel.ncci_Customer = this.customerid;
            //}

            //InvoiceItemModel.ncci_Salesman = $"/ncci_salesmans(ncci_code='ECOM')";
            //InvoiceItemModel.ncci_Branch = $"/kti_branchs(kti_branchcode='EC001')";
            InvoiceItemModel.kti_sourceid = this.kti_sourceid;
            InvoiceItemModel.invoiceid = $"/invoices(kti_sourceid='{this.invoiceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
            return InvoiceItemModel;
        }).ToList();

        #endregion

    }



    public Invoice(KTI.Moo.Extensions.OctoPOS.Model.Invoice _invoiceHeader)
    {

        #region properties
        this.domainType = KTI.Moo.Base.Helpers.DomainType.order;
        this.companyid = _invoiceHeader.companyid;
        this.name = _invoiceHeader.name;
        this.billto_line1 = _invoiceHeader.CustomerDetails.Address1;
        this.description = _invoiceHeader.description;
        this.emailaddress = _invoiceHeader.CustomerEmail;
        this.freightamount = _invoiceHeader.freightamount;
        this.invoicenumber = _invoiceHeader.invoicenumber;
        this.shipto_line1 = _invoiceHeader.CustomerDetails.ShippingAddress;
        this.totalamount = _invoiceHeader.totalamount;
        this.totaltax = _invoiceHeader.totaltax;
        //this.discountamount = _invoiceHeader.discountamount; // Octopos gets total discounts
        //  this.invoicedate = default;

        if (!string.IsNullOrWhiteSpace(_invoiceHeader.salesorderid))
        {
            this.salesorderid = $"/salesorders(kti_sourceid='{_invoiceHeader.salesorderid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
        }
        else
        {
            this.salesorderid = _invoiceHeader.salesorderid;
        }

        CRM.Domain.Customer customer = new(companyid);

        var Contactid = customer.GetContactBy_Email(_invoiceHeader.CustomerEmail).GetAwaiter().GetResult();

        if (!string.IsNullOrWhiteSpace(Contactid))
        {
            this.customerid = $"/contacts({Contactid})";
        }
        else
        {
            this.customerid = _invoiceHeader.CustomerEmail;
        }

        var paymentmethod = _invoiceHeader.PaymentItems.FirstOrDefault().PaymentMode;

        this.kti_paymentmethod = GetPaymentMethodOctopos(paymentmethod);

        //  this.kti_invoicedate = _invoiceHeader.invoicedate;
        this.overriddencreatedon = _invoiceHeader.invoicedate;


        if (_invoiceHeader.Status == 2)
        {
            //this.statecode = 3;
            this.statuscode = 100003;
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


        //this.coffeeqty = _invoiceHeader.coffeeqty;
        //this.machineqty = _invoiceHeader.machineqty;
        //this.coffee = _invoiceHeader.coffee;
        //this.noncoffee = _invoiceHeader.noncoffee;

        this.branch_assigned = $"/kti_branchs(kti_branchcode='{_invoiceHeader.Location}')";

        this.kti_sourceid = _invoiceHeader.kti_sourceid;
        var OCTOPOSchannelorigin = 959080011;
        this.kti_socialchannelorigin = OCTOPOSchannelorigin;

        var linenum = 1;
        this.invoiceItem = _invoiceHeader.InvoiceItems.Select(invoiceitem =>
        {
            invoiceitem.lineitemnumber = linenum;
            invoiceitem.kti_lineitemnumber = linenum.ToString();
            linenum++;

            var InvoiceItemModel = new InvoiceItem(invoiceitem, companyid);

            InvoiceItemModel.kti_sourceid = this.kti_sourceid;
            InvoiceItemModel.salesorderid = $"/salesorders(kti_sourceid='{this.kti_sourceid}',kti_socialchannelorigin={this.kti_socialchannelorigin})";
            //External Mapping of Invoice Items
           // InvoiceItemModel.ncci_CustomerOrdProd = this.customerid;
            InvoiceItemModel.kti_Branch = $"/kti_branchs(kti_branchcode='{_invoiceHeader.Location}')";

            return InvoiceItemModel;
        }).ToList();
        #endregion

    }

    #region NCCI properties
    public DateTime kti_invoicedate { get; set; }

    public int kti_sapdocnum { get; set; }
    public int kti_sapdocentry { get; set; }

    [JsonProperty("kti_BranchAssigned@odata.bind")]

    public string branch_assigned { get; set; }
    [JsonProperty("orderItem")]
    public List<InvoiceItem> invoiceItem { get; set; }

    [JsonProperty("kti_invoiceid")]
    public override string invoicenumber { get; set; }

    [JsonProperty("kti_paymenttermscode")]
    public override int kti_paymentmethod { get; set; }


    #endregion

    public int GetPaymentMethodOctopos(string paymentmethod)
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
}
