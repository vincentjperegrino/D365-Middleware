namespace KTI.Moo.Base.Model;

public class OrderStatusBase
{

    public virtual int companyid { get; set; }
    public virtual string domainType { get; init; } = Helpers.DomainType.orderstatus;

    public virtual string kti_salesorderid { get; set; }
    public virtual string orderstatus { get; set; }



}
