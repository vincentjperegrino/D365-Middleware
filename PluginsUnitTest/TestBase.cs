using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace PluginsUnitTest
{
    public class TestBase
    {

        //Create the tracing service
        public static ITracingService tracingService;

        //Create the context
        public IPluginExecutionContext context;
        public IOrganizationServiceFactory serviceFactory;
        public IOrganizationService _service;
        //public static string accessToken;
        //public static string accessToken;
        public IOrganizationService connectToCRMCreds()
        {
            IOrganizationService _service = null;
            //jfc
            string userName = "admin@britevp.onmicrosoft.com";
            string passWord = "Coroza@123";
            try
            {
                ClientCredentials clientCredentials = new ClientCredentials();
                clientCredentials.UserName.UserName = userName;
                clientCredentials.UserName.Password = passWord;

                // Copy and Paste Organization Service Endpoint Address URL
                _service = (IOrganizationService)new OrganizationServiceProxy(new Uri("https://brite.api.crm5.dynamics.com/XRMServices/2011/Organization.svc"),
                 null, clientCredentials, null);

                if (_service != null)
                {
                    Guid userid = ((WhoAmIResponse)_service.Execute(new WhoAmIRequest())).UserId;
                    if (userid != Guid.Empty)
                    {
                        Console.WriteLine("Connection Successful!...");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to Established Connection!!!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught - " + ex.Message);
            }

            return _service;
        }



        public IOrganizationService connectToCRM()
        {
            //rustans demo
            //string clientId = "21a0d9b7-7f9e-4b4a-a369-0d5d9fa9c7f8"; // Your Azure AD Application ID
            //string clientSecret = "G.oMy5XjsrAw_r4jD9.EmoFFnOm88_Xh7W"; // Client secret generated in your App
            //string organizationUri = "https://kti-moo-cs.crm5.dynamics.com/";

            //bos sandbox
            //string clientId = "85c65961-4b39-4e50-bbab-8759435a3cbd"; // Your Azure AD Application ID
            //string clientSecret = "4~9-SOu_2n9itW3ehwyx-yn~rLdJfuX0TX"; // Client secret generated in your App
            ////string organizationUri = "https://boscoffeesandbox.crm5.dynamics.com";
            //string organizationUri = "https://boscoffee.crm5.dynamics.com";

            //coreminder
            //string clientId = "e5242d88-bb22-484a-9629-c0565ae18a2f"; // Your Azure AD Application ID
            //string clientSecret = "u_2N2Cl_cN36LjrLkZln89s4BwTtS6~S9~"; // Client secret generated in your App
            //string organizationUri = "https://coreminder.crm5.dynamics.com";


            //NCCI sandbox
            string clientId = "b03c7f34-2e11-463a-b63b-653d79faf7ee"; // Your Azure AD Application ID
            string clientSecret = "tXl7Q~nj7FfZsy6CAviIKbax5J~G2JeghLMDl"; // Client secret generated in your App
                                                                           // string organizationUri = "https://nespresso-devt.crm5.dynamics.com";
            string organizationUri = "https://nespresso.crm5.dynamics.com";

            ////kti sandbox
            //string clientId = "da8c5f39-138c-4df6-b750-6b5c45d5ccc4"; // Your Azure AD Application ID
            //string clientSecret = "S.2-85MIvM7im_5_5g~8~Ej92UR2Bdzzy6"; // Client secret generated in your App
            //string organizationUri = "https://ktisalessandbox.crm5.dynamics.com";

            //santinos sandbox
            //string clientId = "b9648ffb-be28-49a0-8ea1-7ea5a5805c3e"; // Your Azure AD Application ID
            //string clientSecret = "zMP_u~Yj2i5XyIt~Nx8ZgL3-_LFs5I4f7W"; // Client secret generated in your App
            //string organizationUri = "https://santionspizza-sandbox.crm5.dynamics.com";

            //RAMPS
            //string clientId = "34a59313-f8a7-4832-af24-0010c4d22da4"; // Your Azure AD Application ID
            //string clientSecret = "HvfcWB0R8X~z_Sfa8~e2~K.vN42WG.C1K-"; // Client secret generated in your App
            //string organizationUri = "https://rampscorpprod.crm5.dynamics.com";
            //string organizationUri = "https://rampscorpsbox.crm5.dynamics.com";

            //Candy Corner
            //string clientId = "cf3754c0-2c86-4db5-9947-f5bcbd04fd20"; // Your Azure AD Application ID
            //string clientSecret = "4Qa99-TKllOj86_XBXH-36AFX4-zCe5i_1"; // Client secret generated in your App
            //string organizationUri = "https://candycorner-uat.crm5.dynamics.com";
            //string organizationUri = "https://candycornersbox.crm5.dynamics.com";

            //Brite
            //string clientId = "b56e71d6-9738-471b-a0d0-216d6defda2c"; // Your Azure AD Application ID
            //string clientSecret = "u~z.3J92KMAyGkw_LmFz1M39_pzbQfnegl"; // Client secret generated in your App
            ////string organizationUri = "https://candycornerprod.crm5.dynamics.com";
            //string organizationUri = "https://britesbox.crm5.dynamics.com";

            //Brite
            //string clientId = "14cebc01-0d1c-4e15-af4a-7950777e400e"; // Your Azure AD Application ID
            //string clientSecret = "68kWYedr-id~i1R57.G5psR4Lm60~8B-a-"; // Client secret generated in your App
            //string organizationUri = "https://pycmoosbox.crm5.dynamics.com/";

            //PYC
            //string clientId = "6579d121-0adb-4fc7-ae85-45c23a3450a5"; // Your Azure AD Application ID
            //string clientSecret = "U8-b8d6a2-48nCUl.vHEBLY5LxjQZz1.8u"; // Client secret generated in your App
            //string organizationUri = "https://taomoosbox.api.crm5.dynamics.com";

            var conn = new CrmServiceClient($@"AuthType=ClientSecret;url={organizationUri};ClientId={clientId};ClientSecret={clientSecret}");

            if (conn.OrganizationWebProxyClient != null)
            {
                Console.WriteLine($"Connected Successfully to [{conn.ConnectedOrgFriendlyName}]");
            }


            try
            {
                return conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured - " + ex.Message);
            }

            return null;
        }

        public IOrganizationService connectToCRM(int companyid)
        {           //NCCI sandbox
            string clientId = "";
            string clientSecret = "";
            string organizationUri = "";

            if (companyid == 3387)
            {

                ////kti sandbox
                clientId = "da8c5f39-138c-4df6-b750-6b5c45d5ccc4"; // Your Azure AD Application ID
                clientSecret = "S.2-85MIvM7im_5_5g~8~Ej92UR2Bdzzy6"; // Client secret generated in your App
                organizationUri = "https://ktisalessandbox.crm5.dynamics.com";
            }

            if (companyid == 3388)
            {

                //NCCI sandbox
                clientId = "b03c7f34-2e11-463a-b63b-653d79faf7ee"; // Your Azure AD Application ID
                clientSecret = "tXl7Q~nj7FfZsy6CAviIKbax5J~G2JeghLMDl"; // Client secret generated in your App
                organizationUri = "https://nespresso-devt.crm5.dynamics.com";
            }

            if (companyid == 3389)
            {

                //NCCI Prod
                clientId = "b03c7f34-2e11-463a-b63b-653d79faf7ee"; // Your Azure AD Application ID
                clientSecret = "tXl7Q~nj7FfZsy6CAviIKbax5J~G2JeghLMDl"; // Client secret generated in your App
                organizationUri = "https://nespresso.crm5.dynamics.com";
            }  
            
 
            var conn = new CrmServiceClient($@"AuthType=ClientSecret;url={organizationUri};ClientId={clientId};ClientSecret={clientSecret}");

            if (conn.OrganizationWebProxyClient != null)
            {
                Console.WriteLine($"Connected Successfully to [{conn.ConnectedOrgFriendlyName}]");
            }


            try
            {
                return conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured - " + ex.Message);
            }

            return null;
        }



        public IOrganizationService connectToCRM(string organizationUri)
        {           //NCCI sandbox
            string clientId = "";
            string clientSecret = "";



            if (organizationUri == "https://ktisalessandbox.crm5.dynamics.com")
            {
                ////kti sandbox
                clientId = "da8c5f39-138c-4df6-b750-6b5c45d5ccc4"; // Your Azure AD Application ID
                clientSecret = "S.2-85MIvM7im_5_5g~8~Ej92UR2Bdzzy6"; // Client secret generated in your App
            }

            if (organizationUri == "https://nespresso-devt.crm5.dynamics.com" || organizationUri == "https://nespresso.crm5.dynamics.com")
            {

                //NCCI sandbox
                clientId = "b03c7f34-2e11-463a-b63b-653d79faf7ee"; // Your Azure AD Application ID
                clientSecret = "tXl7Q~nj7FfZsy6CAviIKbax5J~G2JeghLMDl"; // Client secret generated in your App
               
            }

        
            var conn = new CrmServiceClient($@"AuthType=ClientSecret;url={organizationUri};ClientId={clientId};ClientSecret={clientSecret}");

            if (conn.OrganizationWebProxyClient != null)
            {
                Console.WriteLine($"Connected Successfully to [{conn.ConnectedOrgFriendlyName}]");
            }


            try
            {
                return conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured - " + ex.Message);
            }

            return null;
        }





    }
}
