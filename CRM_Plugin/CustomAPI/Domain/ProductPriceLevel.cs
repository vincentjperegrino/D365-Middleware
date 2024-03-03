using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;
using CRM_Plugin.Core.Domain;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class ProductPriceLevel : IProductPriceLevel<CRM_Plugin.CustomAPI.Model.DTO.ProductPriceLevel>
    {

        private readonly IOrganizationService service;

        public ProductPriceLevel(IOrganizationService _service)
        {
            service = _service;
        }

        public bool create(CRM_Plugin.CustomAPI.Model.DTO.ProductPriceLevel _productPriceLevel)
        {
            Entity newProductPriceLevel = new Entity(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.entity_name);

            //amount
            newProductPriceLevel[CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.amount] = _productPriceLevel.amount;

            //pricelevelid
            newProductPriceLevel[CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.pricelevelid] = GetPriceListByName(_productPriceLevel.pricelevelid).ToEntityReference();

            //pricelevelid
            newProductPriceLevel[CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.productid] = GetProductBySKU(_productPriceLevel.productid).ToEntityReference();

            //pricelevelid
            newProductPriceLevel[CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.uomid] = GetUOM(_productPriceLevel.uomid).ToEntityReference();

            service.Create(newProductPriceLevel);

            return true;
        }

        public bool update(CRM_Plugin.CustomAPI.Model.DTO.ProductPriceLevel _productPriceLevel, Entity _upProductPriceLevel)
        {
            Entity upProductPriceLevel = new Entity(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.entity_name);

            upProductPriceLevel.Id = _upProductPriceLevel.Id;

            //amount
            upProductPriceLevel[CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.amount] = _productPriceLevel.amount;

            service.Update(upProductPriceLevel);

            return true;
        }

        public bool upsert(CRM_Plugin.CustomAPI.Model.DTO.ProductPriceLevel _productPriceLevel)
        {
            if (_productPriceLevel.productid != null)
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.productid);

            if (_productPriceLevel.pricelevelid != null)
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.pricelevelid);

            if (_productPriceLevel.uomid != null)
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.uomid);

            Entity existingProductPriceLevel = GetPriceListItem(_productPriceLevel.productid, _productPriceLevel.pricelevelid, _productPriceLevel.uomid);

            ////Update
            if (Guid.Empty != existingProductPriceLevel.Id)
            {
                this.update(_productPriceLevel, existingProductPriceLevel);

                return true;
            }
            //Create
            else
            {
                this.create(_productPriceLevel);

                return true;
            }
        }

        private Entity GetPriceListItem(string _sku, string _priceList, string _uom)
        {
            var eProduct = GetProductBySKU(_sku);
            var ePriceList = GetPriceListByName(_priceList);
            var eUOM = GetUOM(_uom);

            return ValidateProductPriceLevel(eProduct, ePriceList, eUOM);
        }

        private Entity ValidateProductPriceLevel(Entity _product, Entity _priceList, Entity _uom)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.entity_name,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.productid,
                            Operator = ConditionOperator.Equal,
                            Values = { _product.Id }
                        },
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.pricelevelid,
                            Operator = ConditionOperator.Equal,
                            Values = { _priceList.Id }
                        },
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.uomid,
                            Operator = ConditionOperator.Equal,
                            Values = { _uom.Id }
                        }
                    }
                }
            };

            var ecProductPriceLevel = service.RetrieveMultiple(query);

            if (ecProductPriceLevel.Entities.Count > 0)
                return ecProductPriceLevel.Entities.First();
            else
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.productid);
        }

        private Entity GetProductBySKU(string sku)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name,
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.sku,
                            Operator = ConditionOperator.Equal,
                            Values = { sku }
                        }
                    }
                }
            };

            var ecProduct = service.RetrieveMultiple(query);

            if (ecProduct.Entities.Count > 0)
                return ecProduct.Entities.First();
            else
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.ProductPriceLevel.productid);
        }

        private Entity GetPriceListByName(string _priceListName)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name,
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.name,
                            Operator = ConditionOperator.Contains,
                            Values = { _priceListName }
                        }
                    }
                }
            };

            var ecPriceList = service.RetrieveMultiple(query);

            if (ecPriceList.Entities.Count > 0)
                return ecPriceList.Entities.First();
            else
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.name);
        }

        private Entity GetUOMGroup(string _uomGroup)
        {

            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitGroup.entity_name,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitGroup.name,
                            Operator = ConditionOperator.Contains,
                            Values = { _uomGroup }
                        }
                    }
                }
            };

            var ecPriceList = service.RetrieveMultiple(query);

            if (ecPriceList.Entities.Count > 0)
                return ecPriceList.Entities.First();
            else
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitGroup.name);
        }

        private Entity GetUOM(string _uom)
        {

            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitOfMeasurement.entity_name,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitOfMeasurement.name,
                            Operator = ConditionOperator.Contains,
                            Values = { _uom }
                        }
                    }
                }
            };

            var ecPriceList = service.RetrieveMultiple(query);

            if (ecPriceList.Entities.Count > 0)
                return ecPriceList.Entities.First();
            else
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitOfMeasurement.name);
        }


    }
}
