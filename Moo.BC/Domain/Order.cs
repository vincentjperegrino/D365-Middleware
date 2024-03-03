
using KTI.Moo.BC.Model;
using Moo.BC.Model.DTO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace KTI.Moo.BC.Domain;

public class Order : Base.Domain.IOrder
{

    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public Order(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public async Task<bool> upsertWithCustomer(string content, ILogger log)
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

                    var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + $"/kti_UpsertOrder_WithCustomer", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        var result = retrieveResponseTest.Content.ReadAsStringAsync().Result;
                        
                        //var ResultModel = JsonConvert.DeserializeObject<OrderWithCustomer_Result>(result);

                        //return ResultModel.value;


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

        return false;
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

                var orderHeader = JsonConvert.DeserializeObject<OrderBase>(content);

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

                    //Get Customer if customerid = email
                    if (!orderHeader.customerid.Contains("/contacts") && !string.IsNullOrWhiteSpace(orderHeader.emailaddress))
                    {
                        //CRM.Domain.Customer customer = new(_companyId);

                        //var Contactid = customer.GetContactBy_Email(orderHeader.emailaddress).GetAwaiter().GetResult();

                        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

                        //if (string.IsNullOrWhiteSpace(Contactid))
                        //{
                        //    retryCount++;
                        //    throw new Exception("Customer not exists.");
                        //}

                        if (DomainJObject.ContainsKey("customerid_contact@odata.bind"))
                        {
                            //DomainJObject["customerid_contact@odata.bind"] = $"/contacts({Contactid})";

                            content = JsonConvert.SerializeObject(DomainJObject);

                        }

                    }
                    else if (!orderHeader.customerid.Contains("/contacts") && !string.IsNullOrWhiteSpace(orderHeader.billto_telephone))
                    {
                        //CRM.Domain.Customer customer = new(_companyId);

                        //var Contactid = customer.GetContactBy_Mobile(orderHeader.billto_telephone).GetAwaiter().GetResult();

                        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

                        //if (string.IsNullOrWhiteSpace(Contactid))
                        //{
                        //    retryCount++;
                        //    throw new Exception("Customer not exists.");
                        //}

                        if (DomainJObject.ContainsKey("customerid_contact@odata.bind"))
                        {
                            //DomainJObject["customerid_contact@odata.bind"] = $"/contacts({Contactid})";

                            content = JsonConvert.SerializeObject(DomainJObject);

                        }

                    }

                    var _content = new StringContent(content, Encoding.UTF8, "application/json");

                    //var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + $"(kti_sourceid='{orderHeader.kti_sourceid}',kti_socialchannelorigin={orderHeader.kti_socialchannelorigin})", _content);
                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + $"(kti_sourceid='{orderHeader.kti_sourceid}')", _content);

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

    public async Task<string> upsert(string content, string id, ILogger log)
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

                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + $"({id})", _content);

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
