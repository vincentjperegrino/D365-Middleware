
namespace KTI.Moo.CRM.Domain.ChannelManagement;

public class Inventory : Base.Domain.IChannelManagementInventory<Model.ChannelManagement.Inventory>
{
    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    // private string _accessToken;
    int _companyId = 0;

    public int CompanyID
    {
        get => _companyId;
        set => _companyId = value;
    }
    public Inventory(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        // _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public Model.ChannelManagement.Inventory Get(string StoreCode) => GetAsync(StoreCode).GetAwaiter().GetResult();

    public List<Model.ChannelManagement.Inventory> GetChannelList() => GetChannelListAsync().GetAwaiter().GetResult();

    public async Task<Model.ChannelManagement.Inventory> GetAsync(string StoreCode)
    {
        int retryCount = 1;
        string crmResponse = null;
        var _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
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

                var parameters = new Model.DTO.ChannelManagement.GetParametersProduct();
                parameters.kti_storecode = StoreCode;

                var JSON = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");

                //Add this line for TLS complaience
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetChannelManagementProducts", content);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Channel Management " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ChannelManagementResults = JsonConvert.DeserializeObject<Model.DTO.ChannelManagement.GetProducts>(result);

                if (ChannelManagementResults is null || string.IsNullOrWhiteSpace(ChannelManagementResults.kti_channelmanagementresponse))
                {
                    return new Model.ChannelManagement.Inventory();
                }

                var returnModel = JsonConvert.DeserializeObject<Model.ChannelManagement.Inventory>(ChannelManagementResults.kti_channelmanagementresponse);

                return returnModel;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Channel Management : " + ex.Message);
            }
        }


        return new Model.ChannelManagement.Inventory();
    }

    public async Task<List<Model.ChannelManagement.Inventory>> GetChannelListAsync()
    {
        int retryCount = 1;
        string crmResponse = null;
        var _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();

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
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetAllChannelManagementProducts", null);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Channel Management " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ChannelManagementListResults = JsonConvert.DeserializeObject<Model.DTO.ChannelManagement.GetAllProducts>(result);

                if (ChannelManagementListResults is null || string.IsNullOrWhiteSpace(ChannelManagementListResults.kti_channelmanagementlistresponse))
                {
                    return new List<Model.ChannelManagement.Inventory>();
                }

                var returnList = JsonConvert.DeserializeObject<List<Model.ChannelManagement.Inventory>>(ChannelManagementListResults.kti_channelmanagementlistresponse);

                return returnList;
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Channel Management : " + ex.Message);
            }
        }

        return new List<Model.ChannelManagement.Inventory>();
    }

    public Model.ChannelManagement.Inventory GetbyLazadaSellerID(string SellerID)
    {
        throw new NotImplementedException();
    }

    public bool UpdateToken(Model.ChannelManagement.Inventory ChannelConfig)
    {
        throw new NotImplementedException();
    }
}
