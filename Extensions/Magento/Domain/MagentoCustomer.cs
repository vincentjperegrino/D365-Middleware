using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Service;
using KTI.Moo.Extensions.Magento.Model.DTO.Customers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Model.Helpers;
using KTI.Moo.Extensions.Magento.Helper;
using KTI.Moo.Extensions.Magento.Model.DTO.Validation;
using Azure;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Magento.Domain;

public class Customer : ICustomer<Model.Customer>, ISearch<Model.DTO.Customers.Search, Model.Customer>
{

    private readonly IMagentoService _service;
    public const string APIDirectory = "/customers";
    private readonly string _companyid;

    public Customer(Config config)
    {
        this._service = new MagentoService(config);
        _companyid = config.companyid;
    }

    public Customer(Config config, IDistributedCache cache)
    {
        this._service = new MagentoService(config, cache);
        _companyid = config.companyid;
    }

    public Customer(Config config, IDistributedCache cache, ILogger log)
    {
        this._service = new MagentoService(config, cache, log);
        _companyid = config.companyid;
    }

    public Customer(string defaultDomain, string redisConnectionString, string username, string password)
    {
        this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);
    }

    public Customer(IMagentoService service)
    {
        this._service = service;
    }


    public Model.Customer Add(Model.Customer customerDetails)
    {
        if (customerDetails is null)
        {
            throw new ArgumentNullException(nameof(customerDetails));
        }

        try
        {
            var path = APIDirectory;

            var method = "POST";

            var isAuthenticated = true;


            Model.DTO.Customers.Add CustomerAddModel = new();

            CustomerAddModel.Customer = customerDetails;
            CustomerAddModel.password = customerDetails.password;


            var stringContent = GetContent(CustomerAddModel);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

            if (!string.IsNullOrWhiteSpace(response) && ReturnCustomerData.customer_id == 0)
            {
                throw new System.Exception(response);
            }

            return ReturnCustomerData;
        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + APIDirectory;

            var classname = "MagentoCustomer, Method: Add";

            throw new MagentoIntegrationException(domain, classname, ex.Message);

        }
    }




    public Model.Customer Add(string FromDispatcherQueue)
    {
        if (string.IsNullOrWhiteSpace(FromDispatcherQueue))
        {
            throw new ArgumentException("Invalid FromDispatcherQueue", nameof(FromDispatcherQueue));
        }

        try
        {
            var path = APIDirectory;

            var method = "POST";

            var isAuthenticated = true;

            var customerDetails = JsonConvert.DeserializeObject<dynamic>(FromDispatcherQueue);

            Model.DTO.Customers.ForDispatchers.Add CustomerAddModel = new();

            CustomerAddModel.customer = customerDetails;
            CustomerAddModel.password = customerDetails.password is not null ? customerDetails.password : string.Empty;


            var stringContent = GetContent(CustomerAddModel);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

            if (!string.IsNullOrWhiteSpace(response) && ReturnCustomerData.customer_id == 0)
            {
                throw new System.Exception(response);
            }

            return ReturnCustomerData;

        }
        catch (System.Exception ex)
        {

            var domain = _service.DefaultURL + APIDirectory;

            var classname = "MagentoCustomer, Method: Add";

            throw new MagentoIntegrationException(domain, classname, ex.Message);
        }

    }

    public Model.Customer UpdateWithModel(string FromDispatcherQueue, int customerid)
    {
        if (string.IsNullOrWhiteSpace(FromDispatcherQueue))
        {
            throw new ArgumentException("Invalid FromDispatcherQueue", nameof(FromDispatcherQueue));
        }

        if (customerid <= 0)
        {
            throw new ArgumentException("Invalid customerid", nameof(customerid));
        }

        try
        {
            var customerDetails = JsonConvert.DeserializeObject<dynamic>(FromDispatcherQueue);

            var path = APIDirectory + "/" + customerid;
            var method = "PUT";
            var isAuthenticated = true;

            Model.DTO.Customers.ForDispatchers.Update CustomerUpdateModel = new();

            CustomerUpdateModel.customer = customerDetails;
            CustomerUpdateModel.passwordHash = customerDetails.password is not null ? customerDetails.password : string.Empty;

            var stringContent = GetContent(CustomerUpdateModel);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

            //dynamic MagentoCustomerData = JsonConvert.DeserializeObject(response);
            //ReturnCustomerData.ParseFromMagento(MagentoCustomerData);

            if (ReturnCustomerData.customer_id == 0)
            {
                throw new System.Exception(response);
            }

            return ReturnCustomerData;
        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + APIDirectory + "/" + customerid;

            var classname = "MagentoCustomer, Method: Update";

            throw new MagentoIntegrationException(domain, classname, ex.Message);
        }


    }



    public bool Delete(int customerID)
    {

        if (customerID <= 0)
        {
            throw new ArgumentException("Invalid customerid", nameof(customerID));
        }

        try
        {
            var path = APIDirectory + "/" + customerID.ToString();

            var isAuthenticated = true;

            var method = "DEL";

            var response = _service.ApiCall(path, method, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var returnResponse = JsonConvert.DeserializeObject<bool>(response, settings);

            if (returnResponse == false)
            {
                throw new System.Exception(response);
            }

            return returnResponse;

        }
        catch (System.Exception ex)
        {

            var domain = _service.DefaultURL + APIDirectory + "/" + customerID.ToString();

            var classname = "MagentoCustomer, Method: Delete";

            throw new MagentoIntegrationException(domain, classname, ex.Message);

        }
    }

    public Model.Customer Get(string customerID)
    {
        if (int.TryParse(customerID, out var intcustomer))
        {
            return Get(intcustomer);
        }

        return new Model.Customer();
    }

    public Model.Customer Get(int customerID)
    {
        if (customerID <= 0)
        {
            throw new ArgumentException("Invalid customerID", nameof(customerID));
        }

        try
        {
            string path = APIDirectory + "/" + customerID.ToString();

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
        catch
        {
            return new Model.Customer();
        }
    }


    public Model.Customer Get(int customerID, ILogger log)
    {
        if (customerID <= 0)
        {
            throw new ArgumentException("Invalid customerID", nameof(customerID));
        }

        try
        {
            string path = APIDirectory + "/" + customerID.ToString();

            bool isAuthenticated = true;
            string method = "GET";

            var response = _service.ApiCall(path, method, isAuthenticated);

            log.LogInformation(response);

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
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            return new Model.Customer();
        }
    }


    public bool Update(Model.Customer customerDetails)
    {
        UpdateWithModel(customerDetails);
        return true;

    }

    public Model.Customer UpdateWithModel(Model.Customer customerDetails)
    {
        if (customerDetails is null)
        {
            throw new ArgumentNullException(nameof(customerDetails));
        }

        try
        {
            var path = APIDirectory + "/" + customerDetails.customer_id;
            var method = "PUT";
            var isAuthenticated = true;

            Model.DTO.Customers.Update CustomerUpdateModel = new();

            CustomerUpdateModel.Customer = customerDetails;
            CustomerUpdateModel.passwordHash = customerDetails.password;

            var stringContent = GetContent(CustomerUpdateModel);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnCustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

            //dynamic MagentoCustomerData = JsonConvert.DeserializeObject(response);
            //ReturnCustomerData.ParseFromMagento(MagentoCustomerData);

            if (ReturnCustomerData.customer_id == 0)
            {
                throw new System.Exception(response);
            }

            return ReturnCustomerData;
        }
        catch (System.Exception ex)
        {

            var domain = _service.DefaultURL + APIDirectory + "/" + customerDetails.customer_id;

            var classname = "MagentoCustomer, Method: Update";

            throw new MagentoIntegrationException(domain, classname, ex.Message);

        }


    }

    public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        return GetSearchCustomers(dateFrom, dateTo, pagesize, currentpage);
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

            var currentItemCountCovered = pagesize * currentpage;

            if (currentPageSearch.total_count > currentItemCountCovered)
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


    public List<Model.Customer> GetCustomers(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
    {
        var returnResult = GetSearchCustomers(dateFrom, dateTo, pagesize, currentPage);
        return returnResult.values;
    }

    public Search GetSearchCustomers(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
    {

        if (pagesize <= 0 || currentPage <= 0)
        {
            var message = "GetCustomers: pagesize or currentPage can't be below zero.";

            throw new ArgumentException(message);

        }

        try
        {
            var searchPath = "/search";
            var path = APIDirectory + searchPath;
            var method = "GET";
            var isAuthenticated = true;

            Dictionary<string, string> parameters = new();

            parameters.AddSearchParameterRangeUpdatedDate(dateFrom, dateTo, pagesize, currentPage);

            var response = _service.ApiCall(path, method, parameters, isAuthenticated);

            var result = SearchReponse(response);

            _ = int.TryParse(_companyid, out var companyid);

            if (companyid != 0 && result.values is not null && result.values.Count > 0)
            {

                result.values.Select(customer =>
                {
                    customer.companyid = companyid;
                    return customer;
                }).ToList();
            }

            return result;
        }
        catch
        {
            return new Search();
        }
    }

    public Model.Customer GetCustomersWithEmail(string email)
    {
        var result = GetSearchCustomersWithEmail(email);

        var searchCustomer = result.values;

        if (searchCustomer is not null && searchCustomer.Count > 0)
        {
            return searchCustomer.FirstOrDefault();
        }

        return new Model.Customer();

    }

    public Search GetSearchCustomersWithEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Invalid email", nameof(email));
        }

        try
        {
            var searchPath = "/search";
            var path = APIDirectory + searchPath;
            var method = "GET";
            var isAuthenticated = true;

            Dictionary<string, string> parameters = new();

            parameters.AddSearchParameterEmail(email);

            var response = _service.ApiCall(path, method, parameters, isAuthenticated);

            var result = SearchReponse(response);

            _ = int.TryParse(_companyid, out var companyid);

            if (companyid != 0 && result.values is not null && result.values.Count > 0)
            {
                result.values.Select(customer =>
                {
                    customer.companyid = companyid;
                    return customer;

                }).ToList();
            }

            return result;
        }
        catch
        {
            return new Search();
        }


    }

    public Model.Customer GetByField(string FieldName, string FieldValue)
    {
        try
        {
            var searchPath = "/search";
            var path = APIDirectory + searchPath;
            var method = "GET";
            var isAuthenticated = true;

            Dictionary<string, string> parameters = new();

            List<SearchParameters> searchParameters = new()
            {
                new SearchParameters()
                {
                    field = FieldName,
                    value = FieldValue,
                    condition_type = "eq",
                    filter_groups = 0,
                    filters = 0
                }
            };

            parameters.AddSearchParameters(searchParameters);

            var response = _service.ApiCall(path, method, parameters, isAuthenticated);

            var result = SearchReponse(response);

            _ = int.TryParse(_companyid, out var companyid);

            if (companyid != 0 && result.values is not null && result.values.Count > 0)
            {
                result.values = result.values.Select(customer =>
                {
                    customer.companyid = companyid;
                    return customer;
                }).ToList();
            }

            if (result.values is not null && result.values.Count > 0)
            {
                return result.values.FirstOrDefault();
            }

            return new Model.Customer();

        }
        catch
        {
            return new Model.Customer();
        }

    }



    private static Search SearchReponse(string response)
    {

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        Search SearchResult = JsonConvert.DeserializeObject<Search>(response, settings);

        var result = SearchResult;

        return result;


    }

    private static StringContent GetContent(object models)
    {

        var JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return new StringContent(json, Encoding.UTF8, "application/json");

    }

    public Model.Customer Upsert(Model.Customer customerDetails)
    {
        if (customerDetails is null)
        {
            var message = "Upsert: customerDetails is null.";

            throw new ArgumentException(message);
        }

        if (customerDetails.customer_id > 0 && string.IsNullOrWhiteSpace(customerDetails.email))
        {
            var message = "Upsert: customer_id and email cant be both null.";

            throw new ArgumentException(message);
        }

        if (customerDetails.customer_id > 0)
        {
            return UpsertID(customerDetails);
        }

        return UpsertEmail(customerDetails);
    }


    public Model.Customer UpsertEmail(Model.Customer customerDetails)
    {

        var search = GetSearchCustomersWithEmail(customerDetails.email);

        var searchCustomer = search.values;

        if (searchCustomer is not null && searchCustomer.Count > 0)
        {

            var ExistingCustomer = searchCustomer.FirstOrDefault();

            customerDetails.customer_id = ExistingCustomer.customer_id;

            return UpdateWithModel(customerDetails);
        }

        return Add(customerDetails);
    }

    public Model.Customer UpsertID(Model.Customer customerDetails)
    {

        var search = Get(customerDetails.customer_id);

        if (search.customer_id == customerDetails.customer_id)
        {
            return UpdateWithModel(customerDetails);
        }

        return Add(customerDetails);
    }



    public Model.Customer UpsertID(string FromDispatcherQueue, int customerid)
    {
        if (customerid > 0)
        {
            return UpdateWithModel(FromDispatcherQueue, customerid);
        }

        return new Model.Customer();
    }

}
