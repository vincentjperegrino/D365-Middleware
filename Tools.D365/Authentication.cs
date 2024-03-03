using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;

namespace Tools.D365
{
    /// <summary>  
    /// Manages user authentication with the Dynamics CRM Web API (OData v4) services. This class uses Microsoft Azure  
    /// Active Directory Authentication Library (ADAL v2) to handle the OAuth 2.0 protocol.   
    /// </summary>  
    public class Authentication
    {
        string[] scopes = new string[] { "https://ktisalessandbox.crm5.dynamics.com/.default" };
        Microsoft.Identity.Client.IConfidentialClientApplication app;
        /// <summary>  
        /// Base constructor.  
        /// </summary>  
        public Authentication() 
        {
            Uri authorityUri = new("https://login.microsoftonline.com/705b9777-fb96-49cb-b57a-9a8fe00addad");
            app = Microsoft.Identity.Client.ConfidentialClientApplicationBuilder
                .Create("da8c5f39-138c-4df6-b750-6b5c45d5ccc4")
                .WithClientSecret("S.2-85MIvM7im_5_5g~8~Ej92UR2Bdzzy6")
                .WithAuthority(authorityUri)
                .Build();

            //var appSecretCredentials = new Microsoft.Rest.TokenCredentials(result.AccessToken, result.TokenType);
        }

        public Microsoft.Identity.Client.AuthenticationResult GetToken()
        {
            return app.AcquireTokenForClient(scopes)
                .ExecuteAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}