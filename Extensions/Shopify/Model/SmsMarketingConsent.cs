using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model
{
    public class SmsMarketingConsent
    {

        public SmsMarketingConsent()
        {

        }


        public SmsMarketingConsent(ShopifySharp.CustomerSmsMarketingConsent smsmarketingconsent)
        {
            State = smsmarketingconsent.State;
            OptInLevel = smsmarketingconsent.OptInLevel;
            ConsentUpdatedAt = smsmarketingconsent.ConsentUpdatedAt?.DateTime ?? default;
            ConsentCollectedFrom = smsmarketingconsent.ConsentCollectedFrom;

        }

        public string State { get; set; }
        public string OptInLevel { get; set; }
        public DateTime ConsentUpdatedAt { get; set; }
        public string ConsentCollectedFrom { get; set; }

    }
}

