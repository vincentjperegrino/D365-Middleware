



using KTI.Moo.CRM.Model;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.CRM.Domain;

public class Product : Base.Domain.IProduct<Model.ProductBase>
{


    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public Product(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public async Task<List<ProductBase>> getAll()
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
                var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + "/kti_GetAllProducts", null);

                if (!retrieveResponseTest.IsSuccessStatusCode)
                {
                    retryCount++;
                    throw new Exception("CRM Product " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                }

                var result = await retrieveResponseTest.Content.ReadAsStringAsync();

                var ProductResults = JsonConvert.DeserializeObject<Model.DTO.ProductGet>(result);

                if (ProductResults is null || ProductResults.values is null || ProductResults.values.Count <= 0)
                {
                    return new List<ProductBase>();
                }

                return ProductResults.values;
            }
            catch (Exception ex)
            {               
                throw new Exception("CRM Product: " + ex.Message);
            }
        }


        return new List<ProductBase>();
    }
   
    public async Task<string> upsert(string contents, ILogger log)
    {
        try
        {
            int retryCount = 1;
            string crmResponse = null;
            string mooResponse = null;

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

                    var content = new StringContent(contents, Encoding.UTF8, "application/json");

                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_product_path, content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        throw new Exception("CRM Product " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                        retryCount++;

                        if (retryCount > _retryCounter)
                            throw new Exception("Retry counter exceeded.");
                    }
                    else
                    {

                        return retrieveResponseTest.Content.ReadAsStringAsync().Result;
                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);

                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.LogInformation("CRM Product: " + ex.Message);

            throw new Exception("CRM Product: " + ex.Message);
        }

        return null;
    }
}
