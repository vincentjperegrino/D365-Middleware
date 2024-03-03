using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moo.BC.Interface;
using Moo.BC.Security.Attributes;
//using Microsoft.Identity.Client;

namespace Moo.BC.App.Security
{
    /// <summary>
    /// Implements Authentication class for Bearer Authorization Type
    /// </summary>
    public class Authentication : IAuthentication
    {
        private const string AUTH_CONFIG_NAME = "AuthenticationConfig";
        // Azure AD registrations:
        // Specifies the Azure AD tenant ID
        const string AadTenantId = "<mytenant.onmicrosoft.com>";
        // Specifies the Application (client) ID of the console application registration in Azure AD
        const string ClientId = "<7b235fb6-c01b-47c5-afeb-cbb165abeaf7>";
        // Specifies the redirect URL for the client that was configured for console application registration in Azure AD
        const string ClientRedirectUrl = "<https://mybcclient>";
        // Specifies the APP ID URI that is configured for the registered Business Central application in Azure AD
        const string ServerAppIdUri = "<https://mytenant.onmicrosoft.com/91ce5ad2-c339-46b3-831f-67e43c4c6abd>";

        public async Task<AccessToken> ValidToken(HttpRequest request)
        {
            // Check if we can decode the header.
            try
            {
                // Get access token from Azure AD. This will show the login dialog.
                // Get access token from Azure AD. This will show the login dialog.
                //var client = PublicClientApplicationBuilder.Create(ClientId)
                //.WithAuthority("https://login.microsoftonline.com/" + AadTenantId, false)
                //.WithRedirectUri(ClientRedirectUrl)
                //.Build();
                //AuthenticationResult authenticationResult = client.AcquireTokenInteractive(new string[] { $"{ServerAppIdUri}/.default" }).ExecuteAsync().GetAwaiter().GetResult();
                //// Connect to the Business Central OData web service and display a list of customers
                //var nav = new NAV.NAV(new Uri(<"https://localhost:7048/BC/ODataV4/Company('CRONUS%20International%20Ltd.'>)"));
                //nav.BuildingRequest += (sender, eventArgs) => eventArgs.Headers.Add("Authorization", authenticationResult.CreateAuthorizationHeader());

                //// Retrieve and return a list of the customers 
                //foreach (var customer in nav.Customer)
                //{
                //    Console.WriteLine("Found customer: " + customer.Name);
                //}
                //Console.ReadLine();


                return AccessToken.Success(3382);
            }
            catch (Exception exception)
            {
                return AccessToken.Error(exception);
            }
        }

    }
}
