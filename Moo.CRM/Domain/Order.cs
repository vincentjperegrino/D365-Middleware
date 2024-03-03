
using KTI.Moo.Base.Domain.Dispatchers;
using KTI.Moo.CRM.Model;
using KTI.Moo.CRM.Model.DTO.Orders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace KTI.Moo.CRM.Domain;

public class Order : Base.Domain.IOrder, Base.Domain.ISearch<Model.DTO.Orders.Search, Model.OrderBase>, IReplicate
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

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
                        }

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        var result = retrieveResponseTest.Content.ReadAsStringAsync().Result;

                        var ResultModel = JsonConvert.DeserializeObject<OrderWithCustomer_Result>(result);

                        return ResultModel.value;


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
                        CRM.Domain.Customer customer = new(_companyId);

                        var Contactid = customer.GetContactBy_Email(orderHeader.emailaddress).GetAwaiter().GetResult();

                        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

                        if (string.IsNullOrWhiteSpace(Contactid))
                        {
                            retryCount++;
                            throw new Exception("Customer not exists.");
                        }

                        if (DomainJObject.ContainsKey("customerid_contact@odata.bind"))
                        {
                            DomainJObject["customerid_contact@odata.bind"] = $"/contacts({Contactid})";

                            content = JsonConvert.SerializeObject(DomainJObject);

                        }

                    }
                    else if (!orderHeader.customerid.Contains("/contacts") && !string.IsNullOrWhiteSpace(orderHeader.billto_telephone))
                    {
                        CRM.Domain.Customer customer = new(_companyId);

                        var Contactid = customer.GetContactBy_Mobile(orderHeader.billto_telephone).GetAwaiter().GetResult();

                        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

                        if (string.IsNullOrWhiteSpace(Contactid))
                        {
                            retryCount++;
                            throw new Exception("Customer not exists.");
                        }

                        if (DomainJObject.ContainsKey("customerid_contact@odata.bind"))
                        {
                            DomainJObject["customerid_contact@odata.bind"] = $"/contacts({Contactid})";

                            content = JsonConvert.SerializeObject(DomainJObject);

                        }

                    }

                    var _content = new StringContent(content, Encoding.UTF8, "application/json");

                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + $"(kti_sourceid='{orderHeader.kti_sourceid}',kti_socialchannelorigin={orderHeader.kti_socialchannelorigin})", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

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

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

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


    public async Task<string> SyncToOrders()
    {
        try
        {
            int retryCount = 1;

            while (retryCount <= _retryCounter)
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
                    var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + $"/kti_OrderSyncToSAP", null);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return retrieveResponseTest.Content.ReadAsStringAsync().Result;
                    }
                }

            }

        }
        catch (Exception ex)
        {
            throw new Exception("CRM Order: " + ex.Message);
        }

        return null;
    }



    public async Task<List<OrderBase>> GetSyncToOrders()
    {
        int retryCount = 1;
        string crmResponse = null;

        while (retryCount <= _retryCounter)
        {

            try
            {
                using HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_crmConfig.crm_base_url);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetOrderSchedule", null);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;

                    if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                    }

                    throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var OrderResults = JsonConvert.DeserializeObject<Model.DTO.Orders.Get>(result);

                if (OrderResults is null || OrderResults.values is null || OrderResults.values.Count <= 0)
                {
                    return new List<OrderBase>();
                }

                return OrderResults.values;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Orders: " + ex.Message);
            }
        }


        return new List<OrderBase>();
    }

    public bool Replicate(string id)
    {
        return ReplicateOrders(id).GetAwaiter().GetResult();
    }

    public async Task<bool> ReplicateOrders(string orderid)
    {
        var parameters = new Dictionary<string, string>();
        parameters.Add("kti_APIReplicateOrderID", orderid);
        parameters.Add("kti_APIReplicateOrderCompanyID", _companyId.ToString());

        var content = JsonConvert.SerializeObject(parameters);


        var _content = new StringContent(content, Encoding.UTF8, "application/json");

        int retryCount = 1;
        string crmResponse = null;

        while (retryCount <= _retryCounter)
        {

            try
            {
                using HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_crmConfig.crm_base_url);
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_APIReplicateOrder", _content);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;

                    if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                    }

                    throw new Exception("CRM Orders " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var OrderResults = JsonConvert.DeserializeObject<bool>(result);

                return OrderResults;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Orders: " + ex.Message);
            }
        }


        return false;
    }


    public async Task<string> getbyID(string id)
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


                    var retrieveResponseTest = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + $"({id})");

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

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

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Order " + ex.Message);
                }
            }
        }

        return null;
    }

    public async Task<string> upsertBatch(List<CRM.Model.DTO.Orders.Plugin.Order> order, ILogger log)
    {
        int retryCount = 1;

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

                    var content = JsonConvert.SerializeObject(order);

                    var _content = new StringContent(content, Encoding.UTF8, "application/json");

                    var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_batch_plugin_path, _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Plugin Order batch " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Plugin Order batch: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Plugin Order batch " + ex.Message);
                }
            }
        }

        return null;
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

                    var SelectClause = $"$select= createdon, modifiedon,kti_socialchannelorigin, kti_sourceid, name";
                    //  var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter=( (createdon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} ) or (modifiedon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} )   )&$orderby=createdon asc&{SelectClause}";
                    //  var filterQueryString = $"?$filter= ({EmailAddressFilter} or {MobilePhoneFilter}) &{SelectClause} &{Top_Limit}";

                    HttpResponseMessage retrieveResponseTest = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + filterQueryString);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;


                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
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

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("CRM Order " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
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

    public async Task<string> RetrieveDataFromJoinedTables(DateTime DateStart, DateTime DateEnd)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(_crmConfig.crm_base_url);
        httpClient.Timeout = new TimeSpan(0, 2, 0);
        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        var formattedDateStart = DateStart.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var formattedDateEnd = DateEnd.ToString("yyyy-MM-ddTHH:mm:ssZ");


        string fetchXml = @"
    <fetch mapping='logical' aggregate='true'>
        <entity name='salesorder'>
            <attribute name='contactid' aggregate='countcolumn' alias='customercount' distinct='true' />
            <link-entity name='salesorderdetail' from='salesorderid' to='salesorderid' link-type='outer' alias='sod'>
                <attribute name='salesorderdetailid' aggregate='countcolumn' alias='salesorderdetailcount' />
                <attribute name='ncci_productcategory' aggregate='group' alias='productcategory' />
                <filter type='and'>
                    <condition attribute='ncci_productcategory' operator='eq' value='714430000' />
                </filter>
                <order attribute='ncci_productcategory' />
                <order attribute='createdon' />
            </link-entity>
            <filter type='and'>
                <condition attribute='createdon' operator='on-or-after' value='" + formattedDateStart + @"' />
                <condition attribute='createdon' operator='on-or-before' value='" + formattedDateEnd + @"' />
            </filter>
        </entity>
    </fetch>";

        string encodedFetchXml = WebUtility.UrlEncode(fetchXml);
        string queryOptions = $"?fetchXml={encodedFetchXml}";


        var response = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_salesorder_path + queryOptions);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();

            throw new Exception("CRM Order " + response.StatusCode + ": " + result);
        }
  
        var results = await response.Content.ReadAsStringAsync();

        return results;
    }

}
