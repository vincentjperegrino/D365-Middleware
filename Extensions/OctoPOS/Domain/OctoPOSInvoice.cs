using Azure;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Exception;
using KTI.Moo.Extensions.OctoPOS.Helper;
using KTI.Moo.Extensions.OctoPOS.Model;
using KTI.Moo.Extensions.OctoPOS.Model.DTO.Invoices;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Domain
{
    public class Invoice : IInvoice<Model.Invoice, Model.InvoiceItem>, ISearch<Model.DTO.Invoices.Search, Model.Invoice>
    {

        private readonly IOctoPOSService _service;
        public const string APIDirectory = "/invoice";
        private readonly string _companyid;


        public Invoice(Config config, IDistributedCache cache)
        {
            this._service = new OctoPOSService(cache, config);
            this._companyid = config.companyid;

        }

        public Invoice(IOctoPOSService service)
        {
            this._service = service;
        }



        public bool Add(Model.Invoice InvoiceModel)
        {
            if (InvoiceModel is null)
            {
                throw new ArgumentNullException(nameof(InvoiceModel));
            }

            try
            {
                var path = APIDirectory + "/add";

                var isAuthenticated = true;
                var method = "POST";

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };

                var InvoiceDetailsJSON = JsonConvert.SerializeObject(InvoiceModel, settings);

                var InvoiceAddModel = JsonConvert.DeserializeObject<Model.DTO.Invoices.Add>(InvoiceDetailsJSON, settings);

                InvoiceAddModel.CustomerDetails = InvoiceModel.CustomerDetails;


                var stringContent = GetContent(InvoiceAddModel);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                if (response != "OK")
                {
                    throw new System.Exception(response);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                string domain = _service.DefaultURL + APIDirectory + "/add";

                string classname = "OctoPOSInvoice, Method: add";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }

        }

        public virtual Model.Invoice Get(string invoiceID)
        {
            if (string.IsNullOrWhiteSpace(invoiceID))
            {
                throw new ArgumentException("Invalid invoiceID", nameof(invoiceID));
            }

            try
            {

                string path = APIDirectory + "/details/" + invoiceID;

                bool isAuthenticated = true;
                string method = "POST";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);

                if (HasInvoiceValidInvoiceItem(ReturnInvoiceData))
                {
                    ReturnInvoiceData.InvoiceItems = AddInvoiceIdToInvoiceItems(ReturnInvoiceData.InvoiceItems, ReturnInvoiceData.kti_sourceid);
                }

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0)
                {
                    ReturnInvoiceData.companyid = companyid;
                }

                return ReturnInvoiceData;

            }
            catch
            {
                return new Model.Invoice();
            }

        }

        private static bool HasInvoiceValidInvoiceItem(Model.Invoice ReturnInvoiceData)
        {
            return ReturnInvoiceData.InvoiceItems is not null && ReturnInvoiceData.InvoiceItems.Count > 0;
        }

        private static List<Model.InvoiceItem> AddInvoiceIdToInvoiceItems(List<Model.InvoiceItem> ReturnInvoiceDataItems, string invoiceId)
        {
            return ReturnInvoiceDataItems.Select(invoiceitems =>
                                          {
                                              invoiceitems.invoiceid = invoiceId;
                                              return invoiceitems;
                                          }).ToList();
        }

        public List<Model.Invoice> SearchInvoiceList(DateTime startdate, DateTime enddate)
        {
            try
            {
                var searchModel = new Model.DTO.Invoices.SearchParameters()
                {
                    startDate = startdate,
                    endDate = enddate,
                };

                return SearchInvoiceList(searchModel).values;
            }
            catch
            {

                return new List<Model.Invoice>();
            }

        }

        public virtual Model.DTO.Invoices.Search SearchInvoiceListWithdetails(DateTime dateFrom, DateTime dateTo, int currentPage)
        {
            try
            {
                var searchModel = new Model.DTO.Invoices.SearchParameters()
                {
                    startDate = dateFrom,
                    endDate = dateTo,
                    pageno = currentPage
                };

                return SearchInvoiceList(searchModel);
            }
            catch
            {
                return new Search();
            }
        }

        public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
        {
            try
            {
                return SearchInvoiceListWithdetails(dateFrom, dateTo, currentPage);
            }
            catch
            {
                return new Search();
            }

        }

        public List<Model.Invoice> GetAll(List<Model.Invoice> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
        {
            try
            {
                if (initialList is null)
                {
                    initialList = new List<Model.Invoice>();
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
                return new List<Model.Invoice>();
            }
        }

        public List<Model.Invoice> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
        {
            try
            {
                var initialList = new List<Model.Invoice>();
                return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
            }
            catch
            {
                return new List<Model.Invoice>();
            }
        }


        /// <summary>
        /// Manual Setting of Parameters
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Model.DTO.Invoices.Search SearchInvoiceList(Model.DTO.Invoices.SearchParameters search)
        {
            if (search is null)
            {
                throw new ArgumentNullException(nameof(search));
            }
            try
            {
                string path = APIDirectory + "/list/";

                bool isAuthenticated = true;
                string method = "POST";

                var stringContent = GetContent(search);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var resultInvoiceList = JsonConvert.DeserializeObject<Model.DTO.Invoices.Search>(response, settings);

                if (resultInvoiceList.values is null || resultInvoiceList.values.Count <= 0)
                {
                    return new Model.DTO.Invoices.Search();
                }


                var ReturnInvoiceList = resultInvoiceList.values;
                if (resultInvoiceList.values is not null && resultInvoiceList.values.Count > 0)
                {
                    resultInvoiceList.values = AddInvoiceIdToInvoiceItemsFromInvoiceList(resultInvoiceList);
                }

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0 && resultInvoiceList.values is not null && resultInvoiceList.values.Count > 0)
                {
                    resultInvoiceList.values.Select(invoice =>
                    {
                        invoice.companyid = companyid;
                        return invoice;
                    }).ToList();

                }

                resultInvoiceList.values = ReturnInvoiceList;

                return resultInvoiceList;
            }
            catch
            {
                return new Search();
            }
        }


        private static List<Model.Invoice> AddInvoiceIdToInvoiceItemsFromInvoiceList(Model.DTO.Invoices.Search resultInvoiceList)
        {
            return resultInvoiceList.values.Select(invoice =>
                                              {
                                                  invoice.InvoiceItems = AddInvoiceIdToInvoiceItems(invoice.InvoiceItems, invoice.kti_sourceid);
                                                  return invoice;
                                              }).ToList();
        }

        public IEnumerable<InvoiceItem> GetItemList(string invoiceID)
        {
            if (string.IsNullOrWhiteSpace(invoiceID))
            {
                throw new ArgumentException("Invalid invoiceID", nameof(invoiceID));
            }

            try
            {
                var Invoice = Get(invoiceID);

                var InvoiceItems = Invoice.InvoiceItems;

                return InvoiceItems;
            }

            catch
            {
                return new List<InvoiceItem>();
            }
        }

        public Model.Invoice Get(long invoiceID)
        {
            try
            {
                return Get(invoiceID.ToString());
            }
            catch
            {
                return new Model.Invoice();
            }

        }

        public IEnumerable<InvoiceItem> GetItemList(long invoiceID)
        {
            try
            {
                return GetItemList(invoiceID.ToString());
            }
            catch
            {
                return new List<InvoiceItem>();
            }
        }

        public bool Void(Model.Invoice InvoiceDetails)
        {
            if (InvoiceDetails is null)
            {
                throw new ArgumentNullException(nameof(InvoiceDetails));
            }

            var VoidDTOmodel = ConvertToVoidDTO(InvoiceDetails);
            VoidDTOmodel.VoidBy = VoidDefaultsHelper.VoidBy;
            VoidDTOmodel.VoidDate = DateTime.UtcNow;
            VoidDTOmodel.VoidReason = VoidDefaultsHelper.VoidReason;
            return Void(VoidDTOmodel);

        }

        public bool Void(Model.DTO.Invoices.Void InvoiceVoidDTO)
        {

            if (string.IsNullOrWhiteSpace(InvoiceVoidDTO.VoidBy))
            {
                string message = "Void: VoidBy cant be null or whitespace";

                throw new ArgumentException(message);

            }

            if (InvoiceVoidDTO.VoidDate == default)
            {
                string message = "Void: VoidDate is required";

                throw new ArgumentException(message);

            }

            if (string.IsNullOrWhiteSpace(InvoiceVoidDTO.VoidReason))
            {
                string message = "Void: VoidReason cant be null or whitespace";

                throw new ArgumentException(message);

            }

            try
            {

                var path = APIDirectory + "/void";
                var isAuthenticated = true;
                var method = "POST";

                var stringContent = GetContent(InvoiceVoidDTO);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                if (response != "OK")
                {
                    throw new System.Exception(response);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                string domain = _service.DefaultURL + APIDirectory + "/void";

                string classname = "OctoPOSInvoice, Method: void";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }


        }

        public static Model.DTO.Invoices.Void ConvertToVoidDTO(Model.Invoice InvoiceModel)
        {
            if (InvoiceModel is null)
            {
                throw new ArgumentNullException(nameof(InvoiceModel));
            }

            return JsonConvert.DeserializeObject<Model.DTO.Invoices.Void>(JsonConvert.SerializeObject(InvoiceModel));
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

        Model.Invoice IInvoice<Model.Invoice, InvoiceItem>.Add(Model.Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Model.Invoice Update(Model.Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Model.Invoice Upsert(Model.Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Model.Invoice Add(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public Model.Invoice Update(string FromDispatcherQueue, string Invoiceid)
        {
            throw new NotImplementedException();
        }

        public Model.Invoice Upsert(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public Model.Invoice GetByField(string FieldName, string FieldValue)
        {
            throw new NotImplementedException();
        }

        public bool IsForDispatch(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public bool IsForReceiver(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }


    }
}
