using Azure;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Exception;
using KTI.Moo.Extensions.OctoPOS.Helper;
using KTI.Moo.Extensions.OctoPOS.Model;
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
    public class Order : IOrder<Model.Order, Model.OrderItem>
    {

        private readonly IOctoPOSService _service;
        public const string APIDirectory = "/salesOrder";
        private readonly string _companyid;


        public Order(Config config, IDistributedCache cache)
        {
            this._service = new OctoPOSService(cache, config);
            this._companyid = config.companyid;
        }

        public Order(IOctoPOSService service)
        {
            this._service = service;
        }

        public bool Add(Model.Order SalesOrderDetails)
        {
            if (SalesOrderDetails is null)
            {
                throw new ArgumentNullException(nameof(SalesOrderDetails));
            }

            try
            {
                var path = APIDirectory + "/add";

                var isAuthenticated = true;
                var method = "POST";

                var stringContent = GetContent(SalesOrderDetails);

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

                string classname = "OctoPOSSalesOrder, Method: add";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }

        }

        public Model.Order Get(string OrderId)
        {
            if (string.IsNullOrWhiteSpace(OrderId))
            {
                throw new ArgumentException("Invalid OrderId", nameof(OrderId));
            }

            try
            {

                var path = APIDirectory + "/details/" + OrderId;

                var isAuthenticated = true;
                var method = "POST";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);

                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0)
                {
                    ReturnOrderData.companyid = companyid;
                }


                return ReturnOrderData;

            }
            catch
            {
                return new Model.Order();
            }

        }

        public Model.Order Get(long Id)
        {
            return Get(Id.ToString());
        }

        public IEnumerable<Model.OrderItem> GetItems(long OrderId)
        {
            if (OrderId <= 0)
            {
                throw new ArgumentException("Invalid OrderId", nameof(OrderId));
            }

            var OrderModel = Get(OrderId.ToString());

            if (OrderModel is null)
            {
                return new List<Model.OrderItem>();
            }

            return OrderModel.SalesOrderItems;
        }

        public IEnumerable<Model.OrderItem> GetItems(string Id)
        {
            var OrderModel = Get(Id);
            return OrderModel.SalesOrderItems;
        }

        public bool Void(Model.Order SalesOrderDetails)
        {
            var VoidDTOmodel = ConvertToVoidDTO(SalesOrderDetails);
            VoidDTOmodel.VoidBy = VoidDefaultsHelper.VoidBy;
            VoidDTOmodel.VoidDate = DateTime.UtcNow;
            VoidDTOmodel.VoidReason = VoidDefaultsHelper.VoidReason;
            return Void(VoidDTOmodel);

        }

        public bool Void(Model.DTO.Orders.Void OrderVoidDTO)
        {

            if (string.IsNullOrWhiteSpace(OrderVoidDTO.VoidBy))
            {
                string message = "Void: VoidBy cant be null or whitespace";

                throw new ArgumentException(message);

            }

            if (OrderVoidDTO.VoidDate == default)
            {
                string message = "Void: VoidDate is required";

                throw new ArgumentException(message);

            }

            if (string.IsNullOrWhiteSpace(OrderVoidDTO.VoidReason))
            {
                string message = "Void: VoidReason cant be null or whitespace";

                throw new ArgumentException(message);

            }

            try
            {

                string path = APIDirectory + "/void";
                bool isAuthenticated = true;
                string method = "POST";

                var stringContent = GetContent(OrderVoidDTO);

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

                string classname = "OctoPOSSalesOrder, Method: void";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);

            }

        }

        public Model.DTO.Orders.Void GetVoidStatus(string OrderId)
        {
            if (string.IsNullOrWhiteSpace(OrderId))
            {
                throw new ArgumentException("Invalid OrderId", nameof(OrderId));
            }

            try
            {
                string path = APIDirectory + "/details/" + OrderId;

                bool isAuthenticated = true;
                string method = "POST";

                var response = _service.ApiCall(path, method, isAuthenticated);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var ReturnOrderData = JsonConvert.DeserializeObject<Model.DTO.Orders.Void>(response, settings);


                _ = int.TryParse(_companyid, out var companyid);

                if (companyid != 0)
                {
                    ReturnOrderData.companyid = companyid;
                }

                return ReturnOrderData;

            }
            catch
            {
                return new Model.DTO.Orders.Void();

            }
        }

        public Model.DTO.Orders.Void GetVoidStatus(long Id)
        {
            return GetVoidStatus(Id.ToString());
        }


        public bool UpdateDeliveryStatus(Model.Order SalesOrderDetails)
        {

            try
            {
                string path = APIDirectory + "/updateDeliveryStatus";
                bool isAuthenticated = true;
                string method = "POST";

                var stringContent = GetContent(SalesOrderDetails);

                var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

                if (response != "OK")
                {
                    throw new System.Exception(response);    
                }

                return true;

            }
            catch (System.Exception ex)
            {
                string domain = _service.DefaultURL + APIDirectory + "/updateDeliveryStatus";

                string classname = "OctoPOSSalesOrder, Method: UpdateDeliveryStatus";

                throw new OctoPOSIntegrationException(domain, classname, ex.Message);
            }

        }

        public bool UpdateDeliveryStatus(string Id, string deliveryStatus)
        {

            Model.Order SalesOrderDetails = new()
            {
                SalesOrderNumber = Id,
                DeliveryStatus = deliveryStatus
            };

            return UpdateDeliveryStatus(SalesOrderDetails);
        }

        public bool UpdateDeliveryStatus_OrderReceived(string Id)
        {
            return UpdateDeliveryStatus(Id, OrderStatusHelper.OrderReceived);
        }

        public bool UpdateDeliveryStatus_PaymentReceived(string Id)
        {
            return UpdateDeliveryStatus(Id, OrderStatusHelper.PaymentReceived);
        }

        public bool UpdateDeliveryStatus_Processing(string Id)
        {
            return UpdateDeliveryStatus(Id, OrderStatusHelper.Processing);
        }

        public bool UpdateDeliveryStatus_ReadyforDelivery(string Id)
        {
            return UpdateDeliveryStatus(Id, OrderStatusHelper.ReadyforDelivery);
        }

        public bool UpdateDeliveryStatus_Delivering(string Id)
        {
            return UpdateDeliveryStatus(Id, OrderStatusHelper.Delivering);
        }

        public bool UpdateDeliveryStatus_Delivered(string Id)
        {
            return UpdateDeliveryStatus(Id, OrderStatusHelper.Delivered);
        }

        private static StringContent GetContent(object models)
        {

            var JsonSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(models, Formatting.Indented, JsonSettings);

            return new StringContent(json, Encoding.UTF8, "application/json");

        }


        public static Model.DTO.Orders.Void ConvertToVoidDTO(Model.Order OrderModel)
        {
            return JsonConvert.DeserializeObject<Model.DTO.Orders.Void>(JsonConvert.SerializeObject(OrderModel));
        }

        Model.Order IOrder<Model.Order, OrderItem>.Add(Model.Order Order)
        {
            throw new NotImplementedException();
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
