using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain;

public class OrderDetails : Base.Domain.IOrderDetails
{
    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public OrderDetails(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }


    public async Task<string> upsert(string content, ILogger log)
    {
        int retryCount = 1;
        string crmResponse = null;
        string mooResponse = null;

        while (retryCount <= _retryCounter)
        {
            try
            {
                var orderDetail = JsonConvert.DeserializeObject<Model.OrderItemBase>(content);

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

                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorderdetail_path + $"(kti_sourceid='{orderDetail.kti_sourceid}',kti_socialchannelorigin={orderDetail.kti_socialchannelorigin},kti_lineitemnumber='{orderDetail.kti_lineitemnumber}')", _content);
                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Order: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Order " + ex.Message);
                }
            }
        }

        return null;
    }





}
