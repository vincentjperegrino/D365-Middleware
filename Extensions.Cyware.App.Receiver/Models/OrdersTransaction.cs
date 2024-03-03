using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class OrdersTransaction
    {
        public FOSalesTransactionHeader Header { get; set; }
        public List<FOSalesTransactionDetail> Details { get; set; }
        public List<FOSalesTenderDetail> Tenders { get; set; }
        public List<FOSalesTransactionDiscount> Discounts { get; set; }
        public D365FOConfig Config { get; set; }
    }
}
