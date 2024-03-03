using Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;

namespace KTI.Moo.FO.App.Helper
{
    public class FOConfig
    {
        private KTIDOMAIN.Models.FOConfig _foConfig;
        private string _accessToken;
        private List<AccountConfig> _listAccountConfig;
        private int _companyId = 0;
        private int _retryCounter = 0;

        public FOConfig(int companyId)
        {
            _companyId = companyId;
            _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
            _foConfig = KTIDOMAIN.Security.Authenticate.GetFOConfig(_listAccountConfig);
            _accessToken = KTIDOMAIN.Security.Authenticate.GetFOAccessToken(_foConfig).GetAwaiter().GetResult();
            _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
        }

        public async Task<D365FOConfig> GetAllConfig(ILogger log)
        {
            D365FOConfig config = new()
            {
                RetailStores = await GetRetailStores(log),
                Warehouses = await GetWarehouses(log),
                Customers = await GetCustomers(log)
            };

            return config;
        }

        public async Task<List<object>> GetRetailStores(ILogger log)
        {
            int retryCount = 1;
            while (retryCount <= _retryCounter)
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                        httpClient.Timeout = new TimeSpan(0, 2, 0);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        //Add this line for TLS compliance
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        UriBuilder builder = new UriBuilder(httpClient.BaseAddress);
                        HttpResponseMessage retrieveResponse = await httpClient.GetAsync("/data/RetailStores?cross-company=true");

                        if (!retrieveResponse.IsSuccessStatusCode)
                        {
                            retryCount++;

                            if (retryCount > _retryCounter)
                            {
                                throw new Exception("Retail Stores FO Config " + retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                            }

                        }
                        else
                        {
                            var valueObject = ODataHelper.ExtractValueObject(retrieveResponse.Content.ReadAsStringAsync().Result);

                            return JsonConvert.DeserializeObject<List<object>>(valueObject);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogInformation("Retail Stores FO Config: " + ex.Message);

                    if (retryCount > _retryCounter)
                    {
                        log.LogError("Retail Stores FO Config " + ex.Message);
                        throw new Exception("Retail Stores FO Config " + ex.Message);
                    }
                }
            }
            return null;
        }

        public async Task<List<object>> GetWarehouses(ILogger log)
        {
            int retryCount = 1;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    //Add this line for TLS compliance
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    UriBuilder builder = new UriBuilder(httpClient.BaseAddress);
                    HttpResponseMessage retrieveResponse = await httpClient.GetAsync("/data/Warehouses?cross-company=true");

                    if (!retrieveResponse.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("Warehouses FO Config " + retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                        }

                    }
                    else
                    {
                        var valueObject = ODataHelper.ExtractValueObject(retrieveResponse.Content.ReadAsStringAsync().Result);

                        return JsonConvert.DeserializeObject<List<object>>(valueObject);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("Warehouses FO Config: " + ex.Message);

                if (retryCount > _retryCounter)
                    log.LogError("Warehouses FO Config " + ex.Message);
            }
            return null;
        }

        public async Task<List<object>> GetCustomers(ILogger log)
        {
            int retryCount = 1;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    //Add this line for TLS compliance
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    UriBuilder builder = new UriBuilder(httpClient.BaseAddress);
                    HttpResponseMessage retrieveResponse = await httpClient.GetAsync("/data/CustomersV3?cross-company=true");

                    if (!retrieveResponse.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("Customers FO Config " + retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                        }

                    }
                    else
                    {
                        var valueObject = ODataHelper.ExtractValueObject(retrieveResponse.Content.ReadAsStringAsync().Result);

                        return JsonConvert.DeserializeObject<List<object>>(valueObject);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("Customers FO Config: " + ex.Message);

                if (retryCount > _retryCounter)
                    log.LogError("Customers FO Config " + ex.Message);
            }
            return null;
        }
    }
}
