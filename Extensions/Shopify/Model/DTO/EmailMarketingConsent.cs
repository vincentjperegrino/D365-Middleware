using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO;

public class EmailMarketingConsent : ShopifySharp.CustomerEmailMarketingConsent
{
    public EmailMarketingConsent()
    {
    }

    public EmailMarketingConsent(Model.EmailMarketingConsent emailMarketingConsent)
    {
        
        
        State = emailMarketingConsent.State;
        

        OptInLevel = emailMarketingConsent.OptInLevel;

        if (emailMarketingConsent.ConsentUpdatedAt != default)
        {
            ConsentUpdatedAt = emailMarketingConsent.ConsentUpdatedAt;
        }
    

    }

}
