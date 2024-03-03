
namespace KTI.Moo.CRM.Model;

public partial class OrderBase : Base.Model.OrderBase
{
    public virtual int kti_socialchannelorigin { get; set; }
    public string kti_sapdocentry { get; set; }
    public string kti_sapdocnum { get; set; }

    public DateTime createdon { get; set; }
    public DateTime modifiedon { get; set; }

    public dynamic customfields { get; set; }


}
