using System.Collections.Generic;

namespace Moo.FO.App.Queue.Model
{
    public class StoreTransactions
    {
        public List<StoreTransactionsHeader> Headers { get; set; }
        public List<StoreTransactionsLine> Lines { get; set; }
        public List<StoreTransactionsDiscount> Discounts { get; set; }
        public List<StoreTransactionsPayment> Payments { get; set; }
    }

    public class StoreTransactionsHeader
    {
        public string TransactionDate { get; set; }
        public string ChannelReferenceId { get; set; }
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
    }

    public class StoreTransactionsLine
    {
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public string LineNumber { get; set; }
        public string ItemId { get; set; }
        public string NetAmount { get; set; }
        public string Quantity { get; set; }
        public string ItemSalesTaxGroup { get; set; }
    }

    public class StoreTransactionsDiscount
    {
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public string LineNumber { get; set; }
        public string DiscountAmount { get; set; }
    }

    public class StoreTransactionsPayment
    {
        public string Terminal { get; set; }
        public string TransactionNumber { get; set; }
        public string LineNumber { get; set; }
        public string AmountTendered { get; set; }
        public string TenderType { get; set; }
    }
}
