using Microsoft.Xrm.Sdk;
using System;

namespace CRM_Plugin.CustomAPI.Model.DTO
{
    public class SerialNumbers : CRM_Plugin.Models.Sales.OrderLineSerialNumber
    {
        CustomAPI.Domain.Product productDomain;
        CustomAPI.Domain.Order orderDomain;
        CRM_Plugin.Domain.OrderLine orderLineDomain;
        CustomAPI.Domain.Currency currencyDomain;

        public SerialNumbers(CRM_Plugin.CustomAPI.Model.DTO.Orders order, Entity en, IOrganizationService service, ITracingService tracingService)
        {
            productDomain = new CustomAPI.Domain.Product(service, tracingService);
            orderDomain = new CustomAPI.Domain.Order(service, tracingService);
            currencyDomain = new CustomAPI.Domain.Currency(service, tracingService);
            orderLineDomain = new CRM_Plugin.Domain.OrderLine(service, tracingService);

            if (en.Id != Guid.Empty)
            {
                this.kti_orderlineserialnumberid = en.Id;
            }

            if (en.Contains("kti_lineitemnumber"))
            {
                this.kti_orderdetaillineitemnumber = (string)en["kti_lineitemnumber"];
            }

            if(String.IsNullOrEmpty(this.kti_orderdetaillineitemnumber))
                throw new Exception("No order detail line item number found.");

            if (en.Contains("kti_socialchannelorigin"))
            {
                this.kti_socialchannelorigin = (int)en["kti_socialchannelorigin"];
            }

            if (this.kti_socialchannelorigin == 0)
                throw new Exception("No channel origin found.");

            //Reference Order number
            if (en.Contains("kti_sourceid"))
            {
                var orderLine = orderLineDomain.GetSalesOrderDetailByKey((string)en["kti_sourceid"], this.kti_socialchannelorigin, this.kti_orderdetaillineitemnumber);

                if (orderLine != null)
                    this.kti_orderline = orderLine.ToEntityReference();
            }

            if (this.kti_orderline == null && (!String.IsNullOrEmpty(order.kti_sourceid) && this.kti_socialchannelorigin != 0))
            {
                var orderLine = orderLineDomain.GetSalesOrderDetailByKey(order.kti_sourceid, this.kti_socialchannelorigin, this.kti_orderdetaillineitemnumber);

                if (orderLine != null)
                    this.kti_orderline = orderLine.ToEntityReference();
            }

            if (Guid.Empty == this.kti_orderline.Id)
                throw new Exception("No source header found.");

            if (en.Contains("productid"))
            {
                var product = productDomain.GetProductByProductNumber((string)en["productid"]);
                
                if(product != null)
                {
                    this.kti_product = product.ToEntityReference();

                    if (product.Contains("productnumber"))
                    {
                        this.kti_sku = (string)product["productnumber"];
                    }
                }
            }

            if (this.kti_product.Id == Guid.Empty)
                throw new Exception("No product found.");

            if (en.Contains("parentproductid"))
            {
                var parentProduct = productDomain.GetProductBySKU((string)en["parentproductid"]);

                if (parentProduct != null)
                    this.kti_parentbundleproduct = parentProduct.ToEntityReference();
            }

            if (en.Contains("serialnumber"))
            {
                this.kti_serialnumber = (string)en["serialnumber"];
            }

            if (String.IsNullOrEmpty(this.kti_serialnumber))
                throw new Exception("Serial number cannot be empty.");

            if (en.Contains("warrantystartdate"))
            {
                this.kti_warrantystartdate = (DateTime)en["warrantystartdate"];
            }
            else
            {
                this.kti_warrantystartdate = DateTime.Now.Date;
            }
        }
    }
}
