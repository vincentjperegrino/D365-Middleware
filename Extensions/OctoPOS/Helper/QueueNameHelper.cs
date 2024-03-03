
namespace KTI.Moo.Extensions.OctoPOS.Helper;

public class QueueName : Core.Helper.QueueName
{
  new public static readonly string Invoice = "-octopos-invoice";
  new public static readonly string Order = "-octopos-order";
  new public static readonly string Customer = "-octopos-customer";
}
