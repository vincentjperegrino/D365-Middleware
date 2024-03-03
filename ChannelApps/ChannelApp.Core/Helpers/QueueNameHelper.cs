

namespace KTI.Moo.ChannelApps.Core.Helpers;

public class QueueName
{

    //public static readonly string Invoice = "-crm-invoice";
    //public static readonly string Order = "-crm-order";
    //public static readonly string Customer = "-crm-customer";

    // change to 1 queue only
    // DTO will delegate
    public static readonly string Invoice = "-crm";
    public static readonly string Order = "-crm";
    public static readonly string Customer = "-crm";

    public static readonly string InvoiceBatch = "-crm-batch";
    public static readonly string OrderBatch = "-crm-order-batch";
    public static readonly string CustomerBatch = "-crm-batch";

    public static readonly string BC_Order = "-bc";

    public static readonly string BC_OrderBatch = "-bc-order-batch";
}
