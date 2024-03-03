

using KTI.Moo.Extensions.OctoPOS.Helper;
using System;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices
{
    public class SearchParameters
    {
        public int pageno { get; set; } = 1;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string invoiceType { get; set; } = InvoiceTypeHelper.ALL;
    }
}
