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
using System.ServiceModel;

namespace CRM_Plugin
{
    public class SyncSalesChannelInventory : CodeActivity
    {
        [Input("Sales Channel")]
        [ReferenceTarget("kti_saleschannel")]
        [RequiredArgument]
        public InArgument<EntityReference> inSalesChannel { get; set; }

        [Input("Filter Type")]
        [RequiredArgument]
        public InArgument<int> inFilterType { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var dateTimeNow = DateTime.Now;

                EntityReference erSalesChannel = inSalesChannel.Get<EntityReference>(executionContext);

                int filterType = inFilterType.Get<int>(executionContext);

                var eSalesChannel = service.Retrieve(erSalesChannel.LogicalName, erSalesChannel.Id, new ColumnSet(true));

                Process(eSalesChannel, dateTimeNow, filterType, service);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in SyncSalesChannelInventory.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("SyncSalesChannelInventory: {0}", ex.ToString());
                throw;
            }
        }

        public bool Process(Entity eSalesChannel, DateTime dateTimeNow, int filterType, IOrganizationService service)
        {
            //var salesChannel = new CRM_Plugin.SalesChannel.Models.SalesChannel(eSalesChannel, service);

            //var instance = salesChannel.GetCompanyInstance();

            //var lastSyncDateTime = salesChannel.GetLastSyncDateTime();

            //var listProduct = salesChannel.GetSalesChannelProductsByModifiedCreatedDate(dateTimeNow, lastSyncDateTime, filterType);

            //foreach (var eProduct in listProduct)
            //{
            //    CRM_Plugin.Models.Items.Products product;
            //    if (instance == "3387" || instance == "3390" || instance == "3391")
            //    {
            //        product = new CRM_Plugin.Models.Items.CCPI.Products(eProduct, eSalesChannel, service, salesChannel, "inventory");
            //    }
            //    else
            //    {
            //        product = new CRM_Plugin.Models.Items.Products(eProduct, service, eSalesChannel, _domainType);
            //    }

            //    product.Replicate().GetAwaiter().GetResult();
            //}

            return true;
        }
    }
}
