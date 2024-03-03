using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Plugin
{
    public class LastCoffeePurchaseDateUpdateOnContactsWithMaxRequest //: IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {

            // Obtain the tracing service
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  


            // Obtain the organization service reference which you will need for  
            // web service calls.
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var MaxRequest = (int)context.InputParameters["kti_MaxRequest"];
                var output = MainProcess(MaxRequest, service, tracingService);

                context.OutputParameters["kti_LastCoffeePurchaseDateUpdateOnContactsWithMaxRequest_Response"] = output;
            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in LastCoffeePurchaseDateUpdateOnContactsWithMaxRequest.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("LastCoffeePurchaseDateUpdateOnContactsWithMaxRequest: {0}", ex.ToString());
                throw;
            }

        }


        public Entity MainProcess(int MaxRequest, IOrganizationService service, ITracingService tracingService)
        {
            var SalesOrderItem = GetSalesOrderDetail(service);

            if (SalesOrderItem == null || SalesOrderItem.Count <= 0)
            {
                var Defaultresult = new Entity();

                Defaultresult["IsSuccess"] = true;
                Defaultresult["MaxRequest"] = MaxRequest;
                Defaultresult["TotalRequest"] = 0;
                Defaultresult["RequestRemaining"] = 0;

                return Defaultresult;
            }

            return Process(MaxRequest, SalesOrderItem, service, tracingService);
        }

        public List<Entity> GetSalesOrderDetail(IOrganizationService service)
        {

            //ncci_LastCoffeePurchaseDate
            var SalesOrderTable = new QueryExpression
            {
                EntityName = "salesorder",
                ColumnSet = new ColumnSet("salesorderid", "customerid", "createdon"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression
                        {
                            AttributeName = "createdon",
                            Operator = ConditionOperator.LastXDays,
                            Values = {1}
                        }
                    }
                },
                LinkEntities =
                {
                    new LinkEntity
                    {
                        LinkFromEntityName = "salesorder",
                        LinkToEntityName = "salesorderdetail",
                        LinkFromAttributeName = "salesorderid",
                        LinkToAttributeName = "salesorderid",
                        JoinOperator = JoinOperator.Inner,
                        Columns = new ColumnSet(false),
                        EntityAlias = "Orderitems",
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
                            }
                        }
                    },
                    new LinkEntity
                    {
                        LinkFromEntityName = "salesorder",
                        LinkToEntityName = "contact",
                        LinkFromAttributeName = "contactid",
                        LinkToAttributeName = "contactid",
                        JoinOperator = JoinOperator.Inner,
                        Columns = new ColumnSet("ncci_lastcoffeepurchasedate" , "ncci_customerjoineddate"),
                        EntityAlias = "customer",
                    }
                }

            };

            var Result = RetrieveAllRecords(service, SalesOrderTable);

            if (Result == null || Result.Count <= 0)
            {
                return null;
            }

            return Result;
        }

        //private bool IsValid(Entity SalesOrderItem)
        //{
        //    if (SalesOrderItem == null)
        //    {
        //        return false;
        //    }

        //    return false;
        //}

        public Entity Process(int MaxRequest, List<Entity> Salesorder, IOrganizationService service, ITracingService tracingService)
        {

            var ListOfCustomer = Salesorder.Select(order => ((EntityReference)order["customerid"]).Id).Distinct().ToList();


            // Create a list to hold the update requests
            List<OrganizationRequest> requests = new List<OrganizationRequest>();

            foreach (var customer in ListOfCustomer)
            {
                var customerEntity = new Entity("contact", customer);

                var IsForUpdate = false;

                var latestCreatedOrder = Salesorder.Where(order => ((EntityReference)order["customerid"]).Id == customer)
                                                          .OrderByDescending(order => (DateTime)order["createdon"])
                                                          .Select(order => (DateTime)order["createdon"])
                                                          .FirstOrDefault();

                var firstcoffeePurchasedDateInTheDay = Salesorder.Where(order => ((EntityReference)order["customerid"]).Id == customer)
                                                          .OrderBy(order => (DateTime)order["createdon"])
                                                          .Select(order => (DateTime)order["createdon"])
                                                          .FirstOrDefault();


                if (Salesorder.Any(order => ((EntityReference)order["customerid"]).Id == customer && !order.Contains("customer.ncci_lastcoffeepurchasedate")))
                {
                    customerEntity["ncci_lastcoffeepurchasedate"] = latestCreatedOrder;
                    IsForUpdate = true;
                }


                if (Salesorder.Any(order => ((EntityReference)order["customerid"]).Id == customer && order.Contains("customer.ncci_lastcoffeepurchasedate")))
                {
                    var current_lastcoffeepurchasedate = Salesorder.Where(order => ((EntityReference)order["customerid"]).Id == customer)
                                                               .Select(order => (DateTime)((AliasedValue)order["customer.ncci_lastcoffeepurchasedate"]).Value)
                                                               .FirstOrDefault();

                    if (latestCreatedOrder > current_lastcoffeepurchasedate)
                    {
                        customerEntity["ncci_lastcoffeepurchasedate"] = latestCreatedOrder;
                        IsForUpdate = true;
                    }
                }

                if (Salesorder.Any(order => ((EntityReference)order["customerid"]).Id == customer && !order.Contains("customer.ncci_customerjoineddate")))
                {
                    customerEntity["ncci_customerjoineddate"] = firstcoffeePurchasedDateInTheDay;
                    IsForUpdate = true;
                }

                if (Salesorder.Any(order => ((EntityReference)order["customerid"]).Id == customer && order.Contains("customer.ncci_customerjoineddate")))
                {
                    var current_customerjoineddate = Salesorder.Where(order => ((EntityReference)order["customerid"]).Id == customer)
                                                   .Select(order => (DateTime)((AliasedValue)order["customer.ncci_customerjoineddate"]).Value)
                                                   .FirstOrDefault();

                    if (current_customerjoineddate > firstcoffeePurchasedDateInTheDay)
                    {
                        customerEntity["ncci_customerjoineddate"] = firstcoffeePurchasedDateInTheDay;
                        IsForUpdate = true;
                    }

                }

                if (IsForUpdate)
                {
                    customerEntity["ncci_boughtcoffee"] = IsForUpdate;
                    UpdateRequest updateRequest = new UpdateRequest { Target = customerEntity };
                    requests.Add(updateRequest);
                }
            }

            if (requests.Count > 0)
            {

                // Set the batch size
                int batchSize = 10;

                // Create a batch request containing the update requests
                ExecuteMultipleRequest batchRequest = new ExecuteMultipleRequest
                {
                    Settings = new ExecuteMultipleSettings
                    {
                        ContinueOnError = true, // Set to true if you want the plugin to continue even if some updates fail
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                // Loop through the requests and add them to the batch request in batches
                for (int i = 0; i < MaxRequest || i < requests.Count; i += batchSize)
                {
                    // Get the current batch of requests
                    List<OrganizationRequest> currentBatch = requests.Skip(i).Take(batchSize).ToList();

                    // Add the current batch of requests to the batch request
                    batchRequest.Requests.AddRange(currentBatch);

                    // Execute the batch request
                    ExecuteMultipleResponse batchResponse = (ExecuteMultipleResponse)service.Execute(batchRequest);

                    // Process the batch response
                    foreach (var responseItem in batchResponse.Responses)
                    {
                        if (responseItem.Fault != null)
                        {
                            tracingService.Trace(responseItem.Fault.Message);
                            continue;
                        }

                        tracingService.Trace("Success");
                    }

                    // Clear the batch request for the next iteration
                    batchRequest.Requests.Clear();
                }
            }

            var result = new Entity();

            result["IsSuccess"] = true;
            result["MaxRequest"] = MaxRequest;
            result["TotalRequest"] = requests.Count;
            result["RequestRemaining"] = GetRemainingRequest(MaxRequest, requests.Count);

            return result;
        }

        private static int GetRemainingRequest(int MaxRequest, int requestsCount)
        {
            return requestsCount - MaxRequest < 0 ? 0 : requestsCount - MaxRequest;
        }

        private static List<Entity> RetrieveAllRecords(IOrganizationService service, QueryExpression query)
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