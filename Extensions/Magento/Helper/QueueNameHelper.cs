namespace KTI.Moo.Extensions.Magento.Helper;

public class QueueName : Core.Helper.QueueName
{
    new public static readonly string Invoice = "-magento-invoice";
    new public static readonly string Order = "-magento-order";
    new public static readonly string Customer = "-magento-customer";

}
