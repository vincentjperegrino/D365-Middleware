using KTI.Moo.Base.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain;

public class Shipment : IShipment
{

    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public Shipment(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public async Task<string> update(string content, string id, ILogger log)
    {
        int retryCount = 1;
        string crmResponse = null;
        string mooResponse = null;

        while (retryCount <= _retryCounter)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_crmConfig.crm_base_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    //Add this line for TLS complaience
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var _content = new StringContent(content, Encoding.UTF8, "application/json");

                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + "/kti_shipments" + $"({id})", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        //Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        string error = retrieveResponseTest.Content.ReadAsStringAsync().Result;
                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Shipment " + retrieveResponseTest.StatusCode + ": " + error);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Shipment: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Shipment " + ex.Message);
                }
            }
        }

        return string.Empty;
    }

    public Task<string> upsert(string content, ILogger log)
    {
        throw new NotImplementedException();
    }
}
