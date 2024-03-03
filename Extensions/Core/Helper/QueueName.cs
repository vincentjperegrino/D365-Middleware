namespace KTI.Moo.Extensions.Core.Helper;

public class QueueName
{
    public static readonly string Invoice = "{companyid}-{channel}-invoice";
    public static readonly string Order = "{companyid}-{channel}-order";
    public static readonly string Customer = "{companyid}-{channel}-customer";
}
