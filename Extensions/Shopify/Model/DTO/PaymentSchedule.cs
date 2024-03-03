using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class PaymentSchedule : ShopifySharp.PaymentSchedule {


        public PaymentSchedule() { }

        public PaymentSchedule(Model.PaymentSchedule paymentSchedule) {

            amount = paymentSchedule.amount;
            Currency = paymentSchedule.currency;
            if (paymentSchedule.issued_at != default) 
            
            {
                IssuedAt = paymentSchedule.issued_at;
            }
            if (paymentSchedule.due_at != default)

            {
                DueAt = paymentSchedule.due_at;
            }
            if (paymentSchedule.completed_at != default)

            {
                CompletedAt = paymentSchedule.completed_at;
            }
            ExpectedPaymentMethod = paymentSchedule.expected_payment_method;


        }

    }
}
