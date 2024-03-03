using CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class ChannelManagementInventory : Core.Domain.IChannelManagement<Model.DTO.ChannelManagement.Inventory>
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly CRM_Plugin.SalesChannel.Domain.SalesChannel _salesChannelDomain;

        public ChannelManagementInventory(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
            _salesChannelDomain = new CRM_Plugin.SalesChannel.Domain.SalesChannel();
        }

        public ChannelManagementInventory(IOrganizationService service)
        {
            _service = service;
            _salesChannelDomain = new CRM_Plugin.SalesChannel.Domain.SalesChannel();
        }


        public Model.DTO.ChannelManagement.Inventory Get(string ChannelCode)
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
                var SalesChannel = new Model.DTO.ChannelManagement.Inventory(SalesChannelEntity);
                SalesChannel.kti_appkey = DecrytSalesChannel(SalesChannel.kti_appkey);
                SalesChannel.kti_appsecret = DecrytSalesChannel(SalesChannel.kti_appsecret);
                SalesChannel.kti_password = DecrytSalesChannel(SalesChannel.kti_password);
                SalesChannel.InventoryList = GetProductsFromStore(new Guid(SalesChannel.kti_saleschannelId));
                return SalesChannel;

            }

            return new Model.DTO.ChannelManagement.Inventory();

        }

        public List<Model.DTO.ChannelManagement.Inventory> GetChannelList()
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = "kti_saleschannel";
            qeEntity.ColumnSet = new ColumnSet(true);

            var returnSalesChannel = _service.RetrieveMultiple(qeEntity);

            if (returnSalesChannel.Entities.Any())
            {
                var SalesListChannel = returnSalesChannel.Entities.Select(saleschannel =>
                {
                    var SalesChannels = new Model.DTO.ChannelManagement.Inventory(saleschannel);
                    SalesChannels.kti_appkey = DecrytSalesChannel(SalesChannels.kti_appkey);
                    SalesChannels.kti_appsecret = DecrytSalesChannel(SalesChannels.kti_appsecret);
                    SalesChannels.kti_password = DecrytSalesChannel(SalesChannels.kti_password);
                    return SalesChannels;

                }).ToList();

                SalesListChannel = GetProductsFromStoreList(SalesListChannel);

                return SalesListChannel;
            }

            return new List<Model.DTO.ChannelManagement.Inventory>();
        }


        public List<Model.DTO.Inventory> GetProductsFromStore(Guid SalesChannelGUID)
        {

            var SalesChannelTable = new QueryExpression();
            SalesChannelTable.EntityName = "kti_saleschannelproduct";
            SalesChannelTable.ColumnSet = new ColumnSet("kti_saleschannel", "kti_product");

            var Filter = new FilterExpression(LogicalOperator.And);
            Filter.AddCondition("kti_saleschannel", ConditionOperator.Equal, SalesChannelGUID);

            SalesChannelTable.Criteria.AddFilter(Filter);

            var InnerJoinProduct = new LinkEntity("kti_saleschannelproduct", "product", "kti_product", "productid", JoinOperator.Inner);
            InnerJoinProduct.Columns = new ColumnSet(true);
            InnerJoinProduct.EntityAlias = "ProductsTable";

            SalesChannelTable.LinkEntities.Add(InnerJoinProduct);

            var returnProduct = _service.RetrieveMultiple(SalesChannelTable);

            if (returnProduct.Entities.Any())
            {
                var ListOfProduct = returnProduct.Entities.Select(product => new Model.DTO.Inventory()
                {
                    qtyonhand = product.Contains("ProductsTable.quantityonhand") ? (decimal)(((AliasedValue)product["ProductsTable.quantityonhand"]).Value) : default,
                    product = product.Contains("ProductsTable.productnumber") ? (string)(((AliasedValue)product["ProductsTable.productnumber"]).Value) : default

                }).ToList();

                return ListOfProduct;
            }

            return new List<Model.DTO.Inventory>();
        }


        public List<Model.DTO.ChannelManagement.Inventory> GetProductsFromStoreList(List<Model.DTO.ChannelManagement.Inventory> salesChannels)
        {
            if (salesChannels is null || salesChannels.Count <= 0)
            {
                throw new ArgumentNullException(nameof(salesChannels));
            }

            var SalesChannelTable = new QueryExpression();
            SalesChannelTable.EntityName = "kti_saleschannelproduct";
            SalesChannelTable.ColumnSet = new ColumnSet("kti_saleschannel", "kti_product");

            var InnerJoinProduct = new LinkEntity("kti_saleschannelproduct", "product", "kti_product", "productid", JoinOperator.Inner);
            InnerJoinProduct.Columns = new ColumnSet("productnumber", "quantityonhand");
            InnerJoinProduct.EntityAlias = "ProductsTable";

            SalesChannelTable.LinkEntities.Add(InnerJoinProduct);

            var returnProduct = _service.RetrieveMultiple(SalesChannelTable);

            if (returnProduct.Entities.Any())
            {

                salesChannels = salesChannels.Select(store =>
                {
                    store.InventoryList = returnProduct.Entities.Where(product => ProductOfStore(store, product))
                                                                .Select(product => new Model.DTO.Inventory()
                                                                {
                                                                    qtyonhand = product.Contains("ProductsTable.quantityonhand") ? (decimal)(((AliasedValue)product["ProductsTable.quantityonhand"]).Value) : default,
                                                                    product = product.Contains("ProductsTable.productnumber") ? (string)(((AliasedValue)product["ProductsTable.productnumber"]).Value) : default

                                                                }).ToList();

                    return store;
                }).ToList();

                return salesChannels;
            }

            return salesChannels;
        }

        private static bool ProductOfStore(Model.DTO.ChannelManagement.Inventory store, Entity product)
        {
            return product.Contains("kti_saleschannel") && ((EntityReference)product["kti_saleschannel"]).Id.ToString() == store.kti_saleschannelId;
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

        private string DecrytSalesChannel(string encryptedstring)
        {
            if (string.IsNullOrWhiteSpace(encryptedstring))
            {
                return encryptedstring;
            }

            return _salesChannelDomain.DecryptKey(encryptedValue: encryptedstring);
        }

        public bool UpdateToken(Inventory ChannelConfig)
        {
            throw new NotImplementedException();
        }

        public Inventory GetbyLazadaSellerID(string SellerID)
        {
            throw new NotImplementedException();
        }
    }
}
