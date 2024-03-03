using Domain.Models;
using KTI.Moo.Base.Domain;
using KTI.Moo.CRM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain.Dispatchers.NCCI;

public class CustomerSearch : ISearch<Model.DTO.CustomerSearch, Model.CustomerBase>
{

    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public CustomerSearch(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public async Task<Model.DTO.CustomerSearch> GetAsync(DateTime DateStart, DateTime DateEnd, int PageSize = 100)
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


                    var Address1 = $"address1_line1, address1_line2, address1_line3, address1_city, address1_postalcode, address1_telephone1, address1_stateorprovince, telephone1 , address1_country";
                    var Address2 = $"address2_line1, address2_line2, address2_line3, address2_city, address2_postalcode, address2_telephone1, address2_stateorprovince, telephone2 , address2_country";

                    var SelectClause = $"$select= createdon, modifiedon , contactid, kti_socialchannelorigin , kti_sourceid ,kti_magentoid, firstname, lastname, emailaddress1, ncci_newclubmembershipid, kti_sapbpcode, mobilephone, {Address1}, {Address2} ";
                    //  var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter=( (createdon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} ) or (modifiedon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} )   )&$orderby=createdon asc&{SelectClause}";
                    //  var filterQueryString = $"?$filter= ({EmailAddressFilter} or {MobilePhoneFilter}) &{SelectClause} &{Top_Limit}";

                    HttpResponseMessage retrieveResponseTest = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + filterQueryString);
                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("CRM Customer " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                        }

                    }
                    else
                    {
                        var result = retrieveResponseTest.Content.ReadAsStringAsync().Result;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.CustomerSearch>(result, settings);

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

        return new Model.DTO.CustomerSearch();
    }

    private async Task<Model.DTO.CustomerSearch> GetNextAsync(string NextLink)
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

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("CRM Customer " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                        }

                    }
                    else
                    {
                        var result = retrieveResponseTest.Content.ReadAsStringAsync().Result;

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.CustomerSearch>(result, settings);

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

        return new Model.DTO.CustomerSearch();
    }

    public async Task<List<CustomerBase>> GetAllAsync(List<CustomerBase> allCustomers, DateTime DateStart, DateTime DateEnd, int PageSize, int CurrentPage)
    {
        return await GetAllCustomerListCRM(allCustomers, DateStart, DateEnd, PageSize);
    }

    private async Task<List<CustomerBase>> GetAllCustomerListCRM(List<CustomerBase> allCustomers, DateTime DateStart, DateTime DateEnd, int PageSize, string NextLink = "")
    {
        if (allCustomers is null)
        {
            allCustomers = new List<CustomerBase>();
        }

        var currentPageCustomersSearch = string.IsNullOrWhiteSpace(NextLink) ? await GetAsync(DateStart, DateEnd) : await GetNextAsync(NextLink);

        if (currentPageCustomersSearch is not null && currentPageCustomersSearch.values is not null && currentPageCustomersSearch.values.Count > 0)
        {
            allCustomers.AddRange(currentPageCustomersSearch.values);

            // Check if there are more results to retrieve
            if (!string.IsNullOrWhiteSpace(currentPageCustomersSearch.NextLink))
            {
                await GetAllCustomerListCRM(allCustomers, DateStart, DateEnd, PageSize, currentPageCustomersSearch.NextLink);
            }

        }


        return allCustomers;
    }


    public Model.DTO.CustomerSearch Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
    {
        return GetAsync(dateFrom, dateTo, pagesize).GetAwaiter().GetResult();
    }

    public List<CustomerBase> GetAll(List<CustomerBase> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        return GetAllAsync(initialList, dateFrom, dateTo, pagesize, currentpage).GetAwaiter().GetResult();
    }

    public List<CustomerBase> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
    {
        var initialList = new List<CustomerBase>();

        return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
    }


}
