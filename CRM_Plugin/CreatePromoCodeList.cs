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
using System.Collections;

namespace CRM_Plugin
{
    public class CreatePromoCodeList : CodeActivity
    {
        ITracingService tracingService;
        IWorkflowContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        Entity eGeneratePromo;

        [ReferenceTarget("campaignactivity")]
        [Input("Approved Campaign Activity")]
        public InArgument<EntityReference> inCampaignActivity { get; set; }


        [ReferenceTarget("kti_promo")]
        [Input("Approved Promo")]
        public InArgument<EntityReference> inPromo { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var erCampaignAcitivty = this.inCampaignActivity.Get(executionContext);
            var erPromo = this.inPromo.Get(executionContext);

            tracingService = executionContext.GetExtension<ITracingService>();
            context = executionContext.GetExtension<IWorkflowContext>();
            serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            service = serviceFactory.CreateOrganizationService(context.UserId);

            if (service == null)
                tracingService.Trace("Service is empty and cannot continue the logic of workflow");

            if (erCampaignAcitivty.Id != Guid.Empty || erCampaignAcitivty != null)
            {
                eGeneratePromo = service.Retrieve(erCampaignAcitivty.LogicalName, erCampaignAcitivty.Id, new ColumnSet(true));
            }
            else
            {
                eGeneratePromo = service.Retrieve(erPromo.LogicalName, erPromo.Id, new ColumnSet(true));
            }

            ProcessPromoCode();
        }

        private void ProcessPromoCode()
        {
            CRM_Plugin.Promo.Interface.IPromo promo = null;


            switch(eGeneratePromo.LogicalName)
            {
                case "kti_promo":
                    promo = new CRM_Plugin.Promo.Domain.PromoDefault(service, tracingService);
                    break;

                case "campaignactivity":
                    promo = new CRM_Plugin.Promo.Domain.PromoCampaignActivity(service, tracingService);
                    break;
            }

            if(promo != null)
            {
                if (!promo.Process(eGeneratePromo))
                    throw new InvalidPluginExecutionException("There is something wrong with the process");
            }
            else
            {
                throw new InvalidPluginExecutionException("No business process loaded in this plugin");
            }
        }
    }
}
