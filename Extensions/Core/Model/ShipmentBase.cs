using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class ShipmentBase
{
    public string kti_shipmentid { get; set; }
    public DateTime kti_actualdeliverydate { get; set; }
    public string kti_customer { get; set; }
    public string kti_description { get; set; }
    public DateTime kti_estimateddeliverydate { get; set; }
    public decimal kti_freightamount { get; set; }
    public decimal kti_heighttotal { get; set; }
    public decimal kti_lengthtotal { get; set; }
    public int kti_paymentmethod { get; set; }
    public string kti_salesorder { get; set; }
    public string kti_shipment_id { get; set; }
    public string kti_shipmentmethod { get; set; }
    public string kti_shipmentprovidertrackingnumber { get; set; }
    public int kti_shipment_status { get; set; }
    public string kti_shipto_city { get; set; }
    public string kti_shipto_contactname { get; set; }
    public string kti_shipto_country { get; set; }
    public string kti_shipto_fax { get; set; }
    public string kti_shipto_line1 { get; set; }
    public string kti_shipto_line2 { get; set; }
    public string kti_shipto_line3 { get; set; }
    public string kti_shipto_name { get; set; }
    public string kti_shipto_postalcode { get; set; }
    public string kti_shipto_stateorprovince { get; set; }
    public string kti_shipto_telephone { get; set; }
    public decimal kti_totalamount { get; set; }
    public decimal kti_weighttotal { get; set; }
    public decimal kti_widthtotal { get; set; }
    public string kti_name { get; set; }
    public string kti_latitude { get; set; }
    public string kti_longitude { get; set; }
    public int kti_shipmenttype { get; set; }
    public string kti_location { get; set; }
    public int kti_delivery_method { get; set; }
    public bool kti_paid { get; set; }
    public DateTime CreatedOn { get; set; }
    public string kti_shipmentprovider { get; set; }
    public string kti_awb_url_link { get; set; }
    public string kti_packageid { get; set; }

    public virtual string createdby { get; set; }
    public virtual DateTime createdon { get; set; }
    public virtual string createdonbehalfby { get; set; }
    public virtual int importsequencenumber { get; set; }
    public virtual string modifiedby { get; set; }
    public virtual DateTime modifiedon { get; set; }
    public virtual string modifiedonbehalfby { get; set; }
    public virtual DateTime overriddencreatedon { get; set; }
    public virtual string ownerid { get; set; }
    public virtual string owningbusinessunit { get; set; }
    public virtual string owningteam { get; set; }
    public virtual string owninguser { get; set; }
    public virtual int statecode { get; set; }
    public virtual int statuscode { get; set; }
}
