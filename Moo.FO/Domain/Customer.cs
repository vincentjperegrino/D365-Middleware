using Microsoft.Extensions.Logging;
using KTI.Moo.FO.Model;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;

namespace KTI.Moo.FO.Domain
{
    public class Customer : Base.Domain.ICustomer
    {
        List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
        List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
        int _retryCounter = 0;
        private KTIDOMAIN.Models.FOConfig _foConfig;
        private string _accessToken;
        int _companyId = 0;


        public Customer(int companyId)
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

            var customer = new Moo.FO.Model.DTO.CustomerDTO(content);
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

            if(!String.IsNullOrEmpty(customer.OrganizationName) && !String.IsNullOrWhiteSpace(customer.OrganizationName) && customer.OrganizationName != "  ")
            {
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
                            HttpResponseMessage retrieveResponseTest = await httpClient.PostAsync("/data/CustomersV3?cmp=SRDF", new StringContent(postModel, Encoding.UTF8, "application/json"));

                            if (!retrieveResponseTest.IsSuccessStatusCode)
                            {
                                retryCount++;

                                if (retryCount > _retryCounter)
                                {
                                    throw new Exception("FO Customer " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
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
                        log.LogInformation("FO Customer: " + ex.Message);

                        if (retryCount > _retryCounter)
                            log.LogError("FO Customer " + ex.Message);
                        //throw new Exception();
                    }

                }
            }

            return null;

        }
        public async Task<string> upsert(string content, ILogger log)
        {
            int retryCount = 1;

            var customer = JsonConvert.DeserializeObject<CustomerBase>(content);
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
                                throw new Exception("FO Customer " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
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
                    log.LogInformation("FO Customer: " + ex.Message);

                    if (retryCount > _retryCounter)
                        throw new Exception("FO Customer " + ex.Message);
                    //throw new Exception();
                }

            }

            return null;

        }

        //public async Task<string> upsert(string content, string id, ILogger log)
        //{
        //    int retryCount = 1;

        //    while (retryCount <= _retryCounter)
        //    {
        //        try
        //        {
        //            using (HttpClient httpClient = new HttpClient())
        //            {
        //                httpClient.BaseAddress = new Uri(_foConfig.fo_base_url);
        //                httpClient.Timeout = new TimeSpan(0, 2, 0);
        //                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        //                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
        //                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        //                //Add this line for TLS complaience
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        //                var _content = new StringContent(content, Encoding.UTF8, "application/json");

        //                var retrieveResponseTest = await httpClient.PatchAsync(_foConfig.fo_api_path + _foConfig.fo_customer_path + $"({id})", _content);

        //                if (!retrieveResponseTest.IsSuccessStatusCode)
        //                {
        //                    retryCount++;
        //                    Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

        //                    if (retryCount > _retryCounter)
        //                        throw new Exception("FO Customer " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.LogInformation("FO Customer: " + ex.Message);

        //            if (retryCount > _retryCounter)
        //            {
        //                throw new Exception("FO Customer " + ex.Message);
        //            }
        //        }
        //    }

        //    return null;
        //}
    }
}
