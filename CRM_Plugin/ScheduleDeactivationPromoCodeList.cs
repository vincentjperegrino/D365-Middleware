using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;

namespace CRM_Plugin
{
    public class ScheduleDeactivationPromoCodeList : CodeActivity
    {
        ITracingService tracingService;
        IWorkflowContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        EntityReference erPromoCode;
        string colPromoCode = "",
               colStateCode = "",
               colStatusCode = "";

        [RequiredArgument]
        [ReferenceTarget("ncci_promocode")]
        [Input("Promo code")]
        public InArgument<EntityReference> inPromoCode { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            tracingService = executionContext.GetExtension<ITracingService>();
            context = executionContext.GetExtension<IWorkflowContext>();
            serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            service = serviceFactory.CreateOrganizationService(context.UserId);

            if (service == null)
                tracingService.Trace("Service is empty and cannot continue the logic of workflow");

            erPromoCode = this.inPromoCode.Get(executionContext);

            ProcessDeactivationPromoCodeList();
        }

        private void ProcessDeactivationPromoCodeList()
        {
            try
            {
                colPromoCode = Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.ncci_promocode);
                colStateCode = Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.statecode);
                colStatusCode = Helper.GetPropertyDisplayName<Models.Marketing.PromoCodeList>(i => i.statuscode);

                EntityCollection ecPromoCodeList = this.GetActivePromoCodeList();

                // Create an ExecuteMultipleRequest object.
                var multipleRequest = new ExecuteMultipleRequest()
                {
                    // Assign settings that define execution behavior: continue on error, return responses. 
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    // Create an empty organization request collection.
                    Requests = new OrganizationRequestCollection()
                };

                int countThousand = 0;
                int countPromoCodeList = 0;
                int totalPromoCodeList = ecPromoCodeList.Entities.Count;

                foreach (var ePromoCodeList in ecPromoCodeList.Entities)
                {
                    try
                    {
                        Entity eUpdate = new Entity(Models.Marketing.PromoCodeList.entity_name, ePromoCodeList.Id);

                        eUpdate[colStateCode] = new OptionSetValue(1);
                        eUpdate[colStatusCode] = new OptionSetValue(714430001);

                        UpdateRequest updateRequest = new UpdateRequest { Target = eUpdate };
                        multipleRequest.Requests.Add(updateRequest);

                        countThousand += 1;
                        countPromoCodeList += 1;

                        if (countThousand == 1000 || countPromoCodeList == totalPromoCodeList)
                        {
                            // Execute all the requests in the request collection using a single web method call.
                            ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);

                            if (countThousand == 1000)
                            {
                                multipleRequest = new ExecuteMultipleRequest()
                                {
                                    Settings = new ExecuteMultipleSettings()
                                    {
                                        ContinueOnError = true,
                                        ReturnResponses = true
                                    },

                                    Requests = new OrganizationRequestCollection()
                                };

                                countThousand = 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                tracingService.Trace($"Somewthing went wrong with the process of the workflow. Error Details: {e.Message}");
            }
        }

        private EntityCollection GetActivePromoCodeList()
        {
            QueryExpression qePromoCodeList = new QueryExpression();
            qePromoCodeList.EntityName = Models.Marketing.PromoCodeList.entity_name;
            qePromoCodeList.ColumnSet = new ColumnSet(colStateCode, colStatusCode, colPromoCode);

            var promoCodeListFilter1 = new FilterExpression(LogicalOperator.And);
            promoCodeListFilter1.AddCondition(colPromoCode, ConditionOperator.Equal, erPromoCode.Id);

            qePromoCodeList.Criteria.AddFilter(promoCodeListFilter1);

            return service.RetrieveMultiple(qePromoCodeList);
        }
    }
}
