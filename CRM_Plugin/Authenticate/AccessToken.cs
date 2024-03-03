using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CRM_Plugin.Authenticate
{
    public class AccessToken
    {
        public static async Task<string> Generate(int _companyId)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Moo.Config.authurl);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("OC-Api-App-Key", Moo.Config.company_33388_occapikey);
                    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Moo.Config.company_33388_subkey);

                    //Add this line for TLS complaience
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var retrieveResponseTest = await httpClient.GetAsync(Moo.Config.auth_path);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        throw new Exception("CRM Product " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);

                    }
                    else
                    {
                        return ((JObject)JsonConvert.DeserializeObject(retrieveResponseTest.Content.ReadAsStringAsync().Result))["token"].Value<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return null;
        }
    }
}
