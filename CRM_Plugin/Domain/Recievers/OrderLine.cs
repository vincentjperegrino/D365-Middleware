using CRM_Plugin.Core.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Domain.Recievers
{
    public class OrderLine : IOrderLine
    {

        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
  

        public OrderLine(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;     
        }

        public bool Create(Guid SalesOrderID, Entity orderLine)
        {
            orderLine.LogicalName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            orderLine[Core.Helper.EntityHelper.SalesOrderDetail.salesorderid] = new EntityReference(Core.Helper.EntityHelper.SalesOrder.entity_name, SalesOrderID);
            _service.Create(orderLine);
            return true;
        }

        public Guid CreateWithGuid(Guid SalesOrderID, Entity orderLine)
        {
            orderLine.LogicalName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            orderLine[Core.Helper.EntityHelper.SalesOrderDetail.salesorderid] = new EntityReference(Core.Helper.EntityHelper.SalesOrder.entity_name, SalesOrderID);
            return _service.Create(orderLine);
        }

        public Entity Get(Guid ExistingID)
        {
            return _service.Retrieve(Core.Helper.EntityHelper.SalesOrderDetail.entity_name, ExistingID, new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid));
        }

        public Entity Get(Guid ExistingSalesOrderID, string lineNumber)
        {
            QueryExpression Query = new QueryExpression();
            Query.EntityName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            Query.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.SalesOrderDetail.kti_sourceid, Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.salesorderid, ConditionOperator.Equal, ExistingSalesOrderID);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber, ConditionOperator.Equal, lineNumber);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.parentbundleid, ConditionOperator.Null);

            Query.Criteria.AddFilter(entityFilter);

            var result = _service.RetrieveMultiple(Query);

            if (result.Entities.Any())
            {
                return result.Entities.First();
            }

            return null;

        }

        public EntityCollection GetOrderItemList(Guid ExistingSalesOrderID)
        {
            QueryExpression Query = new QueryExpression();
            Query.EntityName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            Query.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.SalesOrderDetail.kti_sourceid, Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber , Core.Helper.EntityHelper.SalesOrderDetail.parentbundleid, Core.Helper.EntityHelper.SalesOrderDetail.salesorderispricelocked);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrderDetail.salesorderid, ConditionOperator.Equal, ExistingSalesOrderID);

            Query.Criteria.AddFilter(entityFilter);

            var returnEntity = _service.RetrieveMultiple(Query);

            return returnEntity;
        }

        public bool Update(Guid SalesOrderID, Entity orderLine, Guid ExistingID)
        {
            orderLine.LogicalName = Core.Helper.EntityHelper.SalesOrderDetail.entity_name;
            orderLine.Id = ExistingID;
       
            orderLine[Core.Helper.EntityHelper.SalesOrderDetail.salesorderid] = new EntityReference(Core.Helper.EntityHelper.SalesOrder.entity_name, SalesOrderID);

            _service.Update(orderLine);
            return true;
        }

        public bool Upsert(Guid SalesOrderID, Entity orderLine)
        {
            var Erroron = "";
            try
            {
                Erroron = Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber;
                string kti_lineitemnumber = (string)orderLine[Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber];


                Erroron = "Get(SalesOrderID, kti_lineitemnumber)";
                var existingOrderLine = Get(SalesOrderID, kti_lineitemnumber);

                if (existingOrderLine == null)
                {
                    Erroron = "Create(SalesOrderID, orderLine)";
                    return Create(SalesOrderID, orderLine);
                }

                Erroron = "Update(SalesOrderID, orderLine, existingOrderLine.Id)";
                return Update(SalesOrderID, orderLine, existingOrderLine.Id);

            }
            catch (Exception ex)
            {
                throw new Exception($"OrderLine Error on : {Erroron}. {ex}");

            }
        }

        public Guid UpsertWithGuid(Guid SalesOrderID, Entity orderLine)
        {
            var Erroron = "";
            try
            {
                Erroron = Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber;
                string kti_lineitemnumber = (string)orderLine[Core.Helper.EntityHelper.SalesOrderDetail.kti_lineitemnumber];


                Erroron = " Get(SalesOrderID, kti_lineitemnumber)";
                var existingOrderLine = Get(SalesOrderID, kti_lineitemnumber);

                if (existingOrderLine == null)
                {
                    Erroron = "CreateWithGuid(SalesOrderID, orderLine)";
                    return CreateWithGuid(SalesOrderID, orderLine);
                }

                Erroron = "Update(SalesOrderID, orderLine, existingOrderLine.Id)";
                Update(SalesOrderID, orderLine, existingOrderLine.Id);
                return existingOrderLine.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"OrderLine Error on : {Erroron}. {ex}");

            }
        }
    }
}
