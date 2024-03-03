
using KTI.Moo.Base.Domain;
using KTI.Moo.CRM.Model;
using KTI.Moo.CRM.Model.DTO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Drawing.Printing;
using System.Reflection;

namespace KTI.Moo.CRM.Domain;

public class Customer : Base.Domain.ICustomer, Base.Domain.ISearch<Model.DTO.CustomerSearch, Model.CustomerBase>
{
    List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;


    public Customer(int companyId)
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

        var customer = JsonConvert.DeserializeObject<CustomerBase>(content);

        if (string.IsNullOrWhiteSpace(customer.kti_sourceid))
        {
            log.LogInformation("CRM Customer: Attribute kti_sourceid Missing");
            throw new Exception("CRM Customer: Attribute kti_sourceid Missing");
        }

        if (customer.kti_socialchannelorigin == 0)
        {
            log.LogInformation("CRM Customer: Attribute kti_socialchannelorigin can't be zero");
            throw new Exception("CRM Customer: Attribute kti_socialchannelorigin can't be zero");
        }

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


                    HttpResponseMessage retrieveResponseTest = await SendHttPrequest(customer, httpClient, content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

                        if(retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
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
                        return retrieveResponseTest.Content.ReadAsStringAsync().Result;
                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Customer: " + ex.Message);

                if (retryCount > _retryCounter)
                    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
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

                    var retrieveResponseTest = await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + $"({id})", _content);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;
                        Console.WriteLine(retrieveResponseTest.Content.ReadAsStringAsync().Result);

                        if (retrieveResponseTest.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _accessToken = await KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig);
                        }

                        if (retryCount > _retryCounter)
                            throw new Exception("CRM Customer " + retrieveResponseTest.StatusCode + ": " + retrieveResponseTest.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("CRM Customer: " + ex.Message);

                if (retryCount > _retryCounter)
                {
                    throw new Exception("CRM Customer " + ex.Message);
                }
            }
        }

        return null;
    }

    private async Task<HttpResponseMessage> SendHttPrequest(CustomerBase customer, HttpClient httpClient, string content)
    {
        if (customer == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(customer.contactid))
        {
            var ExistingDomainJObject = JsonConvert.DeserializeObject<JObject>(content);

            if (ExistingDomainJObject.ContainsKey("kti_socialchannelorigin"))
            {
                ExistingDomainJObject.Remove("kti_socialchannelorigin");
            }

            if (ExistingDomainJObject.ContainsKey("kti_sourceid"))
            {
                ExistingDomainJObject.Remove("kti_sourceid");
            }

            if (ExistingDomainJObject.ContainsKey("contactid"))
            {
                ExistingDomainJObject.Remove("contactid");
            }

            var ExistingNewContent = JsonConvert.SerializeObject(ExistingDomainJObject);

            var _ExistingcontentNew = new StringContent(ExistingNewContent, Encoding.UTF8, "application/json");
            return await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + $"({customer.contactid})", _ExistingcontentNew);


        }

        var CustomerList = await GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName(customer);


        if (CustomerList is null || CustomerList.Count <= 0)
        {
            var _content = new StringContent(content, Encoding.UTF8, "application/json");
            return await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + $"(kti_sourceid='{customer.kti_sourceid}',kti_socialchannelorigin={customer.kti_socialchannelorigin})", _content);
        }


        var ContactID = "";

        if (CustomerList.Any(customerfromlist => customerfromlist.kti_sourceid == customer.kti_sourceid && customerfromlist.kti_socialchannelorigin == customer.kti_socialchannelorigin))
        {
            ContactID = CustomerList.Where(customerfromlist => customerfromlist.kti_sourceid == customer.kti_sourceid && customerfromlist.kti_socialchannelorigin == customer.kti_socialchannelorigin).FirstOrDefault().contactid;
        }

        if (string.IsNullOrWhiteSpace(ContactID))
        {
            ContactID = CustomerList.FirstOrDefault().contactid;
        }

        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

        if (DomainJObject.ContainsKey("kti_socialchannelorigin"))
        {
            DomainJObject.Remove("kti_socialchannelorigin");
        }

        if (DomainJObject.ContainsKey("kti_sourceid"))
        {
            DomainJObject.Remove("kti_sourceid");
        }

        var NewContent = JsonConvert.SerializeObject(DomainJObject);

        var _contentNew = new StringContent(NewContent, Encoding.UTF8, "application/json");
        return await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + $"({ContactID})", _contentNew);





        //if (!string.IsNullOrWhiteSpace(customer.emailaddress1))
        //{
        //    var ContactID = GetContactBy_Email(customer.emailaddress1).GetAwaiter().GetResult();

        //    if (!string.IsNullOrWhiteSpace(ContactID))
        //    {
        //        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

        //        if (DomainJObject.ContainsKey("kti_socialchannelorigin"))
        //        {
        //            DomainJObject.Remove("kti_socialchannelorigin");
        //        }

        //        if (DomainJObject.ContainsKey("kti_sourceid"))
        //        {
        //            DomainJObject.Remove("kti_sourceid");
        //        }

        //        var NewContent = JsonConvert.SerializeObject(DomainJObject);

        //        var _contentNew = new StringContent(NewContent, Encoding.UTF8, "application/json");
        //        return await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + $"({ContactID})", _contentNew);
        //    }

        //}

        //if (!string.IsNullOrWhiteSpace(customer.mobilephone))
        //{
        //    var ContactID = GetContactBy_Mobile(customer.mobilephone).GetAwaiter().GetResult();

        //    if (!string.IsNullOrWhiteSpace(ContactID))
        //    {
        //        var DomainJObject = JsonConvert.DeserializeObject<JObject>(content);

        //        if (DomainJObject.ContainsKey("kti_socialchannelorigin"))
        //        {
        //            DomainJObject.Remove("kti_socialchannelorigin");
        //        }

        //        if (DomainJObject.ContainsKey("kti_sourceid"))
        //        {
        //            DomainJObject.Remove("kti_sourceid");
        //        }

        //        var NewContent = JsonConvert.SerializeObject(DomainJObject);

        //        var _contentNew = new StringContent(NewContent, Encoding.UTF8, "application/json");
        //        return await httpClient.PatchAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + $"({ContactID})", _contentNew);
        //    }

        //}

    }

    public async Task<string> GetContactBy_Email_MobileNumber_FirstNameLastName(CustomerBase customer)
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

                    var EmailAddressFilter = $"(emailaddress1 eq '{customer.emailaddress1}')";
                    var MobilePhoneFilter = $"(mobilephone eq '{customer.mobilephone}')";
                    var FirstNameLastNameFilter = $"(fullname eq '{customer.firstname} {customer.lastname}')";
                    var SelectClause = "$select=contactid";
                    var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter= ({EmailAddressFilter} or {MobilePhoneFilter}) and {FirstNameLastNameFilter} &{SelectClause} &{Top_Limit}";
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

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.CustomerSearch>(result);

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return default;
                        }

                        foreach (var item in contactResults.values)
                        {
                            return item.contactid;
                        }

                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                {

                    return default;

                }


                //    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return default;
    }

    public async Task<string> GetContactBy_Email(string Email)
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

                    var EmailAddressFilter = $"(emailaddress1 eq '{Email}')";
                    var SelectClause = "$select=contactid";
                    var Top_Limit = "$top=1";


                    var filterQueryString = $"?$filter= {EmailAddressFilter} &{SelectClause} &{Top_Limit}";

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

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.CustomerSearch>(result);

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return default;
                        }

                        foreach (var item in contactResults.values)
                        {
                            return item.contactid;
                        }

                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return default;
    }

    public async Task<string> GetContactBy_Mobile(string Mobile)
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

                    var Filter = $"(mobilephone eq '{Mobile}')";
                    var SelectClause = "$select=contactid";
                    var Top_Limit = "$top=1";


                    var filterQueryString = $"?$filter= {Filter} &{SelectClause} &{Top_Limit}";

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

                        var contactResults = JsonConvert.DeserializeObject<Model.DTO.CustomerSearch>(result);

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return default;
                        }

                        foreach (var item in contactResults.values)
                        {
                            return item.contactid;
                        }

                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return default;
    }

    public async Task<string> GetContactBy_MagentoID_Mobile_Email(string MagentoID, string Mobile, string Email)
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

                    var Filter = $"(kti_magentoid eq '{MagentoID}'  or mobilephone eq '{Mobile}'  or emailaddress1 eq '{Email}')";

                    var SelectClause = "$select=contactid ,kti_magentoid , emailaddress1, mobilephone ";
                    var Top_Limit = "$top=1";
                    var Orderby = "$orderby=kti_magentoid";

                    var filterQueryString = $"?$filter= {Filter} &{SelectClause} &{Top_Limit} &{Orderby}";

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

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return default;
                        }

                        foreach (var item in contactResults.values)
                        {
                            return item.contactid;
                        }

                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return default;
    }

    public async Task<string> GetContactBy_Email_OR_Mobile(string Email, string Mobile)
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

                    var EmailAddressFilter = $"(emailaddress1 eq '{Email}' )";
                    var MobilePhoneFilter = $"(mobilephone eq '{Mobile}')";

                    var SelectClause = "$select=contactid";
                    var Top_Limit = "$top=1";


                    var filterQueryString = $"?$filter= {EmailAddressFilter} or {MobilePhoneFilter}  &{SelectClause} &{Top_Limit}";

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

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return default;
                        }

                        foreach (var item in contactResults.values)
                        {
                            return item.contactid;
                        }

                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return default;
    }


    public async Task<List<CustomerBase>> GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName(object CustomerBase)
    {

        var customer = (CustomerBase)CustomerBase;
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

                    var EmailAddressFilter = $"(emailaddress1 eq '{customer.emailaddress1}')";
                    var MobilePhoneFilter = $"(mobilephone eq '{customer.mobilephone}')";
                    var FirstNameLastNameFilter = $"(fullname eq '{customer.firstname} {customer.lastname}')";
                    var SourceIdWithChannel = $"(kti_socialchannelorigin eq {customer.kti_socialchannelorigin} and kti_sourceid eq '{customer.kti_sourceid}')";

                    var Address1 = $"address1_line1, address1_line2, address1_line3, address1_city, address1_postalcode, address1_telephone1, address1_stateorprovince, telephone1 , address1_country";
                    var Address2 = $"address2_line1, address2_line2, address2_line3, address2_city, address2_postalcode, address2_telephone1, address2_stateorprovince, telephone2 , address2_country";

                    var SelectClause = $"$select= contactid, kti_socialchannelorigin , kti_sourceid ,kti_magentoid, firstname, lastname, emailaddress1, {Address1}, {Address2} ";
                    //  var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter= (({EmailAddressFilter} or {MobilePhoneFilter}) and {FirstNameLastNameFilter} ) or {SourceIdWithChannel} &{SelectClause}";
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

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return new List<CustomerBase>();
                        }

                        return contactResults.values;


                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {

                if (retryCount > _retryCounter)
                {

                    return new List<CustomerBase>();

                }


                //    throw new Exception("CRM Customer " + ex.Message);
                //throw new Exception();
            }

        }

        return default;
    }



    public async Task<List<CustomerBase>> GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName_WithMagentoID(object CustomerBase)
    {

        var customer = (CustomerBase)CustomerBase;
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

                    var EmailAddressFilter = $"(emailaddress1 eq '{customer.emailaddress1}')";
                    var MobilePhoneFilter = $"(mobilephone eq '{customer.mobilephone}')";


                    var MagentoID = $"(kti_magentoid eq '{customer.kti_magentoid}')";


                    var FirstNameLastNameFilter = $"(fullname eq '{customer.firstname} {customer.lastname}')";
                    var SourceIdWithChannel = $"(kti_socialchannelorigin eq {customer.kti_socialchannelorigin} and kti_sourceid eq '{customer.kti_sourceid}')";
                    var Address1 = $"address1_line1, address1_line2, address1_line3, address1_city, address1_postalcode, address1_telephone1, address1_stateorprovince, telephone1 , address1_country";
                    var Address2 = $"address2_line1, address2_line2, address2_line3, address2_city, address2_postalcode, address2_telephone1, address2_stateorprovince, telephone2 , address2_country";

                    var SelectClause = $"$select= contactid, kti_socialchannelorigin , kti_sourceid ,kti_magentoid, firstname, lastname, emailaddress1, ncci_newclubmembershipid, mobilephone , {Address1}, {Address2} ";
                    //  var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter= (({EmailAddressFilter} or {MobilePhoneFilter}) and {FirstNameLastNameFilter} ) or {SourceIdWithChannel} or {MagentoID} &{SelectClause}";
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

                        if (contactResults is null || contactResults.values is null || contactResults.values.Count <= 0)
                        {
                            return new List<CustomerBase>();
                        }

                        return contactResults.values;


                        //mooResponse = await mooAsyncRepo.AddAsync(_uom);              
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CRM Customer " + ex.Message);
                //if (retryCount > _retryCounter)
                //{

                //    //return new List<CustomerBase>();
                //    throw new Exception("CRM Customer " + ex.Message);
                //}
            }

        }

        return default;
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

                    var SelectClause = $"$select= createdon, modifiedon , contactid, kti_socialchannelorigin , kti_sourceid , firstname, lastname, emailaddress1, mobilephone, {Address1}, {Address2} ";
                    //  var Top_Limit = "$top=1";

                    var filterQueryString = $"?$filter=( (createdon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} ) or (modifiedon ge {DateStart:yyyy-MM-ddTHH:mm:ssZ} and createdon le {DateEnd:yyyy-MM-ddTHH:mm:ssZ} )   )&$orderby=createdon asc&{SelectClause}";
                    //  var filterQueryString = $"?$filter= ({EmailAddressFilter} or {MobilePhoneFilter}) &{SelectClause} &{Top_Limit}";

                    HttpResponseMessage retrieveResponseTest = await httpClient.GetAsync(_crmConfig.crm_api_path + _crmConfig.crm_customer_path + filterQueryString);

                    if (!retrieveResponseTest.IsSuccessStatusCode)
                    {
                        retryCount++;

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
                    throw new Exception("CRM Customer " + ex.Message);
                }


                //   
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
                    throw new Exception("CRM Customer " + ex.Message);
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


    public CustomerSearch Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
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
