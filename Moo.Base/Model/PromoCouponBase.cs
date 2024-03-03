using Newtonsoft.Json;

namespace KTI.Moo.Base.Model;

public class PromoCouponBase : CustomEntityBase
{
    public virtual int companyid { get; set; }
    public virtual string kti_name { get; set; }
    public virtual string kti_promocouponid { get; set; }
    public virtual string kti_uniquepromocode { get; set; }

    [JsonProperty(PropertyName = "kti_customerid@odata.bind")]
    public virtual string kti_customerid { get; set; }

    [JsonProperty(PropertyName = "kti_promocode@odata.bind")]
    public virtual string kti_promocode { get; set; }

}
