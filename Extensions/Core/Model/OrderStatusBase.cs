using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class OrderStatusBase
{
    public int companyid { get; set; }
    public string domainType { get; set; }
    public virtual string kti_sourcesalesorderid { get; set; }
    public virtual List<string> kti_sourcesalesorderitemids { get; set; }
    public virtual string orderstatus { get; set; }
    public virtual int kti_orderstatus { get; set; }
    public virtual string packageid { get; set; }
    public virtual string tracking_number { get; set; }
    public virtual string shipment_provider { get; set; }
    public virtual string pdf_url { get; set; }
    public virtual string cancelreason { get; set; }
    public virtual int kti_cancelreason { get; set; }
    public virtual string kti_shipmentid { get; set; }
    public virtual string kti_salesorderid { get; set; }
    public virtual string kti_shipmentitemid { get; set; }
    public virtual string kti_salesorderitemid { get; set; }
    public virtual int kti_socialchannelorigin { get; set; }
    public virtual string kti_storecode { get; set; }
    public virtual bool successful { get; set; }


}
