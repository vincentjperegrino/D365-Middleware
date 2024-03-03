namespace Moo.BC.Model.Extension
{
    public class PaymentMethod
    {
        public string kti_sourcesalesorderid { get; set; }
        public List<TenderType> tender_type { get; set; }
    }
}
