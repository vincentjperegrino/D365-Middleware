using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class ChannelManagement : Core.Domain.IChannelManagement<Model.DTO.ChannelManagement.SalesChannel>
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly CRM_Plugin.SalesChannel.Domain.SalesChannel _salesChannelDomain;

        public ChannelManagement(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
            _salesChannelDomain = new CRM_Plugin.SalesChannel.Domain.SalesChannel();
        }

        public ChannelManagement(IOrganizationService service)
        {
            _service = service;
            _salesChannelDomain = new CRM_Plugin.SalesChannel.Domain.SalesChannel();
        }


        public Model.DTO.ChannelManagement.SalesChannel Get(string ChannelCode)
        {

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_saleschannel";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_saleschannelcode", ConditionOperator.Equal, ChannelCode);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnSalesChannel = _service.RetrieveMultiple(qeEntity);
            if (returnSalesChannel.Entities.Any())
            {
                var SalesChannelEntity = returnSalesChannel.Entities.First();
                var SalesChannel = new Model.DTO.ChannelManagement.SalesChannel(SalesChannelEntity);
                SalesChannel.kti_appkey = DecrytSalesChannel(SalesChannel.kti_appkey);
                SalesChannel.kti_appsecret = DecrytSalesChannel(SalesChannel.kti_appsecret);
                SalesChannel.kti_password = DecrytSalesChannel(SalesChannel.kti_password);
               // SalesChannel.ChannelCategoryMappingsList = GetChannelCategoryMappingList(SalesChannelEntity.Id);
                SalesChannel.CustomFieldList = GetCustomFieldList(SalesChannelEntity.Id);
                return SalesChannel;

            }

            return new Model.DTO.ChannelManagement.SalesChannel();

        }

        public List<Model.DTO.ChannelManagement.SalesChannel> GetChannelList()
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_saleschannel";
            qeEntity.ColumnSet = new ColumnSet(true);

            var returnSalesChannel = _service.RetrieveMultiple(qeEntity);

            if (returnSalesChannel.Entities.Any())
            {
                var SalesListChannel = returnSalesChannel.Entities.Select(saleschannel =>
                {
                    var SalesChannels = new Model.DTO.ChannelManagement.SalesChannel(saleschannel);
                    SalesChannels.kti_appkey = DecrytSalesChannel(SalesChannels.kti_appkey);
                    SalesChannels.kti_appsecret = DecrytSalesChannel(SalesChannels.kti_appsecret);
                    SalesChannels.kti_password = DecrytSalesChannel(SalesChannels.kti_password);
                  //  SalesChannels.ChannelCategoryMappingsList = GetChannelCategoryMappingList(saleschannel.Id);
                    SalesChannels.CustomFieldList = GetCustomFieldList(saleschannel.Id);

                    return SalesChannels;

                }).ToList();

                return SalesListChannel;
            }

            return new List<Model.DTO.ChannelManagement.SalesChannel>();
        }


        public bool UpdateToken(Model.DTO.ChannelManagement.SalesChannel ChannelConfig)
        {

            if (string.IsNullOrWhiteSpace(ChannelConfig.kti_saleschannelId))
            {
                return false;
            }

            var SalesChannelEntity = new Entity("kti_saleschannel", new Guid(ChannelConfig.kti_saleschannelId));
            SalesChannelEntity["kti_access_token"] = ChannelConfig.kti_access_token;
            SalesChannelEntity["kti_access_expiration"] = ChannelConfig.kti_access_expiration;
            SalesChannelEntity["kti_refresh_token"] = ChannelConfig.kti_refresh_token;
            SalesChannelEntity["kti_refresh_expiration"] = ChannelConfig.kti_refresh_expiration;
            _service.Update(SalesChannelEntity);

            return true;
        }

        public Model.DTO.ChannelManagement.SalesChannel GetbyLazadaSellerID(string SellerID)
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_saleschannel";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_sellerid", ConditionOperator.Equal, SellerID);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnSalesChannel = _service.RetrieveMultiple(qeEntity);
            if (returnSalesChannel.Entities.Any())
            {
                var SalesChannelEntity = returnSalesChannel.Entities.First();
                var SalesChannel = new Model.DTO.ChannelManagement.SalesChannel(SalesChannelEntity);
                SalesChannel.kti_appkey = DecrytSalesChannel(SalesChannel.kti_appkey);
                SalesChannel.kti_appsecret = DecrytSalesChannel(SalesChannel.kti_appsecret);
                SalesChannel.kti_password = DecrytSalesChannel(SalesChannel.kti_password);
                // SalesChannel.ChannelCategoryMappingsList = GetChannelCategoryMappingList(SalesChannelEntity.Id);
                SalesChannel.CustomFieldList = GetCustomFieldList(SalesChannelEntity.Id);
                return SalesChannel;

            }

            return new Model.DTO.ChannelManagement.SalesChannel();
        }

        public List<Model.DTO.ChannelManagement.CustomField> GetCustomFieldList(Guid SalesChannelGUID)
        {

            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_customfieldmapping";
            qeEntity.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_saleschannel", ConditionOperator.Equal, SalesChannelGUID);

            qeEntity.Criteria.AddFilter(entityFilter);

            var returnCustomField = _service.RetrieveMultiple(qeEntity);
            if (returnCustomField.Entities.Any())
            {
                var ListOfCustomFields = returnCustomField.Entities.Select(customfields => new Model.DTO.ChannelManagement.CustomField(customfields)).ToList();

                return ListOfCustomFields;
            }

            return new List<Model.DTO.ChannelManagement.CustomField>();
        }


        //public List<Model.DTO.ChannelManagement.ChannelCategoryMapping> GetChannelCategoryMappingList(Guid SalesChannelGUID)
        //{

        //    QueryExpression qeEntity = new QueryExpression();
        //    qeEntity.EntityName = "kti_channelcategorymapping";
        //    qeEntity.ColumnSet = new ColumnSet(true);

        //    var entityFilter = new FilterExpression(LogicalOperator.And);
        //    entityFilter.AddCondition("kti_saleschannel", ConditionOperator.Equal, SalesChannelGUID);

        //    qeEntity.Criteria.AddFilter(entityFilter);

        //    var returnChannelCategoryMapping = _service.RetrieveMultiple(qeEntity);
        //    if (returnChannelCategoryMapping.Entities.Any())
        //    {
        //        var ListChannelCategoryMapping = returnChannelCategoryMapping.Entities.Select(customfields => new Model.DTO.ChannelManagement.ChannelCategoryMapping(customfields)).ToList();

        //        return ListChannelCategoryMapping;
        //    }

        //    return new List<Model.DTO.ChannelManagement.ChannelCategoryMapping>();
        //}


        private string DecrytSalesChannel(string encryptedstring)
        {
            if (string.IsNullOrWhiteSpace(encryptedstring))
            {
                return encryptedstring;
            }

            return _salesChannelDomain.DecryptKey(encryptedValue: encryptedstring);
        }

        public CRM_Plugin.Models.ChannelManagement.SalesChannel GetStoreByChannelCodeOrigin(string channelCode, int channelOrigin)
        {
            QueryExpression qeSalesChannel = new QueryExpression();

            qeSalesChannel.EntityName = Core.Helper.EntityHelper.SalesChannel.entity_name;
            qeSalesChannel.ColumnSet = new ColumnSet(Core.Helper.EntityHelper.SalesChannel.entity_id,
                Core.Helper.EntityHelper.SalesChannel.account,
                Core.Helper.EntityHelper.SalesChannel.defaultPriceList,
                Core.Helper.EntityHelper.SalesChannel.salePriceList,
                Core.Helper.EntityHelper.SalesChannel.warehouseCode,
                Core.Helper.EntityHelper.SalesChannel.branch);

            var salesChannelFilter = new FilterExpression(LogicalOperator.And);
            salesChannelFilter.AddCondition("kti_saleschannelcode", ConditionOperator.Equal, channelCode);
            salesChannelFilter.AddCondition("kti_channelorigin", ConditionOperator.Equal, channelOrigin);
            qeSalesChannel.Criteria.AddFilter(salesChannelFilter);

            EntityCollection colSalesChannel = _service.RetrieveMultiple(qeSalesChannel);

            if (colSalesChannel.Entities.Count > 0)
            {
                return colSalesChannel.Entities.Select(i => new CRM_Plugin.Models.ChannelManagement.SalesChannel(i)).First();
            }

            return null;
        }
    }
}
