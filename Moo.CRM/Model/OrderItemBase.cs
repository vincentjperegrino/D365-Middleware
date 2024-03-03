namespace KTI.Moo.CRM.Model;
public class OrderItemBase : Base.Model.OrderItemBase
{
    public virtual int kti_socialchannelorigin { get; set; }

    public dynamic customfields { get; set; }
}
