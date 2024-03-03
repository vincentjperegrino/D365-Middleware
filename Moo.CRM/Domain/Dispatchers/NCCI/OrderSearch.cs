using Domain.Models;
using KTI.Moo.CRM.Model;
using KTI.Moo.CRM.Model.DTO.Orders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain.Dispatchers.NCCI;

public class OrderSearch : Base.Domain.ISearch<Model.DTO.Orders.Search, Model.OrderBase>
{

    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;

    public OrderSearch(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public async Task<Model.DTO.Orders.Search> GetAsync(DateTime DateStart, DateTime DateEnd, int PageSize = 100)
    {

        int retryCount = 1;

        while (retryCount <= _retryCounter)
        {
            try
            {
                string crmResponse = null;
                string mooResponse = null;

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

                    var SelectClause = $"$select= createdon, modifiedon,kti_socialchannelorigin, kti_sourceid, name, kti_sapdocnum, kti_sapdocentry";
                    //  var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter=( (createdon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} ) or (modifiedon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} )   )&$orderby=createdon asc&{SelectClause}";
                    //  var filterQueryString = $"?$filter= ({EmailAddressFilter} or {MobilePhoneFilter}) &{SelectClause} &{Top_Limit}";

                    HttpResponseMessage retrieveResponseTest = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + filterQueryString);
                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retryCount > _retryCounter)
                        {
                            var errormessage = retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result;

                            if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                            }

                            throw new Exception("CRM Order " + errormessage);
                        }

                    }
                    else
                    {
                        var result = retrieveResponseTest.Content.ReadAsStringAsync().Result;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.Orders.Search>(result, settings);

                        return contactResults;
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                {

                    throw new Exception(ex.Message);

                }


                //    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return new Model.DTO.Orders.Search();
    }
    private async Task<Model.DTO.Orders.Search> GetNextAsync(string NextLink)
    {

        int retryCount = 1;

        while (retryCount <= _retryCounter)
        {
            try
            {
                string crmResponse = null;
                string mooResponse = null;

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

                    HttpResponseMessage retrieveResponseTest = await httpClient.GetAsync(NextLink);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retryCount > _retryCounter)
                        {
                            var errormessage = retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result;

                            if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                            }

                            throw new Exception("CRM Order " + errormessage);
                        }

                    }
                    else
                    {
                        var result = retrieveResponseTest.Content.ReadAsStringAsync().Result;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.Orders.Search>(result, settings);

                        return contactResults;


                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                {

                    throw new Exception(ex.Message);

                }


                //    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return new Model.DTO.Orders.Search();
    }
    public async Task<List<Model.OrderBase>> GetAllAsync(List<Model.OrderBase> allOrders, DateTime DateStart, DateTime DateEnd, int PageSize, int CurrentPage)
    {
        return await GetAllOrderListCRM(allOrders, DateStart, DateEnd, PageSize);
    }

    private async Task<List<Model.OrderBase>> GetAllOrderListCRM(List<Model.OrderBase> allOrders, DateTime DateStart, DateTime DateEnd, int PageSize, string NextLink = "")
    {
        if (allOrders is null)
        {
            allOrders = new List<Model.OrderBase>();
        }

        var currentPageCustomersSearch = string.IsNullOrWhiteSpace(NextLink) ? await GetAsync(DateStart, DateEnd) : await GetNextAsync(NextLink);

        if (currentPageCustomersSearch is not null && currentPageCustomersSearch.values is not null && currentPageCustomersSearch.values.Count > 0)
        {
            allOrders.AddRange(currentPageCustomersSearch.values);

            // Check if there are more results to retrieve
            if (!string.IsNullOrWhiteSpace(currentPageCustomersSearch.NextLink))
            {
                await GetAllOrderListCRM(allOrders, DateStart, DateEnd, PageSize, currentPageCustomersSearch.NextLink);
            }

        }

        return allOrders;
    }


    public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
    {
        return GetAsync(dateFrom, dateTo, pagesize).GetAwaiter().GetResult();
    }

    public List<OrderBase> GetAll(List<OrderBase> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        return GetAllAsync(initialList, dateFrom, dateTo, pagesize, currentpage).GetAwaiter().GetResult();
    }

    public List<OrderBase> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
    {
        var initialList = new List<OrderBase>();

        return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
    }
}
