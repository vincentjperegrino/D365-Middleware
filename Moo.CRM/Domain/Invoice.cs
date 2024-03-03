
using KTI.Moo.CRM.Model;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.CRM.Domain;

public class Invoice : Base.Domain.IInvoice
{

    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public Invoice(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
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

                var invoiceHeader = JsonConvert.DeserializeObject<InvoiceBase>(content);

                using (HttpClient httpClient = new HttpClient())
                {
                    //var jsonResolver = new KTIDOMAIN.PropertyRenameAndIgnoreSerializerContractResolver();
                    //var settings = new JsonSerializerSettings();

                    //settings.NullValueHandling = NullValueHandling.Ignore;
                    //settings.DefaultValueHandling = DefaultValueHandling.Ignore;

                    httpClient.BaseAddress = new Uri(_crmConfig.crm_base_url);
                    httpClient.Timeout = new TimeSpan(0, 2, 0);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);



                    //if (!string.IsNullOrEmpty(invoiceHeader.pricelevelid))
                    //{
                    //    invoiceHeader.pricelevelid = $"{_crmConfig.crm_pricelevel_path}(name='{invoiceHeader.pricelevelid}')";
                    //}
                    //else
                    //{
                    //    invoiceHeader.pricelevelid = $"{_crmConfig.crm_pricelevel_path}(name='Standard')";
                    //}

                    //if (!(string.IsNullOrEmpty(invoiceHeader.salesorderid) || string.IsNullOrWhiteSpace(invoiceHeader.salesorderid)))
                    //{
                    //    //Relate order header
                    //    invoiceHeader.salesorderid = $"{_crmConfig.crm_salesorder_path}(kti_sourceid='{invoiceHeader.salesorderid}',kti_socialchannelorigin={invoiceHeader.kti_socialchannelorigin})";
                    //}
                    //else
                    //{
                    //    invoiceHeader.salesorderid = "";
                    //    jsonResolver.IgnoreProperty(typeof(KTIDOMAIN.Models.Sales.InvoiceHeader), "salesorderid@odata.bind");
                    //}

                    //Get Customer if customerid = email
                    if (invoiceHeader.customerid is not null && !invoiceHeader.customerid.Contains("/contacts") )
                    {
                        CRM.Domain.Customer customer = new(_companyId);
 

                        var Contactid = customer.GetContactBy_Email(invoiceHeader.emailaddress).GetAwaiter().GetResult();

                        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

                        if (DomainJObject.ContainsKey("customerid_contact@odata.bind"))
                        {
                            DomainJObject["customerid_contact@odata.bind"] = $"/contacts({Contactid})";

                            content = JsonConvert.SerializeObject(DomainJObject);

                        }
                    }


                    //settings.ContractResolver = jsonResolver;

                    //Add this line for TLS complaience
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var _content = new StringContent(content, Encoding.UTF8, "application/json");
                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_invoice_path + $"(kti_sourceid='{invoiceHeader.kti_sourceid}',kti_socialchannelorigin={invoiceHeader.kti_socialchannelorigin})", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Invoice " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Invoice: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Invoice " + ex.Message);
                }
            }
        }

        return null;
    }



    public async Task<string> upsert(string content, string invoiceid, ILogger log)
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
          

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var _content = new StringContent(content, Encoding.UTF8, "application/json");
                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_invoice_path + $"({invoiceid})", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Invoice " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Invoice: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Invoice " + ex.Message);
                }
            }
        }

        return null;
    }



    public async Task<string> getbyid(string invoiceid)
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
                    //var jsonResolver = new KTIDOMAIN.PropertyRenameAndIgnoreSerializerContractResolver();
                    //var settings = new JsonSerializerSettings();

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

           
                    var retrieveResponseTest = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_invoice_path + $"({invoiceid})");

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Invoice " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
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
                    throw new Exception("CRM Invoice " + ex.Message);
                }
            }
        }

        return null;
    }


    public async Task<string> SyncToInvoice()
    {
        try
        {
            int retryCount = 1;
            string crmResponse = null;

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
                    var retrieveResponseTest = await httpClient.PostAsync(_crmConfig.crm_api_path + $"/kti_InvoiceSyncToSAP", null);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        throw new Exception("CRM Invoice " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                        retryCount++;

                        if (retryCount > _retryCounter)
                            throw new Exception("Retry counter exceeded.");
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
            throw new Exception("CRM Invoice: " + ex.Message);
        }

        return null;
    }



}