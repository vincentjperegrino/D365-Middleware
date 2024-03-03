using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class PaymentDetails : ShopifySharp.PaymentDetails {


        public PaymentDetails() { }

        public PaymentDetails(Model.PaymentDetails paymentDetails) {

            AvsResultCode = paymentDetails.avs_result_code;
            CreditCardBin = paymentDetails.credit_card_bin;
            CvvResultCode = paymentDetails.cvv_result_code;
            CreditCardNumber = paymentDetails.credit_card_number;
            CreditCardCompany = paymentDetails.credit_card_company;
            //CreditCardName = paymentDetails.credid_card_name
            //CreditCardExpirationMonth = paymentDetails.credit



        }
    }
}
