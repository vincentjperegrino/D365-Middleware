using Microsoft.Extensions.Logging;


namespace KTI.Moo.CRM.Domain;

public class InvoiceDetails : Base.Domain.IInvoiceDetails
{

    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;

    public InvoiceDetails(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public async Task<string> upsert(string contents, ILogger log)
    {
        int retryCount = 1;
        string crmResponse = null;
        string mooResponse = null;

        while (retryCount <= _retryCounter)
        {
            try
            {
                var invoiceDetails = JsonConvert.DeserializeObject<Model.InvoiceItemBase>(contents);


                using (HttpClient httpClient = new HttpClient())
                {
                    // var settings = new JsonSerializerSettings();

                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                    httpClient.BaseAddress = new Uri(_crmConfig.crm_base_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);


                    //Add this line for TLS complaience
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    //var json = JsonConvert.SerializeObject(invoiceDetails, Formatting.Indented, settings);

                    var _content = new StringContent(contents, Encoding.UTF8, "application/json");
                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_invoicedetail_path + $"(kti_sourceid='{HttpUtility.UrlEncode(invoiceDetails.kti_sourceid)}',kti_socialchannelorigin={invoiceDetails.kti_socialchannelorigin},kti_lineitemnumber='{invoiceDetails.kti_lineitemnumber}')", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if (retryCount > _retryCounter)
                        {
                            throw new Exception("CRM Invoice Detail " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
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
                log.LogInformation("CRM Invoice Detail: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Invoice Detail: " + ex.Message);
                }
                //throw new Exception("CRM Invoice Detail: " + ex.Message);
            }

        }
        return null;
    }


}
