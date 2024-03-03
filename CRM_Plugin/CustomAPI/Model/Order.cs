using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.CustomAPI.Model
{
    public class Order : Entity
    {
        IOrganizationService _service;
        ITracingService _tracingService;
        CRM_Plugin.Domain.Order orderDomain;
        CustomAPI.Domain.Customer customerDomain;
        CustomAPI.Domain.PriceLevel priceLevelDomain;
        CustomAPI.Domain.ChannelManagement channelDomain;
        CustomAPI.Domain.Currency currencyDomain;
        CustomAPI.Domain.Branch branchDomain;
        CRM_Plugin.Domain.Employee employeeDomain;

        public Order(Entity en, IOrganizationService service, ITracingService tracingService)
        {
            string channelCode = "";
            _service = service;
            customerDomain = new CustomAPI.Domain.Customer(service, tracingService);
            priceLevelDomain = new CustomAPI.Domain.PriceLevel(service, tracingService);
            channelDomain = new CustomAPI.Domain.ChannelManagement(service, tracingService);
            orderDomain = new CRM_Plugin.Domain.Order(service, tracingService);
            currencyDomain = new CustomAPI.Domain.Currency(service, tracingService);
            branchDomain = new CustomAPI.Domain.Branch(service, tracingService);
            employeeDomain = new CRM_Plugin.Domain.Employee(service, tracingService);

            if (en.Contains("overriddencreatedon"))
            {
                this["overriddencreatedon"] = (DateTime)en["overriddencreatedon"];
            }

            if (en.Contains("kti_socialchannelorigin"))
            {
                this["kti_socialchannelorigin"] = new OptionSetValue((int)en["kti_socialchannelorigin"]);
            }

            if ((int)this["kti_socialchannelorigin"] != 959080013)
            {
                if (String.IsNullOrEmpty((string)en["name"]))
                    throw new Exception("No order number generated from the source platform.");
            }

            if (en.Contains("name"))
            {
                this["name"] = (string)en["name"];
            }
            else
            {
                this["name"] = orderDomain.GenerateOrderNumber((int)this["kti_socialchannelorigin"], (DateTime)this["overriddencreatedon"]);
            }

            if (en.Contains("billto_city"))
            {
                this["billto_city"] = (string)en["billto_city"];
            }

            if (en.Contains("billto_contactName"))
            {
                this["billto_contactname"] = (string)en["billto_contactName"];
            }
            if (en.Contains("billto_country"))
            {
                this["billto_country"] = (string)en["billto_country"];
            }
            if (en.Contains("billto_fax"))
            {
                this["billto_fax"] = (string)en["billto_fax"];
            }
            if (en.Contains("billto_line1"))
            {
                this["billto_line1"] = (string)en["billto_line1"];
            }
            if (en.Contains("billto_line2"))
            {
                this["billto_line2"] = (string)en["billto_line2"];
            }
            if (en.Contains("billto_line3"))
            {
                this["billto_line3"] = (string)en["billto_line3"];
            }
            if (en.Contains("billto_name"))
            {
                this["billto_name"] = (string)en["billto_name"];
            }
            if (en.Contains("billto_postalcode"))
            {
                this["billto_postalcode"] = (string)en["billto_postalcode"];
            }
            if (en.Contains("billto_stateorprovince"))
            {
                this["billto_stateorprovince"] = (string)en["billto_stateorprovince"];
            }
            if (en.Contains("billto_telephone"))
            {
                this["billto_telephone"] = (string)en["billto_telephone"];
            }
            if (en.Contains("kti_sourceid"))
            {
                this["kti_sourceid"] = (string)en["kti_sourceid"];
            }
            if (en.Contains("emailaddress"))
            {
                this["emailaddress"] = (string)en["emailaddress"];
            }
            if (en.Contains("shipto_name"))
            {
                this["shipto_name"] = (string)en["shipto_name"];
            }
            if (en.Contains("shipto_telephone"))
            {
                this["shipto_telephone"] = (string)en["shipto_telephone"];
            }
            if (en.Contains("shipto_city"))
            {
                this["shipto_city"] = (string)en["shipto_city"];
            }
            if (en.Contains("shipto_contactName"))
            {
                this["shipto_contactname"] = (string)en["shipto_contactName"];
            }
            if (en.Contains("shipto_country"))
            {
                this["shipto_country"] = (string)en["shipto_country"];
            }
            if (en.Contains("shipto_fax"))
            {
                this["shipto_fax"] = (string)en["shipto_fax"];
            }
            if (en.Contains("shipto_freighttermscode"))
            {
                this["shipto_freighttermscode"] = (int)en["shipto_freighttermscode"];
            }
            if (en.Contains("shipto_line1"))
            {
                this["shipto_line1"] = (string)en["shipto_line1"];
            }
            if (en.Contains("shipto_line2"))
            {
                this["shipto_line2"] = (string)en["shipto_line2"];
            }
            if (en.Contains("shipto_line3"))
            {
                this["shipto_line3"] = (string)en["shipto_line3"];
            }
            if (en.Contains("shipto_postalcode"))
            {
                this["shipto_postalcode"] = (string)en["shipto_postalcode"];
            }
            if (en.Contains("shipto_stateorprovince"))
            {
                this["shipto_stateorprovince"] = (string)en["shipto_stateorprovince"];
            }
            if (en.Contains("customerid")) //customercode
            {
                if(Guid.TryParse((string)en["customerid"], out Guid _customerID))
                {
                    this["customerid"] = customerDomain.GetCustomerEntityBySourceIDChannelOrigin((string)en["customerid"], (int)en["kti_socialchannelorigin"]).ToEntityReference();
                }
                else if(!String.IsNullOrEmpty((string)en["customerid"]) &&
                (!String.IsNullOrEmpty((string)this["emailaddress"]) || !String.IsNullOrEmpty((string)this["shipto_telephone"])))
                {
                    this["customerid"] = customerDomain.GetCustomerEntityByNameEmailContact((string)this["customerid"], (string)this["emailaddress"], (string)this["shipto_telephone"]);
                }
            }

            if ((EntityReference)this["customerid"] != null)
                throw new Exception("No customer found.");

            if (en.Contains("datefulfilled"))
            {
                this["datefulfilled"] = (DateTime)en["datefulfilled"];
            }
            if (en.Contains("description"))
            {
                this["description"] = (string)en["description"];
            }
            if (en.Contains("discountamount"))
            {
                this["discountamount"] = (decimal)en["discountamount"];
            }
            if (en.Contains("discountpercentage"))
            {
                this["discountpercentage"] = (decimal)en["discountpercentage"];
            }
            if (en.Contains("freightamount"))
            {
                this["freightamount"] = (decimal)en["freightamount"];
            }

            if (en.Contains("kti_channelcode"))
            {
                channelCode = (string)en["kti_channelcode"];
            }
            if (en.Contains("pricelevelid"))
            {
                this["pricelevelid"] = priceLevelDomain.GetPriceListByPriceListNameChannelOrigin((string)en["pricelevelid"], (int)this["kti_socialchannelorigin"]).pricelevelid;
            }
            else if (!String.IsNullOrEmpty(channelCode))
            {
                this["pricelevelid"] = channelDomain.GetStoreByChannelCodeOrigin(channelCode, (int)this["kti_socialchannelorigin"]).defaultPriceList;
            }

            if (((EntityReference)this["pricelevelid"]) != null)
                throw new Exception("No price level found.");

            if (en.Contains("submitdate"))
            {
                this["submitdate"] = (DateTime)en["submitdate"];
            }

            if (en.Contains("submitstatusdescription"))
            {
                this["submitstatusdescription"] = (string)en["submitstatusdescription"];
            }

            if (en.Contains("totalamount"))
            {
                this["totalamount"] = (decimal)en["totalamount"];
            }

            if (en.Contains("totaltax"))
            {
                this["totaltax"] = (decimal)en["totaltax"];
            }

            if (en.Contains("transactioncurrencyid"))
            {
                this["transactioncurrencyid"] = currencyDomain.GetCurrecyByISOCurrencyCode((string)en["transactioncurrencyid"]).transactioncurrencyid;
            }
            else if (((EntityReference)this["pricelevelid"]) != null)
            {
                this["transactioncurrencyid"] = priceLevelDomain.GetByID(((EntityReference)this["pricelevelid"]).Id.ToString()).transactioncurrencyid;
            }

            if (en.Contains("kti_branchassigned"))
            {
                this["kti_branchassigned"] = branchDomain.GetBranchByBranchCode((string)en["kti_branchassigned"]).kti_branchid;
            }
            else if (!String.IsNullOrEmpty(channelCode) && (int)this["kti_socialchannelorigin"] != 0)
            {
                this["kti_branchassigned"] = channelDomain.GetStoreByChannelCodeOrigin(channelCode, (int)this["kti_socialchannelorigin"]).branch;
            }
            if (en.Contains("order_status"))
            {
                this["kti_orderstatus"] = new OptionSetValue((int)en["order_status"]);
            }
            if (en.Contains("kti_channelurl"))
            {
                this["kti_channelurl"] = (string)en["kti_channelurl"];
            }
            if (en.Contains("kti_sourceid"))
            {
                this["kti_sourceid"] = (string)en["kti_sourceid"];
            }
            if (en.Contains("kti_staff"))
            {
                this["kti_staff"] = employeeDomain.GetEmployeeByStaffCode((string)en["kti_staff"]).ToEntityReference();
            }
            if (en.Id != Guid.Empty)
            {
                this.Id = en.Id;
            }
        }
    }
}
