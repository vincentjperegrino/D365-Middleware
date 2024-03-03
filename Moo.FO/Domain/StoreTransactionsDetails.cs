using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using Microsoft.Extensions.Logging;
using Moo.FO.Model.DTO;

namespace KTI.Moo.FO.Domain
{
    public class StoreTransactionsDetails : Base.Domain.IStoreTransactionsDetails
    {
        List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
        List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
        int _retryCounter = 0;
        private KTIDOMAIN.Models.FOConfig _foConfig;
        private string _accessToken;
        int _companyId = 0;

        public StoreTransactionsDetails(int companyId)
        {
            _companyId = companyId;
            _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
            _foConfig = KTIDOMAIN.Security.Authenticate.GetFOConfig(_listAccountConfig);
            _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
            _accessToken = KTIDOMAIN.Security.Authenticate.GetFOAccessToken(_foConfig).GetAwaiter().GetResult();
            _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
        }

        public async Task<string> upsert(OrdersTransaction content, ILogger log)
        {
            int retryCount = 1;
            int lineNumber = 1;
            foreach (var lines in content.Details)
            {
                while (retryCount <= _retryCounter)
                {
                    try
                    {
                        var details = new StoreTransactionLinesDTO(lines, content);
                        var postModel = JsonConvert.SerializeObject(details);

                        string foResponse = null;
                        string mooResponse = null;

                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                            httpClient.Timeout = new TimeSpan(0, 2, 0);
                            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                            //Add this line for TLS complaience
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                            UriBuilder builder = new UriBuilder(httpClient.BaseAddress);
                            HttpResponseMessage retrieveResponseTest = await httpClient.PostAsync("/data/RetailTransactionSalesLinesV2?cmp=SRDF", new StringContent(postModel, Encoding.UTF8, "application/json"));

                            if (!retrieveResponseTest.IsSuccessStatusCode)
                            {
                                retryCount++;

                                if (retryCount > _retryCounter)
                                {
                                    throw new Exception("FO Lines " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                                }

                            }
                            else
                            {
                                break;          
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.LogInformation("FO Lines: " + ex.Message);

                        if (retryCount >= _retryCounter)
                        {
                            log.LogError("FO Lines " + ex.Message);
                            throw new Exception("FO Lines " + ex.Message);
                        }

                        retryCount++;
                    }

                }
                lineNumber++;
            }

            return null;

        }

        public async Task<string> upsert(string content, ILogger log)
        {
            int retryCount = 1;

            var customer = JsonConvert.DeserializeObject<StoreTransactionsDetails>(content);
            var postModel = JsonConvert.SerializeObject(customer);


            //if (string.IsNullOrWhiteSpace(customer.kti_sourceid))
            //{
            //    log.LogInformation("FO Customer: Attribute kti_sourceid Missing");
            //    throw new Exception("FO Customer: Attribute kti_sourceid Missing");
            //}

            //if (customer.kti_socialchannelorigin == 0)
            //{
            //    log.LogInformation("FO Customer: Attribute kti_socialchannelorigin can't be zero");
            //    throw new Exception("FO Customer: Attribute kti_socialchannelorigin can't be zero");
            //}

            while (retryCount <= _retryCounter)
            {
                try
                {
                    string foResponse = null;
                    string mooResponse = null;

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url);
                        httpClient.Timeout = new TimeSpan(0, 2, 0);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        //Add this line for TLS complaience
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        UriBuilder builder = new UriBuilder(httpClient.BaseAddress);
                        HttpResponseMessage retrieveResponseTest = await httpClient.PostAsync(builder.Uri, new StringContent(postModel, Encoding.UTF8, "application/json"));

                        if (!retrieveResponseTest.IsSuccessStatusCode)
                        {
                            retryCount++;

                            if (retryCount > _retryCounter)
                            {
                                throw new Exception("FO Lines " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                            }

                        }
                        else
                        {
                            return retrieveResponseTest.Content.ReadAsStringAsync().Result;
                            //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogInformation("FO Lines: " + ex.Message);

                    if (retryCount > _retryCounter)
                        throw new Exception("FO Lines " + ex.Message);
                    //throw new Exception();
                }

            }

            return null;

        }

    }
}
