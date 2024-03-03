using Domain.Models;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using KTI.Moo.FO.App.Helper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.App.Helper
{
    public class FOBatchJob
    {
        private KTIDOMAIN.Models.FOConfig _foConfig;
        private string _accessToken;
        private List<AccountConfig> _listAccountConfig;
        private int _companyId = 0;
        private int _retryCounter = 0;

        public FOBatchJob(int companyId)
        {
            _companyId = companyId;
            _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
            _foConfig = KTIDOMAIN.Security.Authenticate.GetFOConfig(_listAccountConfig);
            _accessToken = KTIDOMAIN.Security.Authenticate.GetFOAccessToken(_foConfig).GetAwaiter().GetResult();
            _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
        }

        public async Task<T> GetPOSParameters<T>(ILogger log)
        {
            int retryCount = 0;
            while (retryCount <= _retryCounter)
            {
                try
                {
                    using (HttpClient httpClient = new())
                    {
                        httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                        httpClient.Timeout = new TimeSpan(0, 2, 0);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        //Add this line for TLS compliance
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        HttpResponseMessage retrieveResponse = await httpClient.GetAsync($"/data/POSParameters(dataAreaId='rwri',KationPOS_CompanyId='{_companyId}')?cross-company=true");

                        if (!retrieveResponse.IsSuccessStatusCode)
                        {
                            if (retryCount >= _retryCounter)
                            {
                                throw new Exception(retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                            }
                            retryCount++;
                        }
                        else
                        {
                            return JsonConvert.DeserializeObject<T>(await retrieveResponse.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount >= _retryCounter)
                    {
                        log.LogError("[GET POS Parameters] " + ex.Message);
                        throw new Exception("[GET POS Parameters] " + ex.Message);
                    }
                    retryCount++;
                }
            }
            return default;
        }

        public async Task<T> GetBatchJobStatus<T>(ILogger log, string batchId)
        {
            int retryCount = 0;
            while (retryCount <= _retryCounter)
            {
                try
                {
                    using (HttpClient httpClient = new())
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
                        HttpResponseMessage retrieveResponse = await httpClient.GetAsync($"/data/BatchJobs({batchId})");

                        if (!retrieveResponse.IsSuccessStatusCode)
                        {
                            if (retryCount >= _retryCounter)
                            {
                                throw new Exception(retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                            }
                            retryCount++;
                        }
                        else
                        {
                            return JsonConvert.DeserializeObject<T>(await retrieveResponse.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount >= _retryCounter)
                    {
                        log.LogError("[GET Batch Job Status] " + ex.Message);
                        throw new Exception("[GET Batch Job Status] " + ex.Message);
                    }
                    retryCount++;
                }
            }
            return default;
        }

        public async Task UpdateSync(ILogger log, string content)
        {
            int retryCount = 0;
            while (retryCount <= _retryCounter)
            {
                try
                {
                    using (HttpClient httpClient = new())
                    {
                        httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                        httpClient.Timeout = new TimeSpan(0, 2, 0);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        //Add this line for TLS compliance
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        HttpResponseMessage retrieveResponse = await httpClient.PatchAsync($"/data/POSParameters(dataAreaId='rwri',KationPOS_CompanyId='{_companyId}')?cross-company=true", stringContent);

                        if (!retrieveResponse.IsSuccessStatusCode)
                        {
                            if (retryCount >= _retryCounter)
                            {
                                throw new Exception(retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                            }
                            retryCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount >= _retryCounter)
                    {
                        log.LogError($"[UPDATE Sync] {content}: " + ex.Message);
                        throw new Exception($"[UPDATE Sync] {content}: " + ex.Message);
                    }
                    retryCount++;
                }
            }
        }

        public async Task SetBatchJobToWaiting(ILogger log, string content)
        {
            int retryCount = 0;
            while (retryCount <= _retryCounter)
            {
                try
                {
                    using (HttpClient httpClient = new())
                    {
                        httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                        httpClient.Timeout = new TimeSpan(0, 2, 0);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        //Add this line for TLS compliance
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        HttpResponseMessage retrieveResponse = await httpClient.PostAsync($"/data/BatchJobs/Microsoft.Dynamics.DataEntities.SetBatchJobToWaiting", stringContent);

                        if (!retrieveResponse.IsSuccessStatusCode)
                        {
                            if (retryCount >= _retryCounter)
                            {
                                throw new Exception(retrieveResponse.StatusCode + ": " + retrieveResponse.Content.ReadAsStringAsync().Result);
                            }
                            retryCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount >= _retryCounter)
                    {
                        log.LogError("[Set Batch Job To Waiting] " + ex.Message);
                        throw new Exception("[Set Batch Job To Waiting] " + ex.Message);
                    }
                    retryCount++;
                }
            }
        }

    }
}
