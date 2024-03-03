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
    public class SyncSalesChannelProduct : CodeActivity
    {
        [Input("Sales Channel")]
        [ReferenceTarget("kti_saleschannel")]
        [RequiredArgument]
        public InArgument<EntityReference> inSalesChannel { get; set; }

        [Input("Filter Type")]
        [RequiredArgument]
        public InArgument<int> inFilterType { get; set; }

        [Input("Inventory")]
        [RequiredArgument]
        public InArgument<bool> inInventory { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                string domainType = "product";

                var dateTimeNow = DateTime.UtcNow;

                EntityReference erSalesChannel = inSalesChannel.Get<EntityReference>(executionContext);

                int filterType = inFilterType.Get<int>(executionContext);

                bool inventory = inInventory.Get<bool>(executionContext);

                if (inventory)
                    domainType = "inventory";

                var eSalesChannel = service.Retrieve(erSalesChannel.LogicalName, erSalesChannel.Id, new ColumnSet(true));

                Process(eSalesChannel, dateTimeNow, filterType, domainType, service, tracingService);

                UpdateLastSyncDateTime(eSalesChannel, service);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in SyncSalesChannelProduct.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("SyncSalesChannelProduct: {0}", ex.ToString());
                throw;
            }
        }

        public bool Process(Entity eSalesChannel, DateTime dateTimeNow, int filterType, string domainType, IOrganizationService service, ITracingService tracingService)
        {
            var salesChannel = new CRM_Plugin.SalesChannel.Models.SalesChannel(eSalesChannel, service);

            var instance = salesChannel.GetCompanyInstance();

            var lastSyncDateTime = salesChannel.GetLastSyncDateTime();

            var listProduct = salesChannel.GetSalesChannelProductsByModifiedDate(dateTimeNow, lastSyncDateTime, filterType);

            foreach (var eProduct in listProduct)
            {
                CRM_Plugin.Models.Items.Products product;

                if (instance == "3387" || instance == "3390" || instance == "3391")
                {
                    product = new CRM_Plugin.Models.Items.CCPI.Products(eProduct, eSalesChannel, service, salesChannel, domainType);
                }
                else
                {
                    product = new CRM_Plugin.Models.Items.Products(eProduct , service , salesChannel, eSalesChannel, domainType);
                }

                try
                {
                    product.Replicate().GetAwaiter().GetResult();
                }
                catch(Exception ex)
                {
                    tracingService.Trace(ex.Message);
                }
            }

            return true;
        }

        private bool UpdateLastSyncDateTime(Entity entity, IOrganizationService service)
        {
            Entity updateSalesChannel = new Entity(entity.LogicalName, entity.Id);

            updateSalesChannel["kti_lastsyncdateandtime"] = DateTime.UtcNow;

            service.Update(updateSalesChannel);

            return true;
        }
    }
}
