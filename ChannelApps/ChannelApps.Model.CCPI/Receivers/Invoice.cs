namespace KTI.Moo.ChannelApps.Model.CCPI.Receivers;

public class Invoice : CRM.Model.InvoiceBase
{

    public Invoice()
    {

    }

    #region NCCI properties
    public DateTime kti_invoicedate { get; set; }

    public int kti_sapdocnum { get; set; }
    public int kti_sapdocentry { get; set; }

    public List<InvoiceItem> invoiceItem { get; set; }
    #endregion

}
