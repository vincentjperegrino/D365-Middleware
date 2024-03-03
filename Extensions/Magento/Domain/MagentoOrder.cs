using Azure;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Helper;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Model.DTO.Orders;
using KTI.Moo.Extensions.Magento.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Domain
{
    public class Order : IOrder<Model.Order, Model.OrderItem>, ISearch<Model.DTO.Orders.Search, Model.Order>
    {

        private readonly IMagentoService _service;
        public const string APIDirectory = "/orders";
        private readonly string _companyid;
        private readonly ILogger _log;

        public Order(Config config)
        {
            this._service = new MagentoService(config);
            _companyid = config.companyid;
        }

        public Order(Config config, IDistributedCache cache)
        {
            this._service = new MagentoService(config, cache);
            _companyid = config.companyid;
        }
        public Order(Config config, IDistributedCache cache, ILogger log)
        {
            this._service = new MagentoService(config, cache, log);
            _companyid = config.companyid;
            _log = log;
        }

        public Order(string defaultDomain, string redisConnectionString, string username, string password)
        {

            this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);

        }
        public Order(IMagentoService service)
        {
            this._service = service;
        }

        public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
        {
            return GetSearchOrders(dateFrom, dateTo, pagesize, currentPage);
        }

        public List<Model.Order> GetAll(List<Model.Order> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
        {
            if (initialList is null)
            {
                initialList = new List<Model.Order>();
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

        public List<Model.Order> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
        {
            var initialList = new List<Model.Order>();
            return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
        }

        public virtual Search GetSearchOrders(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
        {
            if (pagesize <= 0 || currentPage <= 0)
            {
                var message = "GetSearchOrders: pagesize or currentPage can't be below zero.";
                throw new ArgumentException(message);
            }

            var path = APIDirectory;
            var method = "GET";
            var isAuthenticated = true;

            Dictionary<string, string> parameters = new();

            parameters.AddSearchParameterRangeUpdatedDate(dateFrom, dateTo, pagesize, currentPage);

            var response = _service.ApiCall(path, method, parameters, isAuthenticated);

            var result = SearchReponse(response);

            _ = int.TryParse(_companyid, out var companyid);

            if (result.values is not null && result.values.Count > 0)
            {
                result.values.Select(order =>
                {

                    order.order_items = MapAddressToOrderItems(order);
                    order.order_items = HandleConfigurableItems(order.order_items);
                    order.CustomerDetails.address = MapAddressCustomerAddress(order);

                    if (companyid != 0)
                    {
                        order.companyid = companyid;
                    }

                    return order;
                }).ToList();
            }
            return result;
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

        public Model.Order Get(string id)
        {
            if (long.TryParse(id, out var longid))
            {
                return Get(longid);
            }

            return new Model.Order();
        }

        public virtual Model.Order Get(long OrderId)
        {
            if (OrderId <= 0)
            {
                throw new ArgumentException("Invalid invoiceID", nameof(OrderId));
            }

            try
            {
                var path = APIDirectory + "/" + OrderId.ToString();
                var isAuthenticated = true;
                var method = "GET";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);

                ReturnOrderData.order_items = MapAddressToOrderItems(ReturnOrderData);
                ReturnOrderData.order_items = HandleConfigurableItems(ReturnOrderData.order_items);
                ReturnOrderData.CustomerDetails.address = MapAddressCustomerAddress(ReturnOrderData);


                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0)
                {
                    ReturnOrderData.companyid = companyid;
                }

                return ReturnOrderData;

            }
            catch (System.Exception ex)
            {
                return new Model.Order();
            }
        }

        public IEnumerable<Model.OrderItem> GetItems(long OrderId)
        {
            if (OrderId <= 0)
            {
                throw new ArgumentException("Invalid invoiceID", nameof(OrderId));
            }

            try
            {
                var path = APIDirectory + "/" + OrderId.ToString();

                var isAuthenticated = true;
                var method = "GET";


                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);

                var ReturnItemsData = MapAddressToOrderItems(ReturnOrderData);
                ReturnItemsData = HandleConfigurableItems(ReturnItemsData);


                return ReturnItemsData;
            }
            catch
            {
                return new List<Model.OrderItem>();
            }

        }

        public bool Cancel(int OrderId)
        {

            var Operation = "/cancel";
            var path = APIDirectory + "/" + OrderId.ToString() + Operation;

            var isAuthenticated = true;
            var method = "POST";


            var response = _service.ApiCall(path, method, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            try
            {
                var ReturnOrderData = JsonConvert.DeserializeObject<bool>(response, settings);

                return ReturnOrderData;
            }

            catch
            {
                var domain = _service.DefaultURL + path;

                var classname = "MagentoOrder, Method: Cancel";

                throw new MagentoIntegrationException(domain, classname, response);

            }

        }

        public bool Hold(int OrderId)
        {

            var Operation = "/hold";
            var path = APIDirectory + "/" + OrderId.ToString() + Operation;

            var isAuthenticated = true;
            var method = "POST";


            var response = _service.ApiCall(path, method, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };


            try
            {
                var ReturnOrderData = JsonConvert.DeserializeObject<bool>(response, settings);

                return ReturnOrderData;
            }

            catch
            {
                var domain = _service.DefaultURL + path;

                var classname = "MagentoOrder, Method: Hold";

                throw new MagentoIntegrationException(domain, classname, response);

            }

        }

        public bool UnHold(int OrderId)
        {

            var Operation = "/unhold";
            var path = APIDirectory + "/" + OrderId.ToString() + Operation;

            var isAuthenticated = true;
            var method = "POST";


            var response = _service.ApiCall(path, method, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            try
            {
                var ReturnOrderData = JsonConvert.DeserializeObject<bool>(response, settings);

                return ReturnOrderData;
            }

            catch
            {
                var domain = _service.DefaultURL + path;

                var classname = "MagentoOrder, Method: UnHold";

                throw new MagentoIntegrationException(domain, classname, response);

            }


        }

        public string status(int OrderId)
        {
            if (OrderId <= 0)
            {
                throw new ArgumentException("Invalid invoiceID", nameof(OrderId));
            }

            try
            {
                var Operation = "/statuses";
                var path = APIDirectory + "/" + OrderId.ToString() + Operation;

                var isAuthenticated = true;
                var method = "GET";


                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnOrderData = JsonConvert.DeserializeObject<string>(response, settings);

                return ReturnOrderData;
            }
            catch
            {
                return string.Empty;
            }
        }

        private List<OrderItem> MapAddressToOrderItems(Model.Order OrderModel)
        {

            if (OrderModel.extension_attributes.shipping_assignments.FirstOrDefault().shipping.shipping_address is null)
            {
                return OrderModel.order_items;
            }


            var ListOfShippingAssignments = OrderModel.extension_attributes.shipping_assignments;


            List<OrderItem> returnList = new();


            foreach (var ShippingAssignments in ListOfShippingAssignments)
            {

                var currentShippingAddressDetails = ShippingAssignments.shipping.shipping_address;

                if (currentShippingAddressDetails is null)
                {
                    continue;
                }

                var currentList = ShippingAssignments.shipping_items;

                //Update List
                currentList = currentList.Select(shippingDetails =>
                {
                    shippingDetails.shipto_city = currentShippingAddressDetails.address_city;
                    shippingDetails.shipto_contactname = currentShippingAddressDetails.address_primarycontactname;
                    shippingDetails.shipto_country = currentShippingAddressDetails.address_country;
                    shippingDetails.shipto_line1 = currentShippingAddressDetails.address_line1;
                    shippingDetails.shipto_line2 = currentShippingAddressDetails.address_line2;
                    shippingDetails.shipto_line3 = currentShippingAddressDetails.address_line3;
                    shippingDetails.shipto_name = currentShippingAddressDetails.address_type;
                    shippingDetails.shipto_postalcode = currentShippingAddressDetails.address_postalcode;
                    shippingDetails.shipto_telephone = currentShippingAddressDetails.telephone;
                    return shippingDetails;
                }).ToList();

                returnList.AddRange(currentList);


            }

            return returnList;

        }


        private List<Address> MapAddressCustomerAddress(Model.Order OrderModel)
        {

            var BillingAddress = OrderModel.billing_address;
            var CustomerID = OrderModel.CustomerDetails.customer_id;

            List<Address> returnList = new();
            returnList = AddToCustomerAddressList(returnList, BillingAddress, CustomerID);


            var ListOfShippingAssignments = OrderModel.extension_attributes.shipping_assignments;
            foreach (var ShippingAssignments in ListOfShippingAssignments)
            {

                var currentShippingAddressDetails = ShippingAssignments.shipping.shipping_address;

                if (Is_BillingAddressCustomerID_EqualTo_ShippingAddressCustomerID_AND_BillingAddressID_NotEqualTo_ShippingAddressID(OrderModel, ShippingAssignments))
                {

                    returnList = AddToCustomerAddressList(returnList, currentShippingAddressDetails, CustomerID);

                }

            }

            return returnList;

        }


        private static bool Is_BillingAddressCustomerID_EqualTo_ShippingAddressCustomerID_AND_BillingAddressID_NotEqualTo_ShippingAddressID(Model.Order OrderModel, Model.ShippingAssignment ShippingAssignments)
        {
            try
            {


                if (ShippingAssignments.shipping.shipping_address is null)
                {
                    return false;
                }

                var BillingAddress = OrderModel.billing_address;
                var CustomerID = OrderModel.CustomerDetails.customer_id;

                if (CustomerID == ShippingAssignments.shipping.shipping_address.customer_id
                       && BillingAddress.address_id != ShippingAssignments.shipping.shipping_address.address_id)
                {
                    return true;
                }
                return false;
            }
            catch
            {

                return false;
            }


        }




        private static List<Address> AddToCustomerAddressList(List<Address> currentList, OrderAddress OrderAddress, int CustomerID)
        {

            currentList.Add(new()
            {
                address_id = OrderAddress.address_id,
                address_city = OrderAddress.address_city,
                address_country = OrderAddress.address_country,
                address_line1 = OrderAddress.address_line1,
                address_line2 = OrderAddress.address_line2,
                address_line3 = OrderAddress.address_line3,
                address_name = OrderAddress.address_name,
                address_addresstypecode = OrderAddress.address_addresstypecode,
                address_primarycontactname = OrderAddress.address_primarycontactname,
                address_postalcode = OrderAddress.address_postalcode,
                region = new()
                {
                    region_id = OrderAddress.region_id,
                    region_code = OrderAddress.region_code,
                },
                region_id = OrderAddress.region_id,
                customer_id = CustomerID,
                country_id = OrderAddress.country_id,
                Telephone = OrderAddress.Telephone,
                first_name = OrderAddress.first_name,
                last_name = OrderAddress.last_name,
                middle_name = OrderAddress.middle_name,
                company = OrderAddress.company,
                address_fax = OrderAddress.address_fax,
                prefix = OrderAddress.prefix,
                vat_id = OrderAddress.vat_id

            });

            return currentList;


        }



        public Model.Order Add(Model.Order order)
        {
            var path = APIDirectory;
            var isAuthenticated = true;
            var method = "POST";


            Model.DTO.Orders.Add AddModel = new();

            var jsonorder = JsonConvert.SerializeObject(order);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var entityFromOrder = JsonConvert.DeserializeObject<Model.DTO.Orders.EntityOrder>(jsonorder, settings);

            AddModel.entity = entityFromOrder;

            var stringContent = GetContent(AddModel);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);


            if (!string.IsNullOrWhiteSpace(response) && ReturnOrderData.order_id == 0)
            {

                var domain = _service.DefaultURL + path;

                var classname = "MagentoOrder, Method: Add";

                throw new MagentoIntegrationException(domain, classname, response);

            }

            _ = int.TryParse(_companyid, out var companyid);

            if (companyid != 0)
            {
                ReturnOrderData.companyid = companyid;
            }

            return ReturnOrderData;
        }




        public int AddInvoice(Model.Order order)
        {

            Model.DTO.Orders.AddInvoice AddModel = new()
            {

                appendComment = true,
                capture = true,
                comment = new()
                {
                    comment = "added by kti",
                    is_visible_on_front = 1
                },
                items = order.order_items.Select(items => new Model.DTO.Orders.InvoiceOrderItems()
                {
                    order_item_id = items.item_id,
                    qty = items.quantity

                }).ToList(),
                notify = true

            };

            return AddInvoice(AddModel, order.order_id);

        }


        public int AddInvoice(Model.DTO.Orders.AddInvoice Add, int OrderID)
        {
            var path = $"/order/{OrderID}/invoice";
            var isAuthenticated = true;
            var method = "POST";

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var stringContent = GetContent(Add);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            //var ReturnResponse = JsonConvert.DeserializeObject<dynamic>(response, settings);

            var isInt = int.TryParse(response, out var InvoiceID);

            if (isInt == false)
            {
                var domain = _service.DefaultURL + path;

                var classname = "MagentoOrder, Method: AddInvoice";

                throw new MagentoIntegrationException(domain, classname, response);

            }

            //if (!string.IsNullOrWhiteSpace(response) && ReturnOrderData.order_id == 0)
            //{

            //    var domain = _service.DefaultURL + path;

            //    var classname = "MagentoOrder, Method: Add";

            //    throw new MagentoIntegrationException(domain, classname, response);

            //}

            //_ = int.TryParse(_companyid, out var companyid);

            //if (companyid != 0)
            //{
            //    ReturnOrderData.companyid = companyid;
            //}

            return InvoiceID;
        }


        public bool UpdateStatusHold(int OrderId)
        {
            string status = "holded";
            return UpdateStatus(OrderId, status);
        }

        public bool UpdateStatusCanceled(int OrderId)
        {
            string status = "canceled";
            return UpdateStatus(OrderId, status);
        }


        public bool UpdateStatusPending(int OrderId)
        {
            string status = "pending";
            return UpdateStatus(OrderId, status);
        }

        public bool UpdateStatusProcessing(int OrderId)
        {
            string status = "processing";
            return UpdateStatus(OrderId, status);
        }


        public bool UpdateStatusComplete(int OrderId)
        {
            string status = "complete";
            return UpdateStatus(OrderId, status);
        }

        private bool UpdateStatus(int OrderId, string status)
        {
            try
            {
                var path = APIDirectory + "/create";
                var isAuthenticated = true;
                var method = "PUT";

                var EntityUpdates = new Model.DTO.Orders.EntityUpdate()
                {
                    order_id = OrderId,
                    status = status,
                    status_histories = new()
                {
                    new()
                    {
                        comment = $"Updated to {status} by Moo",
                        parent_id = OrderId,
                        is_customer_notified = 0,
                        is_visible_on_front = 0,
                        status = status
                    }

                }
                };

                var DTOupdateStatus = new Model.DTO.Orders.UpdateStatus()
                {
                    entity = EntityUpdates
                };

                var stringContent = GetContentNoIgnore(DTOupdateStatus);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                var JsonSettings = new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, JsonSettings);

                if (ReturnOrderData.order_id == 0)
                {
                    throw new System.Exception(response);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL + APIDirectory + "/create";

                var classname = "MagentoOrder, Method: UpdateStatus";

                throw new MagentoIntegrationException(domain, classname, ex.Message);
            }


        }


        private List<Model.OrderItem> HandleConfigurableItems(List<Model.OrderItem> orderItemList)
        {

            if (orderItemList.Any(item => IsProductTypeConfigurable(item)) == false)
            {
                return orderItemList;
            }

            var RemovedConfigurableProductTypeList = orderItemList.Where(item => !IsProductTypeConfigurable(item)).ToList();

            RemovedConfigurableProductTypeList = RemovedConfigurableProductTypeList.Select(orderitems =>
            {
                return IsItemParentConfigurable(orderitems) ? HandlePriceInConfigurableItem(orderitems) : orderitems;

            }).ToList();

            return RemovedConfigurableProductTypeList;

        }

        private static bool IsProductTypeConfigurable(OrderItem item)
        {
            return item.product_type == Helper.Product.ProductTypeHelper.configurableProduct;
        }

        private static bool IsItemParentConfigurable(OrderItem orderitems)
        {
            return orderitems.parent_item is not null &&
                                orderitems.item_id != 0 &&
                                orderitems.parent_item.product_type == Helper.Product.ProductTypeHelper.configurableProduct;
        }

        private static OrderItem HandlePriceInConfigurableItem(OrderItem orderitems)
        {
            orderitems.parent_item.product_type = orderitems.product_type;
            orderitems.parent_item.productname = orderitems.productname;
            orderitems.parent_item.productid = orderitems.productid;
            orderitems.parent_item.item_id = orderitems.item_id;

            return orderitems.parent_item;
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

        private static StringContent GetContentNoIgnore(object models)
        {
            var json = JsonConvert.SerializeObject(models, Formatting.None);

            return new StringContent(json, Encoding.UTF8, "application/json");

        }

        public Model.Order Update(Model.Order order)
        {
            throw new NotImplementedException();
        }

        public Model.Order Upsert(Model.Order order)
        {
            throw new NotImplementedException();
        }

        public Model.Order Add(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public Model.Order Update(string FromDispatcherQueue, string Orderid)
        {
            throw new NotImplementedException();
        }

        public Model.Order Upsert(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public Model.Order GetByField(string FieldName, string FieldValue)
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

        public bool CancelOrder(Model.Order FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

    }
}
