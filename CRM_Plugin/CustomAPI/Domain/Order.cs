using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class Order //: Core.Domain.IOrder
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public Order(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Order(IOrganizationService service)
        {
            _service = service;
        }

        public bool createSalesOrder(Entity order)
        {
            try
            {

                order.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.entity_name;

                var MappedOrder = MapCustomFieldsOrder(order);

                _service.Create(MappedOrder);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool updateSalesOrder(Entity order, Guid ExistingID)
        {
            try
            {
                order.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.entity_name;
                order.Id = ExistingID;

                var MappedOrder = MapCustomFieldsOrder(order);

                _service.Update(MappedOrder);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool upsertSalesOrder(Entity order)
        {
            var sourceid = (string)order[CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.kti_sourceid];
            var channelid = (string)order[CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.kti_socialchannelorigin];

            var ExistingOrder = GetSalesOrder(sourceid, channelid);

            if (ExistingOrder is null)
            {
                return createSalesOrder(order);
            }

            return updateSalesOrder(order, ExistingOrder.Id);
        }

        public bool createSalesOrderDetail(Entity orderDetail)
        {
            try
            {

                orderDetail.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrderDetail.entity_name;

                var MappedOrderDetail = MapCustomFieldsOrderDetail(orderDetail );

                _service.Create(MappedOrderDetail);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }

        }

        public bool updateSalesOrderDetail(Entity orderDetail, Guid ExistingID)
        {
            try
            {
                orderDetail.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
                orderDetail.Id = ExistingID;

                var MappedOrderDetail = MapCustomFieldsOrderDetail(orderDetail);

                _service.Update(MappedOrderDetail);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool upsertSalesOrderDetail(Entity orderDetail , EntityCollection orderdetailList)
        {
            var sourceid = orderDetail.Id;

            var ExistingOrderDetails = orderdetailList.Entities.Where(details => details.Id == sourceid).Any();

            if (ExistingOrderDetails)
            {
                return createSalesOrderDetail(orderDetail);
            }

            return updateSalesOrderDetail(orderDetail, sourceid);
        }

        public Entity GetSalesOrderBySourceIDChannel(string sourceId, int channelId)
        {
            if (sourceId == null || channelId == 0)
            {
                throw new Exception("No source id or channel indicated.");
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_sourceid, ConditionOperator.Equal, sourceId);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_socialchannelorigin, ConditionOperator.Equal, Convert.ToInt32(channelId));

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnOrder = _service.RetrieveMultiple(qeEntity);

            if (returnOrder.Entities.Count > 0)
            {
                return returnOrder.Entities.First();
            }

            return null;
        }





        public Entity GetSalesOrder(string sourceid, string channelid)
        {
            if (sourceid == null || channelid == null)
            {
                throw new Exception("No source id or channel indicated.");
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_sourceid, ConditionOperator.Equal, sourceid);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_socialchannelorigin, ConditionOperator.Equal, channelid);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                returnCustomer.Entities.First();
            }

            return null;
        }

        public EntityCollection GetSalesOrderDetail(Guid salesorderid)
        {
            var qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.salesorderid, ConditionOperator.Equal, salesorderid);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnEntityCollection = _service.RetrieveMultiple(qeEntity);

            if (returnEntityCollection.Entities.Any())
            {
                return returnEntityCollection;
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
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.kti_sourceid
                , ConditionOperator.Equal, sourceID);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnEntityCollection = _service.RetrieveMultiple(qeEntity);

            if (returnEntityCollection.Entities.Any())
            {
                return returnEntityCollection.Entities.First();
            }

            return null;
        }

        private EntityReference GetPriceLevelID(string pricelevelid)
        {
            if (string.IsNullOrWhiteSpace(pricelevelid))
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.Branch.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.Branch.kti_branchcode);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.Branch.kti_branchcode, ConditionOperator.Equal, pricelevelid);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnBranch = _service.RetrieveMultiple(qeEntity);

            if (returnBranch.Entities.Any())
            {
                var BranchEntity = returnBranch.Entities.First();

                return new EntityReference(Core.Helper.EntityHelper.Branch.entity_name, BranchEntity.Id);
            }

            return null;
        }

        private EntityReference GetCustomer(string customer, string channelid)
        {
            if (string.IsNullOrWhiteSpace(customer))
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Customer.entity_name;
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid, ConditionOperator.Equal, customer);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid, ConditionOperator.Equal, customer);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomer = _service.RetrieveMultiple(qeEntity);

            if (returnCustomer.Entities.Any())
            {
                var CustomerEntity = returnCustomer.Entities.First();

                return new EntityReference(Core.Helper.EntityHelper.CRM.Customer.entity_name, CustomerEntity.Id);
            }

            return null;
        }





        private Entity MapCustomFieldsOrder(Entity order)
        {
            //if (!string.IsNullOrEmpty(order.pricelevelid))
            //{
            //    order.pricelevelid = $"{crmConfig.crm_pricelevel_path}(name='{order.pricelevelid}')";
            //}
            //else
            //{
            //    order.pricelevelid = $"{crmConfig.crm_pricelevel_path}(name='Standard')";
            //}

            ////Get Customer
            //if (!String.IsNullOrEmpty(order.customerid))
            //{
            //    order.customerid = $"{crmConfig.crm_customer_path}(kti_sourceid='{order.customerid}',kti_socialchannelorigin={order.kti_socialchannelorigin})";
            //}
            //else
            //{
            //    var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            //    jsonResolver.IgnoreProperty(typeof(Domain.Models.Sales.Order), "customerid@odata.bind");
            //    settings.ContractResolver = jsonResolver;
            //}

            //if (order.Contains(Core.Helper.EntityHelper.SalesOrder.customer))
            //{
            //    var customer = GetCustomer((string)order[Core.Helper.EntityHelper.SalesOrder.customer]);

            //    if (customer != null)
            //    {
            //        order[Core.Helper.EntityHelper.SalesOrder.customer] = customer;
            //    }
            //}
            return order;
        }

        private Entity MapCustomFieldsOrderDetail(Entity orderdetail)
        {



            return orderdetail;
        }

        public EntityCollection GetScheduledOrder()
        {

            var query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.entity_name,
                ColumnSet = new ColumnSet(CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.kti_sourceid,
                                           CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.name),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.statecode,
                            Operator = ConditionOperator.Equal,
                            Values = { 0 }
                        },
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.SalesOrder.modifiedon,
                            Operator = ConditionOperator.LastXHours,
                            Values = { 1 }
                        }
                    }
                }
            };

            return new EntityCollection(RetrieveAllRecords(_service, query));

        }




        public static List<Entity> RetrieveAllRecords(IOrganizationService service, QueryExpression query)
        {
            var pageNumber = 1;
            var pagingCookie = string.Empty;
            var result = new List<Entity>();
            EntityCollection resp;
            do
            {
                if (pageNumber != 1)
                {
                    query.PageInfo.PageNumber = pageNumber;
                    query.PageInfo.PagingCookie = pagingCookie;
                }
                resp = service.RetrieveMultiple(query);
                if (resp.MoreRecords)
                {
                    pageNumber++;
                    pagingCookie = resp.PagingCookie;
                }
                //Add the result from RetrieveMultiple to the List to be returned.
                result.AddRange(resp.Entities);
            }
            while (resp.MoreRecords);

            return result;
        }










    }
}
