using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using DTO_EntityHelper = CRM_Plugin.CustomAPI.Helper.EntityHelper;

namespace CRM_Plugin
{
    public class UpsertProduct : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = servicefactory.CreateOrganizationService(context.UserId);

            string erroron = "";

            try
            {
                erroron = "Error on parameters: ";
                var customParameters = context.InputParameters;
                var EntityName = "Product";

                if (Checker_Context_Input(customParameters))
                {           
                    erroron = "Error on process: ";
                    var domainForProduct = new CustomAPI.Function.UpsertProduct(service);
                    string outputMessage = domainForProduct.Process(customParameters);

                    erroron = "Error on process: ";
                    var domainForProductPriceLevel = new CustomAPI.Function.UpsertProductPriceLevel(service);
                    outputMessage = domainForProductPriceLevel.Process(customParameters);

                    context.OutputParameters[DTO_EntityHelper.Product.response] = outputMessage;
                }
            }

            catch (Exception ex)
            {
                tracingService.Trace(erroron + ex.Message);
                throw new Exception(erroron + ex.Message);
            }

        }

        public bool Checker_Context_Input(ParameterCollection customParameters)
        {

            if (!customParameters.Contains(DTO_EntityHelper.Product.productid))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.productid);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.productnumber))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.productnumber);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.companyid))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.companyid);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.name))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.name);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.defaultuomscheduleid))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.defaultuomscheduleid);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.productstructure))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.productstructure);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.moosourcesystem))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.moosourcesystem);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.productstructure))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.productstructure);
            }

            if (!customParameters.Contains(DTO_EntityHelper.Product.pricelistitem))
            {
                throw new Exception("Custom API: Missing field " + DTO_EntityHelper.Product.pricelistitem);
            }

            return true;
        }
    }
}
