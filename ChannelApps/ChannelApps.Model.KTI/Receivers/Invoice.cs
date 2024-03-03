namespace KTI.Moo.ChannelApps.Model.KTIdev.Receivers;

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

            if (this.customerid.Contains("/contacts"))
            {
                InvoiceItemModel.ncci_Customer = this.customerid;
            }

            //InvoiceItemModel.ncci_Salesman = $"/ncci_salesmans(ncci_code='ECOM')";
            //InvoiceItemModel.ncci_Branch = $"/kti_branchs(kti_branchcode='EC001')";

            return InvoiceItemModel;

        }).ToList();

        #endregion

    }



    public Invoice(KTI.Moo.Extensions.OctoPOS.Model.Invoice _invoiceHeader)
    {
        #region properties
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
        this.discountamount = _invoiceHeader.discountamount;
        //  this.invoicedate = default;
        this.salesorderid = _invoiceHeader.salesorderid;

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

        //if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.VISA)
        //{
        //    this.kti_paymentmethod = 959_080_015;
        //}

        //if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.WECHATPAY)
        //{
        //    this.kti_paymentmethod = 959_080_016;
        //}

        //if (paymentmethod == KTI.Moo.Extensions.OctoPOS.Helper.PaymentModeHelper.VISA)
        //{
        //    this.kti_paymentmethod = 959_080_017;
        //}



        this.kti_invoicedate = _invoiceHeader.invoicedate;
        //this.coffeeqty = _invoiceHeader.coffeeqty;
        //this.machineqty = _invoiceHeader.machineqty;
        //this.coffee = _invoiceHeader.coffee;
        //this.noncoffee = _invoiceHeader.noncoffee;

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

            //External Mapping of Invoice Items
            InvoiceItemModel.ncci_Customer = this.customerid;
            InvoiceItemModel.ncci_Branch = $"/kti_branchs(kti_branchcode='{ _invoiceHeader.Location}')"; ;

            return InvoiceItemModel;
        }).ToList();
        #endregion

    }

    #region NCCI properties
    public DateTime kti_invoicedate { get; set; }

    public int kti_sapdocnum { get; set; }
    public int kti_sapdocentry { get; set; }

    public List<InvoiceItem> invoiceItem { get; set; }
    #endregion

}
