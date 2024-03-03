using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class PaymentTerms : ShopifySharp.PaymentTerms {


        public PaymentTerms() { }

        public PaymentTerms(Model.PaymentTerms paymentTerms) {

            amount = paymentTerms.amount;
            Currency = paymentTerms.currency;
            DueInDays = paymentTerms.due_in_days;
            PaymentTermsName = paymentTerms.payment_terms_name;
            PaymentTermsType = paymentTerms.payment_terms_type;
            PaymentSchedules = (ShopifySharp.PaymentSchedule[]?)(paymentTerms.payment_schedules?.Select(paymentSchedule => new PaymentSchedule(paymentSchedule)));

        } 

    }
}
