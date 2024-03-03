using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Model.DTO
{
    public class OrderDetails : CRM_Plugin.Models.Sales.OrderDetails
    {
        CustomAPI.Domain.Product productDomain;
        CustomAPI.Domain.Order orderDomain;
        CustomAPI.Domain.Currency currencyDomain;
        CRM_Plugin.Domain.Employee employeeDomain;

        public OrderDetails(CRM_Plugin.CustomAPI.Model.DTO.Orders order, Entity en, IOrganizationService service, ITracingService tracingService)
        {
            productDomain = new CustomAPI.Domain.Product(service, tracingService);
            orderDomain = new CustomAPI.Domain.Order(service, tracingService);
            currencyDomain = new CustomAPI.Domain.Currency(service, tracingService);
            employeeDomain = new CRM_Plugin.Domain.Employee(service, tracingService);

            if (en.Contains("kti_lineitemnumber"))
            {
                this.kti_lineitemnumber = (string)en["kti_lineitemnumber"];
            }
            if (en.Contains("kti_socialchannelorigin"))
            {
                this.kti_socialchannelorigin = (int)en["kti_socialchannelorigin"];
            }

            if (en.Contains("kti_sourceid"))
            {
                this.kti_sourceid = (string)en["kti_sourceid"];
            }

            if(String.IsNullOrEmpty(this.kti_sourceid))
            {
                this.kti_sourceid = order.kti_sourceid;
            }

            if (String.IsNullOrEmpty(this.kti_sourceid))
                throw new Exception("No sales order header related.");

            if (en.Contains("kti_approvedmanager"))
            {
                this.kti_approvedmanager = employeeDomain.GetEmployeeByStaffCode((string)en["kti_approvedmanager"]).Id.ToString();
            }

            //Reference Order number
            if (en.Contains("salesorderid"))
            {
                var salesOrder = orderDomain.GetSalesOrderBySourceIDChannel((string)en["salesorderid"], this.kti_socialchannelorigin);

                if(salesOrder != null)
                    this.salesorderid = salesOrder.Id.ToString();
            }

            if (String.IsNullOrEmpty(this.salesorderid) && !String.IsNullOrEmpty(order.kti_sourceid))
            {
                this.salesorderid = orderDomain.GetSalesOrderBySourceIDChannel(order.kti_sourceid, this.kti_socialchannelorigin).Id.ToString();
            }

            if (String.IsNullOrEmpty(this.salesorderid))
                throw new Exception("No sales order header related.");

            if (en.Contains("overriddencreatedon"))
            {
                this.overriddencreatedon = (DateTime)en["overriddencreatedon"];
            }
            if(this.overriddencreatedon == null || DateTime.MinValue == this.overriddencreatedon)
            {
                this.overriddencreatedon = DateTime.Now;
            }

            if (en.Contains("description"))
            {
                this.description = (string)en["description"];
            }
            if (en.Contains("extendedamount"))
            {
                this.extendedamount = decimal.Parse(en["extendedamount"].ToString());
            }
            if (en.Contains("ispriceoverridden"))
            {
                this.ispriceoverridden = (bool)en["ispriceoverridden"];
            }
            if (en.Contains("isproductoverridden"))
            {
                this.isproductoverridden = (bool)en["isproductoverridden"];
            }
            if (en.Contains("manualdiscountamount"))
            {
                this.manualdiscountamount = decimal.Parse(en["manualdiscountamount"].ToString());
            }
            if (en.Contains("priceperunit"))
            {
                this.priceperunit = decimal.Parse(en["priceperunit"].ToString());
            }
            if (en.Contains("productdescription"))
            {
                this.productdescription = (string)en["productdescription"];
            }

            //Check Product by SKU
            if (en.Contains("productid"))
            {
                var product = productDomain.GetProductByProductNumber((string)en["productid"]);

                if(product != null)
                    this.productid = product.Id.ToString();
            }

            //if (String.IsNullOrEmpty(this.productid) && this.isproductoverridden == false)
            //    throw new Exception("No product found.");

            if (en.Contains("productdescription"))
            {
                this.productname = (string)en["productdescription"];
            }

            //if(String.IsNullOrEmpty(this.productname) && String.IsNullOrEmpty(this.productid))
            //    throw new Exception("No product found.");

            if (en.Contains("quantity"))
            {
                this.quantity = decimal.Parse(en["quantity"].ToString());
            }

            if(this.quantity <= 0)
                throw new Exception("No quantity found.");

            if (en.Contains("requestdeliveryby"))
            {
                DateTime dtmRequestDeliveryBy;

                if (DateTime.TryParse((string)en["requestdeliveryby"], out dtmRequestDeliveryBy))
                {
                    this.requestdeliveryby = dtmRequestDeliveryBy;
                }
            }
            if (en.Contains("shipTo_name"))
            {
                this.shipTo_name = (string)en["shipTo_name"];
            }
            if (en.Contains("shipto_telephone"))
            {
                this.shipto_telephone = (string)en["shipto_telephone"];
            }
            if (en.Contains("shipto_city"))
            {
                this.shipto_city = (string)en["shipto_city"];
            }
            if (en.Contains("shipto_contactname"))
            {
                this.shipto_contactname = (string)en["shipto_contactname"];
            }
            if (en.Contains("shipto_country"))
            {
                this.shipto_country = (string)en["shipto_country"];
            }
            if (en.Contains("shipto_fax"))
            {
                this.shipto_fax = (string)en["shipto_fax"];
            }
            if (en.Contains("shipto_line1"))
            {
                this.shipto_line1 = (string)en["shipto_line1"];
            }
            if (en.Contains("shipto_line2"))
            {
                this.shipTo_line2 = (string)en["shipto_line2"];
            }
            if (en.Contains("shipto_line3"))
            {
                this.shipTo_line3 = (string)en["shipto_line3"];
            }
            if (en.Contains("shipto_postalcode"))
            {
                this.shipto_postalcode = (string)en["shipto_postalcode"];
            }
            if (en.Contains("shipto_stateorprovince"))
            {
                this.shipto_stateorprovince = (string)en["shipto_stateorprovince"];
            }
            if (en.Contains("tax"))
            {
                this.tax = decimal.Parse(en["tax"].ToString());
            }
            //Check if transaction currency contains isocurrencycode or get blank it out to get the sales order header currency code
            if (en.Contains("transactioncurrencyid"))
            {
                this.transactioncurrencyid = currencyDomain.GetCurrecyByISOCurrencyCode((string)en["transactioncurrencyid"]).transactioncurrencyid;
            }
            //Check unit of measurement or get base unit of measurement of the product from the default unit group
            if (en.Contains("uomid"))
            {
                this.uomid = (string)en["uomid"];
            }
            else if(en.Contains("productid"))
            {
                var product = productDomain.GetProductByProductNumber((string)en["productid"]);

                if (product != null) 
                    this.uomid = ((EntityReference)product["defaultuomid"]).Id.ToString();
            }

            //if(this.uomid == null)
            //    throw new Exception("No unit of measurement setup.");

            if (en.Id != Guid.Empty)
            {
                this.orderdetailsid = en.Id;
            }
        }

        public string kti_approvedmanager { get; set; }
        public string kti_sourceid { get; set; }
    }
}
