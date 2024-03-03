using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

    public class ClientDetails 
{

    public ClientDetails() { 
    
    }

    public ClientDetails(ShopifySharp.ClientDetails clientDetails) {

        accept_language = clientDetails.AcceptLanguage;
        browser_height = clientDetails.BrowserHeight ?? default;
        browser_ip = clientDetails.BrowserIp;
        browser_width = clientDetails.BrowserWidth ?? default;
        session_hash = clientDetails.SessionHash;
        user_agent = clientDetails.UserAgent;

    }

    public string accept_language { get; set; }
    public int browser_height { get; set; }
    public string browser_ip { get; set; }
    public int browser_width { get; set; }
    public string session_hash { get; set; }
    public string user_agent { get; set; }

}
