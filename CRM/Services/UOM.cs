using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CRM.Services
{
    public class UOM
    {
        private CRM.Models.CRMConfig crmConfig;
        private string accessToken = null;

        public UOM(string _companyId)
        {
            crmConfig = CRM.Security.Authenticate.GetCRMConfig(_companyId);
            accessToken = CRM.Security.Authenticate.GetCRMAccessToken(crmConfig).GetAwaiter().GetResult();
        }

        public List<CRM.Models.UOM.UnitGroup> GetUnitGroups(string _filter)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(crmConfig.crmbase_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    //Add this line for TLS complaience
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var retrieveResponseTest = httpClient.GetAsync(crmConfig.unitgroup_path + _filter).Result;

                    if (retrieveResponseTest.IsSuccessStatusCode)
                    {
                        var jRetrieveResponse = JObject.Parse(retrieveResponseTest.Content.ReadAsStringAsync().Result);
                        List<CRM.Models.UOM.UnitGroup> listUnitGroup = JsonConvert.DeserializeObject<List<CRM.Models.UOM.UnitGroup>>(jRetrieveResponse.ToString());

                        return listUnitGroup;
                    }
                    else
                    {
                        throw new Exception("CRM Unit Group " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Unit Group: " + ex.Message);
            }
        }

        public CRM.Models.UOM.UnitGroup CreateUnitGroup(string _filter)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(crmConfig.crmbase_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    //Add this line for TLS complaience
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var retrieveResponseTest = httpClient.GetAsync(crmConfig.unitgroup_path + _filter).Result;

                    if (retrieveResponseTest.IsSuccessStatusCode)
                    {
                        var jRetrieveResponse = JObject.Parse(retrieveResponseTest.Content.ReadAsStringAsync().Result);
                        CRM.Models.UOM.UnitGroup unitGroup = JsonConvert.DeserializeObject<CRM.Models.UOM.UnitGroup>(jRetrieveResponse.ToString());

                        return unitGroup;
                    }
                    else
                    {
                        throw new Exception("CRM Unit Group " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Unit Group: " + ex.Message);
            }
        }
    }
}
