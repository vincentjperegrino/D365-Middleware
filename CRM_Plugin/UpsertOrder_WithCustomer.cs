using CRM_Plugin.CustomAPI.Model.DTO;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin
{
    public class UpsertOrder_WithCustomer : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService)); //for tracinglog, for tracing
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext)); //providing info on what action/event of data, holding parameter
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory)); //initialization of authorazation of instance, crud process
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId); //CRM library
            try
            {
                var output = Process(tracingService, context.InputParameters, service);

                context.OutputParameters["kti_UpsertOrder_Response"] = output;

            }
            catch (Exception e)
            {
                tracingService.Trace(e.Message);

                throw new Exception(e.Message);
            }
        }


        public bool Process(ITracingService tracingService, ParameterCollection customParams, IOrganizationService service)
        {
            var erroron = "";
            try
            {
                //Customer Process
                erroron = "Customer Process";
                var CustomerParameters = (Entity)customParams["kti_UpsertOrder_CustomerParameters"];
                var CustomerDomain = new CRM_Plugin.Domain.Recievers.Customer(service, tracingService);

                erroron = "AddLookUp_OptionsetForCustomer(CustomerParameters)";
                CustomerParameters = AddLookUp_OptionsetForCustomer(CustomerParameters);

                erroron = "GetCustomerID(CustomerParameters, CustomerDomain)";

                var CustomerID = GetCustomerID(CustomerParameters, CustomerDomain);
                //   var CustomerID = CustomerDomain.UpsertWithGuid(CustomerParameters);

                //Order Process
                erroron = "Order Process";
                var OrderParameters = (Entity)customParams["kti_UpsertOrder_OrderParameters"];

                OrderParameters[Core.Helper.EntityHelper.SalesOrder.customer] = new EntityReference(Core.Helper.EntityHelper.CRM.Customer.entity_name, CustomerID);

                var OrderDomain = new CRM_Plugin.Domain.Recievers.Order(service, tracingService);
                erroron = "AddLookUp_OptionsetForOrder(OrderParameters)";
                OrderParameters = AddLookUp_OptionsetForOrder(OrderParameters);

                erroron = "OrderDomain.UpsertWithGuid(OrderParameters)";
                var OrderID = OrderDomain.UpsertWithGuid(OrderParameters);

                var OrderDetails = OrderDomain.Get(OrderID);

                var IsPricedLockedOrder = false;

                if (OrderDetails.Contains(Core.Helper.EntityHelper.SalesOrder.ispricelocked))
                {
                    IsPricedLockedOrder = (bool)OrderDetails[Core.Helper.EntityHelper.SalesOrder.ispricelocked];

                }

                //Order Line Process
                erroron = "Order Line Process";
                var orderLineListCol = (EntityCollection)customParams["kti_UpsertOrder_OrderLineParameters"];
                var OrderLineDomain = new CRM_Plugin.Domain.Recievers.OrderLine(service, tracingService);

                erroron = "OrderLineDomain.GetOrderItemList(OrderID)";
                var ExistingItems = OrderLineDomain.GetOrderItemList(OrderID);
                var WithLineItemList = ExistingItems.Entities.Where(existing => !existing.Contains("parentbundleid")).ToList();

                var ProductDomain = new CustomAPI.Domain.Product(service, tracingService);

                foreach (var items in orderLineListCol.Entities)
                {
                    erroron = "ProductDomain.GetProductByProductNumber(ProductNumber)";
                    var ProductNumber = (string)items["productid"];
                    var ProductDetails = ProductDomain.GetProductByProductNumber(ProductNumber);

                    if (ProductDetails != null)
                    {
                        items["productid"] = new EntityReference("product", (Guid)ProductDetails["productid"]);
                        items["uomid"] = ProductDetails["defaultuomid"];

                        if (items.Contains("productdescription"))
                        {
                            items.Attributes.Remove("productdescription");
                        }
                    }
                    else
                    {
                        if (items.Contains("productid"))
                        {
                            items.Attributes.Remove("productid");
                        }

                        items["isproductoverridden"] = true;
                    }

                    erroron = "AddLookUp_OptionsetForOrderLineItem(items)";
                    var remappeditems = AddLookUp_OptionsetForOrderLineItem(items);

                    erroron = "if WithLineItemList";
                    if (WithLineItemList != null && WithLineItemList.Count > 0 && WithLineItemList.Any(existing => (string)existing["kti_lineitemnumber"] == (string)items["kti_lineitemnumber"]))
                    {
                        var ExistingOrderItem = WithLineItemList.Where(existing => (string)existing["kti_lineitemnumber"] == (string)items["kti_lineitemnumber"])
                                                                  .FirstOrDefault();


                        erroron = "if ProductType_Bundle";
                        var errroraddon = "Not Remove ProductID. ";
                        var ProductType_Bundle = 3;
                        if (ProductDetails != null && ProductDetails.Contains("producttypecode") && ((OptionSetValue)ProductDetails["producttypecode"]).Value == ProductType_Bundle)
                        {
                            errroraddon = "Removed ProductID ";
                            remappeditems.Attributes.Remove("productid");
                        }

                        erroron = "Removed productid, uomid, priceunit if price is locked";
                        if (IsPricedLockedOrder || (ExistingOrderItem.Contains("salesorderispricelocked") && (bool)ExistingOrderItem["salesorderispricelocked"]))
                        {

                            var errroraddonIsLocked = "";
                            if (remappeditems.Contains("productid"))
                            {
                                errroraddonIsLocked += "Price Locked Removed ProductID. ";
                                remappeditems.Attributes.Remove("productid");
                            }

                            if (remappeditems.Contains("uomid"))
                            {
                                errroraddonIsLocked += "Price Locked Removed uomid. ";
                                remappeditems.Attributes.Remove("uomid");
                            }

                            if (remappeditems.Contains("priceperunit"))
                            {
                                errroraddonIsLocked += "Price Locked Removed priceperunit. ";
                                remappeditems.Attributes.Remove("priceperunit");
                            }             
                            
                            if (remappeditems.Contains("ispriceoverridden"))
                            {
                                errroraddonIsLocked += "Price Locked Removed ispriceoverridden. ";
                                remappeditems.Attributes.Remove("ispriceoverridden");
                            }

                            if (remappeditems.Contains("priceperunit_base"))
                            {
                                errroraddonIsLocked += "Price Locked Removed priceperunit_base. ";
                                remappeditems.Attributes.Remove("priceperunit_base");
                            }

                            errroraddon = errroraddonIsLocked;
                        }

                        erroron = "Update(OrderID, remappeditems, ExistingOrderItemID) " + errroraddon;
                        OrderLineDomain.Update(OrderID, remappeditems, ExistingOrderItem.Id);
                        continue;
                    }


                    erroron = "OrderLineDomain.Create(OrderID, remappeditems)";
                    OrderLineDomain.Create(OrderID, remappeditems);
                }

                return true;
            }
            catch (Exception e)
            {
                tracingService.Trace(e.Message);

                throw new Exception($"UpsertOrder_WithCustomer Error on {erroron} {e.Message}");
            }
        }


        private Entity AddLookUp_OptionsetForOrder(Entity Order)
        {

            if (Order.Contains("kti_branchassigned"))
            {
                var kti_branchassigned = (string)Order["kti_branchassigned"];
                Order["kti_branchassigned"] = new EntityReference("kti_branch", "kti_branchcode", kti_branchassigned);
            }

            if (Order.Contains("kti_socialchannel"))
            {
                var kti_socialchannel = (string)Order["kti_socialchannel"];
                Order["kti_socialchannel"] = new EntityReference("kti_saleschannel", "kti_saleschannelcode", kti_socialchannel);
            }

            if (Order.Contains("kti_socialchannelorigin"))
            {
                var kti_socialchannelorigin = (int)Order["kti_socialchannelorigin"];
                Order["kti_socialchannelorigin"] = new OptionSetValue(kti_socialchannelorigin);

            }

            if (Order.Contains("kti_paymenttermscode"))
            {
                var kti_paymenttermscode = (int)Order["kti_paymenttermscode"];
                Order["kti_paymenttermscode"] = new OptionSetValue(kti_paymenttermscode);

            }

            if (Order.Contains("kti_orderstatus"))
            {
                var kti_orderstatus = (int)Order["kti_orderstatus"];
                Order["kti_orderstatus"] = new OptionSetValue(kti_orderstatus);
            }


            if (Order.Contains("statuscode"))
            {
                var statuscode = (int)Order["statuscode"];
                Order["statuscode"] = new OptionSetValue(statuscode);

            }

            if (Order.Contains("discountamount"))
            {
                var discountamount = Order["discountamount"].ToString();
                Order["discountamount"] = new Money(decimal.Parse(discountamount));
            }

            if (Order.Contains("freightamount"))
            {
                var freightamount = Order["freightamount"].ToString();
                Order["freightamount"] = new Money(decimal.Parse(freightamount));
            }

            if (Order.Contains("overriddencreatedon"))
            {
                var overriddencreatedon = Order["overriddencreatedon"].ToString();
                Order["overriddencreatedon"] = DateTime.Parse(overriddencreatedon);
            }


            return Order;
        }


        private Entity AddLookUp_OptionsetForOrderLineItem(Entity OrderItem)
        {
            if (OrderItem.Contains("kti_socialchannelorigin"))
            {
                var kti_socialchannelorigin = (int)OrderItem["kti_socialchannelorigin"];
                OrderItem["kti_socialchannelorigin"] = new OptionSetValue(kti_socialchannelorigin);

            }

            if (OrderItem.Contains("statuscode"))
            {
                var statuscode = (int)OrderItem["statuscode"];
                OrderItem["statuscode"] = new OptionSetValue(statuscode);
            }

            if (OrderItem.Contains("quantity"))
            {
                var quantity = OrderItem["quantity"].ToString();
                OrderItem["quantity"] = decimal.Parse(quantity);
            }

            if (OrderItem.Contains("manualdiscountamount"))
            {
                var manualdiscountamount = OrderItem["manualdiscountamount"].ToString();
                OrderItem["manualdiscountamount"] = new Money(decimal.Parse(manualdiscountamount));
            }

            if (OrderItem.Contains("tax"))
            {
                var tax = OrderItem["tax"].ToString();
                OrderItem["tax"] = new Money(decimal.Parse(tax));
            }
            if (OrderItem.Contains("priceperunit"))
            {
                var priceperunit = OrderItem["priceperunit"].ToString();
                OrderItem["priceperunit"] = new Money(decimal.Parse(priceperunit));
            }

            if (OrderItem.Contains("overriddencreatedon"))
            {
                var overriddencreatedon = OrderItem["overriddencreatedon"].ToString();
                OrderItem["overriddencreatedon"] = DateTime.Parse(overriddencreatedon);
            }

            if (OrderItem.Contains("kti_orderstatus"))
            {
                var kti_orderstatus = (int)OrderItem["kti_orderstatus"];
                OrderItem["kti_orderstatus"] = new OptionSetValue(kti_orderstatus);
            }


            return OrderItem;
        }


        private Entity AddLookUp_OptionsetForCustomer(Entity Customer)
        {
            if (Customer.Contains("kti_socialchannelorigin"))
            {
                var kti_socialchannelorigin = (int)Customer["kti_socialchannelorigin"];
                Customer["kti_socialchannelorigin"] = new OptionSetValue(kti_socialchannelorigin);

            }

            if (Customer.Contains("birthdate"))
            {
                var birthdate = Customer["birthdate"].ToString();
                Customer["birthdate"] = DateTime.Parse(birthdate);
            }

            if (Customer.Contains("overriddencreatedon"))
            {
                var overriddencreatedon = Customer["overriddencreatedon"].ToString();
                Customer["overriddencreatedon"] = DateTime.Parse(overriddencreatedon);
            }

            return Customer;
        }

        private Guid GetCustomerID(Entity CustomerParameters, CRM_Plugin.Domain.Recievers.Customer CustomerDomain)
        {

            if (CustomerParameters.Contains(Core.Helper.EntityHelper.CRM.Customer.entity_id) && !CustomerParameters.Contains("is_modified"))
            {
                var NotModifiedIDcontactid = new Guid(CustomerParameters[Core.Helper.EntityHelper.CRM.Customer.entity_id].ToString());
                return NotModifiedIDcontactid;
            }

            if (CustomerParameters.Contains(Core.Helper.EntityHelper.CRM.Customer.entity_id) && CustomerParameters.Contains("is_modified") && (int)CustomerParameters["is_modified"] == 0)
            {
                var NotModifiedIDcontactid = new Guid(CustomerParameters[Core.Helper.EntityHelper.CRM.Customer.entity_id].ToString());
                return NotModifiedIDcontactid;
            }

            if (CustomerParameters.Contains("is_modified"))
            {
                CustomerParameters.Attributes.Remove("is_modified");
            }

            return CustomerDomain.UpsertWithGuid(CustomerParameters);

        }


    }
}
