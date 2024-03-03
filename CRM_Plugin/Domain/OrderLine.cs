using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Domain;
using CRM_Plugin.CustomAPI.Model.DTO;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.Domain
{
    public class OrderLine : IOrderLine
    {
        IOrganizationService _service;
        ITracingService _tracingService;

        public OrderLine(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Entity Get(Guid ExistingID)
        {
            throw new NotImplementedException();
        }

        public Entity Get(string sourceid, string channel)
        {
            throw new NotImplementedException();
        }

        public bool Upsert(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public bool Create(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public bool Update(Guid SalesOrderID, Entity orderLine, Guid ExistingID)
        {
            throw new NotImplementedException();
        }
        private Entity GetSalesOrderDetailByExternalID(string externalID)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.kti_sourceid, ConditionOperator.Equal, externalID);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnEntityCollection = _service.RetrieveMultiple(qeEntity);

            if (returnEntityCollection.Entities.Any())
            {
                return returnEntityCollection.Entities.First();
            }

            return null;
        }

        public Entity GetSalesOrderDetailByKey(string sourceID, int channelOrigin, string lineItemNumber)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber, ConditionOperator.Equal, lineItemNumber);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.kti_socialchannelorigin, ConditionOperator.Equal, channelOrigin);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.kti_sourceid, ConditionOperator.Equal, sourceID);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnEntityCollection = _service.RetrieveMultiple(qeEntity);

            if (returnEntityCollection.Entities.Any())
            {
                return returnEntityCollection.Entities.First();
            }

            return null;
        }

        private Guid CheckIfOrderLineExist(OrderDetails orderDetails)
        {
            Entity _orderDetail = new Entity();

            if (orderDetails.kti_sourceid == orderDetails.salesorderid)
            {
                _orderDetail = this.GetSalesOrderDetailByKey(orderDetails.kti_sourceid, orderDetails.kti_socialchannelorigin, orderDetails.kti_lineitemnumber);
            }
            else if (!String.IsNullOrEmpty(orderDetails.kti_sourceid))
            {
                _orderDetail = this.GetSalesOrderDetailByExternalID(orderDetails.kti_sourceid);
            }

            if (_orderDetail != null)
                return _orderDetail.Id;

            return Guid.Empty;
        }

        public ExecuteMultipleResponse BulkUpsertOrderItems(List<CustomAPI.Model.DTO.OrderDetails> orderItemsList)
        {
            try
            {
                var multipleRequest = new ExecuteMultipleRequest()
                {
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                foreach (var entity in orderItemsList)
                {
                    Entity orderItemEntity = new Entity("salesorderdetail");

                    //Product Existing
                    if(!String.IsNullOrEmpty(entity.productid))
                    {
                        orderItemEntity["productid"] = new EntityReference(CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name, Guid.Parse(entity.productid));
                        orderItemEntity["uomid"] = new EntityReference(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitOfMeasurement.entity_name, Guid.Parse(entity.uomid));
                    }
                    else
                    {
                        orderItemEntity["isproductoverridden"] = true;
                        orderItemEntity["productdescription"] = entity.productdescription;
                    }

                    orderItemEntity["ispriceoverridden"] = true;
                    orderItemEntity["priceperunit"] = new Money(entity.priceperunit);
                    orderItemEntity["salesorderid"] = new EntityReference("salesorder", Guid.Parse(entity.salesorderid));
                    orderItemEntity["quantity"] = entity.quantity;

                    if (!String.IsNullOrEmpty(entity.kti_lineitemnumber))
                        orderItemEntity["kti_lineitemnumber"] = entity.kti_lineitemnumber;

                    orderItemEntity["kti_socialchannelorigin"] = new OptionSetValue(entity.kti_socialchannelorigin);
                    orderItemEntity["kti_sourceid"] = entity.kti_sourceid;

                    var orderLineID = CheckIfOrderLineExist(entity);

                    if (Guid.Empty != orderLineID)
                    {
                        orderItemEntity.Id = orderLineID;

                        UpdateRequest updateRequest = new UpdateRequest() { Target = orderItemEntity };
                        multipleRequest.Requests.Add(updateRequest);
                    }
                    else
                    {
                        CreateRequest createRequest = new CreateRequest() { Target = orderItemEntity };
                        multipleRequest.Requests.Add(createRequest);
                    }
                }

                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)_service.Execute(multipleRequest);

                foreach (var r in multipleResponse.Responses)
                {
                    if (r.Fault != null)
                    {
                        _tracingService.Trace($"Order Line Failed: {r.Fault.Message}");
                    }
                }

                return multipleResponse;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                return new ExecuteMultipleResponse();
            }
        }

        public Entity Get(Guid ExistingSalesOrderID, string lineNumber)
        {
            throw new NotImplementedException();
        }

        public EntityCollection GetOrderItemList(Guid ExistingSalesOrderID)
        {
            throw new NotImplementedException();
        }

        public Guid UpsertWithGuid(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }

        public Guid CreateWithGuid(Guid SalesOrderID, Entity orderLine)
        {
            throw new NotImplementedException();
        }
    }
}
