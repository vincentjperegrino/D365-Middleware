
using KTI.Moo.Extensions.SAP.Exception;
using KTI.Moo.Extensions.SAP.Model.DTO.Customers;
using KTI.Moo.Extensions.SAP.Service;
using System.Net.Http;

namespace KTI.Moo.Extensions.SAP.Domain;

public class Customer : Core.Domain.ICustomer<Model.Customer>, Core.Domain.ISearch<Model.DTO.Customers.Search, Model.Customer>
{

    private readonly ISAPService _service;
    public const string APIDirectory = "/BusinessPartners";
    private readonly string _companyid;

    public Customer(Config config)
    {
        this._service = new SAPService(config);
        _companyid = config.companyid;
    }

    public Customer(string defaultDomain, string redisConnectionString, string username, string password, string companydb)
    {
        this._service = new SAPService(defaultDomain, redisConnectionString, username, password, companydb);
    }

    public Customer(ISAPService service)
    {
        this._service = service;
    }


    public Model.Customer Add(Model.Customer customerDetails)
    {
        var path = APIDirectory;

        var method = "POST";

        var isAuthenticated = true;

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var customerDetailsJSON = JsonConvert.SerializeObject(customerDetails, settings);

        var CustomerAddModel = JsonConvert.DeserializeObject<Model.DTO.Customers.Upsert>(customerDetailsJSON, settings);

        CustomerAddModel.Addresses = customerDetails.Addresses;

        var stringContent = GetContent(CustomerAddModel);

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

        if (string.IsNullOrWhiteSpace(ReturnCustomerData.kti_sapbpcode))
        {

            var domain = _service.DefaultURL + path;

            var classname = "SAPCustomer, Method: Add";

            throw new SAPIntegrationException(domain, classname, response);

        }

        return ReturnCustomerData;
    }




    public bool Delete(int customerID)
    {
        throw new NotImplementedException();
    }

    public Model.Customer Get(int customerID)
    {
        throw new NotImplementedException();
    }

    public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        return GetBySearch(dateFrom, dateTo, pagesize, currentpage);
    }

    public List<Model.Customer> GetAll(List<Model.Customer> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        if (initialList is null)
        {
            initialList = new List<Model.Customer>();
        }

        var currentPageSearch = Get(dateFrom, dateTo, pagesize, currentpage);

        if (currentPageSearch is not null && currentPageSearch.values is not null && currentPageSearch.values.Count > 0)
        {
            initialList.AddRange(currentPageSearch.values);

            if (!string.IsNullOrWhiteSpace(currentPageSearch.nextLink))
            {
                GetAll(initialList, dateFrom, dateTo, pagesize, ++currentpage);
            }
        }

        return initialList;
    }

    public List<Model.Customer> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
    {
        var initialList = new List<Model.Customer>();
        return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
    }


    public Search GetBySearch(DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        if (pagesize <= 0)
        {
            throw new System.Exception("Invalid Page size");
        }

        if (currentpage <= 0)
        {
            throw new System.Exception("Invalid Current page");
        }

        //always 20
        pagesize = 20;

        var skip = pagesize * (currentpage - 1);


        string path = $"{APIDirectory}?$filter= (CreateDate ge '{dateFrom:yyyy-MM-ddTHH:mm:ss}' and CreateDate le '{dateTo:yyyy-MM-ddTHH:mm:ss}') or (UpdateDate ge '{dateFrom:yyyy-MM-ddTHH:mm:ss}' and UpdateDate le '{dateTo:yyyy-MM-ddTHH:mm:ss}') &$skip={skip}";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnDTO = JsonConvert.DeserializeObject<Model.DTO.Customers.Search>(response, settings);


        if (ReturnDTO is null || ReturnDTO.values is null || ReturnDTO.values.Count == 0)
        {
            return new Model.DTO.Customers.Search();
        }

        return ReturnDTO;
    }

    /// <summary>
    /// Get by Custom Search
    /// </summary>
    /// <param name="FieldName">Variable name in SAP that will be search. Case sensitive</param>
    /// <param name="FieldValue">Variable value</param>
    /// <returns>Top 1 only</returns>
    public Model.Customer GetByField(string FieldName, string FieldValue)
    {
        string path = $"{APIDirectory}?$filter={FieldName} eq '{FieldValue}'&$top=1";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnDTO = JsonConvert.DeserializeObject<Model.DTO.Customers.Search>(response, settings);

        if (ReturnDTO is null || ReturnDTO.values is null || ReturnDTO.values.Count <= 0)
        {
            return new Model.Customer();
        }

        var ReturnCustomerData = ReturnDTO.values.FirstOrDefault();

        _ = int.TryParse(_companyid, out var companyid);

        if (companyid != 0)
        {
            ReturnCustomerData.companyid = companyid;
        }

        return ReturnCustomerData;

    }


    public Model.Customer GetByEmail(string EmailAddress)
    {
        var ReturnCustomerData = GetByField(FieldName: "EmailAddress", FieldValue: EmailAddress);
        return ReturnCustomerData;
    }

    public Model.Customer Get(string SapCardCode)
    {
        string path = $"{APIDirectory}('{SapCardCode}')";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);


        _ = int.TryParse(_companyid, out var companyid);
        if (companyid != 0)
        {
            ReturnCustomerData.companyid = companyid;
        }

        return ReturnCustomerData;
    }

    public bool Update(Model.Customer customerDetails)
    {
        var path = $"{APIDirectory}('{customerDetails.kti_sapbpcode}')";
        //Patch is all Update
        var method = "PATCH";

        var isAuthenticated = true;

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var customerDetailsJSON = JsonConvert.SerializeObject(customerDetails, settings);

        var CustomerAddModel = JsonConvert.DeserializeObject<Model.DTO.Customers.Upsert>(customerDetailsJSON, settings);

        CustomerAddModel.Addresses = customerDetails.Addresses;

        var stringContent = GetContent(CustomerAddModel);

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        if (string.IsNullOrWhiteSpace(response))
        {
            return true;
        }

        var domain = _service.DefaultURL + path;

        var classname = "SAPCustomer, Method: Update";

        throw new SAPIntegrationException(domain, classname, response);

    }

    public Model.Customer Upsert(Model.Customer customerDetails)
    {

        var Existingcustomer = Get(customerDetails.kti_sapbpcode);

        //if (string.IsNullOrWhiteSpace(Existingcustomer.kti_sapcardcode))
        //{
        //    Existingcustomer = GetByEmail(customerDetails.EmailAddress);
        //    customerDetails.kti_sapcardcode = Existingcustomer.kti_sapcardcode;
        //}

        if (string.IsNullOrWhiteSpace(Existingcustomer.kti_sapbpcode))
        {
            return Add(customerDetails);
        }

        Update(customerDetails);

        return Get(customerDetails.kti_sapbpcode);

    }



    public Model.Customer UpsertbyEmail(Model.Customer customerDetails)
    {
        var Existingcustomer = GetByEmail(customerDetails.EmailAddress);

        if (string.IsNullOrWhiteSpace(Existingcustomer.kti_sapbpcode))
        {
            return Add(customerDetails);
        }

        customerDetails.kti_sapbpcode = Existingcustomer.kti_sapbpcode;
        Update(customerDetails);

        return Get(customerDetails.kti_sapbpcode);

    }


    public Model.Customer UpsertbyID(Model.Customer customerDetails)
    {
        var Existingcustomer = Get(customerDetails.kti_sapbpcode);

        if (string.IsNullOrWhiteSpace(Existingcustomer.kti_sapbpcode))
        {
            return Add(customerDetails);
        }

        Update(customerDetails);

        return Get(customerDetails.kti_sapbpcode);

    }


    private static StringContent GetContent(object models)
    {

        var JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return new StringContent(json, Encoding.UTF8, "application/json");

    }


    public Model.Customer Upsert(string FromDispatcherQueue, string sapbpcode)
    {
        //  var customerDetails = JsonConvert.DeserializeObject<Model.Customer>(FromDispatcherQueue);

        if (string.IsNullOrWhiteSpace(sapbpcode))
        {
            return Add(FromDispatcherQueue);
        }

        Update(FromDispatcherQueue, sapbpcode);

        return Get(sapbpcode);

    }



    public Model.Customer Add(string FromDispatcherQueue)
    {
        var path = APIDirectory;

        var method = "POST";

        var isAuthenticated = true;

        var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

        if (string.IsNullOrWhiteSpace(ReturnCustomerData.kti_sapbpcode))
        {
            var domain = _service.DefaultURL + path;
            var classname = "SAPCustomer, Method: Add";
            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnCustomerData;
    }

    public bool Update(string FromDispatcherQueue, string sapbpcode)
    {

        var path = $"{APIDirectory}('{sapbpcode}')";
        //Patch is all Update
        var method = "PATCH";

        var isAuthenticated = true;

        var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        if (string.IsNullOrWhiteSpace(response))
        {
            return true;
        }

        var domain = _service.DefaultURL + path;

        var classname = "SAPCustomer, Method: Update";

        throw new SAPIntegrationException(domain, classname, response);

    }


}
