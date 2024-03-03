using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class PaymentsRefundAttributes { 

    public PaymentsRefundAttributes()
    {


    }

    public PaymentsRefundAttributes(ShopifySharp.PaymentsRefundAttributes paymentsRefundAttributes) { 

        status = paymentsRefundAttributes.Status;
        acquirer_reference_number = paymentsRefundAttributes.AcquirerReferenceNumber;
    
    }


    public string status { get; set; }
    public string acquirer_reference_number { get; set; }




}