using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CRM.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace CRM.Security
{
    public class Authenticate
    {
        public static CRMConfig GetCRMConfig(string company_id)
        {
            try
            {
                var clientId = ConfigurationManager.AppSettings[company_id + "_client_id"];
                var clientSecret = ConfigurationManager.AppSettings[company_id + "_client_secret"];
                var authUrl = ConfigurationManager.AppSettings[company_id + "_auth_url"];
                var resourceUrl = ConfigurationManager.AppSettings[company_id + "_resource_url"];

                if(!String.IsNullOrEmpty(clientId) && !String.IsNullOrEmpty(clientSecret) && 
                    !String.IsNullOrEmpty(authUrl) && !String.IsNullOrEmpty(resourceUrl))
                {
                    return new CRMConfig { client_id = clientId, 
                        client_secret = clientSecret, 
                        resource_url = resourceUrl, 
                        auth_url = authUrl,
                        crmbase_url = ConfigurationManager.AppSettings[company_id + "_crmbase_url"],
                        unitgroup_path = ConfigurationManager.AppSettings[company_id + "_unitgroup_path"]
                    };
                }

                throw new Exception("CRM Config Error: Empty Configuration");
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Config Error: " + ex.Message);
            }
        }

        public static async Task<string> GetCRMAccessToken(CRMConfig _crmConfig)
        {
            try
            {
                AuthenticationContext authContext = new AuthenticationContext(_crmConfig.auth_url);
                ClientCredential clientCredentials = new ClientCredential(_crmConfig.client_id, _crmConfig.client_secret);
                var authResult = await authContext.AcquireTokenAsync(_crmConfig.resource_url, clientCredentials);

                if(!String.IsNullOrEmpty(authResult.AccessToken))
                {
                    return authResult.AccessToken;
                }

                throw new Exception("D365 Token Error: Empty token result");
            }
            catch (Exception ex)
            {
                throw new Exception("D365 Connection Error: " + ex.Message);
            }

            throw new Exception("D365 Connection Error: Cannot generate access token due to wrong configuration");
        }
    }
}
