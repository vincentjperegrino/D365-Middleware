using Microsoft.Xrm.Sdk;
using System;
using ParameterModel = CRM_Plugin.Models.DTO;
using CRM_Plugin.Function;

namespace CRM_Plugin
{
    public class EmployeePostCreate : IPlugin
    {
        ITracingService tracingService;
        IPluginExecutionContext context;
        IOrganizationServiceFactory serviceFactory;
        IOrganizationService service;

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = servicefactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity target = (Entity)context.InputParameters["Target"];

                Entity entity = service.Retrieve(target.LogicalName, target.Id, ParameterModel.EmployeeDTOParameters.columnSet);
                var employeeRecord = TransformEntity(entity);

                try
                {
                    if (!string.IsNullOrEmpty(employeeRecord.password))
                    {
                        var employeeDomain = new Employee.Domain.Employee(service, tracingService);
                        var Function = new EmployeePasswordHashing(employeeDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppPassword(employeeRecord);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error on employee creation: " + ex.Message);
                }
            }
        }

        private Models.Employee TransformEntity(Entity entity)
        {
            var parameter = new ParameterModel.EmployeeDTOParameters(entity);
            var employee = new Models.Employee()
            {
                employeeRecord = parameter.employeeRecord,
                password = parameter.password,
                passwordFlag = parameter.passwordFlag
            };

            return employee;
        }
    }
}
