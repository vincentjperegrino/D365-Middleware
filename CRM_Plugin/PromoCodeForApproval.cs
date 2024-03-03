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
    public class PromoCodeForApproval : CodeActivity
    {
        ITracingService tracingService;
        IWorkflowContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        EntityReference erCampaignActivity;

        [RequiredArgument]
        [ReferenceTarget("campaignactivity")]
        [Input("Campaign Activity")]
        public InArgument<EntityReference> inCampaignActivity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            tracingService = executionContext.GetExtension<ITracingService>();
            context = executionContext.GetExtension<IWorkflowContext>();
            serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            service = serviceFactory.CreateOrganizationService(context.UserId);

            if (service == null)
                tracingService.Trace("Service is empty and cannot continue the logic of workflow");

            erCampaignActivity = this.inCampaignActivity.Get(executionContext);

            ProcessForApproval();
        }

        private void ProcessForApproval()
        {
            try
            {
                Entity eUpdate = new Entity(Models.Marketing.CampaignActivity.entity_name, erCampaignActivity.Id);

                eUpdate[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.statecode)] = new OptionSetValue(0);
                eUpdate[Helper.GetPropertyDisplayName<Models.Marketing.CampaignActivity>(i => i.statuscode)] = new OptionSetValue(4);

                service.Update(eUpdate);
            }
            catch (Exception e)
            {
                tracingService.Trace($"Somewthing went wrong with the process of the workflow. Error Details: {e.Message}");
            }
        }
    }
}
