using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Model.DTO
{
    public class Orders : CRM_Plugin.Models.Sales.Order
    {
        IOrganizationService _service;
        ITracingService _tracingService;
        CustomAPI.Domain.Customer customerDomain;
        CustomAPI.Domain.PriceLevel priceLevelDomain;
        CustomAPI.Domain.ChannelManagement channelDomain;
        CustomAPI.Domain.Currency currencyDomain;
        CustomAPI.Domain.Branch branchDomain;
        CRM_Plugin.Domain.Order orderDomain;
        CRM_Plugin.Domain.Employee employeeDomain;

        public Orders(Entity en, IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
            customerDomain = new CustomAPI.Domain.Customer(service, tracingService);
            priceLevelDomain = new CustomAPI.Domain.PriceLevel(service, tracingService);
            channelDomain = new CustomAPI.Domain.ChannelManagement(service, tracingService);
            currencyDomain = new CustomAPI.Domain.Currency(service, tracingService);
            branchDomain = new CustomAPI.Domain.Branch(service, tracingService);
            orderDomain = new CRM_Plugin.Domain.Order(service, tracingService);
            employeeDomain = new CRM_Plugin.Domain.Employee(service, tracingService);

            if (en.Contains("kti_socialchannelorigin"))
            {
                this.kti_socialchannelorigin = (int)en["kti_socialchannelorigin"];
            }

            if (en.Contains("kti_channelcode"))
            {
                this.kti_channelcode = (string)en["kti_channelcode"];
            }

            if (this.kti_socialchannelorigin == 0)
                throw new Exception("No source channel found.");

            if (en.Contains("kti_specialdiscount"))
            {
                this.kti_specialdiscount = (int)en["kti_specialdiscount"];
            }

            if (en.Contains("kti_specialdiscountrefno"))
            {
                this.kti_specialdiscountrefno = (string)en["kti_specialdiscountrefno"];
            }

            if (en.Contains("kti_branchassigned"))
            {
                this.branch_assigned = branchDomain.GetBranchByBranchCode((string)en["kti_branchassigned"]).kti_branchid;
            }
            else if (!String.IsNullOrEmpty(this.kti_channelcode) && this.kti_socialchannelorigin != 0)
            {
                this.branch_assigned = channelDomain.GetStoreByChannelCodeOrigin(this.kti_channelcode, this.kti_socialchannelorigin).branch;
            }

            if (en.Contains("kti_staffid"))
            {
                if (!String.IsNullOrEmpty((string)en["kti_staffid"]))
                    this.kti_staffid = employeeDomain.GetEmployeeByStaffCode((string)en["kti_staffid"]).Id.ToString();
            }

            if (en.Contains("kti_approvalmanager"))
            {
                if (!String.IsNullOrEmpty((string)en["kti_approvalmanager"]))
                    this.kti_approvalmanager = employeeDomain.GetEmployeeByStaffCode((string)en["kti_approvalmanager"]).Id.ToString();
            }

            if (en.Contains("overriddencreatedon"))
            {
                this.overriddencreatedon = (DateTime)en["overriddencreatedon"];
            }

            if (this.overriddencreatedon == null || DateTime.MinValue == this.overriddencreatedon)
                this.overriddencreatedon = DateTime.Now;

            if (en.Contains("name"))
            {
                this.name = (string)en["name"];
            }

            if (String.IsNullOrEmpty(this.name) && !String.IsNullOrEmpty((string)en["kti_staffid"]) && !String.IsNullOrEmpty((string)en["kti_branchassigned"]))
            {
                this.name = orderDomain.GenerateOrderNumber(this.kti_socialchannelorigin, this.overriddencreatedon, (string)en["kti_branchassigned"], (string)en["kti_staffid"]);
            }

            if (String.IsNullOrEmpty(this.name))
                throw new Exception("No source ID found.");

            if (en.Contains("kti_sourceid"))
            {
                this.kti_sourceid = (string)en["kti_sourceid"];
            }

            if (String.IsNullOrEmpty(this.kti_sourceid))
            {
                this.kti_sourceid = this.name;
            }


            if (String.IsNullOrEmpty(this.kti_sourceid))
                throw new Exception("No source ID found.");

            if (en.Contains("billto_city"))
            {
                this.billto_city = (string)en["billto_city"];
            }
            if (en.Contains("billto_contactName"))
            {
                this.billto_contactName = (string)en["billto_contactName"];
            }
            if (en.Contains("billto_country"))
            {
                this.billto_country = (string)en["billto_country"];
            }
            if (en.Contains("billto_fax"))
            {
                this.billto_fax = (string)en["billto_fax"];
            }
            if (en.Contains("billto_line1"))
            {
                this.billto_line1 = (string)en["billto_line1"];
            }
            if (en.Contains("billto_line2"))
            {
                this.billto_line2 = (string)en["billto_line2"];
            }
            if (en.Contains("billto_line3"))
            {
                this.billto_line3 = (string)en["billto_line3"];
            }
            if (en.Contains("billto_name"))
            {
                this.billto_name = (string)en["billto_name"];
            }
            if (en.Contains("billto_postalcode"))
            {
                this.billto_postalcode = (string)en["billto_postalcode"];
            }
            if (en.Contains("billto_stateorprovince"))
            {
                this.billto_stateorprovince = (string)en["billto_stateorprovince"];
            }
            if (en.Contains("billto_telephone"))
            {
                this.billto_telephone = (string)en["billto_telephone"];
            }
            if (en.Contains("shipto_name"))
            {
                this.shipto_name = (string)en["shipto_name"];
            }
            if (en.Contains("emailaddress"))
            {
                this.emailaddress = (string)en["emailaddress"];
            }
            if (en.Contains("shipto_telephone"))
            {
                this.shipto_telephone = (string)en["shipto_telephone"];
            }
            if (en.Contains("customerid")) //customercode
            {
                if (!String.IsNullOrEmpty((string)en["customerid"]))
                {
                    if (Guid.TryParse((string)en["customerid"], out Guid tryCustomerID))
                    {
                        this.customer = customerDomain.GetCustomerEntityByCustomerID((string)en["customerid"]).ToEntityReference();
                    }
                    else
                    {
                        this.customer = customerDomain.GetCustomerEntityBySourceIDChannelOrigin((string)en["customerid"], this.kti_socialchannelorigin).ToEntityReference();
                    }
                }
            }
            else if (!String.IsNullOrEmpty(this.shipto_name) &&
                (!String.IsNullOrEmpty(this.emailaddress) || !String.IsNullOrEmpty(this.shipto_telephone)))
            {
                this.customer = customerDomain.GetCustomerEntityByNameEmailContact(this.shipto_name, this.emailaddress, this.shipto_telephone);
            }

            if (this.customer == null)
                throw new Exception("No customer found.");

            if (en.Contains("datefulfilled"))
            {
                this.datefulfilled = (DateTime)en["datefulfilled"];
            }
            if (en.Contains("description"))
            {
                this.description = (string)en["description"];
            }
            if (en.Contains("discountamount"))
            {
                this.discountamount = decimal.Parse(en["discountamount"].ToString());
            }
            if (en.Contains("discountpercentage"))
            {
                this.discountpercentage = decimal.Parse(en["discountpercentage"].ToString());
            }
            if (en.Contains("freightamount"))
            {
                this.freightamount = decimal.Parse(en["freightamount"].ToString());
            }

            if (en.Contains("pricelevelid"))
            {
                var priceList = priceLevelDomain.GetPriceListByPriceListNameChannelOrigin((string)en["pricelevelid"], this.kti_socialchannelorigin);

                if (pricelevelid == null)
                {
                    priceList = priceLevelDomain.GetPriceListByPriceListName((string)en["pricelevelid"]);
                }

                if(priceList != null)
                    this.pricelevelid = priceList.pricelevelid;
            }
            else if (!String.IsNullOrEmpty(this.kti_channelcode))
            {
                this.pricelevelid = channelDomain.GetStoreByChannelCodeOrigin(this.kti_channelcode, this.kti_socialchannelorigin).defaultPriceList;
            }

            if (!String.IsNullOrEmpty(this.pricelevelid))
            {
                this.pricelevelid = priceLevelDomain.GetPriceListByDefaultFirst().pricelevelid;
            }

            

            if (String.IsNullOrEmpty(this.pricelevelid))
                throw new Exception("No price level found.");

            if (en.Contains("shipto_city"))
            {
                this.shipto_city = (string)en["shipto_city"];
            }
            if (en.Contains("shipto_contactName"))
            {
                this.shipto_contactName = (string)en["shipto_contactName"];
            }
            if (en.Contains("shipto_country"))
            {
                this.shipto_country = (string)en["shipto_country"];
            }
            if (en.Contains("shipto_fax"))
            {
                this.shipto_fax = (string)en["shipto_fax"];
            }
            if (en.Contains("shipto_freighttermscode"))
            {
                this.shipto_freighttermscode = (int)en["shipto_freighttermscode"];
            }
            if (en.Contains("shipto_line1"))
            {
                this.shipto_line1 = (string)en["shipto_line1"];
            }
            if (en.Contains("shipto_line2"))
            {
                this.shipto_line2 = (string)en["shipto_line2"];
            }
            if (en.Contains("shipto_line3"))
            {
                this.shipto_line3 = (string)en["shipto_line3"];
            }
            if (en.Contains("shipto_postalcode"))
            {
                this.shipto_postalcode = (string)en["shipto_postalcode"];
            }
            if (en.Contains("shipto_stateorprovince"))
            {
                this.shipto_stateorprovince = (string)en["shipto_stateorprovince"];
            }

            if (en.Contains("submitdate"))
            {
                this.submitdate = (DateTime)en["submitdate"];
            }

            if (en.Contains("submitstatusdescription"))
            {
                this.submitstatusdescription = (string)en["submitstatusdescription"];
            }

            if (en.Contains("totalamount"))
            {
                this.totalamount = decimal.Parse(en["totalamount"].ToString());
            }

            if (en.Contains("totaltax"))
            {
                this.totaltax = decimal.Parse(en["totaltax"].ToString());
            }
            if (en.Contains("transactioncurrencyid"))
            {
                this.transactioncurrencyid = currencyDomain.GetCurrecyByISOCurrencyCode((string)en["transactioncurrencyid"]).transactioncurrencyid;
            }
            else if (!String.IsNullOrEmpty(this.pricelevelid))
            {
                this.transactioncurrencyid = (priceLevelDomain.GetByID(pricelevelid)).transactioncurrencyid;
            }

            if (en.Contains("kti_orderstatus"))
            {
                this.kti_orderstatus = (int)en["kti_orderstatus"];
            }
            if (en.Contains("kti_channelurl"))
            {
                this.kti_channelurl = (string)en["kti_channelurl"];
            }

            if (en.Contains("kti_paymenttermscode"))
            {
                this.kti_paymenttermscode = (int)en["kti_paymenttermscode"];
            }

            if (en.Id != Guid.Empty)
            {
                this.orderId = en.Id;
            }
        }
        public int kti_orderstatus { get; set; }
        public string kti_approvalmanager { get; set; }
        public string kti_staffid { get; set; }
        public string kti_specialdiscountrefno { get; set; }
        public int kti_specialdiscount { get; set; }
        public EntityReference customer { get; set; }

    }
}
