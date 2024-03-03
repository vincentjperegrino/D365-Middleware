using Microsoft.Xrm.Sdk;
using System;
using CRM_Plugin.Function;
using ParameterModel = CRM_Plugin.Models.DTO;
using ParameterEntityHelper = CRM_Plugin.EntityHelper;

namespace CRM_Plugin
{
    public class EmployeePostUpdate : IPlugin
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
                    if (IsPasswordChanged(target, employeeRecord))
                    {
                        var employeeDomain = new Employee.Domain.Employee(service, tracingService);
                        var Function = new EmployeePasswordHashing(employeeDomain, tracingService);
                        bool isProcessSuccess = Function.ProcessAppPassword(employeeRecord);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error on employee update: " + ex.Message);
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

        private bool IsPasswordChanged(Entity target, Models.Employee employee)
        {

            if (target.Contains(ParameterEntityHelper.EmployeeParameters.password))
            {
                if (!string.IsNullOrEmpty(employee.password)
                    && string.IsNullOrEmpty(employee.passwordFlag)
                    || !employee.password.Equals(employee.passwordFlag))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
