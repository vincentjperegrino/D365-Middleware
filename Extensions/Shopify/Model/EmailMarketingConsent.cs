
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class EmailMarketingConsent
{
    public EmailMarketingConsent()
    {

    }
    public EmailMarketingConsent(ShopifySharp.CustomerEmailMarketingConsent emailMarketingConsent)
    {
        State = emailMarketingConsent.State;
        OptInLevel = emailMarketingConsent.OptInLevel;
        ConsentUpdatedAt = emailMarketingConsent.ConsentUpdatedAt?.DateTime ?? default;

    }


    public string State { get; set; }
    public string OptInLevel { get; set; }
    public DateTime ConsentUpdatedAt { get; set; }




}
