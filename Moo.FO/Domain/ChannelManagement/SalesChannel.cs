using KTI.Moo.Base.Domain;

namespace KTI.Moo.FO.Domain.ChannelManagement;

public class SalesChannel : Base.Domain.IChannelManagement<Model.ChannelManagement.SalesChannel>
{
    private int _companyId;

    public int CompanyID
    {
        get => _companyId;
        set => _companyId = value;
    }

    public SalesChannel(int companyId)
    {
        _companyId = companyId;
    }

    public SalesChannel()
    {

    }

    public Model.ChannelManagement.SalesChannel Get(string StoreCode) => GetAsync(StoreCode).GetAwaiter().GetResult();

    public List<Model.ChannelManagement.SalesChannel> GetChannelList() => GetChannelListAsync().GetAwaiter().GetResult();

    public async Task<Model.ChannelManagement.SalesChannel> GetAsync(string StoreCode)
    {
        int retryCount = 1;
        string crmResponse = null;

        var _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        var _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        var _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        var _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);

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

                var parameters = new Model.DTO.ChannelManagement.GetParameters();
                parameters.kti_storecode = StoreCode;

                var JSON = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetChannelManagement", content);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Channel Management " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ChannelManagementResults = JsonConvert.DeserializeObject<Model.DTO.ChannelManagement.Get>(result);

                if (ChannelManagementResults is null || string.IsNullOrWhiteSpace(ChannelManagementResults.kti_channelmanagementresponse))
                {
                    return new Model.ChannelManagement.SalesChannel();
                }

                var returnModel = JsonConvert.DeserializeObject<Model.ChannelManagement.SalesChannel>(ChannelManagementResults.kti_channelmanagementresponse);

                return returnModel;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Channel Management : " + ex.Message);
            }
        }


        return new Model.ChannelManagement.SalesChannel();
    }

    public async Task<List<Model.ChannelManagement.SalesChannel>> GetChannelListAsync()
    {
        int retryCount = 1;
        string crmResponse = null;

        var _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        var _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        var _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        var _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);

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
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetAllChannelManagement", null);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Channel Management " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ChannelManagementListResults = JsonConvert.DeserializeObject<Model.DTO.ChannelManagement.GetAll>(result);

                if (ChannelManagementListResults is null || string.IsNullOrWhiteSpace(ChannelManagementListResults.kti_channelmanagementlistresponse))
                {
                    return new List<Model.ChannelManagement.SalesChannel>();
                }

                var returnList = JsonConvert.DeserializeObject<List<Model.ChannelManagement.SalesChannel>>(ChannelManagementListResults.kti_channelmanagementlistresponse);

                return returnList;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Channel Management : " + ex.Message);
            }
        }


        return new List<Model.ChannelManagement.SalesChannel>();
    }

    public Model.ChannelManagement.SalesChannel GetbyLazadaSellerID(string SellerID) => GetbyLazadaSellerIDAsync(SellerID).GetAwaiter().GetResult();
    public bool UpdateToken(Model.ChannelManagement.SalesChannel ChannelConfig) => UpdateTokenAsync(ChannelConfig).GetAwaiter().GetResult();

    public async Task<Model.ChannelManagement.SalesChannel> GetbyLazadaSellerIDAsync(string SellerID)
    {
        int retryCount = 1;
        string crmResponse = null;

        var _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        var _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        var _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        var _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);

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

                var parameters = new Model.DTO.ChannelManagement.GetByLazadaSellerIDParameters();
                parameters.kti_lazadasellerid = SellerID;

                var JSON = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetChannelManagementByLazadaSellerID", content);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Channel Management " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ChannelManagementResults = JsonConvert.DeserializeObject<Model.DTO.ChannelManagement.GetByLazadaSellerID>(result);

                if (ChannelManagementResults is null || string.IsNullOrWhiteSpace(ChannelManagementResults.kti_channelmanagementresponse))
                {
                    return new Model.ChannelManagement.SalesChannel();
                }

                var returnModel = JsonConvert.DeserializeObject<Model.ChannelManagement.SalesChannel>(ChannelManagementResults.kti_channelmanagementresponse);

                return returnModel;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Channel Management : " + ex.Message);
            }
        }


        return new Model.ChannelManagement.SalesChannel();
    }

    public async Task<bool> UpdateTokenAsync(Model.ChannelManagement.SalesChannel ChannelConfig)
    {
        int retryCount = 1;
        string crmResponse = null;

        var _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        var _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        var _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        var _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);

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

                var parameters = new Model.DTO.ChannelManagement.UpdateTokenParameters();
                parameters.kti_updatechannelmanagementtokens_parameters = JsonConvert.SerializeObject(ChannelConfig);

                var JSON = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_updatechannelmanagementtoken", content);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Channel Management " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ChannelManagementResults = JsonConvert.DeserializeObject<Model.DTO.ChannelManagement.UpdateToken>(result);

                if (ChannelManagementResults is null)
                {
                    return false;
                }

               
                return ChannelManagementResults.kti_channelmanagementresponse;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Channel Management : " + ex.Message);
            }
        }


        return false;
    }

}