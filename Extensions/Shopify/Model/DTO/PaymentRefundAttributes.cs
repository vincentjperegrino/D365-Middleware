using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class PaymentRefundAttributes : ShopifySharp.PaymentsRefundAttributes {

        public PaymentRefundAttributes() { 
        }
        public PaymentRefundAttributes(Model.PaymentsRefundAttributes paymentsRefundAttributes) {

            Status = paymentsRefundAttributes.status;
            AcquirerReferenceNumber = paymentsRefundAttributes.acquirer_reference_number;



        }
    }
}
