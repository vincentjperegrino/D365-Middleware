using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.Core.Helper.EntityHelper
{
    public class PaymentTransaction : BaseEntity
    {
        new public static string entity_name = "kti_paymenttransaction";
        new public static string entity_id = "kti_transactionid";

        public static string salesorderid = "kti_order";
        public static string amount = "kti_amount";
        public static string paymentstatus = "kti_paymentstatus";

        public static ColumnSet PaymentTransactionColumns = new ColumnSet(amount);

    }
}
