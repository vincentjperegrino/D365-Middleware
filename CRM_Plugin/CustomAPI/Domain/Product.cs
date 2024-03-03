using Microsoft.Xrm.Sdk;
using System;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;
using CRM_Plugin.Core.Domain;

namespace CRM_Plugin.CustomAPI.Domain
{
    public class Product : IProduct<CRM_Plugin.CustomAPI.Model.DTO.Product>
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;
        private readonly string _sku;

        public Product(IOrganizationService service, ITracingService tracingService, string sku)
        {
            _service = service;
            _tracingService = tracingService;
            _sku = sku;
        }
        public Product(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public Product(IOrganizationService service)
        {
            _service = service;
        }

        public bool create(CRM_Plugin.CustomAPI.Model.DTO.Product _product)
        {
            Entity newProduct = new Entity(CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name);

            //description
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.description] = _product.description;

            //size
            var listSize = _product.size.Split(',').ToList();

            if (listSize.ElementAtOrDefault(0) != null)
            {
                newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_height] = listSize[0];
            }

            if (listSize.ElementAtOrDefault(2) != null)
            {
                newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_weight] = listSize[2];
            }

            if (listSize.ElementAtOrDefault(4) != null)
            {
                newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_width] = listSize[4];
            }

            if (listSize.ElementAtOrDefault(6) != null)
            {
                newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_weight] = listSize[6];
            }

            //current cost
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.currentcost] = _product.currentcost;

            //price
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.price] = _product.price;

            //price
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.name] = _product.name;

            //quantity on hand
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.quantityonhand] = _product.quantityonhand;

            //suppliername
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.suppliername] = _product.suppliername;

            //validfromdate
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.validfromdate] = _product.validfromdate;

            //validfromdate
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.validtodate] = _product.validtodate;

            //vendorid
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorid] = _product.vendorid;

            //vendorname
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorname] = _product.vendorname;

            //vendorpartnumber
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorpartnumber] = _product.vendorpartnumber;

            //pricelist
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.pricelevelid] = this.GetDefaultPriceList(_product.companyid).ToEntityReference();

            //unitgroup
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.pricelevelid] = this.GetUOMGroup(_product.defaultuomscheduleid).ToEntityReference();

            //unitofmeasurement
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.defaultuomscheduleid] = this.GetDefaultPriceList(_product.companyid).ToEntityReference();

            //productnumber
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorpartnumber] = _product.productnumber;

            //sku
            newProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.sku] = _product.kti_sku;

            _service.Create(newProduct);

            return true;
        }

        public bool update(CRM_Plugin.CustomAPI.Model.DTO.Product _product, Entity _upProduct)
        {
            Entity upProduct = new Entity(CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name);

            upProduct.Id = _upProduct.Id;

            //description
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.description] = _product.description;

            //size
            var listSize = _product.size.Split(',').ToList();

            if (listSize.ElementAtOrDefault(0) != null)
            {
                upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_height] = listSize[0];
            }

            if (listSize.ElementAtOrDefault(2) != null)
            {
                upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_weight] = listSize[2];
            }

            if (listSize.ElementAtOrDefault(4) != null)
            {
                upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_width] = listSize[4];
            }

            if (listSize.ElementAtOrDefault(6) != null)
            {
                upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.kti_weight] = listSize[6];
            }

            //current cost
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.currentcost] = _product.currentcost;

            //price
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.price] = _product.price;

            //price
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.name] = _product.name;

            //quantity on hand
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.quantityonhand] = _product.quantityonhand;

            //suppliername
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.suppliername] = _product.suppliername;

            //validfromdate
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.validfromdate] = _product.validfromdate;

            //validfromdate
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.validtodate] = _product.validtodate;

            //vendorid
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorid] = _product.vendorid;

            //vendorname
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorname] = _product.vendorname;

            //vendorpartnumber
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.vendorpartnumber] = _product.vendorpartnumber;

            //pricelist
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.pricelevelid] = this.GetDefaultPriceList(_product.companyid).ToEntityReference();

            //unitgroup
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.pricelevelid] = this.GetUOMGroup(_product.defaultuomscheduleid).ToEntityReference();

            //unitofmeasurement
            upProduct[CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.defaultuomscheduleid] = this.GetDefaultPriceList(_product.companyid).ToEntityReference();

            _service.Update(upProduct);

            return true;
        }

        public bool upsert(CRM_Plugin.CustomAPI.Model.DTO.Product _product)
        {
            if (_product.kti_sku != null)
                throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.sku);

            Entity existingProduct = ValidateSKU(_product.kti_sku);

            //Update
            if (Guid.Empty != existingProduct.Id)
            {
                this.update(_product, existingProduct);

                return true;
            }
            //Create

            this.create(_product);

            return true;

        }

        public Entity GetProductByProductNumber(string productNumber)
        {
            return ValidateProductNumber(productNumber);
        }
        private Entity ValidateProductNumber(string productNumber)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.productnumber,
                            Operator = ConditionOperator.Equal,
                            Values = { productNumber }
                        }
                    }
                }
            };

            var ecProduct = _service.RetrieveMultiple(query);

            if (ecProduct.Entities.Count > 0)
            {
                return ecProduct.Entities.First();
            }

            return null;
        }

        public Entity GetProductBySKU(string sku)
        {
            return ValidateSKU(sku);
        }
        private Entity ValidateSKU(string sku)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name,
                ColumnSet = new ColumnSet(true),
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

            var ecProduct = _service.RetrieveMultiple(query);

            if (ecProduct.Entities.Count > 0)
            {
                return ecProduct.Entities.First();
            }

            return null;
        }

        private Entity GetDefaultPriceList(int _companyID)
        {
            string priceList = Moo.Config.DEV_PRICELIST_DEFAULT;

            if (_companyID == Moo.Config.CompanyID_NCCI_TEST || _companyID == Moo.Config.CompanyID_NCCI_PROD)
            {
                priceList = Moo.Config.NCCI_PRICELIST_DEFAULT;
            }

            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.entity_name,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.PriceList.name.ToLower(),
                            Operator = ConditionOperator.Contains,
                            Values = { priceList }
                        }
                    }
                }
            };

            var ecPriceList = _service.RetrieveMultiple(query);

            if (ecPriceList.Entities.Count > 0)
            {
                return ecPriceList.Entities.First();
            }

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

            var ecPriceList = _service.RetrieveMultiple(query);

            if (ecPriceList.Entities.Count > 0)
            {
                return ecPriceList.Entities.First();
            }
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

            var ecPriceList = _service.RetrieveMultiple(query);

            if (ecPriceList.Entities.Count > 0)
            {
                return ecPriceList.Entities.First();
            }

            throw new ArgumentNullException(CRM_Plugin.Core.Helper.EntityHelper.CRM.Master.UnitOfMeasurement.name);
        }

        public EntityCollection GetAll()
        {

            QueryExpression query = new QueryExpression
            {
                EntityName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.entity_name,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.statecode,
                            Operator = ConditionOperator.Equal,
                            Values = { 0 }
                        },
                        new ConditionExpression
                        {
                            AttributeName = CRM_Plugin.Core.Helper.EntityHelper.CRM.Product.productstructure,
                            Operator = ConditionOperator.In,
                            Values = { 1 , 3 }
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
