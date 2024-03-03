using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Model;

public class ShipmentBase : CustomEntityBase
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
    public string kti_shipmentprovider { get; set; }
    public string kti_awb_url_link { get; set; }
    public string kti_packageid { get; set; }

}
