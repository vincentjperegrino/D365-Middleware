using CRM_Plugin.Core.Domain;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class PriceLevel
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly int _channelOrigin;
        private readonly int _priceListType;

        public PriceLevel(IOrganizationService service, ITracingService tracingService, int channelOrigin, int priceListType)
        {
            _service = service;
            _tracingService = tracingService;
            _priceListType = priceListType;
            _channelOrigin = channelOrigin;
        }

        public PriceLevel(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public PriceLevel(IOrganizationService service)
        {
            _service = service;
        }


        public bool create(Entity priceLevel)
        {
            try
            {
                priceLevel.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name;

                _service.Create(priceLevel);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }


        public bool update(Entity priceLevel, Guid ExistingID)
        {
            try
            {
                priceLevel.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name;
                priceLevel.Id = ExistingID;

                _service.Update(priceLevel);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool upsert(Entity account)
        {
            var priceLevelName = (string)account[Core.Helper.EntityHelper.CRM.Master.PriceList.name];
            var channel = ((OptionSetValue)account[Core.Helper.EntityHelper.CRM.Master.PriceList.kti_socialchannel]).Value;

            var existingAccount = Get(priceLevelName, channel);

            if (existingAccount is null)
            {
                return create(account);
            }

            return update(account, existingAccount.Id);
        }

        public Entity Get(string priceLevelName, int channel)
        {
            if (priceLevelName == null || 
                (channel == null || channel == 0))
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name;
            qeEntity.ColumnSet = Core.Model.PriceLevelBase.columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.PriceList.name, ConditionOperator.Equal, priceLevelName);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.PriceList.kti_socialchannel, ConditionOperator.Equal, channel);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnPriceLevel = _service.RetrieveMultiple(qeEntity);

            if (returnPriceLevel.Entities.Any())
            {
                returnPriceLevel.Entities.First();
            }

            return null;
        }

        public CRM_Plugin.Models.Items.PriceLevel GetByID(string priceLevelID)
        {
            if (priceLevelID == null)
            {
                return null;
            }

            return new CRM_Plugin.Models.Items.PriceLevel(_service.Retrieve("pricelevel", new Guid(priceLevelID), Core.Model.PriceLevelBase.columnSet));
        }
        public CRM_Plugin.Models.Items.PriceLevel GetPriceListByDefaultFirst()
        {
            QueryExpression qePriceList = new QueryExpression();

            qePriceList.EntityName = Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name;
            qePriceList.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Master.PriceList.entity_id);

            EntityCollection colPriceList = _service.RetrieveMultiple(qePriceList);

            if (colPriceList.Entities.Count > 0)
            {
                return colPriceList.Entities.Select(i => new CRM_Plugin.Models.Items.PriceLevel(i)).First();
            }

            return null;
        }

        public CRM_Plugin.Models.Items.PriceLevel GetPriceListByPriceListName(string priceListName)
        {
            QueryExpression qePriceList = new QueryExpression();

            qePriceList.EntityName = Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name;
            qePriceList.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Master.PriceList.entity_id);

            var priceListFilter = new FilterExpression(LogicalOperator.And);
            priceListFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.PriceList.name, ConditionOperator.Equal, priceListName);
            qePriceList.Criteria.AddFilter(priceListFilter);

            EntityCollection colPriceList = _service.RetrieveMultiple(qePriceList);

            if (colPriceList.Entities.Count > 0)
            {
                return colPriceList.Entities.Select(i => new CRM_Plugin.Models.Items.PriceLevel(i)).First();
            }

            return null;
        }

        public CRM_Plugin.Models.Items.PriceLevel GetPriceListByPriceListNameChannelOrigin(string priceListName, int channelOrigin)
        {
            QueryExpression qePriceList = new QueryExpression();

            qePriceList.EntityName = Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name;
            qePriceList.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Master.PriceList.entity_id);

            var priceListFilter = new FilterExpression(LogicalOperator.And);
            priceListFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.PriceList.name, ConditionOperator.Equal, priceListName);
            priceListFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.PriceList.kti_socialchannel, ConditionOperator.Equal, channelOrigin);
            qePriceList.Criteria.AddFilter(priceListFilter);

            EntityCollection colPriceList = _service.RetrieveMultiple(qePriceList);

            if (colPriceList.Entities.Count > 0)
            {
                return colPriceList.Entities.Select(i => new CRM_Plugin.Models.Items.PriceLevel(i)).First();
            }

            return null;
        }

    }
}
