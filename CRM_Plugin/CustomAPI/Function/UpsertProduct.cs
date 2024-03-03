using System;
using Microsoft.Xrm.Sdk;
using CRM_Plugin.CustomAPI.Domain;
using System.Collections.Generic;
using System.Linq;
using CRM_Plugin.Core.Domain;

namespace CRM_Plugin.CustomAPI.Function
{
    public class UpsertProduct
    {
        private readonly Domain.Product _product;

        public UpsertProduct(IOrganizationService service)
        {
            this._product = new Domain.Product(service);
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

                var dtoProduct = new CRM_Plugin.CustomAPI.Model.DTO.Product(customAPIProduct);

                erroron = "Upsert process to D365";

                if (_product.upsert(dtoProduct))
                {
                    return "Success";
                }

                return "Failed";
            }
            catch (Exception ex)
            {
                throw new Exception(erroron + ex);
            }
        }
    }
}
