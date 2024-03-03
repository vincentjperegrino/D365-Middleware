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
    public class Currency
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly string _isoCurrencyCode;

        public Currency(IOrganizationService service, ITracingService tracingService, string isoCurrencyCode)
        {
            _service = service;
            _tracingService = tracingService;
            _isoCurrencyCode = isoCurrencyCode;
        }

        public Currency(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Currency(IOrganizationService service)
        {
            _service = service;
        }
        public bool create(Entity currency)
        {
            try
            {
                currency.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.Currency.entity_name;

                _service.Create(currency);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }


        public bool update(Entity currency, Guid ExistingID)
        {
            try
            {
                currency.LogicalName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.Currency.entity_name;
                currency.Id = ExistingID;

                _service.Update(currency);

                return true;
            }
            catch (Exception ex)
            {
                _tracingService.Trace(ex.Message);

                throw ex;
            }
        }

        public bool upsert(Entity currency)
        {
            var isoCurrencyCode = (string)currency[Core.Helper.EntityHelper.CRM.Master.Currency.isocurrencycode];

            var existingCurrency = Get(isoCurrencyCode);

            if (existingCurrency is null)
            {
                return create(currency);
            }

            return update(currency, existingCurrency.Id);
        }

        public Entity Get(string isoCurrencyCode)
        {
            if (isoCurrencyCode == null)
            {
                return null;
            }

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Core.Helper.EntityHelper.CRM.Master.Currency.entity_name;
            qeEntity.ColumnSet = Core.Model.CurrencyBase.columnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.Currency.isocurrencycode, ConditionOperator.Equal, isoCurrencyCode);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCurrency = _service.RetrieveMultiple(qeEntity);

            if (returnCurrency.Entities.Any())
            {
                returnCurrency.Entities.First();
            }

            return null;
        }

        public Models.Items.Currency GetCurrecyByISOCurrencyCode(string isoCurrencyCode)
        {
            QueryExpression qeCurrency = new QueryExpression();

            qeCurrency.EntityName = Core.Helper.EntityHelper.CRM.Master.Currency.entity_name;
            qeCurrency.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.CRM.Master.Currency.entity_id);

            var currencyFilter = new FilterExpression(LogicalOperator.And);
            currencyFilter.AddCondition(Core.Helper.EntityHelper.CRM.Master.Currency.isocurrencycode, ConditionOperator.Equal, isoCurrencyCode);
            qeCurrency.Criteria.AddFilter(currencyFilter);

            EntityCollection colCurrency = _service.RetrieveMultiple(qeCurrency);

            if (colCurrency.Entities.Count > 0)
            {
                return colCurrency.Entities.Select(i => new Models.Items.Currency(i)).First();
            }

            return null;
        }
    }
}
