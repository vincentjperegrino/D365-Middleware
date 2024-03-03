using Newtonsoft.Json;

namespace KTI.Moo.Base.Model;

public class PromoBase : CustomEntityBase
{
    // public string kti_saleschannel { get; set; }
    public virtual int companyid { get; set; }
    public virtual string kti_name { get; set; }
    public virtual string kti_promoid { get; set; }

    public virtual string kti_channelid { get; set; }
    public virtual string kti_promocode { get; set; }
    public virtual decimal kti_discountamount { get; set; }
    public virtual decimal kti_discountpercent { get; set; }
    public virtual bool kti_freedelivery { get; set; }
    public virtual bool kti_ismaxdiscountamount { get; set; }
    public virtual decimal kti_maxdiscountamount { get; set; }
    public virtual decimal kti_minspent { get; set; }
    public virtual decimal kti_minquantity { get; set; }


    //[JsonProperty(PropertyName = "kti_promo@odata.bind")]
    //public virtual string kti_promo { get; set; }
    public virtual DateTime kti_validfrom { get; set; }
    public virtual DateTime kti_validto { get; set; }

}
