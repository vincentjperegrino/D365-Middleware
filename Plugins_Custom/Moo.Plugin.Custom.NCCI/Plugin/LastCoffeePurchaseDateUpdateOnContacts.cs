using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Plugin
{
    public class LastCoffeePurchaseDateUpdateOnContacts : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {

                    MainProcess(entity, service, tracingService);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in LastCoffeePurchaseDateUpdateOnContacts.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("LastCoffeePurchaseDateUpdateOnContacts: {0}", ex.ToString());
                    throw;
                }
            }
        }


        public bool MainProcess(Entity entity, IOrganizationService service, ITracingService tracingService)
        {
            var OrderDetail = GetSalesOrderDetail(entity, service);

            if (IsValid(OrderDetail))
            {
                return Process(OrderDetail, service, tracingService);
            }
            return false;
        }


        public Entity GetSalesOrderDetail(Entity entity, IOrganizationService service)
        {

            //ncci_LastCoffeePurchaseDate
            var SalesOrderDetailTable = new QueryExpression
            {
                EntityName = "salesorderdetail",
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "salesorderdetailid",
                            Operator = ConditionOperator.Equal,
                            Values = { entity.Id}
                        }
                    }
                },
                LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = "salesorderdetail",
                        LinkToEntityName = "product",
                        LinkFromAttributeName = "productnumber",
                        LinkToAttributeName = "productnumber",
                        JoinOperator = JoinOperator.Inner,
                        Columns = new ColumnSet(false),
                        LinkCriteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression
                                {
                                    AttributeName = "ncci_productcategory",
                                    Operator = ConditionOperator.Equal,
                                    Values = { 714_430_000 }
                                }
                            }
                        }
                    },
                    new LinkEntity
                    {
                        LinkFromEntityName = "salesorderdetail",
                        LinkToEntityName = "salesorder",
                        LinkFromAttributeName = "salesorderid",
                        LinkToAttributeName = "salesorderid",
                        JoinOperator = JoinOperator.Inner,
                        Columns = new ColumnSet("createdon"),
                        EntityAlias = "OrderHeader",
                        LinkEntities =
                        {
                            new LinkEntity
                            {
                                LinkFromEntityName = "salesorder",
                                LinkToEntityName = "contact",
                                LinkFromAttributeName = "contactid",
                                LinkToAttributeName = "contactid",
                                JoinOperator = JoinOperator.Inner,
                                Columns = new ColumnSet("contactid","ncci_lastcoffeepurchasedate" , "ncci_customerjoineddate" ,"ncci_boughtcoffee"),
                                EntityAlias = "customer",
                            }

                        }
                    },

                }

            };

            var Result = service.RetrieveMultiple(SalesOrderDetailTable);

            if (Result == null || Result.Entities.Count <= 0)
            {
                return null;
            }

            return Result.Entities.FirstOrDefault();
        }



        private bool IsValid(Entity order)
        {
            if (order == null)
            {
                return false;
            }

            if (order.Contains("OrderHeader.createdon"))
            {
                if (order.Contains("customer.contactid"))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Process(Entity order, IOrganizationService service, ITracingService tracingService)
        {

            var customerEntity = new Entity("contact", (Guid)(((AliasedValue)order["customer.contactid"]).Value));

            var IsForUpdate = false;

            var latestCreatedOrder = (DateTime)((AliasedValue)order["OrderHeader.createdon"]).Value;

            var boughtcoffee = order.Contains("customer.ncci_boughtcoffee") && (bool)((AliasedValue)order["customer.ncci_boughtcoffee"]).Value;


            var hasLastCoffeePurchaseDate = order.Contains("customer.ncci_lastcoffeepurchasedate");

            var hasCustomerJoinDate = order.Contains("customer.ncci_customerjoineddate");


            if (!hasLastCoffeePurchaseDate)
            {
                customerEntity["ncci_lastcoffeepurchasedate"] = latestCreatedOrder;
                IsForUpdate = true;
            }

            if (hasLastCoffeePurchaseDate)
            {
                var current_lastcoffeepurchasedate = (DateTime)((AliasedValue)order["customer.ncci_lastcoffeepurchasedate"]).Value;

                if (OrderCreatedOn_IsGreaterThan_Curent_LastCoffee_PurchaseDate(latestCreatedOrder, current_lastcoffeepurchasedate))
                {
                    customerEntity["ncci_lastcoffeepurchasedate"] = latestCreatedOrder;
                    IsForUpdate = true;
                }
            }


            if (!hasCustomerJoinDate)
            {
                customerEntity["ncci_customerjoineddate"] = latestCreatedOrder;
                IsForUpdate = true;
            }

            if (hasCustomerJoinDate)
            {
                var current_customerjoineddate = (DateTime)((AliasedValue)order["customer.ncci_customerjoineddate"]).Value;


                if (Current_CustomerJoinedDate_IsGreaterThan_OrderCreatedOn(current_customerjoineddate, latestCreatedOrder))
                {
                    customerEntity["ncci_customerjoineddate"] = latestCreatedOrder;
                    IsForUpdate = true;
                }
            }

      
            if (IsForUpdate)
            {
                customerEntity["ncci_boughtcoffee"] = IsForUpdate;
                service.Update(customerEntity);
                return true;
            }

            if (!boughtcoffee)
            {
                customerEntity["ncci_boughtcoffee"] = true;
                service.Update(customerEntity);
                return true;
            }

            return false;
        }

        private static bool Current_CustomerJoinedDate_IsGreaterThan_OrderCreatedOn(DateTime current_customerjoineddate, DateTime latestCreatedOrder)
        {
            return current_customerjoineddate > latestCreatedOrder;
        }

        private static bool OrderCreatedOn_IsGreaterThan_Curent_LastCoffee_PurchaseDate(DateTime latestCreatedOrder, DateTime current_lastcoffeepurchasedate)
        {
            return latestCreatedOrder > current_lastcoffeepurchasedate;
        }
    }
}
