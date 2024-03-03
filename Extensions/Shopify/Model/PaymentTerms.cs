using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class PaymentTerms
{
    public PaymentTerms() { }

    public PaymentTerms(ShopifySharp.PaymentTerms paymentTerms) {

        amount = paymentTerms.amount ?? default;
        currency = paymentTerms.Currency;
        payment_terms_name = paymentTerms.PaymentTermsName;
        payment_terms_type = paymentTerms.PaymentTermsType;
        due_in_days = paymentTerms.DueInDays ?? default;

        payment_schedules = paymentTerms.PaymentSchedules?.Select(paymentSchedule => new PaymentSchedule(paymentSchedule)).ToList();






    }

    public decimal amount { get; set; }
    public string currency { get; set; }
    public string payment_terms_name { get; set; }
    public string payment_terms_type { get; set; }
    public int due_in_days { get; set; }
    public List<PaymentSchedule> payment_schedules { get; set; }



}