using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class PaymentDetails {

    public PaymentDetails() { }

    public PaymentDetails(ShopifySharp.PaymentDetails paymentDetails) {

        avs_result_code = paymentDetails.AvsResultCode;
        credit_card_bin = paymentDetails.CreditCardBin;
        cvv_result_code = paymentDetails.CvvResultCode;
        credit_card_number = paymentDetails.CreditCardNumber;
        credit_card_company = paymentDetails.CreditCardCompany;
    
    }

    public string avs_result_code { get; set; }
    public string credit_card_bin { get; set; }
    public string cvv_result_code { get; set; }
    public string credit_card_number { get; set; }
    public string credit_card_company { get; set; }


}