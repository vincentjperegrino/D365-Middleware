using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Helper;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Model.DTO.Invoices;
using KTI.Moo.Extensions.Magento.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Domain
{
    public class Invoice : IInvoice<Model.Invoice, Model.InvoiceItem>
    {
        private readonly IMagentoService _service;
        public const string APIDirectory = "/invoices";
        private readonly string _companyid;

        public Invoice(Config config)
        {
            this._service = new MagentoService(config);
            _companyid = config.companyid;
        }

        public Invoice(Config config, IDistributedCache cache)
        {
            this._service = new MagentoService(config, cache);
            _companyid = config.companyid;
        }

        public Invoice(string defaultDomain, string redisConnectionString, string username, string password)
        {

            this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);

        }

        public Invoice(IMagentoService service)
        {
            this._service = service;
        }

        public Model.Invoice Get(string invoiceID)
        {
            if (long.TryParse(invoiceID, out var longid))
            {
                return Get(longid);
            }

            return new Model.Invoice();
        }


        public Model.Invoice Get(long invoiceID)
        {
            if (invoiceID <= 0)
            {
                throw new ArgumentException("Invalid invoiceID", nameof(invoiceID));
            }

            try
            {
                var path = APIDirectory + "/" + invoiceID.ToString();

                var isAuthenticated = true;
                var method = "GET";


                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);

                var OrderIDForAddress = (long)ReturnInvoiceData.order_id;

                var OrderModel = GetFromOrder(OrderIDForAddress);

                if (OrderIsValid(OrderModel))
                {
                    ReturnInvoiceData = MapToInvoiceFromOrder(ReturnInvoiceData, OrderModel);

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


        public Search GetSearchInvoiceByOrderIDList(List<Model.Order> OrderList)
        {
            if (OrderList is null)
            {
                throw new ArgumentNullException(nameof(OrderList));
            }

            try
            {
                var path = APIDirectory;
                var method = "GET";
                var isAuthenticated = true;

                Dictionary<string, string> parameters = new();

                parameters.AddSearchParameter(OrderList);

                var response = _service.ApiCall(path, method, parameters, isAuthenticated);

                var result = SearchReponse(response);

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0 && result.values is not null && result.values.Count > 0)
                {
                    result.values.Select(invoice =>
                    {
                        invoice.companyid = companyid;
                        return invoice;
                    }).ToList();
                }

                return result;
            }
            catch
            {
                return new Search();
            }
           

        }


        public Search GetSearchInvoiceByOrderIDList(List<Model.Order> OrderList, int pagesize, int currentPage)
        {
            if (OrderList is null)
            {
                throw new ArgumentNullException(nameof(OrderList));
            }

            if (pagesize <= 0 || currentPage <= 0)
            {
                var message = "GetSearchInvoice: pagesize or currentPage can't be below zero.";

                throw new ArgumentException(message);

            }

            try
            {
                var path = APIDirectory;
                var method = "GET";
                var isAuthenticated = true;

                Dictionary<string, string> parameters = new();

                parameters.AddSearchParameter(OrderList, pagesize, currentPage);

                var response = _service.ApiCall(path, method, parameters, isAuthenticated);

                var result = SearchReponse(response);

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0 && result.values is not null && result.values.Count > 0)
                {
                    result.values.Select(invoice =>
                    {
                        invoice.companyid = companyid;
                        return invoice;
                    }).ToList();
                }

                return result;
            }
            catch
            {
                return new Search();
            }
        }


        public Search GetSearchInvoice(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
        {
            if (pagesize <= 0 || currentPage <= 0)
            {
                var message = "GetSearchInvoice: pagesize or currentPage can't be below zero.";

                throw new ArgumentException(message);

            }

            try
            {
                var path = APIDirectory;
                var method = "GET";
                var isAuthenticated = true;

                Dictionary<string, string> parameters = new();

                parameters.AddSearchParameterRangeCreatedDate(dateFrom, dateTo, pagesize, currentPage);

                var response = _service.ApiCall(path, method, parameters, isAuthenticated);

                var result = SearchReponse(response);

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0 && result.values is not null && result.values.Count > 0)
                {
                    result.values.Select(invoice =>
                    {
                        invoice.companyid = companyid;
                        return invoice;
                    }).ToList();
                }

                return result;

            }
            catch
            {
                return new Search();
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


        private static bool OrderIsValid(Model.Order OrderModel)
        {
            return OrderModel is not null && OrderModel.order_id != 0;
        }

        public IEnumerable<Model.InvoiceItem> GetItemList(long invoiceID)
        {
            if (invoiceID <= 0)
            {
                throw new ArgumentException("Invalid invoiceID", nameof(invoiceID));
            }

            try
            {
                var path = APIDirectory + "/" + invoiceID.ToString();

                var isAuthenticated = true;
                var method = "GET";


                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);

                var OrderIDForAddress = (long)ReturnInvoiceData.order_id;

                if (OrderIDForAddress != 0)
                {

                    var OrderModel = GetFromOrder(OrderIDForAddress);

                    var ReturnInvoiceItemData = MapToInvoiceItemFromOrderItem(ReturnInvoiceData.invoiceItems, OrderModel.order_items);

                    return ReturnInvoiceItemData;

                }

                return ReturnInvoiceData.invoiceItems;

            }
            catch
            {
                return new List<InvoiceItem>();
            }
        }


        private Model.Invoice MapToInvoiceFromOrder(Model.Invoice invoiceModel, Model.Order orderModel)
        {
            invoiceModel.CustomerDetails = orderModel.CustomerDetails;
            invoiceModel.billing_address = ConvertToInvoiceAddress(orderModel.billing_address);

            invoiceModel.shipto_city = orderModel.shipto_city;
            invoiceModel.shipto_country = orderModel.shipto_contactname;
            invoiceModel.shipto_fax = orderModel.shipto_country;
            invoiceModel.shipto_line1 = orderModel.shipto_line1;
            invoiceModel.shipto_line2 = orderModel.shipto_line2;
            invoiceModel.shipto_line3 = orderModel.shipto_line3;
            invoiceModel.shipto_name = orderModel.shipto_name;
            invoiceModel.shipto_postalcode = orderModel.shipto_postalcode;
            invoiceModel.shipto_telephone = orderModel.shipto_telephone;


            if (OrderItemsIsValid(orderModel))
            {
                invoiceModel.invoiceItems = MapToInvoiceItemFromOrderItem(invoiceModel.invoiceItems, orderModel.order_items);
            }




            return invoiceModel;
        }

        private static bool OrderItemsIsValid(Model.Order orderModel)
        {
            return orderModel.order_items is not null && orderModel.order_items.Count > 0;
        }

        private List<Model.InvoiceItem> MapToInvoiceItemFromOrderItem(List<Model.InvoiceItem> invoiceItemList, List<Model.OrderItem> orderItemList)
        {

            invoiceItemList.Select(invoiceItem =>
            {
                var orderItems = orderItemList.Where(items => items.item_id == invoiceItem.order_item_id)
                                              .FirstOrDefault();

                invoiceItem.shipto_city = orderItems.shipto_city;
                invoiceItem.shipto_country = orderItems.shipto_country;
                invoiceItem.shipto_line1 = orderItems.shipto_line1;
                invoiceItem.shipto_line2 = orderItems.shipto_line2;
                invoiceItem.shipto_line3 = orderItems.shipto_line3;
                invoiceItem.shipto_name = orderItems.shipto_name;
                invoiceItem.shipto_postalcode = orderItems.shipto_postalcode;
                invoiceItem.shipto_telephone = orderItems.shipto_telephone;
                return invoiceItem;

            }).ToList();


            return invoiceItemList;

        }

        private Model.Order GetFromOrder(long OrderID)
        {

            Order MagentoOrder = new(_service);
            return MagentoOrder.Get(OrderID);

        }


        private static Model.InvoiceAddress ConvertToInvoiceAddress(Model.OrderAddress OrderAddressModel)
        {
            return JsonConvert.DeserializeObject<Model.InvoiceAddress>(JsonConvert.SerializeObject(OrderAddressModel));
        }

        public Model.Invoice Add(Model.Invoice invoice)
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
