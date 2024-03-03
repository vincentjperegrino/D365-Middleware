using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class SmsMarketingConsent : ShopifySharp.CustomerSmsMarketingConsent
    {
        public SmsMarketingConsent()
        {

        }

        public SmsMarketingConsent(Model.SmsMarketingConsent smsmarketingconsent)
        {
            
            if (smsmarketingconsent.State != null)
            {
                State = smsmarketingconsent.State;
            }

            OptInLevel = smsmarketingconsent.OptInLevel;

            if (smsmarketingconsent.ConsentUpdatedAt != default)
            {
                ConsentUpdatedAt = smsmarketingconsent.ConsentUpdatedAt;
            }
            ConsentCollectedFrom = smsmarketingconsent.ConsentCollectedFrom;
        
        }

    }

}
