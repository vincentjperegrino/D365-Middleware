

using Newtonsoft.Json;
using System;

namespace KTI.Moo.Extensions.Core.Model;

public class PromoCouponBase
{
    public virtual int companyid { get; set; }
    public virtual string kti_name { get; set; }
    public virtual string kti_promocouponid { get; set; }
    public virtual string kti_uniquepromocode { get; set; }

    [JsonProperty(PropertyName = "kti_customerid@odata.bind")]
    public virtual string kti_customerid { get; set; }

    [JsonProperty(PropertyName = "kti_promocode@odata.bind")]
    public virtual string kti_promocode { get; set; }

    //default fields
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
