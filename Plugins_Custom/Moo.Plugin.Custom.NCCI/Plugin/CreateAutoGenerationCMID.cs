
using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Xrm.Sdk.Query;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace KTI.Moo.Plugin.Custom.NCCI.Plugin
{
    public class CreateAutoGenerationCMID : IPlugin
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
            var Customer = service.Retrieve(entity.LogicalName, entity.Id, Model.Customer.ColumnSet);

            if (IsValid(Customer))
            {
                if (IsCustomerExsistingInCMIND_Table(Customer.Id, service))
                {
                    return false;
                }

                return Process(Customer, service, tracingService);
            }

            return false;
        }

        private bool IsValid(Entity customer)
        {
            if (customer.Contains("ncci_boughtcoffee") && (bool)customer["ncci_boughtcoffee"])
            {
                return true;

                //if (!customer.Contains("ncci_newclubmembershipid"))
                //{
                //    return true;
                //}

                //if (customer.Contains("ncci_newclubmembershipid") && string.IsNullOrWhiteSpace((string)customer["ncci_newclubmembershipid"]))
                //{
                //    return true;
                //}

            }

            return false;
        }

        public bool Process(Entity customer, IOrganizationService service, ITracingService tracingService)
        {
            var Customer = new Model.Customer(customer);

            var ClubMembershipID = new Entity(Model.ClubMemberShipID.EntityName);

            ClubMembershipID["kti_customer"] = new EntityReference(Model.Customer.EntityName, Customer.customerid);

            service.Create(ClubMembershipID);

            return true;
        }



        private bool IsCustomerExsistingInCMIND_Table(Guid customer, IOrganizationService service)
        {
            QueryExpression qeEntity = new QueryExpression();
            qeEntity.EntityName = Model.ClubMemberShipID.EntityName;
            qeEntity.ColumnSet = Model.ClubMemberShipID.ColumnSet;

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_customer", ConditionOperator.Equal, customer);

            qeEntity.Criteria.AddFilter(entityFilter);

            var Result = service.RetrieveMultiple(qeEntity);

            if (Result.Entities.Any())
            {
                var ClubMembership = new Model.ClubMemberShipID(Result.Entities.FirstOrDefault());

                if (!string.IsNullOrWhiteSpace(ClubMembership.kti_clubmembershipautoid))
                {
                    return true;
                }
            }

            return false;

        }

    }
}
