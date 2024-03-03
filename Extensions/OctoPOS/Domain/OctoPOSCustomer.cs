using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Exception;
using KTI.Moo.Extensions.OctoPOS.Model;
using KTI.Moo.Extensions.OctoPOS.Model.DTO.Customers;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Domain
{
    public class Customer : ICustomer<Model.Customer>, ISearch<Model.DTO.Customers.Search, Model.Customer>
    {

        private readonly IOctoPOSService _service;
        public const string APIDirectory = "/customer";
        private readonly string _companyid;


        public Customer(Config config, IDistributedCache cache)
        {
            this._service = new OctoPOSService(cache, config);
            _companyid = config.companyid;
        }

        public Customer(IOctoPOSService service)
        {
            this._service = service;
        }


        public Model.Customer Get(string customerCode)
        {
            if (string.IsNullOrWhiteSpace(customerCode))
            {
                throw new ArgumentException("Invalid customerCode", nameof(customerCode));
            }

            try
            {
                var path = APIDirectory + "/" + customerCode;

                var isAuthenticated = true;
                var method = "GET";

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

        public Model.Customer Add(Model.Customer customerDetails)
        {
            if (customerDetails is null)
            {
                throw new ArgumentNullException(nameof(customerDetails));
            }

            try
            {
                var CustomerChecker = Get(customerDetails.CustomerCode);

                if (!string.IsNullOrWhiteSpace(CustomerChecker.CustomerCode))
                {
                    var errormesage = "Customer already exist";
                    throw new System.Exception(errormesage);
                }

                var ReturnCustomer = AddUpdateCustomer(customerDetails, Method: "Add");

                return ReturnCustomer;
            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + "add";
                var classname = "OctoPOSCustomer, Method: Add";
                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }
        }

        public bool Update(Model.Customer customerDetails)
        {
            if (customerDetails is null)
            {
                throw new ArgumentNullException(nameof(customerDetails));
            }

            try
            {
                var CustomerChecker = Get(customerDetails.CustomerCode);

                if (string.IsNullOrWhiteSpace(CustomerChecker.CustomerCode))
                {
                    var errormesage = "Customer does not exist";
                    throw new System.Exception(errormesage);
                }

                var returnedCustomerData = AddUpdateCustomer(customerDetails, Method: "Update");

                if (string.IsNullOrWhiteSpace(returnedCustomerData.CustomerCode))
                {
                    return false;
                }

                return true;
            }

            catch (System.Exception ex)
            {

                var domain = _service.DefaultURL + "add";
                var classname = "OctoPOSCustomer, Method: Update";
                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }

        }


        public Model.Customer Upsert(Model.Customer customerDetails)
        {
            if (customerDetails is null)
            {
                throw new ArgumentNullException(nameof(customerDetails));
            }

            return AddUpdateCustomer(customerDetails: customerDetails, Method: "Upsert");
        }

        public Model.Customer Upsert(string FromDispatcherQueue)
        {
            if (string.IsNullOrWhiteSpace(FromDispatcherQueue))
            {
                throw new ArgumentException("Invalid FromDispatcherQueue", nameof(FromDispatcherQueue));
            }

            return AddUpdateCustomer(FromDispatcherQueue, Method: "Upsert");
        }


        public Model.Customer Get(int customerID)
        {
            if (customerID <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerID));
            }

            return Get(customerID.ToString());
        }


        private Model.Customer AddUpdateCustomer(string FromDispatcherQueue, string Method)
        {
            try
            {
                var path = APIDirectory + "/add";

                var isAuthenticated = true;
                var method = "POST";

                var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var CustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);

                if (string.IsNullOrWhiteSpace(CustomerData.CustomerCode))
                {
                    throw new System.Exception(response);
                }

                return CustomerData;

            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + APIDirectory + "/add";

                var classname = "OctoPOSCustomer, Method: " + Method;

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }


        }


        private Model.Customer AddUpdateCustomer(Model.Customer customerDetails, string Method)
        {
            try
            {

                var path = APIDirectory + "/add";

                var isAuthenticated = true;
                var method = "POST";

                var stringContent = GetContent(customerDetails);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var CustomerData = JsonConvert.DeserializeObject<Model.Customer>(response, settings);


                if (string.IsNullOrWhiteSpace(CustomerData.CustomerCode))
                {
                    throw new System.Exception(response);
                }


                return CustomerData;

            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + APIDirectory + "/add";

                var classname = "OctoPOSCustomer, Method: " + Method;

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }


        }



        public Model.Customer GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Invalid email", nameof(email));
            }

            try
            {
                Model.DTO.Customers.SearchParameters CustomerSearchParamameters = new()
                {
                    Email = email
                };

                var customersearch = GetSearchListByDate(CustomerSearchParamameters);

                if (customersearch.values is null || customersearch.values.Count <= 0)
                {
                    return new Model.Customer();
                }

                return customersearch.values.FirstOrDefault();
            }
            catch
            {
                return new Model.Customer();
            }
        }

        public Model.Customer GetByPhone(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentException("Invalid mobile", nameof(mobile));
            }

            try
            {
                Model.DTO.Customers.SearchParameters CustomerSearchParamameters = new()
                {
                    HandPhone = mobile
                };

                var customersearch = GetSearchListByDate(CustomerSearchParamameters);

                if (customersearch.values is null || customersearch.values.Count <= 0)
                {
                    return new Model.Customer();
                }

                return customersearch.values.FirstOrDefault();
            }
            catch
            {
                return new Model.Customer();
            }
        }

        public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
        {
            try
            {
                return GetSearchListByDate(dateFrom, dateTo, currentPage);
            }
            catch
            {
                return new Search();
            }

        }

        public List<Model.Customer> GetAll(List<Model.Customer> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
        {
            try
            {
                if (initialList is null)
                {
                    initialList = new List<Model.Customer>();
                }

                var currentPageSearch = Get(dateFrom, dateTo, pagesize, currentpage);

                if (currentPageSearch is not null && currentPageSearch.values is not null && currentPageSearch.values.Count > 0)
                {
                    initialList.AddRange(currentPageSearch.values);

                    if (currentPageSearch.total_pages > currentpage)
                    {
                        GetAll(initialList, dateFrom, dateTo, pagesize, ++currentpage);
                    }
                }

                return initialList;

            }
            catch
            {
                return new List<Model.Customer>();
            }
        }

        public List<Model.Customer> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
        {
            try
            {
                var initialList = new List<Model.Customer>();
                return GetAll(initialList, dateFrom, dateTo, pagesize, 1);

            }
            catch
            {
                return new List<Model.Customer>();
            }
        }


        public List<Model.Customer> GetListByDate(DateTime dateFrom, DateTime dateTo, int currentPage)
        {
            try
            {
                var GetList = GetSearchListByDate(dateFrom, dateTo, currentPage);

                if (GetList.values is null || GetList.values.Count <= 0)
                {
                    return new List<Model.Customer>();
                }

                return GetList.values;
            }
            catch
            {
                return new List<Model.Customer>();
            }
        }

        public Model.DTO.Customers.Search GetSearchListByDate(DateTime dateFrom, DateTime dateTo, int currentPage)
        {
            try
            {
                if (currentPage < 1)
                {
                    var message = "GetList: currentPage can't be below zero.";
                    throw new ArgumentException(message);
                }

                Model.DTO.Customers.SearchParameters CustomerSearchParamameters = new()
                {
                    Pageno = currentPage,
                    StartDate = dateFrom,
                    EndDate = dateTo,
                    LastEditDateTime = dateFrom,
                };

                return GetSearchListByDate(CustomerSearchParamameters);
            }
            catch
            {
                return new Search();
            }
        }

        public Model.DTO.Customers.Search GetSearchListByDate(Model.DTO.Customers.SearchParameters CustomerSearchParamameters)
        {

            if (CustomerSearchParamameters is null)
            {
                throw new ArgumentNullException(nameof(CustomerSearchParamameters));
            }

            if (CustomerSearchParamameters.Pageno < 1)
            {
                var message = "GetList: currentPage can't be below zero.";

                throw new ArgumentException(message);

            }

            try
            {
                var path = APIDirectory + "/list";
                var isAuthenticated = true;
                var method = "POST";

                Dictionary<string, string> parameters = new()
                {
                    { "page", CustomerSearchParamameters.Pageno.ToString() }
                };

                var stringContent = GetContent(CustomerSearchParamameters);

                var response = _service.ApiCall(path, method, parameters, stringContent, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var SearchData = JsonConvert.DeserializeObject<Model.DTO.Customers.Search>(response, settings);

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0 && SearchData.values is not null && SearchData.values.Count > 0)
                {
                    SearchData.values.Select(customer =>
                    {
                        customer.companyid = companyid;
                        return customer;
                    }).ToList();
                }

                return SearchData;
            }
            catch
            {
                return new Search();
            }
        }

        public bool Delete(int customerID)
        {
            throw new NotImplementedException();
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




        //public Model.Customer Upsert(Model.Customer customerDetails)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
