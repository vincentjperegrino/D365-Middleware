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
    public class ScheduledDeactivationPromoCode : CodeActivity
    {
        ITracingService tracingService;
        IWorkflowContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        DateTime executionTime;
        string colValidFrom = "",
               colValidTo = "",
               colStateCode = "",
               colStatusCode = "";

        [RequiredArgument]
        [Input("Execution Date and Time")]
        public InArgument<DateTime> inExecutionDtm { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            tracingService = executionContext.GetExtension<ITracingService>();
            context = executionContext.GetExtension<IWorkflowContext>();
            serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            service = serviceFactory.CreateOrganizationService(context.UserId);

            if (service == null)
                tracingService.Trace("Service is empty and cannot continue the logic of workflow");

            executionTime = Helper.RetrieveLocalTimeFromUTCTime(service, this.inExecutionDtm.Get(executionContext));

            ProcessDeactivationPromoCode();
        }

        private void ProcessDeactivationPromoCode()
        {
            try
            {
                colValidFrom = Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_validfrom);
                colValidTo = Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.ncci_validto);
                colStateCode = Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.statecode);
                colStatusCode = Helper.GetPropertyDisplayName<Models.Marketing.PromoCode>(i => i.statuscode);

                EntityCollection ecPromoCode = this.GetActivePromoCodeExpired();

                var multipleRequest = new ExecuteMultipleRequest()
                {
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                foreach (var ePromoCode in ecPromoCode.Entities)
                {
                    try
                    {
                        Entity eUpdate = new Entity(Models.Marketing.PromoCode.entity_name, ePromoCode.Id);

                        eUpdate[colStateCode] = new OptionSetValue(1);
                        eUpdate[colStatusCode] = new OptionSetValue(714430001);

                        UpdateRequest updateRequest = new UpdateRequest { Target = eUpdate };
                        multipleRequest.Requests.Add(updateRequest);
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace(ex.Message);
                    }
                }

                ExecuteMultipleResponse multipleResponse = (ExecuteMultipleResponse)service.Execute(multipleRequest);

            }
            catch (Exception e)
            {
                tracingService.Trace($"Somewthing went wrong with the process of the workflow. Error Details: {e.Message}");
            }
        }

        private EntityCollection GetActivePromoCodeExpired()
        {
            QueryExpression qePromoCode = new QueryExpression();
            qePromoCode.EntityName = Models.Marketing.PromoCode.entity_name;
            qePromoCode.ColumnSet = new ColumnSet(colStateCode, colValidFrom, colValidTo);

            var promoCodeFilter1 = new FilterExpression(LogicalOperator.And);
            promoCodeFilter1.AddCondition(colStateCode, ConditionOperator.Equal, 0);
            promoCodeFilter1.AddCondition(colValidTo, ConditionOperator.LessThan, executionTime);

            qePromoCode.Criteria.AddFilter(promoCodeFilter1);

            return service.RetrieveMultiple(qePromoCode);
        }
    }
}
