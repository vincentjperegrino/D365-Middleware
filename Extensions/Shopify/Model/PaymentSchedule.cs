using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class PaymentSchedule {

    public PaymentSchedule() { }

    public PaymentSchedule(ShopifySharp.PaymentSchedule paymentSchedule) {

        amount = paymentSchedule.amount ?? default;
        currency = paymentSchedule.Currency;
        issued_at = paymentSchedule.IssuedAt ?? default;
        due_at = paymentSchedule.DueAt ?? default;
        completed_at = paymentSchedule.CompletedAt ?? default;
        expected_payment_method = paymentSchedule.ExpectedPaymentMethod;
    
    }

    public decimal amount { get; set; }
    public string currency { get; set; }
    public DateTimeOffset issued_at { get; set; }
    public DateTimeOffset due_at { get; set; }
    public DateTimeOffset completed_at { get; set; }
    public string expected_payment_method { get; set; }


}