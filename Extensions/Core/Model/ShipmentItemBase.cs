using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class ShipmentItemBase
{

    public string kti_shipmentheader { get; set; }
    public string kti_lineitemnumber { get; set; }
    public string kti_quantity { get; set; }
    public string kti_weight { get; set; }
    public string kti_width { get; set; }
    public string kti_length { get; set; }
    public string kti_height { get; set; }
    public string kti_description { get; set; }
    public string kti_orderdetails { get; set; }
    public string kti_productid { get; set; }
    public string kti_salesperson { get; set; }
    public string kti_name { get; set; }
    public string kti_unit { get; set; }
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
