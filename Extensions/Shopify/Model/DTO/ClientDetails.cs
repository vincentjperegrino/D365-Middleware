using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{


    public class ClientDetails : ShopifySharp.ClientDetails
    
    { 
    

        public ClientDetails() { }

        public ClientDetails(Model.ClientDetails clientDetails) {

            AcceptLanguage = clientDetails.accept_language;
            BrowserHeight = clientDetails.browser_height;
            BrowserIp = clientDetails.browser_ip;
            BrowserWidth = clientDetails.browser_width;
            SessionHash = clientDetails.session_hash;
            UserAgent = clientDetails.user_agent;


        }


    }
}
