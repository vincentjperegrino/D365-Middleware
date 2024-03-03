using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Helper.EntityHelper;

namespace CRM_Plugin.Lazada.Domain
{
    public class OrderStatus : Core.Domain.IOrderStatus<Model.OrderStatus>
    {
        public readonly IOrganizationService _service;
        public readonly ITracingService _tracingService;
        public readonly int _companyid;

        public OrderStatus(IOrganizationService service, ITracingService tracingService, int companyid)
        {
            _service = service;
            _tracingService = tracingService;
            _companyid = companyid;
        }

        public Model.OrderStatus CancelOder(Entity OrderItem)
        {
            var OrderStatusModel = new Lazada.Model.OrderStatus();

            OrderStatusModel.companyid = _companyid;
            OrderStatusModel.domainType = "orderstatus";
            OrderStatusModel.kti_socialchannelorigin = Helpers.ChannelOrigin.OptionSet_lazada;
            OrderStatusModel.kti_orderstatus = OrderItem.Contains("kti_orderstatus") ? ((OptionSetValue)OrderItem["kti_orderstatus"]).Value : default;
            OrderStatusModel.orderstatus = Helpers.OrderStatus.CancelOrderName;
            OrderStatusModel.kti_salesorderitemid = OrderItem.Id.ToString();
            OrderStatusModel.kti_salesorderid = OrderItem.Contains("salesorderid") ? ((EntityReference)OrderItem["salesorderid"]).Id.ToString() : default;
            OrderStatusModel.kti_storecode = OrderItem.Contains("StoreChannels.kti_saleschannelcode") ? (string)((AliasedValue)OrderItem["StoreChannels.kti_saleschannelcode"]).Value : default;

            if (OrderItem.Contains("kti_sourceitemid"))
            {
                OrderStatusModel.kti_sourcesalesorderitemids = new List<string>()
                {
                    (string)OrderItem["kti_sourceitemid"]
                };

            }


            return OrderStatusModel;
        }

        public Model.OrderStatus Checkout(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus DeliveredToCustomer(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus DriverNearby(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus ForPayment(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus InCart(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus Incomplete(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus ForDispatch(Entity Shipment)
        {
            var OrderStatusModel = new Lazada.Model.OrderStatus
            {
                companyid = _companyid,
                domainType = "orderstatus",
                packageid = Shipment.Contains("kti_packageid") ? (string)Shipment["kti_packageid"] : default,
                kti_shipmentid = Shipment.Id.ToString(),
                kti_socialchannelorigin = Helpers.ChannelOrigin.OptionSet_lazada,
                kti_storecode = Shipment.Contains("StoreChannels.kti_saleschannelcode") ? (string)((AliasedValue)Shipment["StoreChannels.kti_saleschannelcode"]).Value : default
            };

            return OrderStatusModel;
        }

        public Model.OrderStatus OrderPacked(Entity Order)
        {
            var SalesOrderDetail = new QueryExpression
            {
                ColumnSet = new ColumnSet("kti_sourceitemid"),
                EntityName = "salesorderdetail"       
            };
            var Filter = new FilterExpression(LogicalOperator.And);
            Filter.AddCondition("salesorderid", ConditionOperator.Equal, Order.Id);
            SalesOrderDetail.Criteria.AddFilter(Filter);

            var SalesOrderDetailEntityCollection = _service.RetrieveMultiple(SalesOrderDetail);

            var SourceItemIDs = SalesOrderDetailEntityCollection.Entities.Where(items => items.Contains("kti_sourceitemid")).Select(items => (string)items["kti_sourceitemid"]).ToList();

            var GetShipmentHeaders = new QueryExpression
            {
                ColumnSet = new ColumnSet("kti_shipmentid"),
                EntityName = "kti_shipment"               
            };

            var FilterShipment = new FilterExpression(LogicalOperator.And);
            FilterShipment.AddCondition("kti_salesorder", ConditionOperator.Equal, Order.Id);
            FilterShipment.AddCondition("modifiedon", ConditionOperator.Today);

            GetShipmentHeaders.Criteria.AddFilter(FilterShipment);

            var ShipmentHeaders = _service.RetrieveMultiple(GetShipmentHeaders);

            var ShipmentHeader = ShipmentHeaders.Entities.Where(header => !header.Contains("kti_packageid")).FirstOrDefault();

            if (ShipmentHeader == null || !ShipmentHeaders.Entities.Any())
            {
                throw new Exception("No Shipment record found");
            }

            var OrderStatusModel = new Lazada.Model.OrderStatus
            {
                companyid = _companyid,
                domainType = "orderstatus",
                kti_sourcesalesorderid = Order.Contains("kti_sourceid") ? (string)Order["kti_sourceid"] : default,
                kti_sourcesalesorderitemids = SourceItemIDs,
                kti_orderstatus = Order.Contains("kti_orderstatus") ? ((OptionSetValue)Order["kti_orderstatus"]).Value : default,
                kti_socialchannelorigin = Helpers.ChannelOrigin.OptionSet_lazada,
                kti_storecode = Order.Contains("StoreChannels.kti_saleschannelcode") ? (string)((AliasedValue)Order["StoreChannels.kti_saleschannelcode"]).Value : default,
                orderstatus = Helpers.OrderStatus.OrderPackedName,
                kti_shipmentid = ShipmentHeader.Id.ToString()
            };

            return OrderStatusModel;
        }

        public Model.OrderStatus OrderPrepared(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus OrderReleased(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus ReceivedByDelivery(Entity Order)
        {
            throw new NotImplementedException();
        }

        public Model.OrderStatus RequestCancelOrder(Entity Order)
        {
            throw new NotImplementedException();
        }
    }
}
