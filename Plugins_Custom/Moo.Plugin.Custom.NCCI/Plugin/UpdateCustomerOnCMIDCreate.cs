using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Plugin
{
    public class UpdateCustomerOnCMIDCreate : IPlugin
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
                    throw new InvalidPluginExecutionException("An error occurred in AutoGenerationCMID.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("AutoGenerationCMID: {0}", ex.ToString());
                    throw;
                }
            }
        }

        public bool MainProcess(Entity entity, IOrganizationService service, ITracingService tracingService)
        {
            var ClubMembershipID = service.Retrieve(entity.LogicalName, entity.Id, Model.ClubMemberShipID.ColumnSet);

            if (IsValid(ClubMembershipID))
            {
                return Process(ClubMembershipID, service, tracingService);
            }
            return false;
        }


        private bool IsValid(Entity ClubMembershipID)
        {
            if (ClubMembershipID.Contains("kti_customer"))
            {
                if (ClubMembershipID.Contains("kti_clubmembershipautoid") && !string.IsNullOrWhiteSpace((string)ClubMembershipID["kti_clubmembershipautoid"]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Process(Entity clubMembershipID, IOrganizationService service, ITracingService tracingService)
        {
            var ClubMembershipID = new Model.ClubMemberShipID(clubMembershipID);


            var Customer = new Entity(Model.Customer.EntityName, ClubMembershipID.kti_customer);
            Customer["ncci_newclubmembershipid"] = ClubMembershipID.kti_clubmembershipautoid;

            service.Update(Customer);

            return true;
        }

    }
}
