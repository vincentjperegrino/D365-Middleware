using CRM_Plugin.Core.Domain;
using CRM_Plugin.CustomAPI.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Domain.Recievers
{
    public class Order : IOrder
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;


        public Order(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;

        }

        public Entity Get(Guid ExistingID)
        {
            return _service.Retrieve(Core.Helper.EntityHelper.SalesOrder.entity_name, ExistingID, new ColumnSet(Core.Helper.EntityHelper.CRM.Customer.kti_sourceid , Core.Helper.EntityHelper.SalesOrder.ispricelocked));
        }

        public Entity Get(string sourceid, OptionSetValue channel)
        {
            if (sourceid == null || channel == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            qeEntity.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.SalesOrder.kti_sourceid, Core.Helper.EntityHelper.SalesOrder.ispricelocked);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_sourceid, ConditionOperator.Equal, sourceid);
            entityFilter.AddCondition(Core.Helper.EntityHelper.SalesOrder.kti_socialchannelorigin, ConditionOperator.Equal, channel.Value);

            qeEntity.Criteria.AddFilter(entityFilter);

            var result = _service.RetrieveMultiple(qeEntity);

            if (result.Entities.Any())
            {
                return result.Entities.First();
            }

            return null;
        }

        public bool Create(Entity order)
        {
            CreateWithGuid(order);
            return true;
        }

        public Guid CreateWithGuid(Entity order)
        {
            order.LogicalName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            return _service.Create(order);
        }

        public bool Update(Entity order, Guid ExistingID)
        {
            order.LogicalName = Core.Helper.EntityHelper.SalesOrder.entity_name;
            order.Id = ExistingID;
            _service.Update(order);
            return true;
        }

        public bool Upsert(Entity order)
        {
            var Erroron = "";
            try
            {
                Erroron = "kti_sourceid";
                string sourceid = (string)order["kti_sourceid"];
                Erroron = "kti_socialchannelorigin";
                var channel = (OptionSetValue)order["kti_socialchannelorigin"];


                Erroron = "Get(sourceid, channel)";
                var existingOrder = Get(sourceid, channel);

                if (existingOrder == null)
                {
                    Erroron = "create(order)";
                    return Create(order);
                }

                Erroron = "update(order, existingOrder.Id)";
                return Update(order, existingOrder.Id);

            }
            catch (Exception ex)
            {
                throw new Exception($"Order Error on : {Erroron}. {ex}");

            }

        }

        public Guid UpsertWithGuid(Entity order)
        {
            var Erroron = "";
            try
            {
                Erroron = "kti_sourceid";
                var sourceid = (string)order["kti_sourceid"];
                Erroron = "kti_socialchannelorigin";
                var channel = (OptionSetValue)order["kti_socialchannelorigin"];

                Erroron = "Get(sourceid, channel)";
                var existingOrder = Get(sourceid, channel);

                if (existingOrder == null)
                {
                    Erroron = "CreateWithGuid(order)";
                    return CreateWithGuid(order);
                }

                Erroron = "update(order, existingOrder.Id)";
                Update(order, existingOrder.Id);
                return existingOrder.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Order Error on : {Erroron}. {ex}");

            }
        }

    }
}
