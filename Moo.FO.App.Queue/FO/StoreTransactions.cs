using Moo.FO.App.Queue.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moo.FO.App.Queue.Model;
using System.Linq;
using System.Reflection;

namespace Moo.FO.App.Queue.FO
{
    public class StoreTransactions
    {
        private List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
        private int _retryCounter = 0;
        private KTIDOMAIN.Models.FOConfig _foConfig;
        private string _accessToken;
        private int _companyId = 0;
        private bool _getAll = false;

        public StoreTransactions(int companyId, bool getAll = false)
        {
            _companyId = companyId;
            _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
            _foConfig = KTIDOMAIN.Security.Authenticate.GetFOConfig(_listAccountConfig);
            _accessToken = KTIDOMAIN.Security.Authenticate.GetFOAccessToken(_foConfig).GetAwaiter().GetResult();
            _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
            _getAll = getAll;
        }

        public async Task<Model.StoreTransactions> GetStoreTransactions(ILogger log, string DateFilter)
        {
            return new Model.StoreTransactions
            {
                Headers = await Get<StoreTransactionsHeader>(log, Endpoint.Header, DateFilter),
                Lines = await Get<StoreTransactionsLine>(log, Endpoint.Line, DateFilter),
                Discounts = await Get<StoreTransactionsDiscount>(log, Endpoint.Discount, DateFilter),
                Payments = await Get<StoreTransactionsPayment>(log, Endpoint.Payment, DateFilter)
            };
        }

        public async Task<List<T>> Get<T>(ILogger log, string endpoint, string DateFilter)
        {
            int retryCount = 0;
            List<T> resultList = new List<T>();
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
                        httpClient.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=1000");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        //Add this line for TLS compliance
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                        string apiURL = $"/data{endpoint}?cross-company=true&$select=Terminal, TransactionNumber, LineNumber";
                        if (endpoint == Endpoint.Header)
                        {
                            if (_getAll)
                            {
                                apiURL = $"/data{endpoint}?cross-company=true&$select=TransactionDate, ChannelReferenceId, Terminal, TransactionNumber";
                            }
                            else
                            {
                                apiURL = $"/data{endpoint}?cross-company=true&$select=TransactionDate, ChannelReferenceId, Terminal, TransactionNumber&$filter=TransactionDate eq {DateFilter}";
                            }
                        }
                        else if (endpoint == Endpoint.Line)
                        {
                            if (_getAll)
                            {
                                apiURL = $"/data{endpoint}?cross-company=true&$select=TransactionDate, Terminal, TransactionNumber, LineNumber";
                            }
                            else
                            {
                                apiURL = $"/data{endpoint}?cross-company=true&$select=TransactionDate, Terminal, TransactionNumber, LineNumber&$filter=TransactionDate eq {DateFilter}";
                            }
                        }
                        else if (endpoint == Endpoint.Payment)
                        {

                            apiURL = $"/data{endpoint}?cross-company=true&$select=Terminal, TransactionNumber, LineNumber, AmountTendered";
                        }

                        while (!string.IsNullOrEmpty(apiURL))
                        {
                            HttpResponseMessage retrieveResponse = await httpClient.GetAsync(apiURL);

                            string strResponse = retrieveResponse.Content.ReadAsStringAsync().Result;

                            if (!retrieveResponse.IsSuccessStatusCode)
                            {
                                if (retryCount >= _retryCounter)
                                {
                                    throw new Exception(retrieveResponse.StatusCode + ": " + strResponse);
                                }
                                retryCount++;
                            }
                            else
                            {
                                string valueObject = ODataHelper.ExtractValueObject(strResponse);

                                var resultChunk = JsonConvert.DeserializeObject<List<T>>(valueObject)
                                    .Select(obj => UpdateTerminalProperty(obj));

                                resultList.AddRange(resultChunk);

                                // Check for "nextLink" in the response headers
                                if (strResponse.Contains("nextLink"))
                                {
                                    apiURL = ODataHelper.ExtractNextLink(strResponse);
                                }
                                else
                                {
                                    apiURL = null; // No "nextLink" found, exit the loop
                                }
                            }
                        }

                        return resultList;
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount >= _retryCounter)
                    {
                        log.LogError($"[{endpoint}] " + ex.Message);
                        throw new Exception($"[{endpoint}] " + ex.Message);
                    }
                    retryCount++;
                }
            }
            return null;
        }

        private T UpdateTerminalProperty<T>(T obj)
        {
            PropertyInfo terminalProperty = obj.GetType().GetProperty("Terminal");

            if (terminalProperty != null && terminalProperty.CanWrite)
            {
                // Update the "Terminal" property if it exists
                string currentValue = (string)terminalProperty.GetValue(obj);
                terminalProperty.SetValue(obj, currentValue.TrimStart('0'));
            }

            return obj;
        }
    }
}
