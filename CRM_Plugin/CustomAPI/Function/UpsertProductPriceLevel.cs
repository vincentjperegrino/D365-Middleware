using System;
using Microsoft.Xrm.Sdk;
using CRM_Plugin.CustomAPI.Domain;
using System.Collections.Generic;
using System.Linq;


namespace CRM_Plugin.CustomAPI.Function
{
    public class UpsertProductPriceLevel
    {
        private readonly Domain.ProductPriceLevel _productPriceLevel;

        public UpsertProductPriceLevel(IOrganizationService service)
        {
            this._productPriceLevel = new Domain.ProductPriceLevel(service);
        }

        public string Process(ParameterCollection customAPIProduct)
        {
            if (customAPIProduct is null)
            {
                throw new ArgumentNullException(nameof(customAPIProduct));
            }

            string erroron = "";

            try
            {
                erroron = "Data mapping of custom API parameters. ";
                EntityCollection priceListItem = (EntityCollection)customAPIProduct[CustomAPI.Helper.EntityHelper.Product.pricelistitem];
                var listPriceListItem = priceListItem.Entities.Select(entity => new CustomAPI.Model.DTO.ProductPriceLevel(entity)).ToList();

                erroron = "Loop Price List Item";
                foreach(var eProductPriceLevel in listPriceListItem)
                {
                    erroron = "Upsert process to D365";
                    _productPriceLevel.upsert(eProductPriceLevel);
                }

                return "Success";
            }
            catch (Exception ex)
            {
                throw new Exception(erroron + ex);
            }
        }
    }
}
