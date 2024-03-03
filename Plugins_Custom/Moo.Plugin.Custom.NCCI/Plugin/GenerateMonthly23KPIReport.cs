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
using System.Threading;
using System;

namespace KTI.Moo.Plugin.Custom.NCCI.Plugin
{
    public class GenerateMonthly23KPIReport //: CodeActivity
    {

        //protected override void Execute(CodeActivityContext executionContext)
        //{
 
        //    var tracingService = executionContext.GetExtension<ITracingService>();
        //    var context = executionContext.GetExtension<IWorkflowContext>();
        //    var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
        //    var service = serviceFactory.CreateOrganizationService(context.UserId);

        //    if (context.InputParameters.Contains("Target") &&
        //        context.InputParameters["Target"] is Entity)
        //    {             
        //        try
        //        {
        //            Process(service, tracingService);
        //        }

        //        catch (FaultException<OrganizationServiceFault> ex)
        //        {
        //            throw new InvalidPluginExecutionException("An error occurred in GenerateMonthly23KPIReport.", ex);
        //        }
        //        catch (Exception ex)
        //        {
        //            tracingService.Trace("GenerateMonthly23KPIReport: {0}", ex.ToString());
        //            throw;
        //        }
        //    }
        //}


        public bool Process(IOrganizationService service, ITracingService tracingService)
        {









          return true;
        }





    }
}
